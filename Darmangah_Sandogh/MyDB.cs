using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;

namespace Darmangah_Sandogh
{
    internal class MyDB
    {
        private SqlConnection cnn = null;
        public SqlCommand cmd = null;
        public string strCon = string.Empty;

        public MyDB()
        {

        }
        public string CreateConnectionString()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.StartupPath + "\\WinConfig.xml");
                XmlElement rootElem = xmlDoc.DocumentElement;
                strCon = "Data Source=" + rootElem.ChildNodes[0]["servername"].InnerText + ";" + "Initial Catalog=" +
                      rootElem.ChildNodes[0]["dbname"].InnerText + ";" + "User ID=" +
                      CryptorEngine.Decrypt(rootElem.ChildNodes[0]["username"].InnerText, true) + ";" + "Password = " +
                      CryptorEngine.Decrypt(rootElem.ChildNodes[0]["password"].InnerText, true);

            }
            catch (Exception ex)
            {
                LogDA.LogError(ex);
            }

            return strCon;
        } // end method CreateConnectionString

        public void SetCommand(string sql)
        {
            try
            {
                if (cnn == null)
                    cnn = new SqlConnection();
                if (cmd == null)
                    cmd = new SqlCommand();

                cnn.ConnectionString = CreateConnectionString();
                cmd.Parameters.Clear();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex);
            }

        } // end method SetCommand

        public void SetCommand_SP(string sql)
        {
            try
            {
                if (cnn == null)
                    cnn = new SqlConnection();
                if (cmd == null)
                    cmd = new SqlCommand();

                cnn.ConnectionString = CreateConnectionString();
                cmd.Parameters.Clear();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex);
            }

        } // end method SetCommand_SP

        public DataSet GetData()
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                LogDA.LogError(ex);
                return null;
            }
            finally
            {
                disconnect();
            }

        } // end method GetData

        public DataTable GetData3(SqlCommand mycmd)
        {
            SqlDataAdapter da = new SqlDataAdapter(mycmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error: " + ex.Number);
                LogDA.LogError(ex);
                return null;
            }
            finally
            {

            }
        } // end method GetData3

        public DataTable GetData2()
        {
            DataTable dt = new DataTable();

            if (connect())
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    da.Fill(dt);
                    return dt;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error: " + ex.Number);
                    LogDA.LogError(ex);
                    return null;
                }
                finally
                {
                    disconnect();
                }
            }
            return dt;
        } // end merhod GetData2

        public bool exec()
        {
            if (!connect())
                return false;
            try
            {
                foreach (SqlParameter parameter in cmd.Parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                // cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.Message, "Error: " + ex.Number);
                LogDA.LogError(ex);
                return false;
            }
            finally
            {
                disconnect();
            }
        } // end method exec

        public int execWithReturnID()
        {
            int ID = new int();

            if (!connect())
                return 0;
            try
            {
                foreach (SqlParameter parameter in cmd.Parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                }

                ID = Convert.ToInt32(cmd.ExecuteScalar());
                return ID;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error: " + ex.Number);
                LogDA.LogError(ex);
                return 0;
            }
            finally
            {
                disconnect();
            }
        } // end method execWithReturnID

        public void SetParameter<AnyType>(string parameterName, AnyType parameterValue)
        {
            try
            {
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex);
            }

        } // end method SetParameter

        public bool connect()
        {
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                return true;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Error : 3001 - " + "Connection Lost, Please check your connection");
                LogDA.LogError(ex);
                return false;
            }
        } // end method connect

        public void disconnect()
        {
            try
            {
                if (cnn.State != ConnectionState.Closed)
                    cnn.Close();
            }
            catch (Exception ex)
            {
                LogDA.LogError(ex);
            }

        } // end method disconnect
    }
}