
namespace SepaLib
{
    using System;
    
    public sealed class SepaTransactionInformations : SepaCollection<SepaTransactionInformation>
    {
        internal SepaTransactionInformations(SepaPaymentInformation aPmtInf) : base(aPmtInf)
        {
        }
        
        internal decimal ControlSum
        {
            get
            {
                decimal num = 0M;
                foreach (SepaTransactionInformation information in this)
                {
                    num += information.Amount;
                }
                return num;
            }
        }
    }
}
