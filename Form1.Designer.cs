namespace AverySecretProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbl_DisplayText = new Label();
            lblColumnData = new Label();
            lblRowData = new Label();
            SuspendLayout();
            // 
            // lbl_DisplayText
            // 
            lbl_DisplayText.AutoSize = true;
            lbl_DisplayText.Font = new Font("Courier New", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lbl_DisplayText.Location = new Point(357, 39);
            lbl_DisplayText.Name = "lbl_DisplayText";
            lbl_DisplayText.Size = new Size(0, 27);
            lbl_DisplayText.TabIndex = 0;
            // 
            // lblColumnData
            // 
            lblColumnData.AutoSize = true;
            lblColumnData.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblColumnData.Location = new Point(710, 23);
            lblColumnData.Name = "lblColumnData";
            lblColumnData.Size = new Size(58, 19);
            lblColumnData.TabIndex = 1;
            lblColumnData.Text = "Column\r\n";
            // 
            // lblRowData
            // 
            lblRowData.AutoSize = true;
            lblRowData.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblRowData.Location = new Point(634, 23);
            lblRowData.Name = "lblRowData";
            lblRowData.Size = new Size(37, 19);
            lblRowData.TabIndex = 2;
            lblRowData.Text = "Row\r\n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblRowData);
            Controls.Add(lblColumnData);
            Controls.Add(lbl_DisplayText);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label lbl_DisplayText;
        private Label lblColumnData;
        private Label lblRowData;
    }
}