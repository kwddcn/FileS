using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using file.Interface;
using file.DAL;
namespace file.BLL
{
    public class DBBLL: DBDAL,IDALInterface
    {
        /// <summary>
        /// 根据条件查询第一行第一列或者指定列第一行
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="SearchFileName">SearchFileName:要查询的列（null）</param> 
        /// <param name="where">where：查询条件，如（a=b）（null）</param> 
        /// <returns>
        ///返回string类型
        /// </returns>
        public string SearchOneRowOneLine<T>(string SearchFileName,string SelectWhereSql)
        {
            Type type = typeof(T);
            //获取字段名以及字段名类型
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            if (SearchFileName != null)
            {
                strSql.Append("select " + SearchFileName + " from ");
            }
            else {
                strSql.Append("select * from ");
            }
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            if (SelectWhereSql != null)
            {
                strSql.Append(" where "+ SelectWhereSql);
            }


            return GetSingle(strSql.ToString()).ToString();
        }
        /// <summary>
        /// 查询是否存在该记录
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="ID">ID:查询条件之一（null）</param> 
        /// <param name="FileName">FileName：查询条件的字段名（null）</param> 
        /// <param name="FileNameValue">FileNameValue：查询条件的字段值和字段明一起用（null）</param>
        /// <param name="other">other：查询条件，如（a=b）（null）</param> 
        /// <returns>
        ///返回bool类型，是否存在该数据
        /// </returns>
        public bool Exists<T>(string ID, string FileName,string FileNameValue,string SelectWhereSql)
        {
            Type type = typeof(T);
            //获取字段名以及字段名类型
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));

            if (ID != null)
            {
                strSql.Append(" where ");
                strSql.Append(properties[0].Name + "=@ID");
                if (FileName != null)
                {
                    strSql.Append(" and ");
                    strSql.Append(FileName);
                    strSql.Append(" = ");
                    strSql.Append("@FileNameValue ");
                }

            }
            else if (FileName != null)
            {
              strSql.Append(" where ");
              strSql.Append(FileName);
              strSql.Append(" = ");
              strSql.Append("@FileNameValue");
            }
            if ((FileName == null && ID == null)&& SelectWhereSql != null)
            {
                strSql.Append(" where ");
                strSql.Append(SelectWhereSql);
            }
            if ((FileName != null || ID != null) && SelectWhereSql != null)
            {
                strSql.Append(" and ");
                strSql.Append(SelectWhereSql);
            }
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,11),
                    new MySqlParameter("@FileNameValue", MySqlDbType.VarChar,11)
            };
            parameters[0].Value = ID;
            parameters[1].Value = FileNameValue;

            int count = 0;
            if (ID != null)
            {
                 count = Exists(strSql.ToString(), null);
            }
            else {
                 count = Exists(strSql.ToString(), parameters);
            }
            
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="model">model：数据模型（null）</param> 
        /// <returns>
        ///返回bool类型，是否添加成功
        /// </returns>
        public bool Add<T>(T model)
        {

            int count = 0;
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            strSql.Append("(");
            foreach (var AddFileName in proper)
            {
                if (count == 0)
                {
                    count++;
                    continue;
                }
                if (count > 1)
                {
                    strSql.Append(",");
                }
                strSql.Append(AddFileName.Name);
                count++;
            }
            count = 0;
            //(a,b,c)
            strSql.Append(")");
            strSql.Append(" values (");
            foreach (var AddFileName in proper)
            {
                object value = AddFileName.GetValue(model, null);
                if (count == 0)
                {
                    count++;
                    continue;
                }
                if (count > 1)
                {
                    strSql.Append(",");
                }
                
                strSql.Append("@"+ AddFileName.Name);
                
                count++;
            }
            //values('a','a','a')
            strSql.Append(")");
            List<MySqlParameter> parametersList = new List<MySqlParameter>();
            foreach (var AddFileName in proper)
            {
                object value = AddFileName.GetValue(model, null);
                parametersList.Add(new MySqlParameter("@"+AddFileName.Name, MySqlDbType.VarChar, 255));

            };
            MySqlParameter[] parameters = parametersList.ToArray();
            count = 0;
            foreach (var AddFileName in proper)
            {
                object value = AddFileName.GetValue(model, null);
                parameters[count].Value = value;
                count++;
            };
            int rows = ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="model">model：数据模型（null）</param> 
        /// <returns>
        ///返回bool类型，是否更新成功
        /// </returns>
        public bool Update<T>(T model)
        {
            int count = 0;
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            strSql.Append(" set ");
            foreach (var updateFile in proper)
            {
                if (count == 0)
                {
                    count++;
                    continue;
                }
                if (count > 1)
                {
                    strSql.Append(",");
                }
                object value = updateFile.GetValue(model, null);
                strSql.Append(updateFile.Name + "=@"+ updateFile.Name);
                
                count++;
            }
            strSql.Append(" where " + proper[0].Name + "=@" + proper[0].Name);
            List<MySqlParameter> parametersList = new List<MySqlParameter>();
            foreach (var AddFileName in proper)
            {
                object value = AddFileName.GetValue(model, null);
                parametersList.Add(new MySqlParameter("@"+AddFileName.Name, MySqlDbType.VarChar, 255));

            };
            MySqlParameter[] parameters = parametersList.ToArray();
            count = 0;
            foreach (var AddFileName in proper)
            {
                object value = AddFileName.GetValue(model, null);
                parameters[count].Value = value;
                count++;
            };

            int rows = ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 以修改数据状态来删除，保持数据完整性
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="FileName">FileName：修改的字段名（null）</param> 
        /// <param name="FileNameValue">FileNameValue：修改的字段名的值（null）</param>
        /// <param name="UpdataWhere">UpdataWhere：查询条件，如（a=b）（null）</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        public bool UpdateFor<T>(string FileName, string FileNameValue ,string UpdataWhereSql)
        {
            Type type = typeof(T);
            PropertyInfo[] parameter = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            strSql.Append(" set ");
            strSql.Append( FileName );
            strSql.Append(" = @FileNameValue");
            strSql.Append(" where ");
            strSql.Append(UpdataWhereSql);
            MySqlParameter[] parameters = {
              new MySqlParameter("@FileNameValue", MySqlDbType.VarChar,255)
            };
            parameters[0].Value = FileNameValue;
            int rows = ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 以修改数据状态来删除，保持数据完整性
        /// 删除一条数据
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="ID">ID：查询条件（null）</param> 
        /// <param name="deleteConditionSql">deleteConditionSql：修改的字段名以及该字段的值，如：（“a=b”）</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        public bool Delete<T>(int ID, string deleteConditionSql)
        {
            Type type = typeof(T);
            PropertyInfo[] parameter = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            strSql.Append(" set ");
            strSql.Append(deleteConditionSql);
            strSql.Append(" where " + parameter[0].Name + "=@ID");
            MySqlParameter[] parameters = {
              new MySqlParameter("@ID", MySqlDbType.Int32)
            };
            parameters[0].Value = ID;
            int rows = ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="IDlist">IDlist：修改的字段名的值的集合字符串，如："1，2，3"</param> 
        /// <param name="deleteConditionSql">deleteConditionSql：修改的字段名以及该字段的值，如：（“a=b”</param>
        /// <returns>
        ///返回bool类型，是否删除成功
        /// </returns>
        public bool DeleteList<T>(string ConditionFileName,string IDlist, string deleteConditionSql)
        {
            Type type = typeof(T);
            PropertyInfo[] parameter = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            strSql.Append(" set ");
            strSql.Append(deleteConditionSql);
            if (IDlist != null)
            {
                strSql.Append(" where " + ConditionFileName + " in (" + IDlist + ")");
            }
            int rows = ExecuteSql(strSql.ToString(), null);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ID得到一个对象实体
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="ID">ID:查询条件（null）</param> 
        /// <param name="FileName">FileName：查询条件之字段名（null）</param>
        /// <param name="FileNameValue">FileName：查询条件之字段名的值，与字段名同时是否为空（null）</param>
        /// <returns>
        ///返回一个实体类
        /// </returns>
        public T GetModel<T>(int ID,string FileName,string FileNameValue)
        {
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            foreach (var fileName in proper)
            {
                strSql.Append(fileName.Name);
            }
            strSql.Append("  from  ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            if (ID.ToString() != null)
            {
                strSql.Append(" where ");
                strSql.Append(proper[0].Name + "=@ID");
                if (FileName != null)
                {
                    strSql.Append(" and ");
                    strSql.Append(FileName);
                    strSql.Append(" = ");
                    strSql.Append(FileNameValue);
                }
            }
            else if (FileName != null)
            {
                strSql.Append(" where ");
                strSql.Append(FileName);
                strSql.Append(" = ");
                strSql.Append(FileNameValue);
            }
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.Int32)
            };
            parameters[0].Value = ID;
            T model = Activator.CreateInstance<T>();
            DataSet ds = Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel<T>(ds.Tables[0].Rows[0]);
            }
            else
            {
                return model;
            }
        }
        /// <summary>
        /// 根据dataset行得到一个对象实体
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="row">row:dataset行</param> 
        /// <returns>
        ///返回一个实体类
        /// </returns>
        public T DataRowToModel<T>(DataRow row)
        {
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();
            T model = Activator.CreateInstance<T>();
            if (row != null)
            {
                foreach (var FaliName in proper)
                {
                    if (row[Convert.ToString(FaliName.Name)] != null && row[Convert.ToString(FaliName.Name)].ToString() != "")
                    {
                        object value = row[Convert.ToString(FaliName.Name)];
                        FaliName.SetValue(model, value);
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="defute">defute:查询条件，如：（a = b）（null）</param> 
        /// <param name="strWhere">strWhere:查询条件，如：（a = b）（null）</param> 
        /// <returns>
        ///返回一个dataset
        /// </returns> 
        public DataSet GetList<T>(string defute, string strWhere)
        {
            Type type = typeof(T);
            
            PropertyInfo[] proper = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM  ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            if (defute != null)
            {
                strSql.Append(" where " + defute);
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }
            }
            else
            {
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
            }
            
            return Query(strSql.ToString(), null);
        }

        /// <summary>
        /// 获取记录总数(创建视图)
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="strWhere">defute:创建视图的查询条件，如：（a = b）（null）</param> 
        /// <param name="FileNamesList">FileNamesList:查询条件之字段名，如：（a = b）（null）</param> 
        /// <param name="SearchStr">search:用户输入的查询条件之值（null）</param> 
        /// <returns>
        ///返回一个int
        /// </returns> 
        public int GetRecordCount<T>(string strWhere, List<string> FileNamesList, string SearchStr)
        {
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DROP view if exists search; create view search as select* from ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            if (strWhere != null)
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ; SELECT count(1) FROM search");
            if (SearchStr != null)
            {
                if (strWhere != null)
                {
                    strSql.Append(" where ");
                
                    int count = 0;
                    foreach (var FileName in proper)
                    {
                        if (FileNamesList!=null&&FileNamesList.Count > 0)
                        {
                            foreach (var GetFileList in FileNamesList)
                            {
                                if (FileName.Name == GetFileList)
                                {
                                    strSql.Append(FileName.Name);
                                    strSql.Append(" LIKE'%" + SearchStr + "%'");
                                    if (count < FileNamesList.Count - 1)
                                    {
                                        strSql.Append(" or ");
                                    }
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            strSql.Append(FileName.Name);
                            strSql.Append(" LIKE'%" + SearchStr + "%'");
                            if (count < proper.Length - 1)
                            {
                                strSql.Append(" or ");
                            }
                            count++;
                        }
                    }
                }
            }

            object obj = GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 分页获取数据列表(创建视图)
        /// </summary>
        /// <param name="T">T：数据类型</param> 
        /// <param name="strWhere">defute:创建视图的查询条件，如：（a = b）（null）</param> 
        /// <param name="FileNamesList">FileNamesList:查询条件之字段名，如：（a = b）（null）</param> 
        /// <param name="SearchStr">search:用户输入的查询条件之值（null）</param> 
        /// <param name="OrderbyStr">OrderbyStr:排序字段（null）</param> 
        /// <param name="OrderbyType">OrderbyType:排序字段（null）</param> 
        /// <param name="startIndex">startIndex:当前页起始值（null）</param> 
        /// <param name="length">length:一页数据长度（null）</param> 
        /// <returns>
        ///返回一个int
        /// </returns> 
        public DataSet GetListByPage<T>(string strWhere,List<string> FileNamesList, string SearchStr, string OrderbyStr, string OrderbyType, int startIndex, int length)
        {
            Type type = typeof(T);
            PropertyInfo[] proper = type.GetProperties();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DROP view if exists search; create view search as select* from ");
            strSql.Append(type.Name.IndexOf("Model") < 0 ? type.Name : type.Name.Remove(type.Name.IndexOf("Model")));
            if (strWhere != null)
            {
                strSql.Append(" WHERE "+ strWhere);
            }
            strSql.Append(" ; SELECT* FROM search");
            if (SearchStr != null)
            {
                if (strWhere != null)
                {
                    strSql.Append(" where ");
                    int count = 0;
                    foreach (var FileName in proper)
                    {
                        if (FileNamesList != null && FileNamesList.Count > 0)
                        {
                            foreach (var GetFileList in FileNamesList)
                            {
                                if (FileName.Name == GetFileList)
                                {
                                    strSql.Append(FileName.Name);
                                    strSql.Append(" LIKE'%" + SearchStr + "%'");
                                    if (count < FileNamesList.Count - 1)
                                    {
                                        strSql.Append(" or ");
                                    }
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            strSql.Append(FileName.Name);
                            strSql.Append(" LIKE'%" + SearchStr + "%'");
                            if (count < proper.Length - 1)
                            {
                                strSql.Append(" or ");
                            }
                            count++;
                        }
                    }
                }
            }
            if (OrderbyStr != null)
            {
                strSql.AppendFormat("ORDER BY {0} {1}", OrderbyStr, OrderbyStr);
            }
            strSql.AppendFormat(" limit {0},{1}", startIndex, length);
            return Query(strSql.ToString(), null);
        }

        
    }
}