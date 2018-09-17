using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace IKTA.Common.Components.WCFBehaviors
{
    class SanitizedFaultBehaviour : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels. BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            return;
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            SanitizedFaultHandler handler = new SanitizedFaultHandler();
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(handler);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }
    }
}
