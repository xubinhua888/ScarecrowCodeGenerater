using Mapster;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using Spectre.Console;

namespace Scarecrow.CodeGenerater;

internal sealed class CodeGeneraterCommand : Command<CodeGeneraterCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<config file path>")]
        public string ConfigFile { get; set; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (File.Exists(settings.ConfigFile) == false)
        {
            return Program.Error($"指定的配置文件不存在：[{settings.ConfigFile}]");
        }

        var configJson = File.ReadAllText(settings.ConfigFile, System.Text.Encoding.UTF8);
        if (configJson.IsNullOrWhiteSpace())
        {
            return Program.Error($"指定的配置文件为空文件：[{settings.ConfigFile}]");
        }

        try
        {
            var config = JsonConvert.DeserializeObject<GeneraterConfig>(configJson);
            var tableList = SqlSugarHelper.GetTableList(config);

            Engine.Razor = RazorEngineService.Create(new TemplateServiceConfiguration()
            {
                EncodedStringFactory = new RawStringFactory()
            });

            Table dt = new Table();
            dt.AddColumn("TableName");
            foreach (var item in config.Templates)
            {
                dt.AddColumn(item.FileNameFormat.Replace("{0}", ""));
                item.RazorID = Guid.NewGuid().ToString("N");
                Engine.Razor.Compile(File.ReadAllText(item.RazorTemplate, System.Text.Encoding.UTF8), item.RazorID);
            }
            foreach (var table in tableList)
            {
                var values = new List<string>() { table.Name };
                foreach (var item in config.Templates)
                {
                    values.Add(GenerateCode(item, table));
                }
                dt.AddRow(values.ToArray());
            }
            AnsiConsole.Write(dt);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return Program.Exit();
        }

        return Program.Exit();
    }

    private string GenerateCode(TemplateConfig template, DbTableModel table)
    {
        var dir = template.OutPath;
        if (template.SubfolderDepth > 0)
        {
            dir = Path.Combine(dir, string.Join("\\", table.GetPrefix(template.SubfolderDepth)));
        }
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        var fileName = Path.Combine(dir, string.Format(template.FileNameFormat, table.NameFirstUpper));
        if (File.Exists(fileName) && template.OverrideOldFile == false)
        {
            return "[green]已存在[/]";
        }
        var sw = new StringWriter();
        Engine.Razor.Run(template.RazorID, sw, null, table);
        File.WriteAllText(fileName, sw.ToString());
        return "[blue]已生成[/]";
    }
}
