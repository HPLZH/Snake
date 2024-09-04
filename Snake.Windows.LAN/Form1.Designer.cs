namespace Snake.Windows.LAN
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			hostIP = new TextBox();
			serverPort = new TextBox();
			clientPort = new TextBox();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			okButton = new Button();
			label4 = new Label();
			userName = new TextBox();
			SuspendLayout();
			// 
			// hostIP
			// 
			hostIP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			hostIP.Font = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point);
			hostIP.Location = new Point(197, 65);
			hostIP.Margin = new Padding(4);
			hostIP.Name = "hostIP";
			hostIP.Size = new Size(290, 40);
			hostIP.TabIndex = 0;
			// 
			// serverPort
			// 
			serverPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			serverPort.Font = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point);
			serverPort.Location = new Point(197, 113);
			serverPort.Margin = new Padding(4);
			serverPort.Name = "serverPort";
			serverPort.Size = new Size(121, 40);
			serverPort.TabIndex = 1;
			serverPort.Text = "22000";
			serverPort.TextAlign = HorizontalAlignment.Right;
			// 
			// clientPort
			// 
			clientPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			clientPort.Font = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point);
			clientPort.Location = new Point(197, 161);
			clientPort.Margin = new Padding(4);
			clientPort.Name = "clientPort";
			clientPort.Size = new Size(121, 40);
			clientPort.TabIndex = 2;
			clientPort.Text = "22001";
			clientPort.TextAlign = HorizontalAlignment.Right;
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label1.AutoSize = true;
			label1.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(9, 65);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(180, 36);
			label1.TabIndex = 3;
			label1.Text = "服务器IP地址";
			// 
			// label2
			// 
			label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label2.AutoSize = true;
			label2.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point);
			label2.Location = new Point(34, 113);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(155, 36);
			label2.TabIndex = 4;
			label2.Text = "服务器端口";
			// 
			// label3
			// 
			label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label3.AutoSize = true;
			label3.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point);
			label3.Location = new Point(62, 161);
			label3.Margin = new Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new Size(127, 36);
			label3.TabIndex = 5;
			label3.Text = "数据端口";
			label3.Click += label3_Click;
			// 
			// okButton
			// 
			okButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			okButton.Font = new Font("微软雅黑", 16F, FontStyle.Regular, GraphicsUnit.Point);
			okButton.Location = new Point(326, 113);
			okButton.Margin = new Padding(4);
			okButton.Name = "okButton";
			okButton.Size = new Size(161, 88);
			okButton.TabIndex = 6;
			okButton.Text = "开始";
			okButton.UseVisualStyleBackColor = true;
			okButton.Click += okButton_Click;
			// 
			// label4
			// 
			label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label4.AutoSize = true;
			label4.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point);
			label4.Location = new Point(90, 16);
			label4.Margin = new Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new Size(99, 36);
			label4.TabIndex = 7;
			label4.Text = "用户名";
			// 
			// userName
			// 
			userName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			userName.Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point);
			userName.Location = new Point(197, 13);
			userName.Margin = new Padding(4);
			userName.Name = "userName";
			userName.Size = new Size(290, 44);
			userName.TabIndex = 8;
			// 
			// Form1
			// 
			AcceptButton = okButton;
			AutoScaleDimensions = new SizeF(11F, 24F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(500, 214);
			Controls.Add(userName);
			Controls.Add(label4);
			Controls.Add(okButton);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(clientPort);
			Controls.Add(serverPort);
			Controls.Add(hostIP);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(4);
			MaximizeBox = false;
			Name = "Form1";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "连接到服务器";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox hostIP;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.TextBox clientPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox userName;
    }
}

