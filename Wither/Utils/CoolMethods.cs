using UnityEngine;
using Wither.Components.Option;

namespace Wither.Utils
{
    public static class CoolMethods
    {
        public static string GetLinkText(string link, string linkText, Color color)
        {
            return $"<link={link}><color={color.ToHtmlString()}>{linkText}</color></link>";
        }
        
        public static string GetLinkText(string link, string linkText)
        {
            return $"<link={link}><color=#FFFFFFFF>{linkText}</color></link>";
        }    
    }
}