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

namespace IKTA.Common.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("E2143B2D-4AFC-46C7-B1F0-3D711EFE0457")]
    [ComponentCategory(CategoryTypes.CATID_Validate)]
    public class APIAccessValidator :
      Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent,
      IPersistPropertyBag, IComponentUI
    {
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get { return "API Access Validator"; }
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
            get { return "Verifies that the caller has access to current receive port."; }
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
            classid = new Guid("E2143B2D-4AFC-46C7-B1F0-3D711EFE0457");
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
            val = ReadPropertyBag(pb, "IsActive");
            if ((val != null))
            {
                _isActive = ((bool)(val));
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
            WritePropertyBag(pb, "IsActive", IsActive);
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
        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            if (this.IsActive)
            {
                IBaseMessageContext myContext = pInMsg.Context;
                var inboundHttpHeaders = (string)pInMsg.Context.Read("InboundHttpHeaders", "http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties");
                if (null == inboundHttpHeaders)
                {
                    throw new System.Security.SecurityException("Access to service denied.");
                }

                try
                {
                    var apiKey = Components.Helpers.Security.GetApiKeyFromHttpHeaders(inboundHttpHeaders);
                    var receivePortName = (string)pInMsg.Context.Read("ReceivePortName", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
                    var isValidService = Components.Helpers.Security.IsValidService(apiKey, receivePortName);

                    if (!isValidService)
                    {
                        throw new Exception("Access to service denied.");
                    }
                }
                catch (Exception e)
                {
                    throw new System.Security.SecurityException("Access to service denied.");
                }
            }

            return pInMsg;
        }
        #endregion
    }
}
