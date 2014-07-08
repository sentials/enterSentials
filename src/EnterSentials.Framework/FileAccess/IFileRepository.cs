using System;

namespace EnterSentials.Framework
{
    public interface IFileRepository : IDisposable
    {
        bool Exists(Guid fileId);
        bool Exists(out Guid fileId, string fileName, params object[] keys);
        void Update(Guid fileId, byte[] fileContent);
        Guid AddOrUpdate(string fileName, byte[] fileContent, params object[] keys);
        void Remove(Guid fileId);
        byte[] Get(Guid fileId);
    }
}