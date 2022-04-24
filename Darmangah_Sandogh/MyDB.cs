using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace Darmangah_Sandogh
{
    internal class MyDB
    {
        readonly SqlConnection cnn;
        readonly SqlCommand cmd;
        private SqlDataAdapter da;
        private DataSet ds;
        private DataTable dt;
        internal string Cnn;

        public MyDB()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\WinConfig.xml");
            XmlElement rootElem = xmlDoc.DocumentElement;
            Cnn = "Data Source=" + rootElem.ChildNodes[0]["servername"].InnerText + ";" + "Initial Catalog=" +
                  rootElem.ChildNodes[0]["dbname"].InnerText + ";" + "User ID=" +
                  CryptorEngine.Decrypt(rootElem.ChildNodes[0]["username"].InnerText, true) + ";" + "Password = " +
                  CryptorEngine.Decrypt(rootElem.ChildNodes[0]["password"].InnerText, true);

            cnn = new SqlConnection(Cnn);
            cmd = new SqlCommand();
            cmd.Connection = cnn;
        }

        public void SetCommand(string sql)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = sql;
        }

        public DataSet GetData()
        {
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ds;
        }

        public DataTable GetData2()
        {
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public void exec()
        {

            connect();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //MessageBox.Show("Error : 3002 - " + "خطا در انجام عملیات");
                MessageBox.Show(ex.Message, "Error: " + ex.Number);
            }
            disconnect();

        }

        public void SetParameter<AnyType>(string parameterName, AnyType parameterValue)
        {
            cmd.Parameters.AddWithValue(parameterName, parameterValue);
        }

        public void connect()
        {
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Error : 3001 - " + "خطا در اتصال به بانک اطلاعاتی - با مدیر تماس بگیرید");
            }
        }

        public void disconnect()
        {
            if (cnn.State != ConnectionState.Closed)
                cnn.Close();
        }
    }
}