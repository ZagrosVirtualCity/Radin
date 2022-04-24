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

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, ISNULL(SUM(dbo.khUsage.Cost), 0) AS SumTotal,khType.khName
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '2' Group By khUsage.KhID,khType.khName");
            DataTable DoctorList = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, dbo.khType.khName, ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khType.khGrpID = '1' Group By khUsage.KhID,khType.khName");
            DataTable NurseList = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, ISNULL(SUM(dbo.khUsage.Cost), 0) AS SumTotal,khType.khName
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'True' AND dbo.khType.khGrpID = '2' Group By khUsage.KhID,khType.khName");
            DataTable DoctorListFree = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(COUNT(*), 0) AS cnt, dbo.khType.khName, ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.khType ON dbo.khUsage.KhID = dbo.khType.khID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'True' AND dbo.khType.khGrpID = '1' Group By khUsage.KhID,khType.khName");
            DataTable NurseListFree = db.GetData2();


            /////////////////
            db.SetCommand(@"SELECT ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False'");
            DataTable Daramad = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khUsage.IDPaymentType = '1'");
            DataTable DaramadCash = db.GetData2();

            db.SetCommand(@"SELECT ISNULL(SUM(dbo.khUsage.cost), 0) AS SumTotal
FROM            dbo.Patients INNER JOIN
                         dbo.khUsage ON dbo.Patients.patientid = dbo.khUsage.PatientID INNER JOIN
                         dbo.Salary ON dbo.khUsage.KhUsageID = dbo.Salary.khUsageID
WHERE        dbo.Patients.reg_date = '" + regBox1.Date + "' AND dbo.khUsage.ShiftID = '" + GlobalVariables.shftID + "' AND dbo.patients.Free = 'False' AND dbo.khUsage.IDPaymentType = '2'");
            DataTable DaramadCard = db.GetData2();


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
            Summury.Rows.Add("سهم پرستار آقا", NurseListMail.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم پرستار خانم", NurseListFemail.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("سهم درمانگاه", Darmangah.Rows[0]["SumTotal"].ToString());
            if (DoctorList.Rows.Count > 0)
            {
                Summury.Rows.Add("ویزیت رایگان", DoctorListFree.Rows[0]["SumTotal"].ToString());
            }
            else
            {
                Summury.Rows.Add("ویزیت رایگان", "");
            }

            if (DoctorList.Rows.Count > 0)
            {
                Summury.Rows.Add("تزریقات رایگان", NurseListFree.Rows[0]["SumTotal"].ToString() == string.Empty ? string.Empty : NurseListFree.Rows[0]["SumTotal"].ToString());
            }
            else
            {
                Summury.Rows.Add("تزریقات رایگان","");
            }

            Summury.Rows.Add("جمع کل درآمد - نقدی", DaramadCash.Rows[0]["SumTotal"].ToString());
            Summury.Rows.Add("جمع کل درآمد - کارت", DaramadCard.Rows[0]["SumTotal"].ToString());

            StiReport stiReport1 = new StiReport();
            string path = Application.StartupPath + "\\Reports\\ShiftReport.mrt";
            stiReport1.Load(path);
            DataSet ds1 = new DataSet();
            info.TableName = "info";
            DoctorList.TableName = "DoctorList";
            NurseList.TableName = "NurseList";
            DoctorListFree.TableName = "DoctorListFree";
            NurseListFree.TableName = "NurseListFree";
            Summury.TableName = "Summury";
            ds1.Tables.Add(info);
            ds1.Tables.Add(DoctorList);
            ds1.Tables.Add(NurseList);
            ds1.Tables.Add(DoctorListFree);
            ds1.Tables.Add(NurseListFree);
            ds1.Tables.Add(Summury);
            stiReport1.ResetRenderedState();
            stiReport1.RegData(ds1);
            stiReport1.Dictionary.Synchronize();
            stiReport1.Show();
        }
    }
}