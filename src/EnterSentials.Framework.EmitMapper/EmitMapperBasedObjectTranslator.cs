using EmitMapper;
using System;

namespace EnterSentials.Framework.EmitMapper
{
    internal class EmitMapperBasedObjectTranslator<TFrom, TTo> : IObjectTranslator<TFrom, TTo>
    {
        private readonly Type fromType = typeof(TFrom);
        private readonly Type toType = typeof(TTo);
        private readonly ObjectsMapper<TFrom, TTo> mapper = null;
        
        public Type FromType { get { return fromType; } }
        public Type ToType { get { return toType; } }
        

        public TTo Translate(TFrom from)
        { return mapper.Map(from); }

        public object Translate(object from)
        { return Translate((TFrom)from); }


        public EmitMapperBasedObjectTranslator(ObjectsMapper<TFrom, TTo> mapper)
        {
            Guard.AgainstNull(mapper, "mapper");
            this.mapper = mapper;
        }
    }
}
