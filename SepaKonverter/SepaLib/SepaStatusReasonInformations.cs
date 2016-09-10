
namespace SepaLib
{
    using System;
    
    public sealed class SepaStatusReasonInformations : SepaCollection<SepaStatusReasonInformation>
    {
        internal SepaStatusReasonInformations(SepaObject aParent) : base(aParent)
        {
        }
    }
}
