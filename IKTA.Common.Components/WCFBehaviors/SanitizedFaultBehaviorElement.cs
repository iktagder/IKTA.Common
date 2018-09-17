using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace IKTA.Common.Components.WCFBehaviors
{
    class SanitizedFaultBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(SanitizedFaultBehaviour);
            }
        }

        protected override object CreateBehavior()
        {
            return new SanitizedFaultBehaviour();
        }
    }
}
