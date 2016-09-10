
namespace SepaLib
{
    using System;
    
    public sealed class SepaStatementEntries : SepaCollection<SepaStatementEntry>
    {
        internal SepaStatementEntries(SepaStatement aParent) : base(aParent)
        {
        }
    }
}
