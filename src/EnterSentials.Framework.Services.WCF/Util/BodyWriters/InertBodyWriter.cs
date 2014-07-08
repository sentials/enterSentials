using System.ServiceModel.Channels;
using System.Xml;

namespace EnterSentials.Framework.Services.WCF
{
    public class InertBodyWriter : BodyWriter
    {
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        { }

        public InertBodyWriter() : base(true)
        { }
    }
}
