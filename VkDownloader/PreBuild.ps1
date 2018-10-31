Param(
    [string] $appId
 )

[string]$original=
@'
namespace VkDownloader.Desktop.Settings
{{
    public partial class Settings
    {{
        string _appId = "{0}";
    }}
}}
'@

$original -f $appId | Out-File -FilePath "VkDownloader.Desktop\RealAppId.cs"