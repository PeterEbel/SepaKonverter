
namespace SepaLib
{
    using System;
    
    public sealed class SepaOriginalPaymentInformations : SepaCollection<SepaOriginalPaymentInformation>
    {
        internal SepaOriginalPaymentInformations(SepaPaymentStatusReport aPmtStsRpt) : base(aPmtStsRpt)
        {
        }
    }
}
