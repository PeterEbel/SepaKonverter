
namespace SepaLib
{
    using System;
    
    public sealed class SepaCreditTransferPaymentInitiation : SepaPaymentInitiation
    {
        public SepaCreditTransferPaymentInitiation(string sMessageTag) : base(sMessageTag, SepaMessageType.CreditTransferPaymentInitiation)
        {
        }
        
        internal override string GroupHeader_GetGrpg(SepaMessageInfo aMessageInfo)
        {
            switch (aMessageInfo.XmlNamespace)
            {
                case "urn:sepade:xsd:pain.001.001.02":
                    return "GRPD";
                
                case "urn:swift:xsd:$pain.001.002.02":
                case "APC:STUZZA:payments:ISO:pain:001:001:02:austrian:002":
                    return "MIXD";
            }
            if (aMessageInfo.Version >= 3)
            {
                return null;
            }
            return base.GetDefaultGroupingCode();
        }
        
        internal override bool GroupHeader_HasBtchBookg(SepaMessageInfo aMessageInfo)
        {
            return ((aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.001.001.02") && (aMessageInfo.Version == 2));
        }
        
        public override SepaPaymentInformation NewPaymentInformation()
        {
            return new SepaCreditTransferPaymentInformation();
        }
    }
}
