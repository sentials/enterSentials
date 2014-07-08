using System;

namespace EnterSentials.Framework
{
    public interface IWebServiceClient : IDisposable
    {
        bool Get<TResponse>(string addressWithQueryParameters, out TResponse deserializedResponse);
        bool Get<TResponse>(Uri uri, out TResponse deserializedResponse);
        bool Post<TResponse>(string address, object data, out TResponse deserializedResponse);
        bool Post<TResponse>(Uri uri, object data, out TResponse deserializedResponse);
    }
}
