using UnityEngine;

namespace Wither.Utils
{
    public static class Extensions
    {
        public static string ToHtmlString(this Color color)
        {
            string html = $"#{ToByte(color.r):X2}{ToByte(color.g):X2}{ToByte(color.b):X2}{ToByte(color.a):X2}";
            return html;
        }

        public static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte) (f * 255);
        }
    }
}