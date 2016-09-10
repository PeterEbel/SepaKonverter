
namespace SepaLib
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    
    [StructLayout(LayoutKind.Sequential)]
    public struct SepaBIC
    {
        public const int MINLENGTH = 8;
        public const int MAXLENGTH = 11;
        public static readonly SepaBIC NullBIC;
        private string f_sBIC;
        private char f_chTerminalCode;
        public static string Capture(string sBIC)
        {
            sBIC = SepaUtil.Trim(sBIC);
            if ((sBIC == null) || (sBIC == ""))
            {
                return null;
            }
            sBIC = sBIC.ToUpper(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder(11);
            for (int i = 0; i < sBIC.Length; i++)
            {
                char ch = sBIC[i];
                if (((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z')))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }
        
        public static bool IsValid(string sBIC)
        {
            if (sBIC == null)
            {
                return false;
            }
            if ((sBIC.Length != 8) && (sBIC.Length != 11))
            {
                return false;
            }
            if (!SepaUtil.IsAlpha(sBIC.Substring(0, 6)))
            {
                return false;
            }
            if (!SepaUtil.IsAlphaNumeric(sBIC.Substring(6)))
            {
                return false;
            }
            return true;
        }
        
        public static string GetCountryCode(string sBIC)
        {
            sBIC = SepaUtil.Trim(sBIC);
            if ((sBIC == null) || (sBIC == ""))
            {
                return null;
            }
            if (!IsValid(sBIC))
            {
                throw new ArgumentException();
            }
            return sBIC.Substring(4, 2);
        }
        
        public static SepaBIC ParseAddress(string sSwiftAddress)
        {
            if (sSwiftAddress == null)
            {
                throw new ArgumentNullException();
            }
            if (sSwiftAddress.Length != 12)
            {
                throw new ArgumentException();
            }
            char ch = sSwiftAddress[8];
            return new SepaBIC(sSwiftAddress.Substring(0, 8) + sSwiftAddress.Substring(9)) { f_chTerminalCode = ch };
        }
        
        public SepaBIC(string sBIC)
        {
            if ((sBIC == null) || (sBIC == ""))
            {
                this.f_sBIC = null;
            }
            else
            {
                if (!IsValid(sBIC))
                {
                    throw new ArgumentException();
                }
                this.f_sBIC = sBIC;
            }
            this.f_chTerminalCode = '\0';
        }
        
        public string BIC
        {
            get
            {
                return this.f_sBIC;
            }
        }
        public string BIC11
        {
            get
            {
                if (this.f_sBIC == null)
                {
                    return null;
                }
                return this.f_sBIC.PadRight(11, 'X');
            }
        }
        public string CountryCode
        {
            get
            {
                return GetCountryCode(this.f_sBIC);
            }
        }
        public char TerminalCode
        {
            get
            {
                return this.f_chTerminalCode;
            }
        }
        public bool IsNull
        {
            get
            {
                return (this.f_sBIC == null);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                SepaBIC abic = (SepaBIC) obj;
                return (this.BIC11 == abic.BIC11);
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            if (this.f_sBIC != null)
            {
                return this.BIC11.GetHashCode();
            }
            return 0;
        }
        
        public override string ToString()
        {
            if (this.f_sBIC == null)
            {
                return "";
            }
            if (this.f_chTerminalCode != '\0')
            {
                return (this.f_sBIC.Substring(0, 8) + this.f_chTerminalCode + this.f_sBIC.Substring(8));
            }
            return this.f_sBIC;
        }
/*     
        public static bool operator ==(SepaBIC lhs, SepaBIC rhs)
        {
            if (lhs == 0)
            {
                return (rhs == 0);
            }
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(SepaBIC lhs, SepaBIC rhs)
        {
            if (lhs == 0)
            {
                return (rhs != 0);
            }
            return !lhs.Equals(rhs);
        }
*/        
        static SepaBIC()
        {
            NullBIC = new SepaBIC();
        }
    }
}
