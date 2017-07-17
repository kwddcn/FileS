using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace file.DAL
{
    public class DBDAL
    {
        string _conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
        //返回true or false 是否存在该记录
        public int Exists(string sqlstr, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_conn))//连接对象类
            {
                connection.Open();
                int count = 0;

                //命令类 命令什么对象 命令搞什么 
                MySqlCommand mycommand = new MySqlCommand(sqlstr, connection);
                PrepareCommand(mycommand, connection, null, sqlstr, parameters);
                //mycommand.ExecuteScalar();
                //mycommand.ExecuteReader();
                count = Convert.ToInt32(mycommand.ExecuteScalar());

                return count;
            }
        }
        //响应行数
        public int ExecuteSql(string sqlstr, MySqlParameter[] parameters)
        {
            int count = 0;
            using (MySqlConnection connection = new MySqlConnection(_conn))
            {
                connection.Open();
                MySqlCommand mycommand = new MySqlCommand(sqlstr, connection);
                PrepareCommand(mycommand, connection, null, sqlstr, parameters);
                count = mycommand.ExecuteNonQuery();
            }
            return count;
        }
        //返回DataSet
        public DataSet Query(string sqlstr, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(_conn))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sqlstr, connection);
                PrepareCommand(cmd, connection, null, sqlstr, parameters);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
        //总记录条数
        public object GetSingle(string sqlstr)
        {
            using (MySqlConnection connection = new MySqlConnection(_conn))
            {
                connection.Open();
                MySqlCommand mycommand = new MySqlCommand(sqlstr, connection);
                return mycommand.ExecuteScalar();
            }
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

    }
}