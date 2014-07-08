using System;
using Id = System.Int32;

namespace EnterSentials.Framework
{
    public abstract class BusinessObjectBase<TKey> : DataTransferObjectBase
    {
        [NonSerialized]
        private readonly Func<TKey> idProvider;

        private TKey id = default(TKey);

        [Id]
        public TKey Id
        { 
            get { return idProvider == null ? id : idProvider(); }
            set { id = value; }
        }


        public BusinessObjectBase(Func<TKey> idProvider)
        {
            Guard.AgainstNull(idProvider, "idProvider");
            this.idProvider = idProvider;
        }

        public BusinessObjectBase()
        { }
    }


    public abstract class BusinessObjectBase : BusinessObjectBase<Id>
    {
        public BusinessObjectBase(Func<Id> idProvider) : base(idProvider)
        { }

        public BusinessObjectBase()
        { }
    }
}