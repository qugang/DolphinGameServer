using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Free.Dolphin.Common
{
    public class SerializerUtil
    {
        public static string Serialize(Type t, object o)
        {
            using (StringWriter sw = new StringWriter())
            {

                XmlSerializer serializer = new XmlSerializer(t);

                serializer.Serialize(sw, o);

                return sw.ToString();
            }
        }

        public static object Deserialize(Type t, string str)
        {
            using (StringReader sr = new StringReader(str))
            {

                XmlSerializer serializer = new XmlSerializer(t);

                return serializer.Deserialize(sr);
            }
        }

        public static string JavaScriptJosnSerialize(object o)
        {
            JavaScriptSerializer _jsonConverter = new JavaScriptSerializer();

            return _jsonConverter.Serialize(o);
        }


        public static T JavaScriptJosnDeserialize<T>(string o)
        {
            JavaScriptSerializer _jsonConverter = new JavaScriptSerializer();

            return (T)_jsonConverter.Deserialize(o, typeof(T));
        }

        public static byte[] BinarySerialize(object o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(ms, o);
                return ms.ToArray();
            }
        }

        public static object BinaryDeserialize(byte[] o)
        {
            using (MemoryStream ms = new MemoryStream(o))
            {
                BinaryFormatter b = new BinaryFormatter();
                return b.Deserialize(ms);
            }
        }
    }
}
