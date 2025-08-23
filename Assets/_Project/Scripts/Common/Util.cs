// Ivan Postarnak
// https://github.com/IvanPostarnak/cup-heroes-clone

namespace CupHeroesClone.Common
{
    public class Util
    {
        public static T Clamp<T>(T value, T min, T max) where T : System.IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
        
        public static float Clamp01(float value, float min, float max)
        {
            float value0100 = (value - min) / ((max - min) / 100f);
            float value01 = value0100 / 100f;
            return value01;
        }
        
        public static string StringFromNumber(float number)
        {
            return number
                .ToString("#,0", System.Globalization.CultureInfo.InvariantCulture)
                .Replace(',', ' ');
        }
    }
}