namespace GcatCSWF
{
    partial class fmGcat
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.lblStart1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlStart = new System.Windows.Forms.Panel();
            this.explanationTextBox = new System.Windows.Forms.TextBox();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.chkBPhone = new System.Windows.Forms.CheckBox();
            this.tbPhone1 = new System.Windows.Forms.TextBox();
            this.tbPhone2 = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.cbSystem = new System.Windows.Forms.ComboBox();
            this.lblInput11 = new System.Windows.Forms.Label();
            this.lblInput13 = new System.Windows.Forms.Label();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.lblInput12 = new System.Windows.Forms.Label();
            this.lblInput5 = new System.Windows.Forms.Label();
            this.lblInputNaisen = new System.Windows.Forms.Label();
            this.tbAssign = new System.Windows.Forms.TextBox();
            this.lblInput4 = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblInput3 = new System.Windows.Forms.Label();
            this.lblInput1 = new System.Windows.Forms.Label();
            this.cbRank = new System.Windows.Forms.ComboBox();
            this.tbFamilyName = new System.Windows.Forms.TextBox();
            this.lblInput2 = new System.Windows.Forms.Label();
            this.tbUnit3 = new System.Windows.Forms.TextBox();
            this.lblInput10 = new System.Windows.Forms.Label();
            this.tbUnit2 = new System.Windows.Forms.TextBox();
            this.lblInput9 = new System.Windows.Forms.Label();
            this.tbUnit1 = new System.Windows.Forms.TextBox();
            this.lblInput8 = new System.Windows.Forms.Label();
            this.cbStation = new System.Windows.Forms.ComboBox();
            this.lblInput7 = new System.Windows.Forms.Label();
            this.lblInput6 = new System.Windows.Forms.Label();
            this.cbArmy = new System.Windows.Forms.ComboBox();
            this.pnlStart.SuspendLayout();
            this.pnlResult.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStart1
            // 
            this.lblStart1.AutoSize = true;
            this.lblStart1.Location = new System.Drawing.Point(38, 26);
            this.lblStart1.Name = "lblStart1";
            this.lblStart1.Size = new System.Drawing.Size(0, 12);
            this.lblStart1.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.AutoSize = true;
            this.btnStart.Location = new System.Drawing.Point(12, 386);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(91, 23);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "調査を開始する";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlStart
            // 
            this.pnlStart.Controls.Add(this.explanationTextBox);
            this.pnlStart.Controls.Add(this.lblStart1);
            this.pnlStart.Location = new System.Drawing.Point(3, 3);
            this.pnlStart.Name = "pnlStart";
            this.pnlStart.Size = new System.Drawing.Size(479, 58);
            this.pnlStart.TabIndex = 4;
            // 
            // explanationTextBox
            // 
            this.explanationTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.explanationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.explanationTextBox.Location = new System.Drawing.Point(3, 3);
            this.explanationTextBox.Multiline = true;
            this.explanationTextBox.Name = "explanationTextBox";
            this.explanationTextBox.ReadOnly = true;
            this.explanationTextBox.Size = new System.Drawing.Size(473, 53);
            this.explanationTextBox.TabIndex = 101;
            this.explanationTextBox.TabStop = false;
            this.explanationTextBox.Text = "このツールの説明";
            // 
            // pnlResult
            // 
            this.pnlResult.Controls.Add(this.tbResult);
            this.pnlResult.Controls.Add(this.btnEnd);
            this.pnlResult.Location = new System.Drawing.Point(3, 63);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Size = new System.Drawing.Size(479, 421);
            this.pnlResult.TabIndex = 5;
            this.pnlResult.Visible = false;
            // 
            // tbResult
            // 
            this.tbResult.BackColor = System.Drawing.SystemColors.Control;
            this.tbResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbResult.Location = new System.Drawing.Point(29, 26);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(425, 344);
            this.tbResult.TabIndex = 102;
            this.tbResult.TabStop = false;
            this.tbResult.Text = "調査をしています。しばらくお待ちください...";
            // 
            // btnEnd
            // 
            this.btnEnd.AutoSize = true;
            this.btnEnd.Location = new System.Drawing.Point(194, 384);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(91, 23);
            this.btnEnd.TabIndex = 103;
            this.btnEnd.Text = "閉じる";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Visible = false;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.button1);
            this.pnlInput.Controls.Add(this.chkBPhone);
            this.pnlInput.Controls.Add(this.tbPhone1);
            this.pnlInput.Controls.Add(this.tbPhone2);
            this.pnlInput.Controls.Add(this.btnExit);
            this.pnlInput.Controls.Add(this.cbSystem);
            this.pnlInput.Controls.Add(this.btnStart);
            this.pnlInput.Controls.Add(this.lblInput11);
            this.pnlInput.Controls.Add(this.lblInput13);
            this.pnlInput.Controls.Add(this.tbLocation);
            this.pnlInput.Controls.Add(this.lblInput12);
            this.pnlInput.Controls.Add(this.lblInput5);
            this.pnlInput.Controls.Add(this.lblInputNaisen);
            this.pnlInput.Controls.Add(this.tbAssign);
            this.pnlInput.Controls.Add(this.lblInput4);
            this.pnlInput.Controls.Add(this.tbFirstName);
            this.pnlInput.Controls.Add(this.lblInput3);
            this.pnlInput.Controls.Add(this.lblInput1);
            this.pnlInput.Controls.Add(this.cbRank);
            this.pnlInput.Controls.Add(this.tbFamilyName);
            this.pnlInput.Controls.Add(this.lblInput2);
            this.pnlInput.Controls.Add(this.tbUnit3);
            this.pnlInput.Controls.Add(this.lblInput10);
            this.pnlInput.Controls.Add(this.tbUnit2);
            this.pnlInput.Controls.Add(this.lblInput9);
            this.pnlInput.Controls.Add(this.tbUnit1);
            this.pnlInput.Controls.Add(this.lblInput8);
            this.pnlInput.Controls.Add(this.cbStation);
            this.pnlInput.Controls.Add(this.lblInput7);
            this.pnlInput.Controls.Add(this.lblInput6);
            this.pnlInput.Controls.Add(this.cbArmy);
            this.pnlInput.Location = new System.Drawing.Point(3, 63);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(479, 421);
            this.pnlInput.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(291, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 114;
            this.button1.Text = "入力内容保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkBPhone
            // 
            this.chkBPhone.AutoSize = true;
            this.chkBPhone.Location = new System.Drawing.Point(212, 67);
            this.chkBPhone.Name = "chkBPhone";
            this.chkBPhone.Size = new System.Drawing.Size(91, 16);
            this.chkBPhone.TabIndex = 7;
            this.chkBPhone.Text = "電話番号なし";
            this.chkBPhone.UseVisualStyleBackColor = true;
            this.chkBPhone.CheckedChanged += new System.EventHandler(this.chkBPhone_CheckedChanged);
            // 
            // tbPhone1
            // 
            this.tbPhone1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbPhone1.Location = new System.Drawing.Point(57, 65);
            this.tbPhone1.MaxLength = 3;
            this.tbPhone1.Name = "tbPhone1";
            this.tbPhone1.Size = new System.Drawing.Size(45, 20);
            this.tbPhone1.TabIndex = 5;
            // 
            // tbPhone2
            // 
            this.tbPhone2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbPhone2.Location = new System.Drawing.Point(134, 65);
            this.tbPhone2.MaxLength = 5;
            this.tbPhone2.Name = "tbPhone2";
            this.tbPhone2.Size = new System.Drawing.Size(45, 20);
            this.tbPhone2.TabIndex = 6;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Location = new System.Drawing.Point(386, 386);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(84, 23);
            this.btnExit.TabIndex = 16;
            this.btnExit.Text = "今回はやめる";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbSystem
            // 
            this.cbSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSystem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbSystem.FormattingEnabled = true;
            this.cbSystem.IntegralHeight = false;
            this.cbSystem.Location = new System.Drawing.Point(9, 312);
            this.cbSystem.MaxDropDownItems = 20;
            this.cbSystem.Name = "cbSystem";
            this.cbSystem.Size = new System.Drawing.Size(460, 20);
            this.cbSystem.TabIndex = 13;
            // 
            // lblInput11
            // 
            this.lblInput11.AutoSize = true;
            this.lblInput11.Location = new System.Drawing.Point(12, 297);
            this.lblInput11.Name = "lblInput11";
            this.lblInput11.Size = new System.Drawing.Size(43, 12);
            this.lblInput11.TabIndex = 112;
            this.lblInput11.Text = "システム";
            // 
            // lblInput13
            // 
            this.lblInput13.AutoSize = true;
            this.lblInput13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInput13.Location = new System.Drawing.Point(347, 358);
            this.lblInput13.Name = "lblInput13";
            this.lblInput13.Size = new System.Drawing.Size(116, 12);
            this.lblInput13.TabIndex = 23;
            this.lblInput13.Text = "※ （例）タワマン最上階";
            // 
            // tbLocation
            // 
            this.tbLocation.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbLocation.Location = new System.Drawing.Point(9, 354);
            this.tbLocation.MaxLength = 30;
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(332, 20);
            this.tbLocation.TabIndex = 14;
            // 
            // lblInput12
            // 
            this.lblInput12.AutoSize = true;
            this.lblInput12.Location = new System.Drawing.Point(12, 339);
            this.lblInput12.Name = "lblInput12";
            this.lblInput12.Size = new System.Drawing.Size(53, 12);
            this.lblInput12.TabIndex = 113;
            this.lblInput12.Text = "設置場所";
            // 
            // lblInput5
            // 
            this.lblInput5.AutoSize = true;
            this.lblInput5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInput5.Location = new System.Drawing.Point(18, 69);
            this.lblInput5.Name = "lblInput5";
            this.lblInput5.Size = new System.Drawing.Size(117, 12);
            this.lblInput5.TabIndex = 20;
            this.lblInput5.Text = "０１　－　　　　　　　　－";
            // 
            // lblInputNaisen
            // 
            this.lblInputNaisen.AutoSize = true;
            this.lblInputNaisen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInputNaisen.Location = new System.Drawing.Point(12, 50);
            this.lblInputNaisen.Name = "lblInputNaisen";
            this.lblInputNaisen.Size = new System.Drawing.Size(53, 12);
            this.lblInputNaisen.TabIndex = 106;
            this.lblInputNaisen.Text = "電話番号";
            // 
            // tbAssign
            // 
            this.tbAssign.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbAssign.Location = new System.Drawing.Point(285, 24);
            this.tbAssign.MaxLength = 20;
            this.tbAssign.Name = "tbAssign";
            this.tbAssign.Size = new System.Drawing.Size(182, 20);
            this.tbAssign.TabIndex = 4;
            // 
            // lblInput4
            // 
            this.lblInput4.AutoSize = true;
            this.lblInput4.Location = new System.Drawing.Point(288, 9);
            this.lblInput4.Name = "lblInput4";
            this.lblInput4.Size = new System.Drawing.Size(29, 12);
            this.lblInput4.TabIndex = 105;
            this.lblInput4.Text = "職名";
            // 
            // tbFirstName
            // 
            this.tbFirstName.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbFirstName.Location = new System.Drawing.Point(196, 24);
            this.tbFirstName.MaxLength = 10;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(83, 20);
            this.tbFirstName.TabIndex = 3;
            // 
            // lblInput3
            // 
            this.lblInput3.AutoSize = true;
            this.lblInput3.Location = new System.Drawing.Point(199, 9);
            this.lblInput3.Name = "lblInput3";
            this.lblInput3.Size = new System.Drawing.Size(17, 12);
            this.lblInput3.TabIndex = 104;
            this.lblInput3.Text = "名";
            // 
            // lblInput1
            // 
            this.lblInput1.AutoSize = true;
            this.lblInput1.Location = new System.Drawing.Point(12, 9);
            this.lblInput1.Name = "lblInput1";
            this.lblInput1.Size = new System.Drawing.Size(29, 12);
            this.lblInput1.TabIndex = 102;
            this.lblInput1.Text = "地位";
            // 
            // cbRank
            // 
            this.cbRank.BackColor = System.Drawing.SystemColors.Window;
            this.cbRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRank.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbRank.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbRank.FormattingEnabled = true;
            this.cbRank.IntegralHeight = false;
            this.cbRank.Location = new System.Drawing.Point(9, 24);
            this.cbRank.MaxDropDownItems = 20;
            this.cbRank.Name = "cbRank";
            this.cbRank.Size = new System.Drawing.Size(84, 20);
            this.cbRank.TabIndex = 1;
            // 
            // tbFamilyName
            // 
            this.tbFamilyName.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbFamilyName.Location = new System.Drawing.Point(99, 24);
            this.tbFamilyName.MaxLength = 10;
            this.tbFamilyName.Name = "tbFamilyName";
            this.tbFamilyName.Size = new System.Drawing.Size(91, 20);
            this.tbFamilyName.TabIndex = 2;
            // 
            // lblInput2
            // 
            this.lblInput2.AutoSize = true;
            this.lblInput2.Location = new System.Drawing.Point(102, 9);
            this.lblInput2.Name = "lblInput2";
            this.lblInput2.Size = new System.Drawing.Size(17, 12);
            this.lblInput2.TabIndex = 103;
            this.lblInput2.Text = "姓";
            // 
            // tbUnit3
            // 
            this.tbUnit3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbUnit3.Location = new System.Drawing.Point(9, 270);
            this.tbUnit3.MaxLength = 40;
            this.tbUnit3.Name = "tbUnit3";
            this.tbUnit3.Size = new System.Drawing.Size(460, 20);
            this.tbUnit3.TabIndex = 12;
            // 
            // lblInput10
            // 
            this.lblInput10.AutoSize = true;
            this.lblInput10.Location = new System.Drawing.Point(12, 255);
            this.lblInput10.Name = "lblInput10";
            this.lblInput10.Size = new System.Drawing.Size(25, 12);
            this.lblInput10.TabIndex = 111;
            this.lblInput10.Text = "３次";
            // 
            // tbUnit2
            // 
            this.tbUnit2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbUnit2.Location = new System.Drawing.Point(9, 229);
            this.tbUnit2.MaxLength = 40;
            this.tbUnit2.Name = "tbUnit2";
            this.tbUnit2.Size = new System.Drawing.Size(460, 20);
            this.tbUnit2.TabIndex = 11;
            // 
            // lblInput9
            // 
            this.lblInput9.AutoSize = true;
            this.lblInput9.Location = new System.Drawing.Point(12, 214);
            this.lblInput9.Name = "lblInput9";
            this.lblInput9.Size = new System.Drawing.Size(25, 12);
            this.lblInput9.TabIndex = 110;
            this.lblInput9.Text = "２次";
            // 
            // tbUnit1
            // 
            this.tbUnit1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbUnit1.Location = new System.Drawing.Point(9, 188);
            this.tbUnit1.MaxLength = 40;
            this.tbUnit1.Name = "tbUnit1";
            this.tbUnit1.Size = new System.Drawing.Size(460, 20);
            this.tbUnit1.TabIndex = 10;
            // 
            // lblInput8
            // 
            this.lblInput8.AutoSize = true;
            this.lblInput8.Location = new System.Drawing.Point(12, 173);
            this.lblInput8.Name = "lblInput8";
            this.lblInput8.Size = new System.Drawing.Size(25, 12);
            this.lblInput8.TabIndex = 109;
            this.lblInput8.Text = "１次";
            // 
            // cbStation
            // 
            this.cbStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbStation.FormattingEnabled = true;
            this.cbStation.IntegralHeight = false;
            this.cbStation.Location = new System.Drawing.Point(9, 147);
            this.cbStation.MaxDropDownItems = 20;
            this.cbStation.Name = "cbStation";
            this.cbStation.Size = new System.Drawing.Size(460, 20);
            this.cbStation.TabIndex = 9;
            // 
            // lblInput7
            // 
            this.lblInput7.AutoSize = true;
            this.lblInput7.Location = new System.Drawing.Point(12, 132);
            this.lblInput7.Name = "lblInput7";
            this.lblInput7.Size = new System.Drawing.Size(29, 12);
            this.lblInput7.TabIndex = 108;
            this.lblInput7.Text = "都市";
            // 
            // lblInput6
            // 
            this.lblInput6.AutoSize = true;
            this.lblInput6.Location = new System.Drawing.Point(12, 91);
            this.lblInput6.Name = "lblInput6";
            this.lblInput6.Size = new System.Drawing.Size(29, 12);
            this.lblInput6.TabIndex = 107;
            this.lblInput6.Text = "地方";
            // 
            // cbArmy
            // 
            this.cbArmy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArmy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbArmy.FormattingEnabled = true;
            this.cbArmy.IntegralHeight = false;
            this.cbArmy.Location = new System.Drawing.Point(9, 106);
            this.cbArmy.MaxDropDownItems = 20;
            this.cbArmy.Name = "cbArmy";
            this.cbArmy.Size = new System.Drawing.Size(460, 20);
            this.cbArmy.TabIndex = 8;
            // 
            // fmGcat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 491);
            this.ControlBox = false;
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.pnlResult);
            this.Controls.Add(this.pnlStart);
            this.MaximumSize = new System.Drawing.Size(500, 530);
            this.MinimumSize = new System.Drawing.Size(500, 530);
            this.Name = "fmGcat";
            this.Text = "GCAT ver.4 PC調査ツール";
            this.pnlStart.ResumeLayout(false);
            this.pnlStart.PerformLayout();
            this.pnlResult.ResumeLayout(false);
            this.pnlResult.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblStart1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlStart;
        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox explanationTextBox;
        private System.Windows.Forms.ComboBox cbArmy;
        private System.Windows.Forms.Label lblInput6;
        private System.Windows.Forms.Label lblInput1;
        private System.Windows.Forms.ComboBox cbRank;
        private System.Windows.Forms.TextBox tbFamilyName;
        private System.Windows.Forms.Label lblInput2;
        private System.Windows.Forms.TextBox tbUnit3;
        private System.Windows.Forms.Label lblInput10;
        private System.Windows.Forms.TextBox tbUnit2;
        private System.Windows.Forms.Label lblInput9;
        private System.Windows.Forms.TextBox tbUnit1;
        private System.Windows.Forms.Label lblInput8;
        private System.Windows.Forms.ComboBox cbStation;
        private System.Windows.Forms.Label lblInput7;
        private System.Windows.Forms.TextBox tbPhone1;
        private System.Windows.Forms.Label lblInputNaisen;
        private System.Windows.Forms.TextBox tbAssign;
        private System.Windows.Forms.Label lblInput4;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.Label lblInput3;
        private System.Windows.Forms.Label lblInput5;
        private System.Windows.Forms.ComboBox cbSystem;
        private System.Windows.Forms.Label lblInput11;
        private System.Windows.Forms.Label lblInput13;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Label lblInput12;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox tbPhone2;
        private System.Windows.Forms.CheckBox chkBPhone;
        private System.Windows.Forms.Button button1;
    }
}

