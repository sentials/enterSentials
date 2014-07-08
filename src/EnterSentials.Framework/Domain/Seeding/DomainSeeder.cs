using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class DomainSeeder<TEntity>
    {
        protected virtual void BeforeInsert()
        { }

        protected abstract void Insert(TEntity entity, bool isFirstInsert, bool isLastInsert);

        protected virtual void Insert(IEnumerable<TEntity> entities)
        {
            Guard.AgainstNull(entities, "entities");
            Guard.Against(entities, es => es.Any(e => e == null), "No provided entity can be null.", "entities");

            var lastInsertIndex = entities.Count() - 1;
            var insertIndex = 0;
            foreach (var entity in entities)
                Insert(entity, insertIndex == 0, (insertIndex++ == lastInsertIndex));
        }

        protected virtual void AfterInsert()
        { }

        public void Seed(IEnumerable<TEntity> entities)
        {
            Guard.AgainstNull(entities, "entities");

            BeforeInsert();
            Insert(entities);
            AfterInsert();
        }
    }
}