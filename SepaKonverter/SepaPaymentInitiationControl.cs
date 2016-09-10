using System;
using System.Diagnostics;
using System.Windows.Forms;

using SepaLib;

namespace SepaKonverter.SepaPanel
{
    /// <summary>
    /// base class for SepaCreditTransferPaymentInitiationPanel and
    /// SepaDirectDebitPaymentInitiationPanel. not abstract because
    /// of Visual Studio Designer.
    /// </summary>

    public class SepaPaymentInitiationControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>

        ErrorProvider m_aErrorProvider;

        /// <summary>
        /// 
        /// </summary>

        protected SepaPaymentInitiationControl()
        {
            m_aErrorProvider = new ErrorProvider();
			m_aErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aMessageInfo"></param>
        /// <param name="aPaymentMessage"></param>
        /// <param name="aPaymentInformation"></param>
        /// <param name="aTransactionInformation"></param>

        internal virtual void Update(
            SepaMessageInfo aMessageInfo,
            SepaPaymentInitiation aPaymentMessage,
            SepaPaymentInformation aPaymentInformation,
            SepaTransactionInformation aTransactionInformation)
        {
            // not abstract because of Designer!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="x"></param>

        protected void SetError(Control control, Exception x)
        {
            string text = GetErrorText(x);
            Debug.Assert(!string.IsNullOrEmpty(text));

            SetError(control, text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>

        protected void SetError(Control control, string text)
        {
            m_aErrorProvider.SetError(control, text);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>

        protected void RemoveError(Control control)
        {
            SetError(control, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>

        protected string GetErrorText(Exception x)
        {
            return x.ToString();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SepaPaymentInitiationControl
            // 
            this.Name = "SepaPaymentInitiationControl";
            this.Size = new System.Drawing.Size(175, 156);
            this.ResumeLayout(false);

        }
    }
}
