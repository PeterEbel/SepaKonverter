
namespace SepaLib
{
    using System;
    
    public sealed class SepaOriginalTransactionInformations : SepaCollection<SepaOriginalTransactionInformation>
    {
        internal SepaOriginalTransactionInformations(SepaOriginalPaymentInformation aOrgnlPmtInf) : base(aOrgnlPmtInf)
        {
        }
    }
}
