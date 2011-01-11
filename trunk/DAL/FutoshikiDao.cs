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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DAL
{
    /// <summary>
    /// This class is a futoshiki data access object.
    /// </summary>
    public class FutoshikiDao : DbAccessor, IDao
    {
        public int Create(object o)
        {
            Futoshiki f = o as Futoshiki; 
            string[] sqls = new string[f.Length+1];

            string[] fields = new[] {Const.Id, Const.Uid, Const.Scale, 
                Const.Status};
            string[] values = new[] { "'" + f.Id + "'", "'" + f.Uid + "'", 
                f.Scale.ToString(), f.Status.ToString() };
            sqls[0] = BuildSql(Const.CrtGrid, fields, values);

            fields = new[] {Const.Gid, Const.Row, Const.Col, Const.Val, 
                    Const.IsWritable };            
            for (int i = 0; i < sqls.Length-1; i++)
            {
                values = new[] { "'" + f.Id + "'", f[i].Row.ToString(), 
                    f[i].Col.ToString(), "'" + f[i].Val + "'", 
                    f[i].IsWritable ? 1.ToString() : 0.ToString()};
                sqls[i + 1] = BuildSql(Const.CrtCell, fields, values);
            }
            return ExecuteBatchUpdate(sqls);

        }

        public object Read(string uId, string scale)
        {
            Futoshiki f = null;
            string[] fields = new[] {Const.Uid, Const.Scale};
            string[] values = new[] {"'" + uId + "'", scale};
            
            // read ongoing game first
            string sql = BuildSql(Const.Read1, fields, values);
            SqlDataReader sdr = BeginQuery(sql);

            // read new game if cannot find ongoing game.
            if (!sdr.HasRows)
            {
                EndQuery();
                sql = BuildSql(Const.Read0, fields, values);
                sdr = BeginQuery(sql);
            }
         
            for (int i = 0; sdr.Read(); i++)
            {
                if (null == f)
                {
                    Byte s = sdr.GetByte(2);
                    f = new Futoshiki(s);
                    f.Status = sdr.GetByte(3);
                    f.Id = sdr.GetGuid(1).ToString();
                    f.Uid = sdr.GetGuid(0).ToString();
                }
                f[i].Row = sdr.GetInt32(4);
                f[i].Col = sdr.GetInt32(5);
                f[i].Val = sdr.GetString(6);
                f[i].IsWritable = sdr.GetBoolean(7);
            }
            EndQuery();
            return f;
        }

        public int Update(object o)
        {
            Futoshiki f = o as Futoshiki;
            if (f.Status == (int) Futoshiki.Mode.Completed)
            {
                //TODO: Update dbo.puzzel_Grids.Status
                throw new NotImplementedException();
            }
            List<string> list = new List<string>();
            string[] fields = new[]{Const.Val, Const.Uid, Const.Scale, Const.Gid,
                Const.Row, Const.Col};

            for (int i = 0; i < f.Length; i++)
            {
                if (f[i].IsNum && f[i].IsWritable)
                {
                    string[] values = new[]{"'" + f[i].Val + "'", "'" +
                        f.Uid + "'", f.Scale.ToString(), "'" + f.Id + 
                        "'", f[i].Row.ToString(), f[i].Col.ToString()};
                    list.Add(BuildSql(Const.UpdtCells, fields, values));
                }
            }
            return ExecuteBatchUpdate(list.ToArray());
        }

        public int Delete(object o)
        {
            throw new NotImplementedException();
        }

        private string BuildSql(string root, string[] fields, string[] value)
        {
            StringBuilder builder = new StringBuilder(root,root.Length*2);
            for (int i = 0; i < fields.Length; i++)
            {
                builder.Replace(fields[i], value[i]);
            }
            return builder.ToString();
        }

    }
}
