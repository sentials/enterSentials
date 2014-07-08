using System;

namespace EnterSentials.Framework
{
    public abstract class NameKeyedBusinessObjectBase : DataTransferObjectBase
    {
        [NonSerialized]
        private readonly Func<string> nameProvider;

        private string name = null;

        [Id]
        public string Name
        { 
            get { return nameProvider == null ? name : nameProvider(); }
            set { name = value; }
        }


        public NameKeyedBusinessObjectBase(Func<string> nameProvider)
        {
            Guard.AgainstNull(nameProvider, "nameProvider");
            this.nameProvider = nameProvider;
        }

        public NameKeyedBusinessObjectBase()
        { }
    }
}