Param(
    [string] $appId
 )

[string]$original=
@'
namespace VkDownloader.Settings
{{
    public partial class Settings
    {{
        string _appId = "{0}";
    }}
}}
'@

$original -f $appId | Out-File -FilePath "VkDownloader.Settings\RealAppId.cs" 