using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Meridium.Xml.Serialization {
    /// <summary>
    /// The XmlSerializerUtil provides utility methods for serializing and deserializing
    /// objects. It also caches created Serializers for faster access.
    /// </summary>
    public class XmlSerializerUtil {
        #region public static string SerializeToXml(object obj)
        /// <summary>
        /// Serializes an object to Xml
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>The serialized xml</returns>
        public static string SerializeToXml(object obj) {
            return SerializeToXml(obj, null, null);
        }
        #endregion
        #region public static string SerializeToXml(object obj, XmlSerializer xser, Encoding encoding)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="xser"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="obj"/> is null.</exception>
        public static string SerializeToXml(object obj, XmlSerializer xser, Encoding encoding) {
            if (obj == null) {
                throw new ArgumentNullException("obj");
            }
            if (xser == null) {
                xser = new XmlSerializer(obj.GetType());
            }
            if (encoding == null)
                encoding = new UnicodeEncoding(false, false);

            var memoryStream = new MemoryStream();
            using (var xmlTextWriter = new XmlTextWriter(memoryStream, encoding)) {
                xmlTextWriter.Formatting = Formatting.Indented;
                xser.Serialize(xmlTextWriter, obj);
            }
            return encoding.GetString(memoryStream.ToArray());
        }
        #endregion
        #region public static string SerializeToXml(object obj, Encoding encoding)
        /// <summary>
        /// Serializes an object to Xml
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use for the xml serialized string</param>
        /// <returns>The serialized xml</returns>
        public static string SerializeToXml(object obj, Encoding encoding) {
            return SerializeToXml(obj, null, encoding);
        }
        #endregion
        #region public static T DeserializeFromXml<T>(string xml)
        /// <summary>
        /// Deserializes an xml text to an object.
        /// </summary>
        /// <param name="xml">The xml to deserialize</param>
        /// <typeparam name="T">The type to serialize the xml to</typeparam>
        /// <returns>The deserialized object.</returns>
        public static T DeserializeFromXml<T>(string xml) where T : class {
            return DeserializeFromXml<T>(xml, null);
        }
        #endregion
        #region public static T DeserializeFromXml<T>(string xml, XmlSerializer xser)
        /// <summary>
        /// Deserializes an xml text to an object.
        /// </summary>
        /// <param name="xml">The xml to deserialize</param>
        /// <param name="xser">The <see cref="XmlSerializer"/> to use or null if the default serializer for the type should be used</param>
        /// <typeparam name="T">The type to serialize the xml to</typeparam>
        /// <returns>The deserialized object.</returns>
        public static T DeserializeFromXml<T>(string xml, XmlSerializer xser) where T : class {
            if (xser == null)
                xser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(xml)) {
                return xser.Deserialize(sr) as T;
            }
        }
        #endregion
        #region public static void SerializeToXmlFile(object obj, string path, Encoding encoding)
        /// <summary>
        /// Serializes an object to Xml and stores it in a file
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="path">The path to the file to write to. If it exists, it will be overwritten.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use</param>
        public static void SerializeToXmlFile(object obj, string path, Encoding encoding) {
            if (encoding == null) {
                encoding = new UTF8Encoding(false);
            }
            string xml = SerializeToXml(obj, encoding);
            using (var sw = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write), encoding)) {
                sw.Write(xml);
            }
        }
        #endregion
        #region public static T DeserializeFromXmlFile<T>(string path)
        /// <summary>
        ///  Deserializes a file (xml) to an object.
        /// </summary>
        /// <param name="path">The path to the file to deserialize.</param>
        /// <returns>The deserialized <typeparamref name="T"/></returns>
        public static T DeserializeFromXmlFile<T>(string path) where T : class {
            using (var sr = new StreamReader(path)) {
                string xml = sr.ReadToEnd();
                return DeserializeFromXml<T>(xml);
            }
        }
        #endregion
    }

}