
namespace SepaLib
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    
    public class SepaUtil
    {
        private static object g_aLock = new object();
        private static long g_nLastTimeStamp = 0L;
        private static readonly byte[] g_vCharset = new byte[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0
         };
        public const int MaxIdLen = 0x23;
        public const int MaxNmLen = 70;
        public const int MaxRmtInfLen = 140;
        public const string NOTPROVIDED = "NOTPROVIDED";
        public const string UNKNOWN = "UNKNOWN";
        
        private static int _Mod97(string s)
        {
            uint num = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (num > 0x5f5e0ff)
                {
                    num = num % 0x61;
                }
                num *= 10;
                num += (uint) s[i] - '0';
            }
            return (int) (num % 0x61);
        }
        
        private static string _ToDigits(string sID)
        {
            StringBuilder builder = new StringBuilder(40);
            for (int i = 0; i < sID.Length; i++)
            {
                char ch = sID[(i + 4) % sID.Length];
                if ((ch >= '0') && (ch <= '9'))
                {
                    builder.Append(ch);
                }
                else
                {
                    int num2 = (ch - 'A') + 10;
                    builder.Append((char) ((num2 / 10) + 0x30));
                    builder.Append((char) ((num2 % 10) + 0x30));
                }
            }
            return builder.ToString();
        }
        
        public static string BuildID(string sCountryCode, string sReference)
        {
            if ((sCountryCode == null) || (sReference == null))
            {
                throw new ArgumentNullException();
            }
            if ((sCountryCode.Length != 2) || !IsAlpha(sCountryCode))
            {
                throw new ArgumentException();
            }
            StringBuilder builder = new StringBuilder(40);
            builder.Append(sCountryCode);
            builder.Append("00");
            builder.Append(sReference);
            int num = 0x62 - _Mod97(_ToDigits(builder.ToString()));
            builder[2] = (char) ((num / 10) + 0x30);
            builder[3] = (char) ((num % 10) + 0x30);
            return builder.ToString();
        }
        
        public static bool CheckCharset(char ch)
        {
            if (ch >= '\x0080')
            {
                return false;
            }
            if (g_vCharset[ch] == 0)
            {
                return false;
            }
            return true;
        }
        
        public static bool CheckCharset(string sText)
        {
            if (sText == null)
            {
                throw new ArgumentNullException();
            }
            foreach (char ch in sText)
            {
                if (!CheckCharset(ch))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool CheckTagName(string sTagName)
        {
            if (sTagName == null)
            {
                throw new ArgumentNullException();
            }
            if (sTagName == "")
            {
                throw new ArgumentException();
            }
            int length = sTagName.Length;
            for (int i = 0; i < length; i++)
            {
                char ch = sTagName[i];
                bool flag = ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || (ch == ':')) || (ch == '_');
                bool flag2 = ((flag || ((ch >= '0') && (ch <= '9'))) || (ch == '-')) || (ch == '.');
                if (i == 0)
                {
                    if (!flag)
                    {
                        return false;
                    }
                }
                else if (!flag2)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static int DecimalPlaces(decimal d)
        {
            if (d < 0M)
            {
                throw new ArgumentOutOfRangeException();
            }
            int num = 0;
            while (true)
            {
                d -= decimal.Truncate(d);
                if (d == 0M)
                {
                    return num;
                }
                d *= 10M;
                num++;
            }
        }
        
        public static string EnforceCharset(string sText, char chSubstituteChar = '.')
        {
            if (!CheckCharset(chSubstituteChar))
            {
                throw new ArgumentException();
            }
            if (sText == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(sText.Length);
            foreach (char ch in sText)
            {
                char ch2 = ch;
                if (!CheckCharset(ch))
                {
                    switch (ch)
                    {
                        case '\x00c4':
                            ch2 = 'A';
                            goto Label_00BB;
                        
                        case '\x00d6':
                            ch2 = 'O';
                            goto Label_00BB;
                        
                        case '\x00dc':
                            ch2 = 'U';
                            goto Label_00BB;
                        
                        case '\x00f6':
                            ch2 = 'o';
                            goto Label_00BB;
                        
                        case '\x00fc':
                            ch2 = 'u';
                            goto Label_00BB;
                        
                        case '\x00df':
                            ch2 = 's';
                            goto Label_00BB;
                        
                        case '\x00e4':
                            ch2 = 'a';
                            goto Label_00BB;
                    }
                    ch2 = chSubstituteChar;
                }
            Label_00BB:
                builder.Append(ch2);
            }
            return builder.ToString();
        }
        
        public static string EnforceIdentification(string sIdentification)
        {
            if ((sIdentification == null) || (sIdentification == ""))
            {
                return "NOTPROVIDED";
            }
            if (sIdentification.Length > 0x23)
            {
                sIdentification = sIdentification.Substring(0, 0x23).Trim();
            }
            sIdentification = EnforceCharset(sIdentification, '.');
            return sIdentification;
        }
        
        public static string EnforceName(string sName)
        {
            if ((sName == null) || (sName == ""))
            {
                return "UNKNOWN";
            }
            if (sName.Length > 70)
            {
                sName = sName.Substring(0, 70).Trim();
            }
            sName = EnforceCharset(sName, '.');
            return sName;
        }
        
        public static string GenerateIdentification(string sPrefix)
        {
            if ((sPrefix != null) && (sPrefix.Length >= 0x23))
            {
                throw new ArgumentException();
            }
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - time);
            long totalSeconds = (long) span.TotalSeconds;
            lock (g_aLock)
            {
                if (totalSeconds <= g_nLastTimeStamp)
                {
                    totalSeconds = g_nLastTimeStamp + 1L;
                }
                g_nLastTimeStamp = totalSeconds;
            }
            string str = sPrefix + time.AddSeconds((double) totalSeconds).ToString("yyyyMMddHHmmss");
            if (str.Length > 0x23)
            {
                str = str.Substring(str.Length - 0x23);
            }
            return str;
        }
        
        public static bool IsAlpha(char ch)
        {
            return ((ch >= 'A') && (ch <= 'Z'));
        }
        
        public static bool IsAlpha(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            foreach (char ch in s)
            {
                if (!IsAlpha(ch))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsAlphaNumeric(char ch)
        {
            return (((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'Z')));
        }
        
        public static bool IsAlphaNumeric(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            foreach (char ch in s)
            {
                if (!IsAlphaNumeric(ch))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsLatin1(char ch)
        {
            return (((ch >= ' ') && (ch <= '~')) || ((ch >= '\x00a0') && (ch <= '\x00ff')));
        }
        
        public static bool IsLatin1(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            foreach (char ch in s)
            {
                if (!IsLatin1(ch))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsNumeric(char ch)
        {
            return ((ch >= '0') && (ch <= '9'));
        }
        
        public static bool IsNumeric(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            foreach (char ch in s)
            {
                if (!IsNumeric(ch))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsValidID(string sID)
        {
            if ((sID == null) || (sID.Length < 4))
            {
                return false;
            }
            if ((!IsAlpha(sID[0]) || !IsAlpha(sID[1])) || (!IsNumeric(sID[2]) || !IsNumeric(sID[3])))
            {
                return false;
            }
            if (!IsAlphaNumeric(sID))
            {
                return false;
            }
            return (_Mod97(_ToDigits(sID)) == 1);
        }
        
        internal static SepaIBAN ReadAcctXml(XmlReader aXmlReader, string sTagName, out string sCcy)
        {
            SepaIBAN nullIBAN = SepaIBAN.NullIBAN;
            aXmlReader.ReadStartElement(sTagName);
            aXmlReader.ReadStartElement("Id");
            if (aXmlReader.IsStartElement("IBAN"))
            {
                nullIBAN = new SepaIBAN(aXmlReader.ReadElementString());
            }
            if (aXmlReader.IsStartElement("Othr"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("Tp"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("Ccy"))
            {
                sCcy = aXmlReader.ReadElementString();
            }
            else
            {
                sCcy = null;
            }
            if (aXmlReader.IsStartElement("Nm"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            return nullIBAN;
        }
        
        internal static SepaBIC ReadAgtBIC(XmlReader aXmlReader, string sTagName)
        {
            SepaBIC nullBIC = SepaBIC.NullBIC;
            aXmlReader.ReadStartElement(sTagName);
            aXmlReader.ReadStartElement("FinInstnId");
            if (aXmlReader.IsStartElement("BIC"))
            {
                nullBIC = new SepaBIC(aXmlReader.ReadElementString());
            }
            if (aXmlReader.IsStartElement("Othr"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            aXmlReader.ReadEndElement();
            return nullBIC;
        }
        
        internal static string ReadBkTxCd(XmlReader aXmlReader, out string sIssr)
        {
            string str = null;
            sIssr = null;
            bool isEmptyElement = aXmlReader.IsEmptyElement;
            aXmlReader.ReadStartElement("BkTxCd");
            if (!isEmptyElement)
            {
                if (aXmlReader.IsStartElement("Domn"))
                {
                    aXmlReader.Skip();
                }
                if (aXmlReader.IsStartElement("Prtry"))
                {
                    aXmlReader.ReadStartElement();
                    str = aXmlReader.ReadElementString("Cd");
                    if (aXmlReader.IsStartElement("Issr"))
                    {
                        sIssr = aXmlReader.ReadElementString();
                    }
                    aXmlReader.ReadEndElement();
                }
                aXmlReader.ReadEndElement();
            }
            return str;
        }
        
        internal static string ReadCdtrSchmeIdXml(XmlReader aXmlReader)
        {
            aXmlReader.ReadStartElement("CdtrSchmeId");
            string str = ReadSepaIdXml(aXmlReader);
            aXmlReader.ReadEndElement();
            return str;
        }
        
        internal static DateTime ReadDtOrDtTmXml(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("Dt"))
            {
                return ReadDtXml(aXmlReader, "Dt");
            }
            return ToLocalDateTime(aXmlReader.ReadElementString("DtTm"));
        }
        
        internal static DateTime ReadDtXml(XmlReader aXmlReader, string sTagName)
        {
            return XmlConvert.ToDateTime(aXmlReader.ReadElementString(sTagName), "yyyy-MM-dd");
        }
        
        internal static string ReadOthrId(XmlReader aXmlReader, string sIssr = null)
        {
            string str = null;
            aXmlReader.ReadStartElement("Othr");
            str = aXmlReader.ReadElementString("Id");
            if (aXmlReader.IsStartElement("SchmeNm"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("Issr"))
            {
                string str2 = aXmlReader.ReadElementString();
                if ((sIssr != null) && (sIssr != str2))
                {
                    str = null;
                }
            }
            aXmlReader.ReadEndElement();
            return str;
        }
        
        internal static string ReadSepaIdXml(XmlReader aXmlReader)
        {
            string str;
            aXmlReader.ReadStartElement("Id");
            aXmlReader.ReadStartElement("PrvtId");
            if (aXmlReader.IsStartElement("OthrId"))
            {
                aXmlReader.ReadStartElement();
                str = aXmlReader.ReadElementString("Id");
                while (aXmlReader.IsStartElement())
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            else
            {
                aXmlReader.ReadStartElement("Othr");
                str = aXmlReader.ReadElementString("Id");
                while (aXmlReader.IsStartElement())
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            aXmlReader.ReadEndElement();
            aXmlReader.ReadEndElement();
            return str;
        }
        
        public static DateTime ToLocalDateTime(string s)
        {
            DateTime minValue = DateTime.MinValue;
            if ((s == null) || !(s != ""))
            {
                return minValue;
            }
            if (s.EndsWith("OZ"))
            {
                s = s.Substring(0, s.Length - 2) + "Z";
            }
            return XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local);
        }
        
        public static string Trim(string s)
        {
            if (s != null)
            {
                s = s.Trim();
            }
            return s;
        }
        
        internal static void WriteAcctXml(XmlWriter aXmlWriter, string sTagName, SepaIBAN tIBAN, string sCcy = null)
        {
            aXmlWriter.WriteStartElement(sTagName);
            aXmlWriter.WriteStartElement("Id");
            aXmlWriter.WriteElementString("IBAN", tIBAN.IBAN);
            aXmlWriter.WriteEndElement();
            if ((sCcy != null) && (sCcy != ""))
            {
                aXmlWriter.WriteElementString("Ccy", sCcy);
            }
            aXmlWriter.WriteEndElement();
        }
        
        internal static void WriteAgtBIC(XmlWriter aXmlWriter, string sTagName, SepaBIC tBIC)
        {
            aXmlWriter.WriteStartElement(sTagName);
            aXmlWriter.WriteStartElement("FinInstnId");
            if (tBIC.IsNull)
            {
                aXmlWriter.WriteStartElement("Othr");
                aXmlWriter.WriteElementString("Id", "NOTPROVIDED");
                aXmlWriter.WriteEndElement();
            }
            else
            {
                aXmlWriter.WriteElementString("BIC", tBIC.BIC);
            }
            aXmlWriter.WriteEndElement();
            aXmlWriter.WriteEndElement();
        }
        
        internal static void WriteBkTxCd(XmlWriter aXmlWriter, string sBkTxCd, string sIssr = "ZKA")
        {
            aXmlWriter.WriteStartElement("BkTxCd");
            if (sBkTxCd != null)
            {
                aXmlWriter.WriteStartElement("Prtry");
                aXmlWriter.WriteElementString("Cd", sBkTxCd);
                if ((sIssr != null) && (sIssr != ""))
                {
                    aXmlWriter.WriteElementString("Issr", sIssr);
                }
                aXmlWriter.WriteEndElement();
            }
            aXmlWriter.WriteEndElement();
        }
        
        internal static void WriteCdtrSchmeIdXml(XmlWriter aXmlWriter, string sCdtrSchmeId, bool fOldFormat = false)
        {
            aXmlWriter.WriteStartElement("CdtrSchmeId");
            WriteSepaIdXml(aXmlWriter, sCdtrSchmeId, fOldFormat);
            aXmlWriter.WriteEndElement();
        }
        
        internal static void WriteDtXml(XmlWriter aXmlWriter, string sTagName, DateTime tDate)
        {
            aXmlWriter.WriteElementString(sTagName, XmlConvert.ToString(tDate, "yyyy-MM-dd"));
        }
        
        internal static void WriteOthrId(XmlWriter aXmlWriter, string sId, string sIssr = null)
        {
            aXmlWriter.WriteStartElement("Othr");
            aXmlWriter.WriteElementString("Id", sId);
            if (sIssr != null)
            {
                aXmlWriter.WriteElementString("Issr", sIssr);
            }
            aXmlWriter.WriteEndElement();
        }
        
        internal static void WriteSepaIdXml(XmlWriter aXmlWriter, string sSepaId, bool fOldFormat = false)
        {
            aXmlWriter.WriteStartElement("Id");
            aXmlWriter.WriteStartElement("PrvtId");
            if (fOldFormat)
            {
                aXmlWriter.WriteStartElement("OthrId");
                aXmlWriter.WriteElementString("Id", sSepaId);
                aXmlWriter.WriteElementString("IdTp", "SEPA");
                aXmlWriter.WriteEndElement();
            }
            else
            {
                aXmlWriter.WriteStartElement("Othr");
                aXmlWriter.WriteElementString("Id", sSepaId);
                aXmlWriter.WriteStartElement("SchmeNm");
                aXmlWriter.WriteElementString("Prtry", "SEPA");
                aXmlWriter.WriteEndElement();
                aXmlWriter.WriteEndElement();
            }
            aXmlWriter.WriteEndElement();
            aXmlWriter.WriteEndElement();
        }
    }
}
