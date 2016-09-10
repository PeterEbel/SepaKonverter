
namespace SepaLib
{
    using System;
    
    public class SepaBankToCustomerAccountReport : SepaBankToCustomerMessage
    {
        public SepaBankToCustomerAccountReport(string sMessageTag) : base(sMessageTag, SepaMessageType.BankToCustomerAccountReport)
        {
        }
    }
}
