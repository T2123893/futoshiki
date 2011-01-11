/*
 * $Id$
 *
 * Coursework – Futoshiki.Models.Tools
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */


using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Models.Tools
{
    /// <summary>
    /// This is a tool class. It provides serialize and deserialize operation 
    /// to XML. 
    /// </summary>
    public class XmlHelper
    {
        private XmlHelper() {}

        /// <summary>
        /// To indicate a directory that named user.
        /// </summary>
        public const string U = "user";

        /// <summary>
        /// To indicate a directory that nameed solution.
        /// </summary>
        public const string S = "solution";

        /// <summary>
        /// To indicate a directory that named temp.
        /// </summary>
        public const string T = "temp";

        /// <summary>
        /// Serialize a object to xml string 
        /// </summary>
        /// <typeparam name="T">a type (Class)</typeparam>
        /// <param name="obj">a instance</param>
        /// <returns>string - xml string</returns>
        public static string Serialize<T>(T obj)
        {            
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, obj);
            byte[] bt = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(bt);
            
        }

        /// <summary>
        /// Serialize a object to xml file.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="obj">an object that going to serialize</param>
        /// <param name="f">xml file name without suffix</param>
        /// <param name="dir">create dir after path, one or many</param>
        /// <returns></returns>
        public static bool Serialize<T>(T obj, string f, params string[] dir)
        {
            return Serialize(obj, f, null, dir);
        }

        /// <summary>
        /// Serialize a object to xml file at specified path. If path does not 
        /// give, first it will check <b>XmlStore</b> in web.config file. 
        /// If there is no XmlStore config then use 
        /// <code>AppDomain.CurrentDomain.BaseDirectory</code> to get a path.
        /// </summary>
        /// <typeparam name="T">a type/class</typeparam>
        /// <param name="obj">an object that going to serialize</param>
        /// <param name="fileName">xml file name without suffix</param>
        /// <param name="path">specified a store path</param>
        /// <param name="dir">directories after path, one or many.</param>
        /// <returns>bool</returns>
        public static bool Serialize<T>(T obj, string fileName, string path, 
            params string[] dir)
        {
            bool isOk = true;
            StreamWriter sw = null;
            try
            {
                string appPath = GetPath(path, fileName, dir);
                sw = new StreamWriter(appPath);
                XmlSerializer xs = new XmlSerializer((obj.GetType()));
                xs.Serialize(sw, obj);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Models.Tools.XmlHelper - " + e);
                isOk = false;
            }
            finally
            {
                if (sw != null) { sw.Close(); }
            }
            return isOk;
        }

        /// <summary>
        /// To deserialize a xml string to an object.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="xmlStr">a string in xml format</param>
        /// <returns>a specified type of object</returns>
        public static T Deserialize<T>(string xmlStr)
        {
            T obj = Activator.CreateInstance<T>();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            Debug.WriteLine("Models.Tools.XmlHelper - " + xmlStr);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr));
            obj = (T) xs.Deserialize(ms);
            ms.Close();
            return obj;
        }

        /// <summary>
        /// To deserialize a xml file to an specified object.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="f">the file name</param>
        /// <param name="dir">directories after path</param>
        /// <returns>a specified type of object</returns>
        public static T Deserialize<T>(string f, params string[] dir)
        {
            return Deserialize<T>(f, null, dir);
        }

        /// <summary>
        /// To deserialize a xml file to an specified object.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="f">the file name without suffix</param>
        /// <param name="path">specified store path</param>
        /// <param name="dir">directories after path</param>
        /// <returns>a specified type of object</returns>
        public static T Deserialize<T>(string f, string path, params string[] dir)
        {
            string appPath = GetPath(path, f, dir);
            T obj = Activator.CreateInstance<T>();
            XmlSerializer xs  = new XmlSerializer(obj.GetType());
            StreamReader sr = new StreamReader(appPath);
            obj = (T) xs.Deserialize(sr);
            sr.Close();
            return obj;
        }

        /// <summary>
        /// To delete a xml at config path or default path.
        /// </summary>
        /// <param name="f">the file name</param>
        /// <param name="dir">directories after path</param>
        public static void Del(string f, params  string[] dir)
        {
            Del(f, null, dir);
        }

        /// <summary>
        /// To delete a xml from specified path.
        /// </summary>
        /// <param name="f">the file name</param>
        /// <param name="path">specified store path</param>
        /// <param name="dir">directories after path</param>
        public static void Del(string f, string path, params string[] dir)
        {
            string appPath = GetPath(path, f, dir);
            File.Delete(appPath);            
        }

        private static string GetPath(string path, string f, params string[] dir)
        {
            if (string.IsNullOrEmpty(path))
            {
                // Get path from web.config
                path = ConfigurationManager.AppSettings["XmlStore"];
                if (string.IsNullOrEmpty(path))
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + "\\XML\\";
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (string d in dir)
                {
                    path += "\\" + d + "\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }                    
                }
                path += f;
            }
            Debug.WriteLine("Models.Tools.XmlHelper - " + path);
            return path;
        }

    }
}
