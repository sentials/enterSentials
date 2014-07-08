using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterSentials.Framework
{
    [ComplexType]
    public class NullableDateTimeRange
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public double TotalHours { 
            get
            {
                return !Start.HasValue
                            ? 0.0
                            : !End.HasValue
                                ? 0.0
                                : (End.Value - Start).Value.TotalHours;
            }
        }
        public bool Contains(NullableDateTimeRange other)
        {
            if (!Start.HasValue && !End.HasValue && !other.Start.HasValue && !other.End.HasValue)
            {return false;}

            if (Start.HasValue && End.HasValue && (other.Start.HasValue || other.End.HasValue))
            {

                if (other.Start.HasValue && other.End.HasValue)
                    return other.Start >= Start && other.End <= End;

                if (other.Start.HasValue)
                    return other.Start >= Start && other.Start <= End;

                if (other.End.HasValue)
                    return other.End <= End && other.End >= Start;
            }

            if (other.Start.HasValue && other.End.HasValue && (Start.HasValue || End.HasValue))
            {
                if (Start.HasValue && End.HasValue)
                    return Start <= other.Start && End >= other.End;

                if (Start.HasValue)
                    return Start <= other.Start;

                if (End.HasValue)
                    return End >= other.End;
            }

            if (Start.HasValue && End.HasValue && other.Start.HasValue && other.End.HasValue)
            {return Start <= other.Start && End <= other.End;}

            return false;
        }
        public bool OverlapsNonInclusively(NullableDateTimeRange other)
        {
            if (!Start.HasValue && !End.HasValue && !other.Start.HasValue && !other.End.HasValue)
            { return false;}


            if (!Start.HasValue && !End.HasValue && !other.Start.HasValue && other.End.HasValue)
            { return false; }

            if (!Start.HasValue && !End.HasValue && !other.Start.HasValue && other.End.HasValue)
            { return false; }
            
            if (!Start.HasValue && End.HasValue && other.End.HasValue)
            {
                return End > other.End;
            }

            if (!other.Start.HasValue && End.HasValue && other.End.HasValue)
            {
                return End < other.End;
            }


            if (Start.HasValue && End.HasValue && (other.Start.HasValue || other.End.HasValue))
            {
                if (other.Start.HasValue)
                    return other.Start > Start && other.Start < End;

                if (other.End.HasValue)
                    return other.End < End && other.End > Start;
            }

            if (other.Start.HasValue && other.End.HasValue && (Start.HasValue || End.HasValue))
            {
                if (Start.HasValue)
                    return Start > other.Start && Start < other.End;

                if (End.HasValue)
                    return End < other.End && End > other.Start;
            }

            return false;

        }
    }
}
