using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public class Time
    {
        public TimeType Type { get; private set; }
        public int Value { get; private set; }

        public Time(TimeType Type, int Value = 0)
        {
            this.Type = Type;
            this.Value = Value;
        }

        public override string ToString()
        {
            string result = string.Empty;

            switch (Type)
            {
                case TimeType.AllGame:
                    return string.Empty;
                case TimeType.Half:
                    return string.Format("{0}/2", Value);
                case TimeType.Set:
                    return string.Format("{0}/3", Value);
                case TimeType.Quarter:
                    return string.Format("{0}/4", Value);
            }

            return string.Empty;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Time time)
            {
                return Equals(time);
            }
            else return false;
        }

        public bool Equals(Time time)
        {
            return time.Type == Type && time.Value == Value;
        }
    }

    public enum TimeType
    {
        AllGame,
        Half,
        Set,
        Quarter
    }
}
