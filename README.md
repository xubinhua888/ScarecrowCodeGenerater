<h1 align="center"> 基于Razor和SqlSugar实现的代码生成器 </h1>

- 创建Scarecrow.CodeGenerater.exe快捷方式，并添加锁定到任务栏
- 修改快捷方式的目标：如F:\CodeGenerater\Scarecrow.CodeGenerater.exe “F:\WorkSpace\TMS\CodeGenerater.json”
- 每次从任务栏运行一下代码生成器，代码将直接生成到项目中

> CodeGeneraterConfig.json
```json
{
    "DbType": "MySql",//参考SqlSugar.DbType
    "ConnectionString": "数据库链接",
    "TableNames": "有值则只处理配置的表，多个表用,或空格分隔。为空由生成所有表",
    "TypeMapFile": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\MysqlTypeMap.json",//数据库类型和C#类型的对应关系
    "IgnoreTablePrefix": [ "cap_", "cap." ],//忽略表名以这些字符开头的表
    "Templates": [
        {
            "OutPath": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.Service\\_Service",//文件输出目录，可以直接配置项目的目录
            "RazorTemplate": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.Service\\_Service.template",//razor模板
            "FileNameFormat": "{0}Service.cs",//文件名模板,{0}表名（UpperCamelCase）
            "SubfolderDepth": "1",//为0时所有的文件生成到OutPath目录，为1时在OutPath创建子目录（子目录名为表名UpperCamelCase后的第一个单词）
            "OverrideOldFile": false//生成的目标文件存在时是否覆盖文件
        },
        {
            "OutPath": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.IService\\_IService",
            "RazorTemplate": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.IService\\_IService.template",
            "FileNameFormat": "I{0}Service.cs",
            "SubfolderDepth": "1",
            "OverrideOldFile": false
        },
        {
            "OutPath": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.Model\\_Model",
            "RazorTemplate": "F:\\CodeGenerater\\Scarecrow.CodeGenerater\\Scarecrow.Model\\_Model.template",
            "FileNameFormat": "{0}Model.cs",
            "SubfolderDepth": "1",
            "OverrideOldFile": true
        }
    ]
}

```
