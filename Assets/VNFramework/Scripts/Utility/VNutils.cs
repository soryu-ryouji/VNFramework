using UnityEngine;

namespace VNFramework
{
    public static class VNutils
    {
        public static Color StrToColor(string colorCode)
        {
            ColorUtility.TryParseHtmlString(colorCode, out Color color);
            return color;
        }
    }
}