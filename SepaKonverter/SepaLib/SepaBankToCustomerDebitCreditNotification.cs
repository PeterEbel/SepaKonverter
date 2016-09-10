
namespace SepaLib
{
    using System;
    
    public class SepaBankToCustomerDebitCreditNotification : SepaBankToCustomerMessage
    {
        public SepaBankToCustomerDebitCreditNotification(string sMessageTag) : base(sMessageTag, SepaMessageType.BankToCustomerDebitCreditNotification)
        {
        }
    }
}
