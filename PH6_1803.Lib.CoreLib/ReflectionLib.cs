/**
 * 业务中反射帮助类
 * Date：2020/10/30
 * Author：Max
 * Function：
 *  1、DataTable和List互转
 *  2、Model类转插入、更新Sql语句
 * 
 * **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PH6_1803.Lib.CoreLib
{
    public class ReflectionLib
    {
        public static Tuple<string, List<T>> DataTableToList<T>(DataTable dt)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                Type t = typeof(T);
                PropertyInfo[] p = t.GetProperties();
                List<T> list = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    T obj = (T)Activator.CreateInstance(t);

                    string[] sdrFileName = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sdrFileName[i] = dt.Columns[i].ColumnName.ToLower();
                    }
                    foreach (PropertyInfo item in p)
                    {
                        if (sdrFileName.ToList().IndexOf(item.Name.ToLower()) > -1)
                        {
                            if (dr[item.Name] != System.DBNull.Value)
                            {
                                item.SetValue(obj, dr[item.Name]);
                            }
                            else
                            {
                                item.SetValue(obj, null);
                            }
                        }
                        else
                        {
                            item.SetValue(obj, null);
                        }
                    }
                    list.Add(obj);
                }
                dto.Result = list;
            }
            catch (Exception ex)
            {
                dto.ResultMsg = ex.Message;
            }
            return Tuple.Create(dto.ResultMsg, dto.Result);
        }

        public static Tuple<string, DataTable> ListToDataTable<T>(List<T> ts)
        {
            return null;
        }

        public static Tuple<string, string> ModelToInsertSql<T>(T t,string tableName,string fieldKey)
        {
            LibResultDto dto = new LibResultDto();
            string _strField = ""; 
            string _strValue = ""; 
            string _strSql;
            try
            {
                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;
                    if (_objValue != null)
                    {
                        if (pi.Name.ToLower() != fieldKey.ToLower())
                        {
                            switch (_type.Name.ToLower())
                            {
                                case "datetime":
                                    string _date = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    if (!_date.Equals("0001-01-01 00:00:00"))
                                    {
                                        _strField += pi.Name + ",";
                                        _strValue += string.Format("'{0}',", _date);
                                    }
                                    break;
                                case "guid":
                                    if (!Convert.ToString(_objValue).Equals("00000000-0000-0000-0000-000000000000"))
                                    {
                                        _strField += pi.Name + ",";
                                        _strValue += string.Format("'{0}',", _objValue);
                                    }
                                    break;
                                case "string":
                                    _strField += pi.Name + ",";
                                    _strValue += string.Format("N'{0}',", _objValue == null ? "" : _objValue.ToString().Replace("'", "''"));
                                    break;
                                default:
                                    _strField += pi.Name + ",";
                                    _strValue += string.Format("'{0}',", _objValue == null ? "" : _objValue.ToString().Replace("'", "''"));
                                    break;
                            }
                        }
                    }
                }
                _strField = _strField.TrimEnd(',');
                _strValue = _strValue.TrimEnd(',');
                _strSql = $"insert into {tableName}({_strField}) values({_strValue})";
                dto.Result = _strSql;
            }
            catch (Exception ex)
            {
                dto.ResultMsg = ex.Message;
            }

            return Tuple.Create(dto.ResultMsg, dto.Result);
        }

        public static Tuple<string, string> ModelToUpdateSql<T>(T t,string tableName="", string sKey="")
        {
            LibResultDto dto = new LibResultDto();
            string _strValue = "";
            string _strSet = "";
            string _strWhere = "";
            string _strSql;

            sKey = sKey?.ToLower() ?? "id";
            tableName = tableName ?? t.GetType().Name;
            try
            {
                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    string _strField = pi.Name;
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;

                    if (_objValue != null && !pi.Name.ToLower().Equals(sKey))
                    {
                        switch (_type.Name.ToLower())
                        {
                            case "datetime":
                                _strValue = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                if (!_strValue.Equals("0001-01-01 00:00:00"))
                                    _strSet += string.Format("{0} = '{1}',", _strField, _strValue);
                                break;
                            case "guid":
                                if (!Convert.ToString(_objValue).Equals("00000000-0000-0000-0000-000000000000"))
                                    _strValue += string.Format("'{0}',", _objValue);
                                break;
                            default:
                                _strValue = string.Format("{0}", _objValue);
                                if (_strField.ToLower() == sKey.ToLower())
                                {
                                    _strWhere = string.Format(" and {0} = '{1}'", _strField, _strValue);
                                }
                                else
                                {
                                    _strSet += string.Format("{0} = N'{1}',", _strField, _strValue);
                                }
                                break;
                        }
                    }
                    else if (_objValue != null && pi.Name.ToLower().Equals(sKey))
                    {
                        _strWhere = string.Format(" and {0} = '{1}'", _strField, _objValue);
                    }

                }
                _strSet = _strSet.TrimEnd(',');
                _strSql = $"update {tableName} set {_strSet} where 1 = 1 {_strWhere}";
                dto.Result = _strSql;
            }
            catch (Exception ex)
            {
                dto.ResultMsg = ex.Message;
            }
            return Tuple.Create(dto.ResultMsg,dto.Result);
        }
    }
}
