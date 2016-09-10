
namespace SepaLib
{
    using System;
    
    internal class SepaGermanPseudoAcctInfo
    {
        public string BankCode;
        public string PseudoAcctNo;
        public string RealAcctNo;
        
        public SepaGermanPseudoAcctInfo(string sBankCode, string sPseudoAcctNo, string sRealAcctNo)
        {
            this.BankCode = sBankCode;
            this.PseudoAcctNo = sPseudoAcctNo.TrimStart(new char[] { '0' });
            this.RealAcctNo = sRealAcctNo.TrimStart(new char[] { '0' });
        }
    }
}
