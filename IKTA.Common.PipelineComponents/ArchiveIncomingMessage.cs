using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Messaging;
using Microsoft.BizTalk.Agent.Interop;

namespace IKTA.Common.PipelineComponents
{
    
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("EE717960-A316-445E-8A75-833D60C4BFFD")]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    public class ArchiveIncomingMessage :
      Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent,
      IPersistPropertyBag, IComponentUI
    {
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        private bool _archiveToFile;
        public bool ArchiveToFile
        {
            get { return _archiveToFile; }
            set { _archiveToFile = value; }
        }

        private bool _archiveToDb;
        public bool ArchiveToDb
        {
            get { return _archiveToDb; }
            set { _archiveToDb = value; }
        }

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get { return "Archive Message"; }
        }
        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get { return "1.0"; }
        }
        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get { return "Archives incoming message to a file or database before mapping on incoming port."; }
        }
        #endregion

        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return IntPtr.Zero;
            }
        }
        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build
        /// of a BizTalk project.
        /// </summary>
        /// <param name=”obj”>An Object containing the configuration properties</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add(“This is a compiler error”);
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion

        #region utility functions
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name=”pb”>Property bag</param>
        /// <param name=”propName”>Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (ArgumentException)
            {
                return val;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return val;
        }

        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name=”pb”>Property bag.</param>
        /// <param name=”propName”>Name of property.</param>
        /// <param name=”val”>Value of property.</param>
        private void WritePropertyBag(IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
        #endregion

        #region IPersistPropertyBag members
        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name=”classid”>
        /// Class ID of the component
        /// </param>
        public void GetClassID(out Guid classid)
        {
            classid = new Guid("EE717960-A316-445E-8A75-833D60C4BFFD");
        }
        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }
        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name=”pb”>Configuration property bag</param>
        /// <param name=”errlog”>Error status</param>
        public virtual void Load(IPropertyBag pb, int errlog)
        {
            object val = null;
            val = ReadPropertyBag(pb, "Path");
            if ((val != null))
            {
                _path = ((string)(val));
            }
            val = ReadPropertyBag(pb, "ArchiveToFile");
            if ((val != null))
            {
                _archiveToFile = ((bool)(val));
            }
            val = ReadPropertyBag(pb, "ArchiveToDb");
            if ((val != null))
            {
                _archiveToDb = ((bool)(val));
            }
        }

        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name=”pb”>Configuration property bag</param>
        /// <param name=”fClearDirty”>not used</param>
        /// <param name=”fSaveAllProperties”>not used</param>
        public virtual void Save(IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            WritePropertyBag(pb, "Path", Path);
            WritePropertyBag(pb, "ArchiveToFile", ArchiveToFile);
            WritePropertyBag(pb, "ArchiveToDb", ArchiveToDb);
        }
        #endregion

        #region IComponent members
        /// <summary>
        /// Implements IComponent.Execute method.
        /// </summary>
        /// <param name=”pc”>Pipeline context</param>
        /// <param name=”inmsg”>Input message</param>
        /// <returns>Original input message</returns>
        /// <remarks>
        /// IComponent.Execute method is used to initiate
        /// the processing of the message in this pipeline component.
        /// </remarks>
        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage inMsg)
        {
            if (_archiveToFile || _archiveToDb)
            {
                IBaseMessage passedMessage = inMsg;
                
                if (_archiveToFile)
                {
                    string archiveFileName = null;
                    // get the interchange id from the message
                    string interchangeID = (string)inMsg.Context.Read("InterchangeID",
                        "http://schemas.microsoft.com/BizTalk/2003/system-properties");

                    // if the transport type if file or ftp, get the incoming filename to use
                    // as part of the archive filename (for easier identification)
                    string filePath = null;
                    string adapterType = (string)inMsg.Context.Read("InboundTransportType",
                        "http://schemas.microsoft.com/BizTalk/2003/system-properties");
                    if (adapterType == "FILE")
                    {
                        filePath = (string)inMsg.Context.Read("ReceivedFileName",
                            "http://schemas.microsoft.com/BizTalk/2003/file-properties");
                    }
                    else if (adapterType == "FTP")
                    {
                        filePath = (string)inMsg.Context.Read("ReceivedFileName",
                            "http://schemas.microsoft.com/BizTalk/2003/ftp-properties");
                    }
                    archiveFileName = interchangeID + ".out";
                    if (filePath != null)
                    {
                        archiveFileName = System.IO.Path.GetFileName(filePath) + "_" + archiveFileName;
                    }

                    // write the archive file
                    WriteToFile(inMsg, System.IO.Path.Combine(this._path, archiveFileName));
                }
                if (_archiveToDb)
                {
                    // write the archive file
                    WriteToDb(inMsg);
                }
            }
            return inMsg;
        }
        #endregion

        private void CopyStream(Stream input, Stream output)
        {
            int BUFFER_SIZE = 4096;
            byte[] buffer = new byte[BUFFER_SIZE];
            int bytesRead;
            try
            {
                bytesRead = input.Read(buffer, 0, BUFFER_SIZE);
                while (bytesRead > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                    bytesRead = input.Read(buffer, 0, BUFFER_SIZE);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // rewind input stream
                if (input.CanSeek)
                    input.Position = 0;
            }
        }

        private void WriteToFile(IBaseMessage message, string fileName)
        {
            Stream msgStream = message.BodyPart.GetOriginalDataStream();
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                this.CopyStream(msgStream, fileStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                message.BodyPart.Data.Position = 0;
            }
        }

        private void WriteToDb(IBaseMessage message)
        {
            var strMessageContext = SerializeMessageContext(message);
            var strMessageContent = SerializeMessageContent(message);

            var interchageId = (string)message.Context.Read("InterchangeID", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
            var receivePortName = (string)message.Context.Read("ReceivePortName", "http://schemas.microsoft.com/BizTalk/2003/system-properties");

            Components.Helpers.DataBase.ArchiveMessage(interchageId, message.MessageID.ToString(), receivePortName, strMessageContext, strMessageContent);
        }

        private static IBaseMessageContext CloneMessageContext(IBaseMessageContext context)
        {
            IBaseMessageContext clonedContext = MessageFactory.CreateMessageContext();

            for (int i = 0; i < context.CountProperties; i++)
            {
                string propertyNamespace = String.Empty;
                string propertyName = String.Empty;

                object value = context.ReadAt(i, out propertyName, out propertyNamespace);

                if (context.IsPromoted(propertyName, propertyNamespace))
                    clonedContext.Promote(propertyName, propertyNamespace, value);
                else
                    clonedContext.Write(propertyName, propertyNamespace, value);
            }

            return clonedContext;
        }

        private string SerializeMessageContext(IBaseMessage message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<MessageContext>");

            var context = message.Context;
            var propertyCount = context.CountProperties;
            for (var i = 0; i< propertyCount; i++)
            {
                string propName = string.Empty;
                string nameSpace = string.Empty;

                var value = context.ReadAt(i, out propName, out nameSpace);

                sb.AppendLine("   <Property>");
                sb.AppendLine($"      <Name>{propName}</Name>");
                sb.AppendLine($"      <Namespace>{nameSpace}</Namespace>");
                sb.AppendLine($"      <Value>{value}</Value>");
                sb.AppendLine("   </Property>");
            }
            sb.AppendLine("   </MessageContext>");

            return sb.ToString();
        }

        private string SerializeMessageContent(IBaseMessage message)
        {
            Stream msgStream = message.BodyPart.GetOriginalDataStream();
            var s = message.BodyPart.Data;
            MemoryStream memStream = null;
            string msgAsString;
            try
            {
                memStream = new MemoryStream();
                message.BodyPart.Data.CopyTo(memStream);
                memStream.Position = 0;
                using (StreamReader reader = new StreamReader(memStream, Encoding.UTF8))
                {
                    msgAsString = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (memStream != null)
                    memStream.Close();
                if (message.BodyPart.Data.CanSeek)
                {
                    message.BodyPart.Data.Position = 0;
                }

            }
            return msgAsString;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        internal static IBTMessageAgentFactory MessageFactory
        {
            get
            {
                return (((IBTMessageAgent)new BTMessageAgent()) as IBTMessageAgentFactory);
            }
        }
    }
}
