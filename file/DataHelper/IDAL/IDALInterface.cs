using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace file.Interface
{
    public interface IDALInterface
    {

        /// <summary>
        /// 根据条件查询第一行第一列或者指定列第一行
        /// </summary>
       
        /// <param name="SearchFileName">要查询的列（null）</param> 
        /// <param name="SelectWhereSql">查询条件，如（a=b）（null）</param> 
        /// <returns>
        ///返回string类型
        /// </returns>
        string SearchOneRowOneLine<T>(string SearchFileName, string SelectWhereSql);

        /// <summary>
        /// 查询是否存在该记录
        /// </summary>
      
        /// <param name="ID">查询条件之一（null）</param> 
        /// <param name="FileName">查询条件的字段名（null）</param> 
        /// <param name="FileNameValue">查询条件的字段值和字段明一起用（null）</param>
        /// <param name="SelectWhereSql">查询条件，如（a=b）（null）</param> 
        /// <returns>
        ///返回bool类型，是否存在该数据
        /// </returns>
        bool Exists<T>(string ID, string FileName, string FileNameValue, string SelectWhereSql);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">数据模型（null）</param> 
        /// <returns>
        ///返回bool类型，是否添加成功
        /// </returns>
        bool Add<T>(T model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">数据模型（null）</param> 
        /// <returns>
        ///返回bool类型，是否更新成功
        /// </returns>
        bool Update<T>(T model);


        /// <summary>
        /// 以修改数据状态来删除，保持数据完整性
        /// </summary>
        /// <param name="FileName">修改的字段名（null）</param> 
        /// <param name="FileNameValue">修改的字段名的值（null）</param>
        /// <param name="UpdataWhereSql">查询条件，如（a=b）（null）</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        bool UpdateFor<T>(string FileName, string FileNameValue, string UpdataWhereSql);

        /// <summary>
        /// 以修改数据状态来删除，保持数据完整性
        /// 删除一条数据
        /// </summary>
        /// <param name="ID">查询条件（null）</param> 
        /// <param name="deleteConditionSql">修改的字段名以及该字段的值，如：（“a=b”）</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        bool Delete<T>(int ID, string deleteConditionSql);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="IDlist">修改的字段名的值的集合字符串，如："1，2，3"</param> 
        /// <param name="deleteConditionSql">修改的字段名以及该字段的值，如：（“a=b”</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        bool DeleteList<T>(string ConditionFileName, string IDlist, string deleteConditionSql);

        /// <summary>
        /// ID得到一个对象实体
        /// </summary>
        /// <param name="ID">查询条件（null）</param> 
        /// <param name="FileName">查询条件之字段名（null）</param>
        /// <param name="FileNameValue">查询条件之字段名的值，与字段名同时是否为空（null）</param>
        /// <returns>
        ///返回一个实体类
        /// </returns>
        T GetModel<T>(int ID, string FileName, string FileNameValue);

        /// <summary>
        /// 根据dataset行得到一个对象实体
        /// </summary>
        /// <param name="row">dataset行</param> 
        /// <returns>
        ///返回一个实体类
        /// </returns>
        T DataRowToModel<T>(DataRow row);


        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="defute">查询条件，如：（a = b）（null）</param> 
        /// <param name="strWhere">查询条件，如：（a = b）（null）</param> 
        /// <returns>
        ///返回一个dataset
        /// </returns> 
        DataSet GetList<T>(string defute, string strWhere);

        /// <summary>
        /// 获取记录总数(创建视图)
        /// </summary>
        /// <param name="strWhere">创建视图的查询条件，如：（a = b）（null）</param> 
        /// <param name="FileNamesList">查询条件之字段名，如：（a = b）（null）</param> 
        /// <param name="SearchStr">用户输入的查询条件之值（null）</param> 
        /// <returns>
        ///返回一个int
        /// </returns> 
        int GetRecordCount<T>(string strWhere, List<string> FileNamesList, string SearchStr);


        /// <summary>
        /// 分页获取数据列表(创建视图)
        /// </summary>
        /// <param name="strWhere">创建视图的查询条件，如：（a = b）（null）</param> 
        /// <param name="FileNamesList">查询条件之字段名，如：（a = b）（null）</param> 
        /// <param name="SearchStr">用户输入的查询条件之值（null）</param> 
        /// <param name="OrderbyStr">排序字段（null）</param> 
        /// <param name="OrderbyType">排序字段（null）</param> 
        /// <param name="startIndex">当前页起始值（null）</param> 
        /// <param name="length">一页数据长度（null）</param> 
        /// <returns>
        ///返回一个int
        /// </returns> 
        DataSet GetListByPage<T>(string strWhere, List<string> FileNamesList, string SearchStr, string OrderbyStr, string OrderbyType, int startIndex, int length);

    }
}