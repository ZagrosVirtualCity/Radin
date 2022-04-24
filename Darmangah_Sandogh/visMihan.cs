using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class visMihan : RadForm
    {
        MyDB db = new MyDB();
        DataSet ds = new DataSet();
        private object PID;
        private object khUID;
        StiReport stiReport1 = new StiReport();
        string path = "Reports\\Recipt.mrt";
        DataSet dset = new DataSet();
        private object SaID;
        public visMihan()
        {
            InitializeComponent();
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

        private void Paziresh_Load(object sender, EventArgs e)
        {
            regbox.Text = PersianDateTime.Now.ToString("yyyy/MM/dd");

            db.SetCommand(@"Select * from PaymentType");
            DataTable _PaymentType = db.GetData2();
            PaymentTypeBox.DataSource = _PaymentType;
            PaymentTypeBox.ValueMember = "PaymentTypeID";
            PaymentTypeBox.DisplayMember = "PaymentType";

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (fname.Text == "" || fname.Text == " " || fname.Text == string.Empty || lname.Text == "" ||
                lname.Text == " " || lname.Text == string.Empty)
            {
                RadMessageBox.Show("نام یا نام خانوادگی نوشته نشده است");
                fname.Focus();
            }
            else
            {
                if (GlobalVariables.shftID == 1 || GlobalVariables.shftID == 2 || GlobalVariables.shftID == 3 || GlobalVariables.shftID == 4)
                {
                    db.SetCommand(@"SELECT top 1 visNum FROM dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID Where dbo.Patients.reg_date = '" +
                        PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID +
                        "' Order By visNum DESC");
                    dset = db.GetData();

                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        GlobalVariables.visNum = Convert.ToInt16(dset.Tables[0].Rows[0]["visNum"]) + 1;
                    }
                    else
                    {
                        GlobalVariables.visNum += 1;
                    }
                }

                if (GlobalVariables.shftID == 5 || GlobalVariables.shftID == 6)
                {
                    db.SetCommand(@"SELECT top 1 visNum FROM dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID
Where dbo.Patients.reg_date Between '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND '" + PersianDateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + "' AND " +
                                  "dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' Order By visNum DESC");
                    dset = db.GetData();

                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        GlobalVariables.visNum = Convert.ToInt16(dset.Tables[0].Rows[0]["visNum"]) + 1;
                    }
                    else
                    {
                        GlobalVariables.visNum += 1;
                    }
                }
                try
                {
                    var cnn = new SqlConnection(db.Cnn);
                    cnn.Open();
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UserAdd";
                    cmd.Parameters.AddWithValue(@"fname", fname.Text);
                    cmd.Parameters.AddWithValue(@"lname", lname.Text);
                    cmd.Parameters.AddWithValue(@"doctorID", GlobalVariables.pezID);
                    cmd.Parameters.AddWithValue(@"reg_date", regbox.Text);
                    cmd.Parameters.AddWithValue(@"hourz", DateTime.Now.ToString("HH:mm").ToString());
                    cmd.Parameters.AddWithValue(@"UserID", GlobalVariables.LoggedInUser);
                    if (checkBox1.Checked)
                    {
                        cmd.Parameters.AddWithValue(@"Free", "True");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(@"Free", "False");
                    }
                    cmd.Parameters.AddWithValue(@"tazrighatNum", 0);
                    cmd.Parameters.AddWithValue(@"visNum", GlobalVariables.visNum);
                    var returnParameter = cmd.Parameters.Add("@pid", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    PID = returnParameter.Value;
                    cnn.Close();
                    fname.Text = "";
                    lname.Text = "";
                    fname.Focus();

                    db.SetCommand("Select fname,lname,UserID,reg_date,hourz From patients where patientID = @patientID");
                    db.SetParameter(@"PatientID", PID);
                    ds = db.GetData();

                    var cnn2 = new SqlConnection(db.Cnn);
                    cnn2.Open();
                    SqlCommand cmd2 = cnn2.CreateCommand();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandText = "SaAdd";
                    cmd2.Parameters.AddWithValue(@"PatientID", PID);
                    cmd2.Parameters.AddWithValue(@"FullName", ds.Tables[0].Rows[0]["fname"] + " " + ds.Tables[0].Rows[0]["lname"]);
                    cmd2.Parameters.AddWithValue(@"UserID", ds.Tables[0].Rows[0]["UserID"]);
                    cmd2.Parameters.AddWithValue(@"reg_date", ds.Tables[0].Rows[0]["reg_date"]);
                    cmd2.Parameters.AddWithValue(@"hourz", ds.Tables[0].Rows[0]["hourz"]);
                    var returnParameter2 = cmd2.Parameters.Add("@SaID", SqlDbType.Int);
                    returnParameter2.Direction = ParameterDirection.ReturnValue;
                    cmd2.ExecuteNonQuery();
                    SaID = returnParameter2.Value;
                    cnn.Close();

                    db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                    db.SetParameter(@"SaID", SaID);
                    db.SetParameter(@"khID", "0");
                    db.SetParameter(@"SaOptionID", "1");
                    db.exec();

                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("خطا در ثبت بیمار-کد1");
                    RadMessageBox.Show(ex.Message);
                    /////////////////////////////
                }
                try
                {
                    db.SetCommand("Select * From khType where khID = 1031");
                    ds = db.GetData();
                    db.exec();

                    var cnn3 = new SqlConnection(db.Cnn);
                    cnn3.Open();
                    SqlCommand cmd3 = cnn3.CreateCommand();
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandText = "KhAdd";
                    cmd3.Parameters.AddWithValue(@"PatientID", PID);
                    cmd3.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                    cmd3.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                    cmd3.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                    cmd3.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                    cmd3.Parameters.AddWithValue(@"KhID", 2);
                    cmd3.Parameters.AddWithValue(@"Num", 1);
                    cmd3.Parameters.AddWithValue(@"depID", 1);
                    cmd3.Parameters.AddWithValue(@"cost", ds.Tables[0].Rows[0]["khCost"].ToString());
                    cmd3.Parameters.AddWithValue(@"gender", DBNull.Value);
                    cmd3.Parameters.AddWithValue(@"IDPaymentType", PaymentTypeBox.SelectedValue);
                    var returnParameter3 = cmd3.Parameters.Add("@khUID", SqlDbType.Int);
                    returnParameter3.Direction = ParameterDirection.ReturnValue;
                    cmd3.ExecuteNonQuery();
                    khUID = returnParameter3.Value;
                    cnn3.Close();

                    db.SetCommand(@"Insert into Salary (khUsageID,nurseCost,centerCost,gender)VALUES(@khUsageID,@nurseCost,@centerCost,@gender)");
                    db.SetParameter(@"khUsageID", khUID);
                    db.SetParameter(@"nurseCost", 0);
                    db.SetParameter(@"centerCost", Convert.ToInt32(ds.Tables[0].Rows[0]["khCost"]));
                    db.SetParameter(@"gender", DBNull.Value);
                    db.exec();

                    db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                    db.SetParameter(@"SaID", SaID);
                    db.SetParameter(@"khID", "2");
                    db.SetParameter(@"SaOptionID", "3");
                    db.exec();
                }
                catch
                {
                    RadMessageBox.Show("خطا در ثبت بیمار-کد2");
                }

                db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'بیمار', hourz, visNum as cntNumber FROM dbo.Patients where patientid = '" +
                        PID + "'");
                DataTable info = db.GetData2();

                try
                {
                    db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.khUsage.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID where dbo.khUsage.PatientID = '" +
                                  PID + "'");
                    DataTable KhUsage = db.GetData2();

                    if (checkBox1.Checked)
                    {
                        stiReport1.Load("Reports\\reciptFree.mrt");
                        stiReport1.ResetRenderedState();
                        DataSet ds2 = new DataSet();
                        info.TableName = "info";
                        KhUsage.TableName = "KhUsage";
                        ds2.Tables.Add(info);
                        ds2.Tables.Add(KhUsage);
                        stiReport1.Dictionary.Clear();
                        stiReport1.RegData(ds2);
                        stiReport1.Dictionary.Synchronize();
                        if (Properties.Settings.Default.FishPrinter.ToString() == "True")
                        {
                            stiReport1.Print(false);
                        }
                        else
                        {
                            stiReport1.Show();
                        }
                    }
                    else
                    {
                        stiReport1.Load(path);
                        stiReport1.ResetRenderedState();
                        DataSet ds2 = new DataSet();
                        info.TableName = "info";
                        KhUsage.TableName = "KhUsage";
                        ds2.Tables.Add(info);
                        ds2.Tables.Add(KhUsage);
                        stiReport1.Dictionary.Clear();
                        stiReport1.RegData(ds2);
                        stiReport1.Dictionary.Synchronize();
                        if (Properties.Settings.Default.FishPrinter.ToString() == "True")
                        {
                            stiReport1.Print(false);
                        }
                        else
                        {
                            stiReport1.Show();
                        }
                    }

                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("1خطا در صدور فیش");
                    RadMessageBox.Show(ex.Message);
                }
                ///////////////////
                if (RadMessageBox.Show("آیا مایل به چاپ رسید پزشک می باشید؟", "چاپ رسید پزشک", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                {
                    try
                    {
                        db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'پزشک', hourz, visNum as cntNumber FROM dbo.Patients where patientid = '" +
                            PID + "'");
                        DataTable info2 = db.GetData2();
                        db.exec();

                        db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.khUsage.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID where dbo.khUsage.PatientID = '" +
              PID + "'");
                        DataTable KhUsage2 = db.GetData2();
                        db.exec();

                        if (checkBox1.Checked)
                        {
                            stiReport1.Load("Reports\\reciptFree.mrt");
                            stiReport1.ResetRenderedState();
                            DataSet ds2 = new DataSet();
                            info2.TableName = "info";
                            KhUsage2.TableName = "KhUsage";
                            ds2.Tables.Add(info2);
                            ds2.Tables.Add(KhUsage2);
                            stiReport1.Dictionary.Clear();
                            stiReport1.RegData(ds2);
                            stiReport1.Dictionary.Synchronize();
                            if (Properties.Settings.Default.FishPrinter.ToString() == "True")
                            {
                                stiReport1.Print(false);
                            }
                            else
                            {
                                stiReport1.Show();
                            }
                        }
                        else
                        {
                            stiReport1.Load(path);
                            stiReport1.ResetRenderedState();
                            DataSet ds2 = new DataSet();
                            info2.TableName = "info";
                            KhUsage2.TableName = "KhUsage";
                            ds2.Tables.Add(info2);
                            ds2.Tables.Add(KhUsage2);
                            stiReport1.Dictionary.Clear();
                            stiReport1.RegData(ds2);
                            stiReport1.Dictionary.Synchronize();
                            if (Properties.Settings.Default.FishPrinter.ToString() == "True")
                            {
                                stiReport1.Print(false);
                            }
                            else
                            {
                                stiReport1.Show();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("2خطا در صدور فیش");
                        RadMessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}