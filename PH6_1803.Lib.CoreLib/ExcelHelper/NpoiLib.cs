/*
 * 1、NuGet包安装NPOI/或直接引用NPOI.dll
 * 2、如果需要用到Color，则需添加程序集引用 System.Drawing
 * **/
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;

namespace PH6_1803.Lib.CoreLib
{
    public class NpoiLib
    {
        /// <summary>
        /// NPOI导出/导入Excel
        /// </summary>
        public class NpoiExcelHelper
        {
            /// <summary>
            /// 导出一个sheet的Excel,所见即所得
            /// </summary>
            /// <param name="dt">数据集合</param>
            /// <param name="strValues">表头列及列对应数据库中的字段</param>
            /// <returns>返回一个HSSFWorkbook  (.xls文件格式)</returns>
            public static MemoryStream ExportExcel(DataTable dt, Dictionary<string, string> strValues)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("sheet1");

                #region //Excel样式
                Color LevelOneColor = Color.FromArgb(255, 154, 0);

                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                style.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                //style.FillBackgroundColor = GetXLColour(workbook, LevelOneColor); 
                #endregion

                #region //将数据写入Excel

                #region //表头
                NPOI.SS.UserModel.IRow headerrow = sheet1.CreateRow(0);
                headerrow.Height = 35 * 20;
                if (strValues != null)
                {
                    int h = 0;
                    foreach (var str in strValues)
                    {
                        ICell cell = headerrow.CreateCell(h);
                        cell.SetCellValue(str.Key);
                        cell.CellStyle = style;
                        h++;
                    }
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int h = 0;

                        foreach (DataColumn dc in dt.Columns)
                        {
                            ICell cell = headerrow.CreateCell(h);
                            cell.SetCellValue(dc.ColumnName);
                            cell.CellStyle = style;
                            h++;
                        }
                    }
                }
                #endregion

                #region //表内容
                if (strValues != null)
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int r = 1;
                        foreach (DataRow dr in dt.Rows)
                        {
                            IRow sheetRow = sheet1.CreateRow(r);
                            int k = 0;
                            foreach (var str in strValues)
                            {
                                ICell _cell0 = sheetRow.CreateCell(k);
                                _cell0.SetCellValue(Convert.ToString(dr[str.Value]));
                                _cell0.CellStyle = style; //单元格样式

                                sheetRow.Height = 15 * 20;
                                k++;
                            }
                            r++;
                        }
                    }
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int r = 1;
                        foreach (DataRow dr in dt.Rows)
                        {
                            IRow sheetRow = sheet1.CreateRow(r);
                            int k = 0;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                ICell cell = sheetRow.CreateCell(k);
                                cell.SetCellValue(Convert.ToString(dr[dc]));
                                cell.CellStyle = style; //单元格样式
                                k++;
                            }
                            r++;
                        }
                    }
                }
                #endregion

                #endregion

                //return workbook;

                #region //写入到客户端
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                return ms;
                //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", strExcelFileName));
                //System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                //workbook = null;
                //ms.Close();
                //ms.Dispose();
                #endregion
            }

            /// <summary>
            /// 导入标准的Excel文件到DataTable
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public static DataTable ImportExcel(string filePath)
            {
                IWorkbook workbook = null;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    switch (filePath.Substring(0, filePath.LastIndexOf(".")))
                    {
                        case ".xlsx":
                            workbook = new XSSFWorkbook(file);
                            break;
                        default:
                            workbook = new HSSFWorkbook(file);
                            break;
                    }


                    ISheet sheet = workbook.GetSheetAt(0);


                    DataTable _dt = new DataTable();


                    IRow ir = sheet.GetRow(0);
                    int columns = ir.LastCellNum;//（某行）有多少列
                    int rows = sheet.LastRowNum;//多少行

                    //表结构
                    for (int i = 0; i < columns; i++)
                    {
                        DataColumn dc = new DataColumn(ir.GetCell(i).ToString());
                        _dt.Columns.Add(dc);
                    }

                    //数据
                    for (int i = 1; i <= rows; i++)
                    {
                        DataRow dr = _dt.NewRow();

                        IRow _ir = sheet.GetRow(i);

                        for (int j = _ir.FirstCellNum; j < columns; j++)
                        {
                            if (_ir.GetCell(j) != null)
                                dr[j] = _ir.GetCell(j);
                        }
                        _dt.Rows.Add(dr);
                    }
                    return _dt;
                }
            }
        }
    }
}
