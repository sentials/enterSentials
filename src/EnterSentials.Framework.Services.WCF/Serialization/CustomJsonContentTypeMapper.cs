using System.ServiceModel.Channels;

namespace EnterSentials.Framework.Services.WCF
{
    public class CustomJsonContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        { return WebContentFormat.Raw; }
    }
}