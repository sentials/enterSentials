using System;
using Id = System.Int32;

namespace EnterSentials.Framework
{
    public abstract class AuditabledBusinessObjectBase<TKey> : BusinessObjectBase<TKey>, IAuditable
    {
        public bool IsActive { get; set; }


        public AuditabledBusinessObjectBase(Func<TKey> idProvider) : base(idProvider)
        { }

        public AuditabledBusinessObjectBase()
        { }
    }


    public abstract class AuditableBusinessObjectBase : BusinessObjectBase, IAuditable
    {
        public bool IsActive { get; set; }


        public AuditableBusinessObjectBase(Func<Id> idProvider) : base(idProvider)
        { }

        public AuditableBusinessObjectBase()
        { }
    }
}