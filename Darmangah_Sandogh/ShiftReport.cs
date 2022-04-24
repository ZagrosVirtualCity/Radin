using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Report;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class ShiftReport : RadForm
    {
        MyDB db = new MyDB();


        public ShiftReport()
        {
            InitializeComponent();
        }

        private void ShiftReport_Load(object sender, EventArgs e)
        {
            regBox1.Date = PersianDateTime.Now.ToString("yyyy/mm/dd");
        }

        private void radButton1_Click_old(object sender, EventArgs e)
        {
//            PersianCalendar pc = new PersianCalendar();
//            string hourz = pc.GetHour(DateTime.Now) + ":" + pc.GetMinute(DateTime.Now);
//            try
//            {
               
//                ///////////////////
//                db.SetCommand(@"SELECT SUM(dbo.Salary.nurseCost) AS ManNurse
//FROM            dbo.Patients INNER JOIN
//                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
//                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID 
//WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID +
//                              "' AND dbo.patients.Free = 'False' AND dbo.Salary.sex = 'True'");
//                DataTable ManNurse = db.GetData2();

//                db.SetCommand(@"SELECT SUM(dbo.Salary.nurseCost) AS WomanNurse
//FROM            dbo.Patients INNER JOIN
//                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
//                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID 
//WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID +
//              "' AND dbo.patients.Free = 'False' AND dbo.Salary.sex = 'False'");
//                DataTable WomanNurse = db.GetData2();

//                ////////////////////////
//                db.SetCommand(@"SELECT ISNULL(SUM(dbo.khUsage.cost) + '" + jarahiCostP + "', 0) *50/100 AS SumTotal FROM dbo.Patients INNER JOIN dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID WHERE dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND (dbo.khUsage.KhID = '1' OR dbo.khUsage.KhID = '2' OR dbo.khUsage.KhID = '1033' OR dbo.khUsage.KhID = '3' OR dbo.khUsage.KhID = '13' OR dbo.khUsage.KhID = '14' OR dbo.khUsage.KhID = '15' OR dbo.khUsage.KhID = '16' OR dbo.khUsage.KhID = '17') AND dbo.khUsage.depID = '1' AND dbo.patients.Free = 'False'");
//                DataTable drCOST = db.GetData2();


//                try
//                {
//                    daramad = 0;
//                    try
//                    {
//                        daramad += Convert.ToInt32(drTotal.Rows[0]["SumTotal"]);
//                    }
//                    catch
//                    {
//                    }
//                    try
//                    {
//                        daramad += Convert.ToInt32(mamaTotal.Rows[0]["SumTotal"]);
//                    }
//                    catch
//                    {
//                    }
//                    try
//                    {
//                        daramad += Convert.ToInt32(injectTotal.Rows[0]["SumTotal"]);
//                    }
//                    catch
//                    {
//                    }

//                }
//                catch
//                {
//                }
//                try
//                {
//                    hazine = Convert.ToInt32(drCOST.Rows[0]["SumTotal"]) + Convert.ToInt32(ManNurse.Rows[0]["ManNurse"]) + Convert.ToInt32(WomanNurse.Rows[0]["WomanNurse"]) + Convert.ToInt32(Secretery.Rows[0]["Secretery"]);
//                }
//                catch
//                {
//                }

//                baghi = daramad - hazine;

//                DataTable finish = new DataTable("finish");
//                finish.Columns.Add("daramad", typeof(string));
//                finish.Columns.Add("hazine", typeof(string));
//                finish.Columns.Add("baghi", typeof(string));
//                finish.Rows.Add(daramad, hazine, baghi);


//            }
//            catch (Exception ex)
//            {
//                RadMessageBox.Show("1خطا در صدور فیش");
//                RadMessageBox.Show(ex.Message);
//            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            PersianCalendar pc = new PersianCalendar();
            string hourz = pc.GetHour(DateTime.Now) + ":" + pc.GetMinute(DateTime.Now);

            db.SetCommand(@"SELECT dbo.Doctors.doctorName, dbo.Shifts.ShiftName, sec = N'" + radTextBox3.Text +
                          "', nurse1 = N'" + GlobalVariables.nurseName1 +
                          "' , nurse2 = N'" + GlobalVariables.nurseName2 + "', hourz = N'" + hourz +
                          "', dbo.Patients.reg_date, tozih = N'" + richTextBox1.Text +
                          "' FROM dbo.Patients INNER JOIN dbo.Doctors ON dbo.Patients.doctorID = dbo.Doctors.doctorID INNER JOIN dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN dbo.Shifts ON dbo.khUsage.ShiftID = dbo.Shifts.ShiftID WHERE dbo.Patients.reg_date = '" +
                          regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "'");
            DataTable info = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, ISNULL(SUM(dbo.Salary.drCost), 0) AS SumTotal,khType.khName
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '2' Group By khUsage.KhID,khType.khName");
            DataTable DoctorList = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, ISNULL(SUM(dbo.Salary.nurseCost), 0) AS SumTotal,khType.khName
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '1' Group By khUsage.KhID,khType.khName");
            DataTable NurseList = db.GetData2();


            /////////////////
            db.SetCommand(@"SELECT ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False'");
            DataTable Daramad = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, ISNULL(SUM(dbo.Salary.drCost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '2'");
            DataTable DoctorTotal = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(SUM(dbo.Salary.nurseCost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '1' AND dbo.Salary.gender = 'True'");
            DataTable NurseListMail = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(SUM(dbo.Salary.nurseCost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '1' AND dbo.Salary.gender = 'False'");
            DataTable NurseListFemail = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(SUM(dbo.Salary.centerCost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False'");
            DataTable Darmangah = db.GetData2();


            DataTable Summury = new DataTable("Summury");
            Summury.Columns.Add("CostType", typeof(string));
            Summury.Columns.Add("Cost", typeof(string));
            Summury.Rows.Add("جمع کل درآمد", Daramad.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم پزشک", DoctorTotal.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم پرستار آقا", NurseListMail.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم پرستار خانم", NurseListFemail.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم درمانگاه", Darmangah.Rows[0]["SumTotal"].ToString());


            StiReport stiReport1 = new StiReport();
            string path = Application.StartupPath + "\\Reports\\ShiftReport.mrt";
            stiReport1.Load(path);
            DataSet ds1 = new DataSet();
            info.TableName = "info";
            DoctorList.TableName = "DoctorList";
            NurseList.TableName = "NurseList";
            Summury.TableName = "Summury";
            ds1.Tables.Add(info);
            ds1.Tables.Add(DoctorList);
            ds1.Tables.Add(NurseList);
            ds1.Tables.Add(Summury);
            stiReport1.ResetRenderedState();
            stiReport1.RegData(ds1);
            stiReport1.Dictionary.Synchronize();
            stiReport1.Show();
        }
    }
}