namespace BallGameClient
{
    partial class ClientGUI
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
            this.idField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.ballHolderLabel = new System.Windows.Forms.Label();
            this.playersArea = new System.Windows.Forms.RichTextBox();
            this.logArea = new System.Windows.Forms.RichTextBox();
            this.passBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // idField
            // 
            this.idField.BackColor = System.Drawing.SystemColors.Info;
            this.idField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idField.Location = new System.Drawing.Point(211, 315);
            this.idField.Name = "idField";
            this.idField.Size = new System.Drawing.Size(144, 26);
            this.idField.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID of player to pass to:";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idLabel.Location = new System.Drawing.Point(30, 274);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(68, 20);
            this.idLabel.TabIndex = 2;
            this.idLabel.Text = "Your ID:";
            // 
            // ballHolderLabel
            // 
            this.ballHolderLabel.AutoSize = true;
            this.ballHolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ballHolderLabel.Location = new System.Drawing.Point(207, 274);
            this.ballHolderLabel.Name = "ballHolderLabel";
            this.ballHolderLabel.Size = new System.Drawing.Size(90, 20);
            this.ballHolderLabel.TabIndex = 3;
            this.ballHolderLabel.Text = "Ball Holder:";
            // 
            // playersArea
            // 
            this.playersArea.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.playersArea.Location = new System.Drawing.Point(34, 12);
            this.playersArea.Name = "playersArea";
            this.playersArea.ReadOnly = true;
            this.playersArea.Size = new System.Drawing.Size(150, 250);
            this.playersArea.TabIndex = 4;
            this.playersArea.Text = "";
            // 
            // logArea
            // 
            this.logArea.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.logArea.Location = new System.Drawing.Point(211, 12);
            this.logArea.Name = "logArea";
            this.logArea.ReadOnly = true;
            this.logArea.Size = new System.Drawing.Size(250, 250);
            this.logArea.TabIndex = 5;
            this.logArea.Text = "";
            // 
            // passBtn
            // 
            this.passBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passBtn.Location = new System.Drawing.Point(373, 315);
            this.passBtn.Name = "passBtn";
            this.passBtn.Size = new System.Drawing.Size(75, 26);
            this.passBtn.TabIndex = 6;
            this.passBtn.Text = "Pass Ball";
            this.passBtn.UseVisualStyleBackColor = true;
            this.passBtn.Click += new System.EventHandler(this.passBtn_Click);
            // 
            // ClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.passBtn);
            this.Controls.Add(this.logArea);
            this.Controls.Add(this.playersArea);
            this.Controls.Add(this.ballHolderLabel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idField);
            this.Name = "ClientGUI";
            this.Text = "CE303 Assignment";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientGUI_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox idField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label ballHolderLabel;
        private System.Windows.Forms.RichTextBox playersArea;
        private System.Windows.Forms.RichTextBox logArea;
        private System.Windows.Forms.Button passBtn;
    }
}

