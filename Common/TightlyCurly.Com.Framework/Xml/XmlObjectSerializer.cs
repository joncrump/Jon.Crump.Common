using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Lmts.Framework.Xml.Serialization
{
    public class XmlObjectSerializer<T>
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public string Serialize(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(xmlTextWriter, value);

                    // Rewind the Stream.
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    XmlDocument xmlDocument = new XmlDocument();
                    // load from stream;
                    xmlDocument.Load(memoryStream);
                    String xmlString = xmlDocument.OuterXml;
                    return xmlString;
                }
            }
        }

        public T Deserialize(string serializedValue)
        {
            if (String.IsNullOrEmpty(serializedValue))
            {
                throw new ArgumentNullException("serializedValue");
            }

            serializedValue = serializedValue.Replace("False", "false");
            serializedValue = serializedValue.Replace("True", "true");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(serializedValue))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        #endregion
    }
}
