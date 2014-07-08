using System;

namespace EnterSentials.Framework
{
    public abstract class AuditableNameKeyedBusinessObjectBase : DataTransferObjectBase, IAuditable
    {
        [NonSerialized]
        private readonly Func<string> nameProvider;

        private string name = null;

        public string Name
        { 
            get { return nameProvider == null ? name : nameProvider(); }
            set { name = value; }
        }


        public bool IsActive { get; set; }


        public AuditableNameKeyedBusinessObjectBase(Func<string> nameProvider)
        {
            Guard.AgainstNull(nameProvider, "nameProvider");
            this.nameProvider = nameProvider;
        }

        public AuditableNameKeyedBusinessObjectBase()
        { }
    }
}