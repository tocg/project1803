/**
 * 数据库访问操作接口
 * Date：2020/10/30
 * Author：Max
 * Function：
 *  1、查询sql
 *  2、执行一条和多条Sql
 *  3、执行存储过程和带输出参数
 * 
 * **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PH6_1803.Lib.CoreLib
{
    public interface IDataAccess
    {
        string strConn { get; set; }
        Tuple<string, DataSet> Query(string sql);

        Tuple<string, int> Execute(string sql);
        Tuple<string, int> Execute(List<string> sqls);

        Tuple<string, DataSet> ExecutePro(string proName, Dictionary<string, object> parms);
        Tuple<string, DataSet> ExecutePro(string proName, Dictionary<string, object> parms, string outParamName, out object outValue);

    }
}
