using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace IKTA.Common.Components.Helpers
{
    public class Security
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

        public  static string GetApiKeyFromHttpHeaders(string httpHeaders)
        {
            var result = "";
            using (StringReader sr = new StringReader(httpHeaders))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var keyValuePair = line.Split(':');
                    if (String.Compare(keyValuePair[0].Trim(), "X-Authorization-Key") == 0)
                    {
                        result = keyValuePair[1].Trim();
                    }
                }
            }
            return result;
        }

        public static bool IsValidService(string apiKey, string service)
        {
            string queryString =
                @"SELECT AvailableService FROM [BizTalkResources].[dbo].[APIKey_AvailableService] WHERE 
                     APIKey = @apiKey AND AvailableService = @service";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandTimeout = CommandTimeout;
                    command.Parameters.AddWithValue("@apiKey", apiKey);
                    command.Parameters.AddWithValue("@service", service);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                throw;
            }

            return false;
        }
    }
}
