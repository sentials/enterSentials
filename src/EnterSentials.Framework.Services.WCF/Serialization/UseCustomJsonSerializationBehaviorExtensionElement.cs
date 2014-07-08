using System;
using System.ServiceModel.Configuration;

namespace EnterSentials.Framework.Services.WCF
{
    public class UseCustomJsonSerializationBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        { get { return typeof(CustomJsonSerializationWebHttpBehavior); } }

        protected override object CreateBehavior()
        { return new CustomJsonSerializationWebHttpBehavior(); }
    }
}
