using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaKonverter
{
    public class Debtor
    {
        private String m_MandateID;
        private String m_DateOfSignature;
        private String m_SequenceType;
        private String m_Name;
        private String m_BIC;
        private String m_IBAN;
        private String m_InstructedAmount;
        private String m_UnstructuredRemittanceInfo;
        private String m_EndToEndReference;

        public Debtor()
        {

        }

        public Debtor(String MandateID, String DateOfSignature, String SequenceType, String Name, String BIC, String IBAN, String InstructedAmount, String UnstructuredRemittanceInfo, String EndToEndReference)
        {
            m_MandateID = MandateID;
            m_DateOfSignature = DateOfSignature;
            m_SequenceType = SequenceType;
            m_Name = Name;
            m_BIC = BIC;
            m_IBAN = IBAN;
            m_InstructedAmount = InstructedAmount;
            m_UnstructuredRemittanceInfo = UnstructuredRemittanceInfo;
            m_EndToEndReference = EndToEndReference;
        }

        public String MandateID
        {
            get
            {
                return m_MandateID;
            }
            set
            {
                m_MandateID = value;
            }
        }

        public String DateOfSignature
        {
            get
            {
                return m_DateOfSignature;
            }
            set
            {
                m_DateOfSignature = value;
            }
        }

        public String SequenceType
        {
            get
            {
                return m_SequenceType;
            }
            set
            {
                m_SequenceType = value;
            }
        }

        public String Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public String BIC
        {
            get
            {
                return m_BIC;
            }
            set
            {
                m_BIC = value;
            }
        }

        public String IBAN
        {
            get
            {
                return m_IBAN;
            }
            set
            {
                m_IBAN = value;
            }
        }

        public String InstructedAmount
        {
            get
            {
                return m_InstructedAmount;
            }
            set
            {
                m_InstructedAmount = value;
            }
        }

        public String UnstructuredRemittanceInfo
        {
            get
            {
                return m_UnstructuredRemittanceInfo;
            }
            set
            {
                m_UnstructuredRemittanceInfo = value;
            }
        }

        public String EndToEndReference
        {
            get
            {
                return m_EndToEndReference;
            }
            set
            {
                m_EndToEndReference = value;
            }
        }

    }
}
