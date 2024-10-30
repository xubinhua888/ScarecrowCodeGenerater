using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices;
using System;

namespace Scarecrow.CodeGenerater;

public class DbTableModel : DbTableInfo
{
    private List<string> _nameWord = null;

    public void Init()
    {
        if (_nameWord == null)
        {
            _nameWord = Name.SpitName();
            NameFirstUpper = _nameWord.ToName();
            NameFirstLower = _nameWord.ToNameFirstLower();
        }
    }

    public string NameFirstUpper { get; set; }

    public string NameFirstLower { get; set; }

    public List<string> GetPrefix(int depth) => _nameWord.Take(depth).ToList();

    public List<DbColumnModel> Columns { get; set; } = [];
}

public class DbColumnModel : DbColumnInfo
{
    private static Dictionary<string, Type> _csharpType = new()
    {
        {"int",typeof(int) },
        {"bool",typeof(bool) },
        {"string",typeof(string) },
        {"DateTime",typeof(DateTime) },
        {"decimal",typeof(decimal) },
        {"double",typeof(double) },
        {"Guid",typeof(Guid) },
        {"byte",typeof(byte) },
        {"sbyte",typeof(sbyte) },
        {"Enum",typeof(Enum) },
        {"short",typeof(short) },
        {"long",typeof(long) },
        {"object",typeof(object) },
        {"byte[]",typeof(byte[]) },
        {"float",typeof(float) },
        {"uint",typeof(uint) },
        {"ulong",typeof(ulong) },
        {"ushort",typeof(ushort) },
        {"char",typeof(char) },
        {"DateTimeOffset",typeof(DateTimeOffset) },
        {"TimeSpan",typeof(TimeSpan) }
    };
    private List<string> _nameWord = null;

    public void Init()
    {
        if (_nameWord == null)
        {
            _nameWord = DbColumnName.SpitName();
            NameFirstUpper = _nameWord.ToName();
            NameFirstLower = _nameWord.ToNameFirstLower();
        }
    }

    public string NameFirstUpper { get; set; }

    public string NameFirstLower { get; set; }

    public string CSharpTypeName { get; set; }

    public Type CSharpType
    {
        get
        {
            if (CSharpTypeName.IsNullOrWhiteSpace() || _csharpType.ContainsKey(CSharpTypeName))
            {
                return Type.Missing.GetType();
            }
            return _csharpType[CSharpTypeName];
        }
    }
}
