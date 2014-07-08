using EmitMapper;
using System;

namespace EnterSentials.Framework.EmitMapper
{
    internal class EmitMapperBasedObjectCopier<TFrom, TTo> : IObjectCopier<TFrom, TTo>
    {
        private readonly Type fromType = typeof(TFrom);
        private readonly Type toType = typeof(TTo);
        private readonly ObjectsMapper<TFrom, TTo> mapper = null;
        
        public Type FromType { get { return fromType; } }
        public Type ToType { get { return toType; } }
        

        public void Copy(TFrom from, TTo to)
        { mapper.Map(from, to); }

        public void Copy(object from, object to)
        { Copy((TFrom)from, (TTo)to); }


        public EmitMapperBasedObjectCopier(ObjectsMapper<TFrom, TTo> mapper)
        {
            Guard.AgainstNull(mapper, "mapper");
            this.mapper = mapper;
        }
    }
}
