using System;

namespace EnterSentials.Framework
{
    public class TableFromTypeNameAttribute : System.ComponentModel.DataAnnotations.Schema.TableAttribute
    {
        public TableFromTypeNameAttribute(Type typeToDeriveTableNameFrom) : base(typeToDeriveTableNameFrom.Name)
        { }
    }
}
