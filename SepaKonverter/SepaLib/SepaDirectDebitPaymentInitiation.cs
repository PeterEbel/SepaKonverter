
namespace SepaLib
{
    using System;
    
    public sealed class SepaDirectDebitPaymentInitiation : SepaPaymentInitiation
    {
        public SepaDirectDebitPaymentInitiation(string sMessageTag) : base(sMessageTag, SepaMessageType.DirectDebitPaymentInitiation)
        {
        }
        
        internal override string GroupHeader_GetGrpg(SepaMessageInfo aMessageInfo)
        {
            switch (aMessageInfo.XmlNamespace)
            {
                case "urn:sepade:xsd:pain.008.001.01":
                    return "GRPD";
                
                case "urn:swift:xsd:$pain.008.002.01":
                case "APC:STUZZA:payments:ISO:pain:008:001:01:austrian:002":
                    return "MIXD";
            }
            if (aMessageInfo.Version >= 2)
            {
                return null;
            }
            return base.GetDefaultGroupingCode();
        }
        
        internal override bool GroupHeader_HasBtchBookg(SepaMessageInfo aMessageInfo)
        {
            return ((aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.008.001.01") && (aMessageInfo.Version == 1));
        }
        
        public override SepaPaymentInformation NewPaymentInformation()
        {
            return new SepaDirectDebitPaymentInformation();
        }
    }
}
