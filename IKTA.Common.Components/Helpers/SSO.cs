using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKTA.Common.Components.Helpers
{
    public class SSO
    {
        public static string GetSSOString(string applicationName, string valueName)
        {
            return SSOSettingsFileManager.SSOSettingsFileReader.ReadString(applicationName, valueName);
        }

        public static int GetSSOInt32(string applicationName, string valueName)
        {
            return SSOSettingsFileManager.SSOSettingsFileReader.ReadInt32(applicationName, valueName);
        }
    }
}
