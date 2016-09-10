
namespace SepaLib
{
    using System;
    
    public enum SepaMessageType
    {
        BankToCustomerAccountReport = 0x34,
        BankToCustomerDebitCreditNotification = 0x36,
        BankToCustomerStatement = 0x35,
        CreditTransferPaymentInitiation = 1,
        DirectDebitPaymentInitiation = 8,
        Null = 0,
        PaymentStatusReport = 2
    }
}
