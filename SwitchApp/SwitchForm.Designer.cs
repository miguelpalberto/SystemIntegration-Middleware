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
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.deleteDataButton = new System.Windows.Forms.Button();
            this.getByIdButton = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SwitchApp.Properties.Resources.switchimage;
            this.pictureBox1.Location = new System.Drawing.Point(153, 67);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(356, 409);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOn
            // 
            this.buttonOn.BackColor = System.Drawing.Color.IndianRed;
            this.buttonOn.Location = new System.Drawing.Point(229, 25);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(207, 55);
            this.buttonOn.TabIndex = 1;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = false;
            this.buttonOn.Click += new System.EventHandler(this.ButtonOn_Click);
            // 
            // buttonOff
            // 
            this.buttonOff.BackColor = System.Drawing.SystemColors.MenuText;
            this.buttonOff.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonOff.Location = new System.Drawing.Point(229, 449);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(206, 55);
            this.buttonOff.TabIndex = 2;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = false;
            this.buttonOff.Click += new System.EventHandler(this.ButtonOff_Click);
            // 
            // getDataButton
            // 
            this.getDataButton.Location = new System.Drawing.Point(548, 67);
            this.getDataButton.Name = "getDataButton";
            this.getDataButton.Size = new System.Drawing.Size(137, 35);
            this.getDataButton.TabIndex = 3;
            this.getDataButton.Text = "Get All Data Sent";
            this.getDataButton.UseVisualStyleBackColor = true;
            this.getDataButton.Click += new System.EventHandler(this.GetAllDataButton_Click);
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.Location = new System.Drawing.Point(548, 156);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(137, 147);
            this.listBoxData.TabIndex = 4;
            this.listBoxData.SelectedIndexChanged += new System.EventHandler(this.ListBoxData_SelectedIndexChanged);
            // 
            // deleteDataButton
            // 
            this.deleteDataButton.Location = new System.Drawing.Point(548, 321);
            this.deleteDataButton.Name = "deleteDataButton";
            this.deleteDataButton.Size = new System.Drawing.Size(137, 33);
            this.deleteDataButton.TabIndex = 5;
            this.deleteDataButton.Text = "Delete";
            this.deleteDataButton.UseVisualStyleBackColor = true;
            this.deleteDataButton.Click += new System.EventHandler(this.DeleteDataButton_Click);
            // 
            // getByIdButton
            // 
            this.getByIdButton.Location = new System.Drawing.Point(548, 108);
            this.getByIdButton.Name = "getByIdButton";
            this.getByIdButton.Size = new System.Drawing.Size(77, 27);
            this.getByIdButton.TabIndex = 6;
            this.getByIdButton.Text = "Get By Id";
            this.getByIdButton.UseVisualStyleBackColor = true;
            this.getByIdButton.Click += new System.EventHandler(this.GetByIdButton_Click);
            // 
            // idTextBox
            // 
            this.idTextBox.Location = new System.Drawing.Point(631, 115);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(53, 20);
            this.idTextBox.TabIndex = 7;
            this.idTextBox.TextChanged += new System.EventHandler(this.IdTextBox_TextChanged);
            // 
            // SwitchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 540);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.getByIdButton);
            this.Controls.Add(this.deleteDataButton);
            this.Controls.Add(this.listBoxData);
            this.Controls.Add(this.getDataButton);
            this.Controls.Add(this.buttonOff);
            this.Controls.Add(this.buttonOn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SwitchForm";
            this.Text = "Form1";
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
		private System.Windows.Forms.ListBox listBoxData;
		private System.Windows.Forms.Button deleteDataButton;
		private System.Windows.Forms.Button getByIdButton;
		private System.Windows.Forms.TextBox idTextBox;
	}
}

