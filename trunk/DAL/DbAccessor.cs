/*
 * $Id$
 * 
 * Coursework – Futoshiki.DAL
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL
{
    /// <summary>
    /// Database Accessor. This class is going to provide methods to execute 
    /// sql. Any data access object class can extends this class.
    /// </summary>
    public class DbAccessor
    {
        private readonly SqlConnection _conn = null;

        public DbAccessor() : this(Const.ConnStr) {}

        public DbAccessor(string connStrName)
        {
            string connStr = ConfigurationManager
                .ConnectionStrings[connStrName].ConnectionString;

            _conn = new SqlConnection(connStr);
            if (null == _conn)
            {
                throw new NullReferenceException("CONNOT GET DB CONNECTION");
            }
        }

        /// <summary>
        /// To excute batch sqls.
        /// </summary>
        /// <param name="sqlStrs">A string array contains sql commands</param>
        /// <returns>int - affected rows - 1.</returns>
        public int ExecuteBatchUpdate(string[] sqlStrs)
        {
            int count = -1;
            SqlTransaction trans = null;
            try
            {
                _conn.Open();
                SqlCommand cmd = new SqlCommand();
               trans = _conn.BeginTransaction();
                cmd.Connection = _conn;
                cmd.Transaction = trans; 
                foreach (string t in sqlStrs)
                {
                    cmd.CommandText = t;                    
                    Debug.WriteLine(GetType() + " - " + t);                    
                    count += cmd.ExecuteNonQuery();                   
                }
                trans.Commit();
            }catch (Exception e)
            {
                Debug.WriteLine(GetType() + " - " + e.Message);
                try
                {
                    trans.Rollback();
                } catch(Exception ex)
                {
                    Debug.WriteLine(GetType() + " - " + e.Message);
                }
            }
            finally
            {
                _conn.Close();
            }
            return count;
        }

        /// <summary>
        /// Executes a sql command.
        /// </summary>
        /// <param name="sql">Sql command text</param>
        /// <returns>The number of rows affected</returns>
        private int ExecuteUpdate(string sql)
        {
            int count = 0;
            try
            {   _conn.Open();
                SqlCommand cmd = new SqlCommand(sql, _conn);
                Debug.WriteLine(GetType() + " - " + sql);
                count = cmd.ExecuteNonQuery(); 
            }catch(SqlException e)
            {
                Debug.WriteLine(GetType() + " - " + e.Message);
            }
            finally
            {
                _conn.Close();
            }
            return count;
        }

        /// <summary>
        /// Executes a query operation.
        /// </summary>
        /// <param name="sql">Sql command text</param>
        /// <returns>a SqlDataReader</returns>
        public SqlDataReader BeginQuery(string sql)
        {
            SqlDataReader sdr = null;            
            try
            {
                _conn.Open();
                SqlCommand cmd = new SqlCommand(sql, _conn);
                Debug.WriteLine(GetType() + " - " + sql);
                sdr = cmd.ExecuteReader();

            }catch(SqlException e)
            {                
                Debug.WriteLine(GetType() + " - " + e.Message);   
            } 
            return sdr;
        }

        /// <summary>
        /// End a query operation.
        /// </summary>
        public void EndQuery()
        {
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }

        }

    }
    
}
