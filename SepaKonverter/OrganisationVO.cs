using System;

namespace SepaKonverter
{
    public class OrganisationVO
    {
        private String m_OrganisationsNr;
        private String m_Name;
        private String m_GläubigerID;
        private String m_BIC;
        private String m_IBAN;
     
        public OrganisationVO()
        {

        }

        public OrganisationVO(string _OrganisationsNr, string _Name, string _GläubigerID, string _BIC, string _IBAN)
        {
            m_OrganisationsNr = _OrganisationsNr;
            m_Name = _Name;
            m_GläubigerID = _GläubigerID;
            m_BIC = _BIC;
            m_IBAN = _IBAN;
        }

        public String OrganisationsNr
        {
            get
            {
                return m_OrganisationsNr;
            }
            set
            {
                m_OrganisationsNr = value;
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

        public String GläubigerID
        {
            get
            {
                return m_GläubigerID;
            }
            set
            {
                m_GläubigerID = value;
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
    }
}
