using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.HelperMethods
{
    public static class FloatExtension
    {
        public static float GreatestValue(this float current, float other)
        {
            if (current >= other)
            {
                return current;
            }
            
            return other;
        }

        public static float SmallestValue(this float current, float other)
        {
            if (current <= other)
            {
                return current;
            }

            return other;
        }
    }
}
