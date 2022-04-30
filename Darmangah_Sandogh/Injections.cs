using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Injections : RadForm
    {
        MyDB db = new MyDB();
        private object PID;
        private object khUID;
        private double tarif;
        StiReport stiReport1 = new StiReport();
        string path = "Reports\\Recipt.mrt";
        private double nurseP;
        private double nurseCost;
        private double centerCost;
        private string rowID;
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
            db.SetCommand(@"Select * From khType WHERE khGrpID=@khGrpID");
            db.SetParameter(@"khGrpID", "1");
            DataTable _dtKhedmatType = db.GetData2();
            KhedmatTypeBox.DataSource = _dtKhedmatType;
            KhedmatTypeBox.ValueMember = "khID";
            KhedmatTypeBox.DisplayMember = "khName";

            db.SetCommand("Select * From drug order by drugName");
            DataTable _Drug = db.GetData2();
            comboBox1.DataSource = _Drug;
            comboBox1.DisplayMember = "drugName";
            comboBox1.ValueMember = "drugID";

            db.SetCommand("Select * From PaymentType");
            DataTable _PayBox = db.GetData2();
            PaymentTypeBox.DataSource = _PayBox;
            PaymentTypeBox.DisplayMember = "PaymentType";
            PaymentTypeBox.ValueMember = "PaymentTypeID";
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (PID == null)
                {
                    RadMessageBox.Show(@"لطفا اول بیمار را پذیرش کنید", @"عدم پذیرش", MessageBoxButtons.OK, RadMessageIcon.Error);
                    fname.Focus();
                }
                else
                {
                    if (txtKhCount.Value > 0)
                    {
                        db.SetCommand(@"Select NursePercent From ShftPers where ShiftID = @ShiftID AND PersonelID = @PersonelID");
                        db.SetParameter(@"ShiftID", GlobalVariables.shftID);
                        db.SetParameter(@"PersonelID", GlobalVariables.persID);
                        DataTable _NursePercent = db.GetData2();

                        nurseP = Convert.ToDouble(_NursePercent.Rows[0]["NursePercent"]);

                        db.SetCommand("Select * From khType where khID = @khID");
                        db.SetParameter("khID", KhedmatTypeBox.SelectedValue);
                        DataTable _KhCost = db.GetData2();

                        tarif = Convert.ToDouble(txtKhCount.Value) *
                                Convert.ToDouble(_KhCost.Rows[0]["khCost"].ToString());

                        var cnn2 = new SqlConnection(db.CreateConnectionString());
                        cnn2.Open();
                        SqlCommand cmd2 = cnn2.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "KhAdd";
                        cmd2.Parameters.AddWithValue(@"PatientID", PID);
                        cmd2.Parameters.AddWithValue(@"ShiftID", GlobalVariables.shftID);
                        cmd2.Parameters.AddWithValue(@"PersonelID", GlobalVariables.persID);
                        cmd2.Parameters.AddWithValue(@"nurseID1", GlobalVariables.nurseID1);
                        cmd2.Parameters.AddWithValue(@"nurseID2", GlobalVariables.nurseID2);
                        cmd2.Parameters.AddWithValue(@"KhID", KhedmatTypeBox.SelectedValue);
                        cmd2.Parameters.AddWithValue(@"Num", txtKhCount.Value);
                        cmd2.Parameters.AddWithValue(@"depID", 2);
                        cmd2.Parameters.AddWithValue(@"cost", tarif);
                        cmd2.Parameters.AddWithValue(@"gender", gender.Value);
                        cmd2.Parameters.AddWithValue(@"IDPaymentType", PaymentTypeBox.SelectedValue);
                        var returnParameter2 = cmd2.Parameters.Add("@khUID", SqlDbType.Int);
                        returnParameter2.Direction = ParameterDirection.ReturnValue;
                        cmd2.ExecuteNonQuery();
                        khUID = returnParameter2.Value;
                        cnn2.Close();

                        nurseCost = (nurseP * tarif) / 100;
                        centerCost = ((100 - nurseP) * tarif) / 100;

                        db.SetCommand(@"Insert into Salary (khUsageID,nurseCost,centerCost,gender)VALUES(@khUsageID,@nurseCost,@centerCost,@gender)");
                        db.SetParameter(@"khUsageID", khUID);
                        db.SetParameter(@"nurseCost", nurseCost);
                        db.SetParameter(@"centerCost", centerCost);
                        db.SetParameter(@"gender", gender.Value);
                        db.exec();

                        db.SetCommand("Insert into SysActKh (SaID,khID,SaOptionID) VALUES (@SaID,@khID,@SaOptionID)");
                        db.SetParameter(@"SaID", SaID);
                        db.SetParameter(@"khID", KhedmatTypeBox.SelectedValue);
                        db.SetParameter(@"SaOptionID", "3");
                        db.exec();
                        /////////////////////////////////////////
                        FillGridKhedmat();
                    }
                    else
                    {
                        RadMessageBox.Show("لطفا تعداد خدمت وارد کنید", "خطا تعداد", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("خطا در ثبت بیمار-کد2");
                LogDA.LogError(ex.Message);
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (txtDrugCount.Value > 0)
            {
                db.SetCommand(@"insert into drugUsage (drugID,patientID,countz) VALUES (@drugID,@patientID,@countz)");
                db.SetParameter(@"drugID", comboBox1.SelectedValue);
                db.SetParameter(@"patientID", PID);
                db.SetParameter(@"countz", txtDrugCount.Value);
                db.exec();
                FillGrid();
            }
            else
            {
                RadMessageBox.Show("لطفا تعداد دارو وارد کنید", "خطا تعداد", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
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
                         dbo.drug ON dbo.drugUsage.drugID = dbo.drug.drugID WHERE dbo.drugUsage.patientID = '" + PID + "'");
                DataTable _FillGrid = db.GetData2();

                radGridView1.DataSource = _FillGrid;

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

        private void btnPrint_Click(object sender, EventArgs e)
        {

            db.SetCommand(@"SELECT fname + ' ' + lname as fullName, reg_date, recipt = N'بیمار', hourz, tazrighatNum as cntNumber FROM dbo.Patients where patientid = '" + PID + "'");
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
                RadMessageBox.Show("خطا در صدور فیش");
                LogDA.LogError(ex.Message);
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
                    LogDA.LogError(ex.Message);
                }
            }
        }
        private void FillGridKhedmat()
        {
            try
            {
                db.SetCommand(@"SELECT ROW_NUMBER() OVER(ORDER BY khUsage.KhUsageID) AS RowNo, dbo.khUsage.KhUsageID, dbo.Patients.fname + ' ' + dbo.Patients.lname AS FullName, dbo.Patients.reg_date, dbo.khType.khName, dbo.khUsage.Num, dbo.khType.khID
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID WHERE dbo.khUsage.patientID = @PID");
                db.SetParameter(@"PID", PID);
                DataTable _FillGridKhedmat = db.GetData2();

                radGridView2.DataSource = _FillGridKhedmat;

                radGridView2.EnableFiltering = true;
                radGridView2.MasterTemplate.ShowHeaderCellButtons = true;
                radGridView2.MasterTemplate.ShowFilteringRow = false;
                radGridView2.MasterTemplate.BestFitColumns();
                foreach (GridViewDataColumn column in radGridView2.Columns)
                {
                    column.BestFit();
                }
            }
            catch
            {
            }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            if (fname.Text == "" || fname.Text == " " || fname.Text == string.Empty || lname.Text == "" ||
                lname.Text == " " || lname.Text == string.Empty)
            {
                RadMessageBox.Show("نام یا نام خانوادگی نوشته نشده است", "اطلاعات ناقص", MessageBoxButtons.OK, RadMessageIcon.Error);
                fname.Focus();
            }
            else
            {
                if (GlobalVariables.shftID == 1 || GlobalVariables.shftID == 2 || GlobalVariables.shftID == 3 || GlobalVariables.shftID == 4)

                {
                    db.SetCommand(@"SELECT TOP 1 tazrighatNum FROM dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID Where dbo.Patients.reg_date = '" +
                        PersianDateTime.Now.ToString("yyyy/MM/dd") + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID +
                        "' Order By tazrighatNum DESC");
                    DataTable _TazrigNo = db.GetData2();

                    if (_TazrigNo.Rows.Count > 0)
                    {
                        GlobalVariables.tazrighatNum = Convert.ToInt16(_TazrigNo.Rows[0]["tazrighatNum"]) + 1;
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
                    DataTable _TazrigNo2 = db.GetData2();

                    if (_TazrigNo2.Rows.Count > 0)
                    {
                        GlobalVariables.tazrighatNum = Convert.ToInt16(_TazrigNo2.Rows[0]["tazrighatNum"]) + 1;
                    }
                    else
                    {
                        GlobalVariables.tazrighatNum += 1;
                    }
                }
                try
                {
                    var cnn = new SqlConnection(db.CreateConnectionString());
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
                    DataTable _PatientInfo = db.GetData2();

                    var cnn2 = new SqlConnection(db.CreateConnectionString());
                    cnn2.Open();
                    SqlCommand cmd2 = cnn2.CreateCommand();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandText = "SaAdd";
                    cmd2.Parameters.AddWithValue(@"PatientID", PID);
                    cmd2.Parameters.AddWithValue(@"FullName", _PatientInfo.Rows[0]["fname"] + " " + _PatientInfo.Rows[0]["lname"]);
                    cmd2.Parameters.AddWithValue(@"UserID", _PatientInfo.Rows[0]["UserID"]);
                    cmd2.Parameters.AddWithValue(@"reg_date", _PatientInfo.Rows[0]["reg_date"]);
                    cmd2.Parameters.AddWithValue(@"hourz", _PatientInfo.Rows[0]["hourz"]);
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

                    RadMessageBox.Show("بیمار با موفقیت ثبت شد", "ثبت بیمار", MessageBoxButtons.OK, RadMessageIcon.Info);

                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("خطا در ثبت بیمار-کد1");
                    LogDA.LogError(ex.Message);
                }
            }
        }

        private void KhedmatTypeBox_VisualListItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            Font myFont = new Font("IRANSansMobile", 10, FontStyle.Bold);

            args.VisualItem.Font = myFont;
        }
    }
}