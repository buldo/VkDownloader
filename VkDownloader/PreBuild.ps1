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

$original -f $env:appId | Out-File -FilePath "VkDownloader.Settings\RealAppId.cs" 