
namespace SepaLib
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml;
    
    public class SepaCollection<TSepaObject> : Collection<TSepaObject> where TSepaObject: SepaObject
    {
        private SepaObject m_aParent;
        
        protected SepaCollection(SepaObject aParent)
        {
            this.m_aParent = aParent;
        }
        
        protected override void ClearItems()
        {
            foreach (TSepaObject local in this)
            {
                local.Parent = null;
            }
            base.ClearItems();
        }
        
        protected override void InsertItem(int nIndex, TSepaObject aObj)
        {
            if (aObj == null)
            {
                throw new ArgumentNullException();
            }
            if (aObj.Parent != null)
            {
                throw new InvalidOperationException();
            }
            aObj.Parent = this.m_aParent;
            base.InsertItem(nIndex, aObj);
        }
        
        internal bool IsValid()
        {
            foreach (TSepaObject local in this)
            {
                if (!local.IsValid())
                {
                    return false;
                }
            }
            return true;
        }
        
        protected override void RemoveItem(int nIndex)
        {
            TSepaObject local = base[nIndex];
            local.Parent = null;
            base.RemoveItem(nIndex);
        }
        
        protected override void SetItem(int nIndex, TSepaObject aObj)
        {
            if (aObj == null)
            {
                throw new ArgumentNullException();
            }
            if (aObj.Parent != null)
            {
                throw new InvalidOperationException();
            }
            TSepaObject local = base[nIndex];
            local.Parent = null;
            aObj.Parent = this.m_aParent;
            base.SetItem(nIndex, aObj);
        }
        
        internal void WriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            foreach (TSepaObject local in this)
            {
                local.WriteXml(aXmlWriter, aMessageInfo);
            }
        }
    }
}
