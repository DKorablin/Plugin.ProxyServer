using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("0529e909-b42f-43c9-80a6-91d8d054a84a")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.ProxyServer")]
#else

[assembly: AssemblyTitle("Plugin.ProxyServer")]
[assembly: AssemblyDescription("Http proxy server plugin based on Titanium.Web.Proxy")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.ProxyServer")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2018-2019")]
#endif

/*if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe /targetplatform:v4  "/out:$(ProjectDir)..\bin\$(TargetFileName)" "$(TargetDir)$(TargetFileName)" "$(TargetDir)Titanium.Web.Proxy.dll" "$(TargetDir)BouncyCastle.Crypto.dll" "$(TargetDir)StreamExtended.dll" "/lib:..\..\..\SAL\bin"
)*/