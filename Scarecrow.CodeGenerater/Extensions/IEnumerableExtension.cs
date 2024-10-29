using System.Text;

namespace Scarecrow.CodeGenerater;

public static class IEnumerableExtension
{
    public static string ToName(this List<string> @this) => string.Join("", @this);

    public static string ToNameFirstLower(this List<string> @this)
    {
        var sb = new StringBuilder(@this.First().ToLower());
        for (var i = 1; i < @this.Count(); i++)
        {
            sb.Append(@this[i]);
        }
        return sb.ToString();
    }
}
