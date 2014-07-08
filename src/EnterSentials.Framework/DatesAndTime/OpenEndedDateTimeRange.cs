using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterSentials.Framework
{
    [ComplexType]
    public class OpenEndedDateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public bool IsValid
        { get { return !End.HasValue || Start <= End; } }
    }
}