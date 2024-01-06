namespace SwitchApp
{
    partial class SwitchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonOn = new System.Windows.Forms.Button();
            this.buttonOff = new System.Windows.Forms.Button();
            this.getDataButton = new System.Windows.Forms.Button();
            this.getByNameButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.deleteDataButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SwitchApp.Properties.Resources.switchimage;
            this.pictureBox1.Location = new System.Drawing.Point(204, 82);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(475, 503);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOn
            // 
            this.buttonOn.BackColor = System.Drawing.Color.IndianRed;
            this.buttonOn.Location = new System.Drawing.Point(305, 31);
            this.buttonOn.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(276, 68);
            this.buttonOn.TabIndex = 1;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = false;
            this.buttonOn.Click += new System.EventHandler(this.ButtonOn_Click);
            // 
            // buttonOff
            // 
            this.buttonOff.BackColor = System.Drawing.SystemColors.MenuText;
            this.buttonOff.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonOff.Location = new System.Drawing.Point(305, 553);
            this.buttonOff.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(275, 68);
            this.buttonOff.TabIndex = 2;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = false;
            this.buttonOff.Click += new System.EventHandler(this.ButtonOff_Click);
            // 
            // getDataButton
            // 
            this.getDataButton.Location = new System.Drawing.Point(812, 84);
            this.getDataButton.Margin = new System.Windows.Forms.Padding(4);
            this.getDataButton.Name = "getDataButton";
            this.getDataButton.Size = new System.Drawing.Size(272, 55);
            this.getDataButton.TabIndex = 3;
            this.getDataButton.Text = "Get All Data";
            this.getDataButton.UseVisualStyleBackColor = true;
            this.getDataButton.Click += new System.EventHandler(this.GetAllDataButton_Click);
            // 
            // getByNameButton
            // 
            this.getByNameButton.Location = new System.Drawing.Point(812, 159);
            this.getByNameButton.Margin = new System.Windows.Forms.Padding(4);
            this.getByNameButton.Name = "getByNameButton";
            this.getByNameButton.Size = new System.Drawing.Size(163, 39);
            this.getByNameButton.TabIndex = 4;
            this.getByNameButton.Text = "Get Data By Name";
            this.getByNameButton.UseVisualStyleBackColor = true;
            this.getByNameButton.Click += new System.EventHandler(this.GetByNameButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(995, 167);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(297, 22);
            this.nameTextBox.TabIndex = 5;
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.ItemHeight = 16;
            this.listBoxData.Location = new System.Drawing.Point(812, 215);
            this.listBoxData.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(480, 212);
            this.listBoxData.TabIndex = 6;
            // 
            // deleteDataButton
            // 
            this.deleteDataButton.Location = new System.Drawing.Point(812, 447);
            this.deleteDataButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteDataButton.Name = "deleteDataButton";
            this.deleteDataButton.Size = new System.Drawing.Size(271, 65);
            this.deleteDataButton.TabIndex = 7;
            this.deleteDataButton.Text = "Delete Selected Data";
            this.deleteDataButton.UseVisualStyleBackColor = true;
            this.deleteDataButton.Click += new System.EventHandler(this.DeleteDataButton_Click);
            // 
            // SwitchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 665);
            this.Controls.Add(this.deleteDataButton);
            this.Controls.Add(this.listBoxData);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.getByNameButton);
            this.Controls.Add(this.getDataButton);
            this.Controls.Add(this.buttonOff);
            this.Controls.Add(this.buttonOn);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SwitchForm";
            this.Text = "Switch";
            this.Shown += new System.EventHandler(this.FormSwitch_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button buttonOn;
		private System.Windows.Forms.Button buttonOff;
		private System.Windows.Forms.Button getDataButton;
		private System.Windows.Forms.Button getByNameButton;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.ListBox listBoxData;
		private System.Windows.Forms.Button deleteDataButton;
	}
}

