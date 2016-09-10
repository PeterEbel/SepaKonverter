
namespace SepaKonverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using SepaLib;
    
    public class SepaDocument
    {
        private SepaMessage m_aMessage;
        private SepaMessageInfo m_aMessageInfo;
        private Encoding m_aXmlEncoding;
        private const string XMLSCHEMAINSTANCENAMESPACE = "http://www.w3.org/2001/XMLSchema-instance";
        
        public SepaDocument(SepaMessageInfo aMessageInfo)
        {

        }
        
        public SepaDocument(SepaMessageInfo aMessageInfo, SepaMessage aMessage)
        {
            this.m_aXmlEncoding = new UTF8Encoding(false);
            if (aMessageInfo == null)
            {
                throw new ArgumentNullException();
            }
            this.MessageInfo = aMessageInfo;
            this.Message = aMessage;
        }
        
        private static string[] _Split(string s)
        {
            List<string> list = new List<string>();
            s = s.Trim() + " ";
            int startIndex = 0;
            bool flag = true;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                bool flag2 = (((ch == ' ') || (ch == '\t')) || (ch == '\r')) || (ch == '\n');
                if (flag)
                {
                    if (flag2)
                    {
                        list.Add(s.Substring(startIndex, i - startIndex));
                        flag = false;
                    }
                }
                else if (!flag2)
                {
                    startIndex = i;
                    flag = true;
                }
            }
            return list.ToArray();
        }
        
        public void WriteDocument(Stream aStream)
        {
            if (aStream == null)
            {
                throw new ArgumentNullException();
            }
            XmlWriterSettings settings = new XmlWriterSettings {
                Encoding = this.m_aXmlEncoding
            };
            XmlWriter aXmlWriter = XmlWriter.Create(aStream, settings);
            this.WriteDocumentXml(aXmlWriter);
        }
        
        public void WriteDocument(string sFileName)
        {
            if (sFileName == null)
            {
                throw new ArgumentNullException();
            }
            if (sFileName == string.Empty)
            {
                throw new ArgumentException();
            }
            using (FileStream stream = File.Create(sFileName))
            {
                this.WriteDocument(stream);
                stream.Close();
            }
        }
        
        public void WriteDocumentXml(XmlWriter aXmlWriter)
        {
            aXmlWriter.WriteStartDocument();
            this.WriteXml(aXmlWriter);
            aXmlWriter.WriteEndDocument();
            aXmlWriter.Flush();
        }
        
        public void WriteXml(XmlWriter aXmlWriter)
        {
            if (this.m_aMessage == null)
            {
                throw new InvalidOperationException();
            }
            string xmlNamespace = this.MessageInfo.XmlNamespace;
            string xmlSchemaLocation = this.MessageInfo.XmlSchemaLocation;
            aXmlWriter.WriteStartElement("Document", xmlNamespace);
            if (xmlNamespace != null)
            {
                aXmlWriter.WriteAttributeString("xmlns", xmlNamespace);
                aXmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                if (xmlSchemaLocation != null)
                {
                    aXmlWriter.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", xmlNamespace + " " + xmlSchemaLocation);
                }
            }
            this.m_aMessage.WriteXml(aXmlWriter, this.m_aMessageInfo);
            aXmlWriter.WriteEndElement();
        }
        
        public SepaMessage Message
        {
            get
            {
                return this.m_aMessage;
            }
            set
            {
                if ((value != null) && (value.MessageType != this.MessageType))
                {
                    throw new ArgumentException();
                }
                this.m_aMessage = value;
            }
        }
        
        public SepaMessageInfo MessageInfo
        {
            get
            {
                return this.m_aMessageInfo;
            }
            set
            {
                m_aMessageInfo = value;
            }
        }
        
        public SepaMessageType MessageType
        {
            get
            {
                return this.m_aMessageInfo.MessageType;
            }
        }
        
        public Encoding XmlEncoding
        {
            get
            {
                return this.m_aXmlEncoding;
            }
            set
            {
                this.m_aXmlEncoding = value;
            }
        }
    }
}
