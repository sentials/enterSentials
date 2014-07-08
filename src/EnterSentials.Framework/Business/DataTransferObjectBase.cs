using System;

namespace EnterSentials.Framework
{
    public abstract class DataTransferObjectBase : IDataTransferObject
    {
        public DateTime CreatedOn { get; set; }
    }
}