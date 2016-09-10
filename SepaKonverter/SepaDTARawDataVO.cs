using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaKonverter
{
    public class SepaDTARawDataVO
    {

        #region RawData Members

	    private String m_sBankleitzahl;
	    private String m_sKontonummer;
	    private String m_sInterneKundennummer;
	    private String m_sGegenkontoBankleitzahl;
	    private String m_sGegenkontoNr;
	    private String m_sBetragInEuro;
	    private String m_sValutadatum;
	    private String m_sKontoinhaber;
	    private String m_sGegenkontoInhaber;
	    private String m_sVerwendungszweck;

        #endregion

        public SepaDTARawDataVO()
        {

        }
        
        #region Properties

        public string Bankleitzahl
        {
            get
            {
                return m_sBankleitzahl;
            }
            set
            {
                m_sBankleitzahl = value;
            }
        }

        public string Kontonummer
        {
            get
            {
                return m_sKontonummer;
            }
            set
            {
                m_sKontonummer = value;
            }
        }

        public string InterneKundennummer
        {
            get
            {
                return m_sInterneKundennummer;
            }
            set
            {
                m_sInterneKundennummer = value;
            }
        }

        public string GegenkontoBankleitzahl
        {
            get
            {
                return m_sGegenkontoBankleitzahl;
            }
            set
            {
                m_sGegenkontoBankleitzahl = value;
            }
        }

        public string GegenkontoKontonummer
        {
            get
            {
                return m_sGegenkontoNr;
            }
            set
            {
                m_sGegenkontoNr = value;
            }
        }

        public string BetragInEuro
        {
            get
            {
                return m_sBetragInEuro;
            }
            set
            {
                m_sBetragInEuro = value;
            }
        }

        public string Valutadatum
        {
            get
            {
                return m_sValutadatum;
            }
            set
            {
                m_sValutadatum = value;
            }
        }

        public string Kontoinhaber
        {
            get
            {
                return m_sKontoinhaber;
            }
            set
            {
                m_sKontoinhaber = value;
            }
        }

        public string GegenkontoInhaber
        {
            get
            {
                return m_sGegenkontoInhaber;
            }
            set
            {
                m_sGegenkontoInhaber = value;
            }
        }

        public string Verwendungszweck
        {
            get
            {
                return m_sVerwendungszweck;
            }
            set
            {
                m_sVerwendungszweck = value;
            }
        }

        #endregion

    }
}
