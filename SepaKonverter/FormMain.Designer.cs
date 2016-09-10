using SepaKonverter.SepaPanel;

namespace SepaKonverter
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.saveDirectDebitButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.sepaDirectDebitPaymentInitiationPanel = new SepaKonverter.SepaPanel.SepaDirectDebitPaymentInitiationPanel();
            this.SuspendLayout();
            // 
            // saveDirectDebitButton
            // 
            this.saveDirectDebitButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveDirectDebitButton.Location = new System.Drawing.Point(12, 940);
            this.saveDirectDebitButton.Name = "saveDirectDebitButton";
            this.saveDirectDebitButton.Size = new System.Drawing.Size(181, 31);
            this.saveDirectDebitButton.TabIndex = 17;
            this.saveDirectDebitButton.Text = "Lastschrift speichern";
            this.saveDirectDebitButton.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.Location = new System.Drawing.Point(199, 940);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(187, 31);
            this.exitButton.TabIndex = 18;
            this.exitButton.Text = "Programm beenden";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // sepaDirectDebitPaymentInitiationPanel
            // 
            this.sepaDirectDebitPaymentInitiationPanel.AutoScroll = true;
            this.sepaDirectDebitPaymentInitiationPanel.AutoSize = true;
            this.sepaDirectDebitPaymentInitiationPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sepaDirectDebitPaymentInitiationPanel.Location = new System.Drawing.Point(12, 12);
            this.sepaDirectDebitPaymentInitiationPanel.Name = "sepaDirectDebitPaymentInitiationPanel";
            this.sepaDirectDebitPaymentInitiationPanel.Size = new System.Drawing.Size(1241, 922);
            this.sepaDirectDebitPaymentInitiationPanel.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 986);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveDirectDebitButton);
            this.Controls.Add(this.sepaDirectDebitPaymentInitiationPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sepa Konverter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SepaDirectDebitPaymentInitiationPanel sepaDirectDebitPaymentInitiationPanel;
        private System.Windows.Forms.Button saveDirectDebitButton;
        private System.Windows.Forms.Button exitButton;
    }
}

