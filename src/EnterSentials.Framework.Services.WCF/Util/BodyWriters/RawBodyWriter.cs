using System.ServiceModel.Channels;
using System.Xml;

namespace EnterSentials.Framework.Services.WCF
{
    public class RawBodyWriter : BodyWriter
    {
        public const string BodyFormat = "Binary";


        private readonly byte[] content;


        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(BodyFormat);
            writer.WriteBase64(content, 0, content.Length);
            writer.WriteEndElement();
        }


        public RawBodyWriter(byte[] content) : base(true)
        {
            Guard.AgainstNull(content, "content");
            this.content = content; 
        }
    } 
}
