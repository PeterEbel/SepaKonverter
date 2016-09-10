
namespace SepaLib
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    
    [StructLayout(LayoutKind.Sequential)]
    public struct SepaIBAN
    {
        public const int MINLENGTH = 6;
        public const int MAXLENGTH = 0x22;
        public static readonly SepaIBAN NullIBAN;
        private string f_sIBAN;
        public static string Capture(string sPaperIBAN)
        {
            sPaperIBAN = SepaUtil.Trim(sPaperIBAN);
            if ((sPaperIBAN == null) || (sPaperIBAN == ""))
            {
                return null;
            }
            sPaperIBAN = sPaperIBAN.ToUpper(CultureInfo.InvariantCulture);
            if (sPaperIBAN.StartsWith("IBAN"))
            {
                sPaperIBAN = sPaperIBAN.Substring(4);
            }
            StringBuilder builder = new StringBuilder(0x22);
            for (int i = 0; i < sPaperIBAN.Length; i++)
            {
                char ch = sPaperIBAN[i];
                if (((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }
        
        public static bool IsValid(string sIBAN)
        {
            if (sIBAN == null)
            {
                return false;
            }
            return (((sIBAN.Length >= 6) && (sIBAN.Length <= 0x22)) && SepaUtil.IsValidID(sIBAN));
        }
        
        public static string GetCountryCode(string sIBAN)
        {
            sIBAN = SepaUtil.Trim(sIBAN);
            if ((sIBAN == null) || (sIBAN == ""))
            {
                return null;
            }
            if ((sIBAN.Length < 6) || (sIBAN.Length > 0x22))
            {
                throw new ArgumentException();
            }
            return sIBAN.Substring(0, 2);
        }
        
        public static string GetGermanBankCode(string sIBAN)
        {
            return SepaGermanIBAN.GetBankCode(sIBAN);
        }
        
        public static string GetGermanAcctNo(string sIBAN)
        {
            return SepaGermanIBAN.GetAcctNo(sIBAN);
        }
        
        public SepaIBAN(string sIBAN)
        {
            if ((sIBAN == null) || (sIBAN == ""))
            {
                this.f_sIBAN = null;
            }
            else
            {
                if (!IsValid(sIBAN))
                {
                    throw new ArgumentException();
                }
                this.f_sIBAN = sIBAN;
            }
        }
        
        public SepaIBAN(string sCountryCode, string sBankCode, string sAcctNo)
        {
            if ((sCountryCode == null) || (sAcctNo == null))
            {
                throw new ArgumentNullException();
            }
            if (sBankCode == null)
            {
                sBankCode = "";
            }
            if ((((sCountryCode.Length != 2) || !SepaUtil.IsAlpha(sCountryCode)) || (!SepaUtil.IsAlphaNumeric(sBankCode) || (sAcctNo.Length == 0))) || (!SepaUtil.IsAlphaNumeric(sAcctNo) || ((sBankCode.Length + sAcctNo.Length) > 30)))
            {
                throw new ArgumentException();
            }
            if (sCountryCode == "DE")
            {
                if ((!SepaUtil.IsNumeric(sBankCode) || (sBankCode.Length != 8)) || (!SepaUtil.IsNumeric(sAcctNo) || (sAcctNo.Length > 10)))
                {
                    throw new ArgumentException();
                }
                sAcctNo = sAcctNo.PadLeft(10, '0');
            }
            this.f_sIBAN = SepaUtil.BuildID(sCountryCode, sBankCode + sAcctNo);
        }
        
        public string IBAN
        {
            get
            {
                return this.f_sIBAN;
            }
        }
        public string CountryCode
        {
            get
            {
                return GetCountryCode(this.f_sIBAN);
            }
        }
        public bool IsNull
        {
            get
            {
                return (this.f_sIBAN == null);
            }
        }
        public string Format()
        {
            if (this.f_sIBAN == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder(0x2a);
            for (int i = 0; i < this.f_sIBAN.Length; i += 4)
            {
                int count = Math.Min(4, this.f_sIBAN.Length - i);
                builder.Append(this.f_sIBAN, i, count);
                builder.Append(' ');
            }
            return builder.ToString(0, builder.Length - 1);
        }
        
        public override bool Equals(object obj)
        {
            return ((obj != null) && (this.f_sIBAN == ((SepaIBAN) obj).f_sIBAN));
        }
        
        public override int GetHashCode()
        {
            if (this.f_sIBAN != null)
            {
                return this.f_sIBAN.GetHashCode();
            }
            return 0;
        }
        
        public override string ToString()
        {
            if (this.f_sIBAN == null)
            {
                return "";
            }
            return this.f_sIBAN;
        }
 /*       
        public static bool operator ==(SepaIBAN lhs, SepaIBAN rhs)
        {
            if (lhs == 0)
            {
                return (rhs == 0);
            }
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(SepaIBAN lhs, SepaIBAN rhs)
        {
            if (lhs == 0)
            {
                return (rhs != 0);
            }
            return !lhs.Equals(rhs);
        }
*/        
        static SepaIBAN()
        {
            NullIBAN = new SepaIBAN();
        }
    }
}
