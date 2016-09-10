
namespace SepaLib
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    
    [StructLayout(LayoutKind.Sequential)]
    public struct SepaCreditorID
    {
        public const int MINLENGTH = 8;
        public const int MAXLENGTH = 0x23;
        public static readonly SepaCreditorID Null;
        private string f_sCreditorID;
        public static string Capture(string sCreditorId)
        {
            sCreditorId = SepaUtil.Trim(sCreditorId);
            if ((sCreditorId == null) || (sCreditorId == ""))
            {
                return null;
            }
            sCreditorId = sCreditorId.ToUpper(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder(0x23);
            for (int i = 0; i < sCreditorId.Length; i++)
            {
                char ch = sCreditorId[i];
                if (((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }
        
        public static bool IsValid(string sCreditorID)
        {
            if (sCreditorID == null)
            {
                return false;
            }
            return (((sCreditorID.Length >= 8) && (sCreditorID.Length <= 0x23)) && SepaUtil.IsValidID(sCreditorID.Substring(0, 4) + sCreditorID.Substring(7)));
        }
        
        public static string GetCountryCode(string sCreditorID)
        {
            sCreditorID = SepaUtil.Trim(sCreditorID);
            if ((sCreditorID == null) || (sCreditorID == ""))
            {
                return null;
            }
            if ((sCreditorID.Length < 8) || (sCreditorID.Length > 0x23))
            {
                throw new ArgumentException();
            }
            return sCreditorID.Substring(0, 2);
        }
        
        public SepaCreditorID(string sCreditorID)
        {
            if ((sCreditorID == null) || (sCreditorID == ""))
            {
                this.f_sCreditorID = null;
            }
            else
            {
                if (!IsValid(sCreditorID))
                {
                    throw new ArgumentException();
                }
                this.f_sCreditorID = sCreditorID;
            }
        }
        
        public string CreditorID
        {
            get
            {
                return this.f_sCreditorID;
            }
        }
        public string CountryCode
        {
            get
            {
                return GetCountryCode(this.f_sCreditorID);
            }
        }
        public bool IsNull
        {
            get
            {
                return (this.f_sCreditorID == null);
            }
        }
        public override bool Equals(object obj)
        {
            return ((obj != null) && (this.f_sCreditorID == ((SepaCreditorID) obj).f_sCreditorID));
        }
        
        public override int GetHashCode()
        {
            if (this.f_sCreditorID != null)
            {
                return this.f_sCreditorID.GetHashCode();
            }
            return 0;
        }
        
        public override string ToString()
        {
            if (this.f_sCreditorID == null)
            {
                return "";
            }
            return this.f_sCreditorID;
        }
 /*       
        public static bool operator ==(SepaCreditorID lhs, SepaCreditorID rhs)
        {
            if (lhs == 0)
            {
                return (rhs == 0);
            }
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(SepaCreditorID lhs, SepaCreditorID rhs)
        {
            if (lhs == 0)
            {
                return (rhs != 0);
            }
            return !lhs.Equals(rhs);
        }
*/        
        static SepaCreditorID()
        {
            Null = new SepaCreditorID();
        }
    }
}
