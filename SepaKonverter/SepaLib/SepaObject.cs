
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public abstract class SepaObject
    {
        private SepaObject m_aParent;
        private readonly string m_sTagName;
        
        public SepaObject(string sTagName)
        {
            if (!SepaUtil.CheckTagName(sTagName))
            {
                throw new ArgumentException();
            }
            this.m_sTagName = sTagName;
        }
        
        public abstract void Clear();
        public abstract bool IsValid();
        protected virtual void OnAfterXmlRead(SepaMessageInfo aMessageInfo)
        {
        }
        
        protected abstract void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo);
        protected abstract void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo);
        public void ReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if ((aXmlReader == null) || (aMessageInfo == null))
            {
                throw new ArgumentNullException();
            }
            this.Clear();
            bool isEmptyElement = aXmlReader.IsEmptyElement;
            aXmlReader.ReadStartElement(this.TagName);
            if (!isEmptyElement)
            {
                this.OnReadXml(aXmlReader, aMessageInfo);
                aXmlReader.ReadEndElement();
            }
            this.OnAfterXmlRead(aMessageInfo);
        }
        
        public void WriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if ((aXmlWriter == null) || (aMessageInfo == null))
            {
                throw new ArgumentNullException();
            }
            if (!this.IsValid())
            {
                throw new InvalidOperationException();
            }
            aXmlWriter.WriteStartElement(this.TagName);
            this.OnWriteXml(aXmlWriter, aMessageInfo);
            aXmlWriter.WriteEndElement();
        }
        
        internal SepaObject Parent
        {
            get
            {
                return this.m_aParent;
            }
            set
            {
                this.m_aParent = value;
            }
        }
        
        public string TagName
        {
            get
            {
                return this.m_sTagName;
            }
        }
    }
}
