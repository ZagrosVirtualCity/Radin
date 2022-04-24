using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Sys_Users : RadForm
    {

        private MyDB db = new MyDB();
        private DataSet ds = new DataSet();

        public Sys_Users()
        {
            InitializeComponent();
        }

        private void Sys_Users_Load(object sender, EventArgs e)
        {
            Userz();

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256 && keyData == System.Windows.Forms.Keys.Enter)
            {
                if (this.ActiveControl.ToString().Contains("Telerik.WinControls.UI.RadButton"))
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                {
                    System.Windows.Forms.SendKeys.Send("{TAB}");
                    return true;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void radCheckBox13_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (radCheckBox13.Checked)
            {
                pwd_box.Enabled = true;
            }
            else
            {
                pwd_box.Clear();
                pwd_box.Enabled = false;
            }
        }

        private void Userz()
        {
            db.SetCommand("Select * From Sys_Users");
            ds = db.GetData();
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "uname";
            comboBox1.ValueMember = "uid";
            db.exec();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int uid = Convert.ToInt16(comboBox1.SelectedValue);

                db.SetCommand("Select * From Sys_Users WHERE uid = '" + uid + "'");
                ds = db.GetData();
                uname_box.Text = Convert.ToString(ds.Tables[0].Rows[0]["uname"]);
                checkedListBox1.SetItemChecked(0, Convert.ToBoolean(ds.Tables[0].Rows[0]["paziresh"]));
                checkedListBox1.SetItemChecked(1, Convert.ToBoolean(ds.Tables[0].Rows[0]["virayesh"]));
                checkedListBox1.SetItemChecked(2, Convert.ToBoolean(ds.Tables[0].Rows[0]["savabegh"]));
                checkedListBox1.SetItemChecked(3, Convert.ToBoolean(ds.Tables[0].Rows[0]["gozareshat"]));
                checkedListBox1.SetItemChecked(4, Convert.ToBoolean(ds.Tables[0].Rows[0]["maghadir_paye"]));
                checkedListBox1.SetItemChecked(5, Convert.ToBoolean(ds.Tables[0].Rows[0]["modiriat"]));
                checkedListBox1.SetItemChecked(6, Convert.ToBoolean(ds.Tables[0].Rows[0]["active"]));
                db.exec();
            }
            catch
            {

            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            db.SetCommand("SELECT * FROM Sys_Users WHERE uname='" + uname_box.Text + "'");
            ds = db.GetData();
            db.exec();

            if (ds.Tables[0].Rows.Count == 0)
            {
                db.SetCommand("INSERT INTO Sys_Users(uname,pwd,paziresh,virayesh,savabegh,gozareshat,maghadir_paye,modiriat,active) VALUES (@uname,@pwd,@paziresh,@virayesh,@savabegh,@gozareshat,@maghadir_paye,@modiriat,@active)");
                db.SetParameter("@uname", uname_box.Text);
                db.SetParameter("@pwd", CryptorEngine.Encrypt(pwd_box.Text, true));
                db.SetParameter("@paziresh", checkedListBox1.GetItemChecked(0));
                db.SetParameter("@virayesh", checkedListBox1.GetItemChecked(1));
                db.SetParameter("@savabegh", checkedListBox1.GetItemChecked(2));
                db.SetParameter("@gozareshat", checkedListBox1.GetItemChecked(3));
                db.SetParameter("@maghadir_paye", checkedListBox1.GetItemChecked(4));
                db.SetParameter("@modiriat", checkedListBox1.GetItemChecked(5));
                db.SetParameter("@active", checkedListBox1.GetItemChecked(6));
                db.exec();
                MessageBox.Show("کاربر جدید اضافه شد");
            }
            else
            {
                MessageBox.Show("نام کاربری انتخاب شده موجود است . لطفا نام کاربری دیگری انتخاب کنید");
            }
            Userz();
            checkedListBox1.ClearSelected();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (uname_box.Text == "" || uname_box.Text == " " || uname_box.Text == string.Empty || uname_box.Text == null)
            {
                MessageBox.Show("لطفا یکی از کاربران را انتخاب کنید");
            }
            else
            {
                int uid = Convert.ToInt16(comboBox1.SelectedValue);

                if (radCheckBox13.Checked)
                {
                    db.SetCommand("update Sys_Users SET uname = @uname, pwd = @pwd, paziresh = @paziresh, virayesh = @virayesh, savabegh = @savabegh, gozareshat = @gozareshat, maghadir_paye = @maghadir_paye, modiriat = @modiriat, active = @active WHERE uid = @uid");
                    db.SetParameter("@uid", uid);
                    db.SetParameter("@uname", uname_box.Text);
                    db.SetParameter("@pwd", CryptorEngine.Encrypt(pwd_box.Text, true));
                    db.SetParameter("@paziresh", checkedListBox1.GetItemChecked(0));
                    db.SetParameter("@virayesh", checkedListBox1.GetItemChecked(1));
                    db.SetParameter("@savabegh", checkedListBox1.GetItemChecked(2));
                    db.SetParameter("@gozareshat", checkedListBox1.GetItemChecked(3));
                    db.SetParameter("@maghadir_paye", checkedListBox1.GetItemChecked(4));
                    db.SetParameter("@modiriat", checkedListBox1.GetItemChecked(5));
                    db.SetParameter("@active", checkedListBox1.GetItemChecked(6));

                    db.exec();
                    MessageBox.Show("ویرایش انجام شد");
                    Userz();
                }
                else
                {
                    db.SetCommand("update Sys_Users SET uname = @uname, paziresh = @paziresh, virayesh = @virayesh, savabegh = @savabegh, gozareshat = @gozareshat, maghadir_paye = @maghadir_paye, modiriat = @modiriat, active = @active WHERE uid = @uid");
                    db.SetParameter("@uid", uid);
                    db.SetParameter("@uname", uname_box.Text);
                    db.SetParameter("@paziresh", checkedListBox1.GetItemChecked(0));
                    db.SetParameter("@virayesh", checkedListBox1.GetItemChecked(1));
                    db.SetParameter("@savabegh", checkedListBox1.GetItemChecked(2));
                    db.SetParameter("@gozareshat", checkedListBox1.GetItemChecked(3));
                    db.SetParameter("@maghadir_paye", checkedListBox1.GetItemChecked(4));
                    db.SetParameter("@modiriat", checkedListBox1.GetItemChecked(5));
                    db.SetParameter("@active", checkedListBox1.GetItemChecked(6));
                    db.exec();
                    MessageBox.Show("ویرایش انجام شد");
                    Userz();
                }
                Userz();
            }
        }
    }
}