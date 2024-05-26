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
            lblData = new Label();
            menu_option = new MenuStrip();
            optionToolStripMenuItem = new ToolStripMenuItem();
            btn_analyze_input_data = new ToolStripMenuItem();
            btn_render_image_from_data = new ToolStripMenuItem();
            btn_accuracy_test = new ToolStripMenuItem();
            btn_reset_data = new ToolStripMenuItem();
            lbl_Info = new Label();
            menu_option.SuspendLayout();
            SuspendLayout();
            // 
            // lbl_DisplayText
            // 
            lbl_DisplayText.AutoSize = true;
            lbl_DisplayText.Font = new Font("Courier New", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            lbl_DisplayText.Location = new Point(12, 9);
            lbl_DisplayText.Name = "lbl_DisplayText";
            lbl_DisplayText.Padding = new Padding(10);
            lbl_DisplayText.Size = new Size(20, 53);
            lbl_DisplayText.TabIndex = 0;
            // 
            // lblData
            // 
            lblData.AutoSize = true;
            lblData.Font = new Font("Calibri", 21.75F, FontStyle.Regular, GraphicsUnit.Point);
            lblData.Location = new Point(659, 9);
            lblData.Name = "lblData";
            lblData.Padding = new Padding(10);
            lblData.Size = new Size(20, 56);
            lblData.TabIndex = 2;
            // 
            // menu_option
            // 
            menu_option.Dock = DockStyle.Bottom;
            menu_option.Font = new Font("Calibri", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            menu_option.Items.AddRange(new ToolStripItem[] { optionToolStripMenuItem });
            menu_option.Location = new Point(0, 702);
            menu_option.Name = "menu_option";
            menu_option.Size = new Size(1020, 34);
            menu_option.TabIndex = 3;
            menu_option.Text = "menuStrip1";
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btn_analyze_input_data, btn_render_image_from_data, btn_accuracy_test, btn_reset_data });
            optionToolStripMenuItem.ForeColor = SystemColors.ControlText;
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new Size(83, 30);
            optionToolStripMenuItem.Text = "Option";
            optionToolStripMenuItem.DropDownClosed += optionToolStripMenuItem_DropDownClosed;
            optionToolStripMenuItem.DropDownOpened += optionToolStripMenuItem_DropDownOpened;
            // 
            // btn_analyze_input_data
            // 
            btn_analyze_input_data.Name = "btn_analyze_input_data";
            btn_analyze_input_data.Size = new Size(443, 30);
            btn_analyze_input_data.Text = "DEMO: Analyze Image";
            btn_analyze_input_data.Click += btn_analyze_input_data_Click;
            // 
            // btn_render_image_from_data
            // 
            btn_render_image_from_data.Name = "btn_render_image_from_data";
            btn_render_image_from_data.Size = new Size(443, 30);
            btn_render_image_from_data.Text = "DEMO: Render Image From Data";
            btn_render_image_from_data.Click += btn_render_image_from_data_Click;
            // 
            // btn_accuracy_test
            // 
            btn_accuracy_test.Name = "btn_accuracy_test";
            btn_accuracy_test.Size = new Size(443, 30);
            btn_accuracy_test.Text = "Run Accuracy Test";
            btn_accuracy_test.Click += btn_accuracy_test_Click;
            // 
            // btn_reset_data
            // 
            btn_reset_data.Name = "btn_reset_data";
            btn_reset_data.Size = new Size(443, 30);
            btn_reset_data.Text = "Reset Training Data Using Training_Images";
            btn_reset_data.Click += btn_reset_data_Click;
            // 
            // lbl_Info
            // 
            lbl_Info.AutoSize = true;
            lbl_Info.Font = new Font("Calibri", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_Info.Location = new Point(12, 537);
            lbl_Info.Name = "lbl_Info";
            lbl_Info.Padding = new Padding(10);
            lbl_Info.Size = new Size(20, 53);
            lbl_Info.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1020, 736);
            Controls.Add(lbl_Info);
            Controls.Add(lblData);
            Controls.Add(lbl_DisplayText);
            Controls.Add(menu_option);
            MainMenuStrip = menu_option;
            Name = "Form1";
            Text = "Form1";
            menu_option.ResumeLayout(false);
            menu_option.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label lbl_DisplayText;
        private Label lblData;
        private MenuStrip menu_option;
        private ToolStripMenuItem optionToolStripMenuItem;
        private ToolStripMenuItem btn_analyze_input_data;
        private ToolStripMenuItem btn_render_image_from_data;
        private ToolStripMenuItem btn_accuracy_test;
        private ToolStripMenuItem btn_reset_data;
        private Label lbl_Info;
    }
}