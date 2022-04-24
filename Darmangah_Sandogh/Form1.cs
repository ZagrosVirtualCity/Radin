using System;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Darmangah_Sandogh
{
    public partial class Form1 : RadForm
    {
        HdwCheck hdw = new HdwCheck();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radLabelElement12.Text = Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            radLabelElement15.Text = Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            radLabelElement5.Text = GetTodayOfPersianDate();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            var a = new visAzad();
            a.Show();
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            var a = new visBime();
            a.Show();
        }

        private void radButtonElement5_Click(object sender, EventArgs e)
        {
            var a = new Injections();
            a.Show();
        }

        private void radButtonElement11_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void radButtonElement12_Click(object sender, EventArgs e)
        {
            var a = new Sys_Users();
            a.Show();
        }
        string GetTodayOfPersianDate()
        {
            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();

            string year = pc.GetYear(dt).ToString();
            string month = null;
            string dayOfMonth = pc.GetDayOfMonth(dt).ToString();
            string dayOfWeek = null;

            switch (pc.GetMonth(dt))
            {
                case 1: month = "فروردین"; break;
                case 2: month = "اردیبهشت"; break;
                case 3: month = "خرداد"; break;
                case 4: month = "تیر"; break;
                case 5: month = "مرداد"; break;
                case 6: month = "شهریور"; break;
                case 7: month = "مهر"; break;
                case 8: month = "آبان"; break;
                case 9: month = "آذر"; break;
                case 10: month = "دی"; break;
                case 11: month = "بهمن"; break;
                case 12: month = "اسفند"; break;
            }

            switch ((int)pc.GetDayOfWeek(dt))
            {
                case 0: dayOfWeek = "یک شنبه"; break;
                case 1: dayOfWeek = "دو شنبه"; break;
                case 2: dayOfWeek = "سه شنبه"; break;
                case 3: dayOfWeek = "چهار شنبه"; break;
                case 4: dayOfWeek = "پنج شنبه"; break;
                case 5: dayOfWeek = "جمعه"; break;
                case 6: dayOfWeek = "شنبه"; break;
            }

            return string.Format("{0} {1} {2} {3}", dayOfWeek, dayOfMonth, month, year);
        }


        private void radButtonElement13_Click(object sender, EventArgs e)
        {
            var a = new Patients_Edit();
            a.Show();
        }

        private void radButtonElement14_Click(object sender, EventArgs e)
        {
            var a = new Nosakh_Content();
            a.Show();
        }

        private void radButtonElement15_Click(object sender, EventArgs e)
        {
            var a= new TarefeKhedmat();
            a.Show();
        }

        private void radButtonElement16_Click(object sender, EventArgs e)
        {
            var a = new TarefePercent();
            a.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void radButtonElement17_Click(object sender, EventArgs e)
        {
            var a = new Pezeshk();
            a.Show();
        }

        private void radButtonElement19_Click(object sender, EventArgs e)
        {
            var a = new ListInjection();
            a.Show();
        }

        private void radButtonElement24_Click(object sender, EventArgs e)
        {
            var a = new DrugList();
            a.Show();
        }

        private void radButtonElement27_Click(object sender, EventArgs e)
        {
            var a = new InjectionInvoice();
            a.Show();
        }

        private void radButtonElement26_Click(object sender, EventArgs e)
        {
            var a = new VisInvoice();
            a.Show();
        }

        private void radButtonElement18_Click(object sender, EventArgs e)
        {
            var a = new ListVisits();
            a.Show();
        }

        private void radButtonElement7_Click(object sender, EventArgs e)
        {
            var a = new ShiftReport();
            a.Show();
        }

        private void radButtonElement29_Click(object sender, EventArgs e)
        {
            var a = new visNiroo();
            a.Show();
        }
    }
}