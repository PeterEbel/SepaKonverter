
namespace SepaLib
{
    using System;
    
    public sealed class SepaTransactionDetailsCollection : SepaCollection<SepaTransactionDetails>
    {
        internal SepaTransactionDetailsCollection(SepaStatementEntry aParent) : base(aParent)
        {
        }
    }
}
