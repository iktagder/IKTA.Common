using System;
using log4net.Ext.Serializable;
using log4net.helpers;
using System.Security.Principal;
using Microsoft.Win32;

namespace IKTA.Common.Components.Helpers
{
    public class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new Object();
        private SLog logger;

        private Logger()
        {
            Getlog4netConfigLocation("IKTA.Common");
            // Create the logger using the ProjectName property value from the .btdfproj
            logger = SLogManager.GetLogger("IKTA.Common", CallersTypeName.Name);
  
            // Configure the logger by referencing the registry key written during the deployment process
            logger.RegistryConfigurator();


        }

        private static string Getlog4netConfigLocation(string registryPath)
        {
            RegistryKey rk = Registry.LocalMachine;
            string location = string.Empty;
            try
            {
                location = (string)rk.OpenSubKey(@"SOFTWARE\" + registryPath).GetValue("log4netConfig");
            }
            catch(Exception e)
            {
                var i = 0;
            }

            return location;
        }

        private static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }

        public static SLog GetLogger()
        {
            return SLogManager.GetLogger("IKTA.Common", CallersTypeName.Name);
        }

        public static void LogMessage(string interchangeId, string message)
        {
            PropertiesCollectionEx logProperties = new PropertiesCollectionEx();
            logProperties.Set("BTS.InterchangeID", interchangeId);
            Instance.logger.Debug(logProperties, message);
        }

        public static void LogMessage(string message)
        {
            Instance.logger.Debug(message);
        }

        public static void LogWarning(string interchangeId, string message)
        {
            PropertiesCollectionEx logProperties = new PropertiesCollectionEx();
            logProperties.Set("BTS.InterchangeID", interchangeId);
            Instance.logger.Warn(logProperties, message);
        }

        public static void LogWarning(string message)
        {
            Instance.logger.Warn(message);
        }

        public static void LogWarning(string message, Exception exception)
        {
            Instance.logger.Warn(message, exception);
        }

        public static void LogWarning(string interchangeId, string message, Exception exception)
        {
            PropertiesCollectionEx logProperties = new PropertiesCollectionEx();
            logProperties.Set("BTS.InterchangeID", interchangeId);
            Instance.logger.Warn(logProperties, message, exception);
        }

        public static void LogError(string interchangeId, string message)
        {
            PropertiesCollectionEx logProperties = new PropertiesCollectionEx();
            logProperties.Set("BTS.InterchangeID", interchangeId);
            Instance.logger.Error(logProperties, message);
        }

        internal static void LogError(string message)
        {
            Instance.logger.Error(message);
        }

        public static void LogError(string interchangeId, string message, Exception exception)
        {
            PropertiesCollectionEx logProperties = new PropertiesCollectionEx();
            logProperties.Set("BTS.InterchangeID", interchangeId);
            Instance.logger.Error(logProperties, message, exception);
        }

        public static void LogError(string message, Exception exception)
        {
            Instance.logger.Error(message, exception);
        }
    }

}
