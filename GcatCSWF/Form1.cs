using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GcatCSWF
{
     

    public partial class fmGcat : Form
    {
        private string aesKey;


        public fmGcat() {
            InitializeComponent();

            //暗号化に使用するKeyはこちらを使用（ASCII文字限定・変更可）
            aesKey = "ssmt";

            //コンボボックスの選択肢の編集はここ
            this.cbRank.DataSource = new string[]
                {"－要選択－",
                 "地位"};
            this.cbArmy.DataSource = new string[]
                {"－－選択してください。－－",
                 "地方"};
            this.cbStation.DataSource = new string[]
                {"－－選択してください。－－",
                 "都市"};
            this.cbSystem.DataSource = new string[]
                {"－－選択してください。－－","" +
                "システム"};
            
            if (File.Exists(Directory.GetParent(Application.ExecutablePath) + @"\GCAT.conf"))
            {
                var confJson = new JObject();
                using(var reader = new StreamReader(Directory.GetParent(Application.ExecutablePath) + @"\GCAT.conf"))
                {
                    confJson = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
                    this.cbRank.Text =　confJson["Rank"].ToString();
                    this.tbFamilyName.Text = confJson["FamilyName"].ToString();
                    this.tbFirstName.Text = confJson["Firstname"].ToString();
                    this.tbAssign.Text = confJson["Assign"].ToString();
                    this.tbPhone1.Text = confJson["PhoneNumber1"].ToString();
                    this.tbPhone2.Text = confJson["PhoneNumber2"].ToString();
                    this.cbArmy.Text = confJson["Army"].ToString();
                    this.cbStation.Text = confJson["Station"].ToString();
                    this.tbUnit1.Text = confJson["Unit1"].ToString();
                    this.tbUnit2.Text = confJson["Unit2"].ToString();
                    this.tbUnit3.Text = confJson["Unit3"].ToString();
                    this.cbSystem.Text = confJson["SystemName"].ToString();
                    this.tbLocation.Text = confJson["Location"].ToString();

                    reader.Close();
                }

            }


        }



        private void btnStart_Click(object sender, EventArgs e) {

            //フォーム入力のバリデーション判定
            bool inputWordsValid = true;
            Color invalidBoxColor = Color.FromArgb(255,192,192);
            
            if (this.cbRank.Text == "－要選択－") {
                inputWordsValid = false;
                this.cbRank.BackColor = invalidBoxColor;
            } else {
                this.cbRank.BackColor = SystemColors.Window;
            }
            if (this.tbFamilyName.Text == "") {
                inputWordsValid = false;
                this.tbFamilyName.BackColor = invalidBoxColor;
            } else {
                this.tbFamilyName.BackColor = SystemColors.Window;
            }
            if (this.tbFirstName.Text == "") {
                inputWordsValid = false;
                this.tbFirstName.BackColor = invalidBoxColor;
            } else {
                this.tbFirstName.BackColor = SystemColors.Window;
            }
            if (this.tbAssign.Text == "") {
                inputWordsValid = false;
                this.tbAssign.BackColor = invalidBoxColor;
            } else {
                this.tbAssign.BackColor = SystemColors.Window;
            }
            if (!this.chkBPhone.Checked &&
                !((1 <= this.tbPhone1.Text.Length && this.tbPhone1.Text.Length <= 3) &&
                  (tbPhone1.Text.Length + this.tbPhone2.Text.Length == 6))) {
                inputWordsValid = false;
                this.tbPhone1.BackColor = invalidBoxColor;
                this.tbPhone2.BackColor = invalidBoxColor;
            } else {
                this.tbPhone1.BackColor = SystemColors.Window;
                this.tbPhone2.BackColor = SystemColors.Window;
            }
            if (this.cbArmy.Text == "－－選択してください。－－") {
                inputWordsValid = false;
                this.cbArmy.BackColor = invalidBoxColor;
            } else {
                this.cbArmy.BackColor = SystemColors.Window;
            }
            if (this.cbStation.Text == "－－選択してください。－－") {
                inputWordsValid = false;
                this.cbStation.BackColor = invalidBoxColor;
            } else {
                this.cbStation.BackColor = SystemColors.Window;
            }
            if (this.tbUnit1.Text == "") {
                inputWordsValid = false;
                this.tbUnit1.BackColor = invalidBoxColor;
            } else {
                this.tbUnit1.BackColor = SystemColors.Window;
            }
            if (this.cbSystem.Text == "－－選択してください。－－") {
                inputWordsValid = false;
                this.cbSystem.BackColor = invalidBoxColor;
            } else {
                this.cbSystem.BackColor = SystemColors.Window;
            }
            if (this.tbLocation.Text == "") {
                inputWordsValid = false;
                this.tbLocation.BackColor = invalidBoxColor;
            } else {
                this.tbLocation.BackColor = SystemColors.Window;
            }
            
            //バリデーションを満たしていない項目がある場合はフォーム入力に戻る。
            //補職名が「0」の時はテスト文字列代入
            if (!inputWordsValid && this.tbAssign.Text != "0") {
                return;
            }

            //フォーム入力の取得
            var resultJson = new JObject();
            resultJson.Add("GCATversion", "4.0.0");
            resultJson.Add("ExecDayTime", DateTime.Now.ToString().Replace("/", "-").Replace(" ", "T"));
            if (this.tbAssign.Text != "0")
            {
                resultJson.Add("Rank", Strings.StrConv(this.cbRank.Text, VbStrConv.Narrow));
                resultJson.Add("FullName", Strings.StrConv(this.tbFamilyName.Text + " " + this.tbFirstName.Text, VbStrConv.Narrow));
                resultJson.Add("Assign", Strings.StrConv(this.tbAssign.Text, VbStrConv.Narrow));
                resultJson.Add("PhoneNumber", this.chkBPhone.Checked ? "なし" : Strings.StrConv("8-" + this.tbPhone1.Text + "-" + this.tbPhone2.Text, VbStrConv.Narrow));
                resultJson.Add("Army", Strings.StrConv(this.cbArmy.Text, VbStrConv.Narrow));
                resultJson.Add("Station", Strings.StrConv(this.cbStation.Text, VbStrConv.Narrow));
                resultJson.Add("Unit", Strings.StrConv(this.tbUnit1.Text + "_" +
                                                            (this.tbUnit2.Text == "" ? " " : this.tbUnit2.Text) + "_" +
                                                            (this.tbUnit3.Text == "" ? " " : this.tbUnit3.Text), VbStrConv.Narrow));
                resultJson.Add("SystemName", Strings.StrConv(this.cbSystem.Text, VbStrConv.Narrow));
                resultJson.Add("Location", Strings.StrConv(this.tbLocation.Text, VbStrConv.Narrow));
            }
            else
            {
                resultJson.Add("Rank", "湾岸タワマン最上階貴族");
                resultJson.Add("FullName", "テスト 太郎");
                resultJson.Add("Assign", "テスト入力");
                resultJson.Add("PhoneNumber", "00-0000-0000");
                resultJson.Add("Army", "テスト地方");
                resultJson.Add("Station", "テスト都市");
                resultJson.Add("Unit", "テスト1_テスト2_テスト3");
                resultJson.Add("SystemName", "テストシステム");
                resultJson.Add("Location", "テスト室");
            }

            //処理中画面への移行
            this.pnlResult.Visible = true;
            this.pnlInput.Visible = false;

            //端末データの取得及びファイル出力処理
            Logic.GetNodeData(resultJson);

            //結果表示画面への移行
            this.tbResult.Lines = new string[]{"調査が完了しました。結果ファイルは下記のパスにあるので提出をよろしくお願いします。","",
                                              $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}" };
            this.btnEnd.Visible = true;
        }

        private void btnEnd_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void chkBPhone_CheckedChanged(object sender, EventArgs e) {
            if (this.chkBPhone.Checked) {
                this.tbPhone1.Enabled = false;
                this.tbPhone2.Enabled = false;
            } else {
                this.tbPhone1.Enabled = true;
                this.tbPhone2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confJson = new JObject();
            //JSONファイルに出力
//            using (var writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\GCAT.conf"))
            using (var writer = new StreamWriter(Directory.GetParent(Application.ExecutablePath) + @"\GCAT.conf"))
            {
                confJson.Add("Rank", this.cbRank.Text);
                confJson.Add("FamilyName", this.tbFamilyName.Text);
                confJson.Add("Firstname",this.tbFirstName.Text);
                confJson.Add("Assign", this.tbAssign.Text);
                confJson.Add("PhoneNumber1", this.tbPhone1.Text);
                confJson.Add("PhoneNumber2", this.tbPhone2.Text);
                confJson.Add("Army", this.cbArmy.Text);
                confJson.Add("Station", this.cbStation.Text);
                confJson.Add("Unit1", this.tbUnit1.Text);
                confJson.Add("Unit2", this.tbUnit2.Text);
                confJson.Add("Unit3", this.tbUnit3.Text);
                confJson.Add("SystemName",this.cbSystem.Text);
                confJson.Add("Location", this.tbLocation.Text);
                writer.Write(JsonConvert.SerializeObject(confJson));

                writer.Close();
            }
            MessageBox.Show("ファイル「GCAT.conf」に保存しました。");
        }
    }
}
