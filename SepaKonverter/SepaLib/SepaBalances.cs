
namespace SepaLib
{
    using System;
    
    public sealed class SepaBalances : SepaCollection<SepaBalance>
    {
        internal SepaBalances(SepaStatement aParent) : base(aParent)
        {
        }
    }
}
