using System;
using System.ServiceModel.Configuration;

namespace EnterSentials.Framework.Services.WCF
{
    public class EnableCorsBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        { get { return typeof(EnableCorsAttribute); } }

        protected override object CreateBehavior()
        { return new EnableCorsAttribute(); }
    }
}