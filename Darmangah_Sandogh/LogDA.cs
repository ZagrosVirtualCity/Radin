using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Darmangah_Sandogh
{
    public class LogDA
    {
        public static void LogError(Object ex)
        {
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex);
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                string path = Directory.GetCurrentDirectory();
                path = Path.Combine(path, "ErrorLog"); //, "ErrorLog.txt");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "ErrorLog.txt");
                if (!File.Exists(path))
                    File.Create(path);
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
            catch
            {

            }

        } // end method LogError

        public static void CreateLog(string ConnectionString, string Username, string Type, string TableName, string PrimeryKey, string Description, bool IsDetail = true)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            using (sqlConnection)
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                SqlCommand Command = new SqlCommand(@"INSERT INTO [LogData]
                                   ([Username]
                                   ,[Date]
                                   ,[Time]
                                   ,[Type]
                                   ,[TableName]
                                   ,[PrimeryKey]
                                   ,[Description]
                                   ,[IsDetail])
                             VALUES
                                   (@Username
                                   ,@Date
                                   ,@Time
                                   ,@Type
                                   ,@TableName
                                   ,@PrimeryKey
                                   ,@Description
                                   ,@IsDetail)", sqlConnection);

                Command.Parameters.AddWithValue("Username", Username);
                Command.Parameters.AddWithValue("Date", DateTime.Now);
                Command.Parameters.AddWithValue("Time", DateTime.Now.ToString("HH:mm"));
                Command.Parameters.AddWithValue("Type", Type);
                Command.Parameters.AddWithValue("TableName", TableName);
                Command.Parameters.AddWithValue("PrimeryKey", PrimeryKey);
                Command.Parameters.AddWithValue("Description", Description);
                Command.Parameters.AddWithValue("IsDetail", IsDetail);

                foreach (SqlParameter parameter in Command.Parameters)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                }


                Command.ExecuteNonQuery();
                sqlConnection.Close();
            }

        } // end method CreateLog

    } // end class LogDA

} // end namespace MallLibrary
