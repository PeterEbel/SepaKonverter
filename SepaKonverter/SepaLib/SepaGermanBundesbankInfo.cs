
namespace SepaLib
{
    using System;
    
    internal class SepaGermanBundesbankInfo : IComparable
    {
        public string BankCode;
        public string BIC;
        public string NewBankCode;
        public int RuleID;
        
        public SepaGermanBundesbankInfo(string sBankCode)
        {
            this.BankCode = sBankCode;
            this.RuleID = -1;
        }
        
        public int CompareTo(object obj)
        {
            SepaGermanBundesbankInfo info = obj as SepaGermanBundesbankInfo;
            string strB = (info != null) ? info.BankCode : obj.ToString();
            return this.BankCode.CompareTo(strB);
        }
    }
}
