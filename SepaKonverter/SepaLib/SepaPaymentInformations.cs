
namespace SepaLib
{
    using System;
    
    public sealed class SepaPaymentInformations : SepaCollection<SepaPaymentInformation>
    {
        internal SepaPaymentInformations(SepaPaymentInitiation aPain) : base(aPain)
        {
        }
    }
}
