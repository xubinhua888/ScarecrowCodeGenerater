using System.Text;

namespace Scarecrow.CodeGenerater;

internal static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this);

    public static bool IsNotNullAndWhiteSpace(this string @this) => !string.IsNullOrWhiteSpace(@this);

    public static List<string> SpitName(this string @this)
    {
        if (@this.Contains("_"))
        {
            var arr = @this.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            return arr.Select(a => $"{a.Substring(0, 1).ToUpper()}{a.Substring(1).ToLower()}").ToList();
        }
        else
        {
            var arr = new List<string>();
            var sb = new StringBuilder(@this[0].ToString());
            var previousChar = @this[0];
            var currentChar = @this[0];
            for (var i = 1; i < @this.Length; i++)
            {
                currentChar = @this[i];
                if ((currentChar >= 64 && currentChar <= 90) && (previousChar >= 64 && previousChar <= 90) == false)
                {
                    arr.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append(currentChar);
                previousChar = currentChar;
            }
            if (sb.Length > 0)
            {
                arr.Add(sb.ToString());
            }
            return arr.Select(a => $"{a.Substring(0, 1).ToUpper()}{a.Substring(1).ToLower()}").ToList();
        }
    }
}
