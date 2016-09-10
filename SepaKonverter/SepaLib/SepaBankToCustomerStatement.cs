
namespace SepaLib
{
    using System;
    
    public class SepaBankToCustomerStatement : SepaBankToCustomerMessage
    {
        public SepaBankToCustomerStatement(string sMessageTag) : base(sMessageTag, SepaMessageType.BankToCustomerStatement)
        {
        }
    }
}
