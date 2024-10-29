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
        {"byte",typeof(byte) },
        {"sbyte",typeof(sbyte) },
        {"int",typeof(int) },
        {"uint",typeof(uint) },
        {"long",typeof(long) },
        {"ulong",typeof(ulong) },
        {"short",typeof(short) },
        {"ushort",typeof(ushort) },
        {"bool",typeof(bool) },
        {"float",typeof(float) },
        {"double",typeof(double) },
        {"decimal",typeof(decimal) },
        {"DateTime",typeof(DateTime) },
        {"char",typeof(char) },
        {"string",typeof(string) },
        {"Guid",typeof(Guid) },
        {"byte[]",typeof(byte[]) },
        {"object",typeof(object) },
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
