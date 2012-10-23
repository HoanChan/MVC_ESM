using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Mvc_ESM.Static_Helper
{
    public class XML
    {
        public static void OBJ2XML(object obj, string path_to_xml)
        {
            //serialize and persist it to it's file
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            if (File.Exists(path_to_xml))
            {
                File.Delete(path_to_xml);
            }
            FileStream fs = File.Open(path_to_xml, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            ser.Serialize(fs, obj);
            fs.Flush();
            fs.Close();
            fs.Dispose();
            ser = null;
        }

        public static object XML2OBJ(string path_to_xml, Type ObjectType)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(ObjectType);
                using (TextReader textReader = new StreamReader(path_to_xml))
                {
                    return deserializer.Deserialize(textReader);
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

    }
}