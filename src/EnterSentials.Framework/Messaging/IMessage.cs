using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IMessage
    {
        string SourceDomain
        { get; }

        string Id
        { get; }

        //long SequenceNum
        //{ get; }

        IDictionary<string, object> Properties
        { get; }
        
        T GetBody<T>();
    }
}
