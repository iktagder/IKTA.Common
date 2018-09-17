using System;
using System.Data.SqlClient;

namespace IKTA.Common.Components.Helpers
{
    public class DataBase
    {
        private static string ConnectionString
        {
            get
            {
                return SSO.GetSSOString("IKTA.Common", "ConnectionString_BizTalkResources");
            }
        }
        private static int CommandTimeout
        {
            get
            {
                return SSO.GetSSOInt32("IKTA.Common", "CommandTimeout");
            }
        }

        public static string GetMappedValue(string fromSystem, string fromField, string fromValue, string toSystem, string toField)
        {
            if (String.IsNullOrWhiteSpace(fromSystem))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromSystem");
            if (String.IsNullOrWhiteSpace(fromField))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromField");
            if (String.IsNullOrWhiteSpace(toSystem))
                throw new ArgumentException("Missing or empty mandatory parameter", "toSystem");
            if (String.IsNullOrWhiteSpace(toField))
                throw new ArgumentException("Missing or empty mandatory parameter", "toField");
            if (String.IsNullOrWhiteSpace(fromValue))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromSystem");

            string queryString =
                @"SELECT ToValue FROM [BizTalkResources].[dbo].[ValueMapping] WHERE 
                     fromSystem = @fromSystem and 
                     fromField  = @fromField  and 
                     fromValue  = @fromValue  and
                     toSystem   = @toSystem   and
                     toField    = @toField";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandTimeout = CommandTimeout;
                    command.Parameters.AddWithValue("@fromSystem", fromSystem);
                    command.Parameters.AddWithValue("@fromField", fromField);
                    command.Parameters.AddWithValue("@fromValue", fromValue);
                    command.Parameters.AddWithValue("@toSystem", toSystem);
                    command.Parameters.AddWithValue("@toField", toField);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["toValue"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("IKTA.Common",
                                                       e.Message,
                                                       System.Diagnostics.EventLogEntryType.Error);
                Logger.LogError(e.Message);
                throw;
            }
            var errorMessage = $"Missing Mapping: FromSystem: '{fromSystem}'; FromField: '{fromField}'; FromValue: '{fromValue}'; ToSystem: '{toSystem}'; ToField: '{toField}'";
            Logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }

        public static string GetMappedValueWithDefault(string fromSystem, string fromField, string fromValue, string toSystem, string toField, string defaultValue)
        {
            if (String.IsNullOrWhiteSpace(fromSystem))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromSystem");
            if (String.IsNullOrWhiteSpace(fromField))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromField");
            if (String.IsNullOrWhiteSpace(toSystem))
                throw new ArgumentException("Missing or empty mandatory parameter", "toSystem");
            if (String.IsNullOrWhiteSpace(toField))
                throw new ArgumentException("Missing or empty mandatory parameter", "toField");
            if (String.IsNullOrWhiteSpace(fromValue))
                throw new ArgumentException("Missing or empty mandatory parameter", "fromSystem");

            string queryString =
                @"SELECT ToValue FROM [BizTalkResources].[dbo].[ValueMapping] WHERE 
                     fromSystem = @fromSystem and 
                     fromField  = @fromField  and 
                     fromValue  = @fromValue  and
                     toSystem   = @toSystem   and
                     toField    = @toField";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@fromSystem", fromSystem);
                    command.Parameters.AddWithValue("@fromField", fromField);
                    command.Parameters.AddWithValue("@fromValue", fromValue);
                    command.Parameters.AddWithValue("@toSystem", toSystem);
                    command.Parameters.AddWithValue("@toField", toField);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["toValue"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                throw;
            }
            return defaultValue;
        }

        public static void ArchiveMessage(string interchageId, string messageId, string receivePortName, string strMessageContext, string strMessageContent)
        {
            try
            {
                var queryString = "INSERT INTO [BizTalkResources].[dbo].[ArchivedMessages] (interchangeId, messageId, receivePortName, messageContext, messageContent) " +
                                  "VALUES (@interchageId,@messageId,@receivePortName,@messageContext,@messageContent)";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@interchageId", interchageId+"");
                    command.Parameters.AddWithValue("@messageId", messageId+"");
                    command.Parameters.AddWithValue("@receivePortName", receivePortName+"");
                    command.Parameters.AddWithValue("@messageContext", strMessageContext+"");
                    command.Parameters.AddWithValue("@messageContent", strMessageContent+"");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                throw;
            }
        }
    }
}
