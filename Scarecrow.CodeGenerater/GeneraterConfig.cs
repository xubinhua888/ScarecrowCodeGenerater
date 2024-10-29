namespace Scarecrow.CodeGenerater;

internal class GeneraterConfig
{
    /// <summary>数据库类型</summary>
    public string DbType { get; set; }
    /// <summary>数据库链接</summary>
    public string ConnectionString { get; set; }
    /// <summary>多个表用,分隔，为空由生成所有表</summary>
    public string TableNames { get; set; }
    /// <summary>类型映射文件</summary>
    public string TypeMapFile { get; set; }
    public List<string> IgnoreTablePrefix { get; set; }
    public List<string> IgnoreColumns { get; set; } = new();
    /// <summary>模板文件</summary>
    public List<TemplateConfig> Templates { get; set; }
}

internal class TemplateConfig
{
    /// <summary>代码保存目录</summary>
    public string OutPath { get; set; }
    /// <summary>模板ID</summary>
    public string RazorID { get; set; }
    /// <summary>模板路径</summary>
    public string RazorTemplate { get; set; }
    /// <summary>生成子目录的深度(1则取表名第一个单词做文件夹名，2则取前两个单词两个文件夹)</summary>
    public int SubfolderDepth { get; set; }
    /// <summary>文件名/summary>
    public string FileNameFormat { get; set; }

    /// <summary>文件存在时是否覆盖旧文件</summary>
    public bool OverrideOldFile { get; set; } = false;
}
