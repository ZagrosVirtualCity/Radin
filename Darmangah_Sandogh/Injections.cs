using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Injections : RadForm
    {
        MyDB db = new MyDB();
        DataSet ds = new DataSet();
        private object PID;
        private object khUID;
        private double tarif;
        StiReport stiReport1 = new StiReport();
        string path = "Reports\\Recipt.mrt";
        private double nurseP;
        private double nurseCost;
        private double centerCost;
        DataSet dset = new DataSet();
        private string rowID;
        DataSet dz = new DataSet();
        private object SaID;
        public Injections()
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
            regBox.Text = PersianDateTime.Now.ToString("yyyy/MM/dd");
            /////////////////////
            db.SetCommand("Select * From drug");
            ds = db.GetData();
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "drugName";
            comboBox1.ValueMember = "drugID";
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (fname.Text == "" || fname.Text == " " || fname.Text == string.Empty || lname.Text == "" ||
                lname.Text == " " || lname.Text == string.Empty)
            {
                MessageBox.Show("نام یا نام خانوادگی نوشته نشده است");
                fname.Focus();
            }
            else
            {
                db.SetCommand(@"Select NursePercent From ShftPers where ShiftID = @ShiftID AND PersonelID = @PersonelID");
                db.SetParameter(@"ShiftID", GlobalVariables.shftID);
                db.SetParameter(@"PersonelID", GlobalVariables.persID);
                dz = db.GetData();

                nurseP = Convert.ToDouble(dz.Tables[0].Rows[0]["NursePercent"]);

                if (GlobalVariables.shftID == 1 || GlobalVariables.shftID == 2 || GlobalVariables.shftID == 3 || GlobalVariables.shftID == 4)

                {
                    db.SetCommand(@"SELECT TOP 1 tazrighatNum FROM dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID Where dbo.Patients.reg_date = '" +
                        PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID +
                        "' Order By tazrighatNum DESC");
                    dset = db.GetData();

                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        GlobalVariables.tazrighatNum = Convert.ToInt16(dset.Tables[0].Rows[0]["tazrighatNum"]) + 1;
                    }
                    else
                    {
                        GlobalVariables.tazrighatNum += 1;
                    }
                }

                if (GlobalVariables.shftID == 5 || GlobalVariables.shftID == 6)
                {
                    db.SetCommand(@"SELECT TOP 1 tazrighatNum FROM dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID
Where dbo.Patients.reg_date Between '" + PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND '" +
                                  PersianDateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + "' AND " +
                                  "dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' Order By tazrighatNum DESC");
                    dset = db.GetData();

                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        GlobalVariables.tazrighatNum = Convert.ToInt16(dset.Tables[0].Rows[0]["tazrighatNum"]) + 1;
                    }
                    else
                    {
                        GlobalVariables.tazrighatNum += 1;
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
                    cmd.Parameters.AddWithValue(@"reg_date", regBox.Text);
                    cmd.Parameters.AddWithValue(@"hourz", DateTime.Now.ToString("HH:mm"));
                    cmd.Parameters.AddWithValue(@"UserID", GlobalVariables.LoggedInUser);
                    if (checkBox1.Checked)
                    {
                        cmd.Parameters.AddWithValue(@"Free", "True");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(@"Free", "False");
                    }
                    cmd.Parameters.AddWithValue(@"tazrighatNum", GlobalVariables.tazrighatNum);
                    cmd.Parameters.AddWithValue(@"visNum", 0);
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
                    MessageBox.Show("خطا در ثبت بیمار-کد1");
                    MessageBox.Show(ex.Message);
                }
                ///////////////////////////////
                try
                {
                    if (radCheckBox1.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 5");
                        ds = db.GetData();

                        tarif = Convert.ToDouble(radTextBox1.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 5);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox1.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "5");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    ///////////////////////
                    if (radCheckBox2.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 6");
                        ds = db.GetData();
                        db.exec();
                        tarif = Convert.ToDouble(radTextBox2.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 6);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox2.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "6");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    ///////////////////////
                    if (radCheckBox3.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 7");
                        ds = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(radTextBox3.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 7);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox3.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "7");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    ///////////////////////
                    if (radCheckBox4.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 8");
                        ds = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(radTextBox4.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 8);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox4.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "8");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    ///////////////////////
                    if (radCheckBox5.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 10");
                        ds = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(radTextBox5.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 10);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox5.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "10");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    ///////////////////////
                    if (radCheckBox6.Checked)
                    {
                        db.SetCommand("Select * From khType where khID = 11");
                        ds = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(radTextBox6.Text)*
                                Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 11);
                        cmd2.Parameters.AddWithValue(@"Num", radTextBox6.Text);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "11");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    
                    if (radCheckBox8.Checked)
                    {
                        db.SetCommand("Select khCost From khType where khID = 18");
                        ds = db.GetData();
                        db.exec();

                        tarif = Convert.ToDouble(ds.Tables[0].Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.Cnn);
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", 18);
                        cmd2.Parameters.AddWithValue(@"Num", 1);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", ds.Tables[0].Rows[0]["khCost"].ToString());
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,drCost,nurseCost,centerCost,gender)VALUES(@khUsageID,@drCost,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"drCost", 0);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", "18");
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                    }
                    /////////////////////////////////////////
                    MessageBox.Show("بیمار با موفقیت ثبت شد");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطا در ثبت بیمار-کد2");
                    MessageBox.Show(ex.Message);
                }


                db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'بیمار', hourz, tazrighatNum as cntNumber FROM dbo.Patients where patientid = '" + PID + "'");
                DataTable info = db.GetData2();
                db.exec();

                try
                {
                    db.SetCommand(@"SELECT dbo.khType.khName, dbo.khUsage.Num, dbo.khUsage.cost
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID where dbo.khUsage.PatientID = '" +
                                  PID + "'");
                    DataTable KhUsage = db.GetData2();
                    db.exec();


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
                        stiReport1.Print(false);
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
                        stiReport1.Print(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطا در صدور فیش");
                    MessageBox.Show(ex.Message);
                }
                ///////////////////////
                if (
                    MessageBox.Show("آیا مایل به چاپ رسید تزریقات می باشید؟", "چاپ رسید تزریقات",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    try
                    {
                        db.SetCommand(
                            @"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'تزریقات', hourz, tazrighatNum as cntNumber FROM dbo.Patients where patientid = '" +
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
                            stiReport1.Print(false);
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
                            stiReport1.Print(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("2خطا در صدور فیش");
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void radCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox1.Checked)
            {
                radTextBox1.Enabled = true;
            }
            else
            {
                radTextBox1.Enabled = false;
            }
        }

        private void radCheckBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox2.Checked)
            {
                radTextBox2.Enabled = true;
            }
            else
            {
                radTextBox2.Enabled = false;
            }
        }

        private void radCheckBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox3.Checked)
            {
                radTextBox3.Enabled = true;
            }
            else
            {
                radTextBox3.Enabled = false;
            }
        }

        private void radCheckBox4_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox4.Checked)
            {
                radTextBox4.Enabled = true;
            }
            else
            {
                radTextBox4.Enabled = false;
            }
        }

        private void radCheckBox5_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox5.Checked)
            {
                radTextBox5.Enabled = true;
            }
            else
            {
                radTextBox5.Enabled = false;
            }
        }

        private void radCheckBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox6.Checked)
            {
                radTextBox6.Enabled = true;
            }
            else
            {
                radTextBox6.Enabled = false;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            db.SetCommand(@"insert into drugUsage (drugID,patientID,countz) VALUES (@drugID,@patientID,@countz)");
            db.SetParameter(@"drugID", comboBox1.SelectedValue);
            db.SetParameter(@"patientID", PID);
            db.SetParameter(@"countz", radTextBox8.Text);
            db.exec();
            FillGrid();
        }

        private void radGridView1_UserDeletingRow(object sender, Telerik.WinControls.UI.GridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("آیا مایل به حذف دارو می باشید ؟", "حذف دارو", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                db.SetCommand(@"Delete From drugUsage where drugUsageID = @drugUsageID");
                db.SetParameter(@"drugUsageID", rowID);
                db.exec();
            }
            FillGrid();

        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            rowID = radGridView1.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void FillGrid()
        {
            try
            {
                db.SetCommand(@"SELECT dbo.drugUsage.drugUsageID, dbo.drugUsage.patientID, dbo.Patients.fname, dbo.Patients.lname, dbo.drug.drugName, dbo.drugUsage.countz
FROM            dbo.Patients INNER JOIN
                         dbo.drugUsage ON dbo.Patients.patientid = dbo.drugUsage.patientID INNER JOIN
                         dbo.drug ON dbo.drugUsage.drugID = dbo.drug.drugID WHERE dbo.drugUsage.patientID = '" + PID +
                              "'");
                ds = db.GetData();
                db.exec();

                radGridView1.DataSource = ds.Tables[0];

                radGridView1.EnableFiltering = true;
                radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
                radGridView1.MasterTemplate.ShowFilteringRow = false;
                radGridView1.MasterTemplate.BestFitColumns();
                foreach (GridViewDataColumn column in radGridView1.Columns)
                {
                    column.BestFit();
                }
            }
            catch
            {
            }
        }
    }
}