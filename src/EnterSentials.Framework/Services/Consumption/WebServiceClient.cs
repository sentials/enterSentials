using System;
using System.Net;

namespace EnterSentials.Framework
{
    public abstract class WebServiceClient : ScopeLimitedObject, IWebServiceClient
    {
        private WebClient client = null;


        protected abstract string ContentType { get; }
        protected abstract string SerializeData(object data);
        protected abstract TResponse Deserialize<TResponse>(string response);


        protected virtual void SetContentTypeHeader(WebHeaderCollection headers)
        { headers[HttpRequestHeader.ContentType] = ContentType; }

        protected virtual void InitializeHeaders(WebHeaderCollection headers)
        { SetContentTypeHeader(headers); }


        private bool TryDeserialize<TResponse>(string response, out TResponse deserializedResponse)
        {
            var couldOrNot = false;
            deserializedResponse = default(TResponse);
            try
            {
                deserializedResponse = Deserialize<TResponse>(response);
                couldOrNot = true;
            }
            catch
            { }

            return couldOrNot;
        }


        public bool Get<TResponse>(string addressWithQueryParameters, out TResponse deserializedResponse)
        { 
            var response = client.DownloadString(addressWithQueryParameters);
            return TryDeserialize<TResponse>(response, out deserializedResponse);
        }

        public bool Get<TResponse>(Uri uri, out TResponse deserializedResponse)
        {
            var response = client.DownloadString(uri);
            return TryDeserialize<TResponse>(response, out deserializedResponse);
        }


        public bool Post<TResponse>(string address, object data, out TResponse deserializedResponse)
        { 
            var response = client.UploadString(address, SerializeData(data));
            return TryDeserialize<TResponse>(response, out deserializedResponse);
        }

        public bool Post<TResponse>(Uri uri, object data, out TResponse deserializedResponse)
        {
            var response = client.UploadString(uri, SerializeData(data));
            return TryDeserialize<TResponse>(response, out deserializedResponse);
        }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            client.Dispose();
            client = null;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage", 
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification="This is done safely and is the desired extensibility model."
        )]
        public WebServiceClient()
        {
            client = new WebClient();
            InitializeHeaders(client.Headers);
        }

    }
}
