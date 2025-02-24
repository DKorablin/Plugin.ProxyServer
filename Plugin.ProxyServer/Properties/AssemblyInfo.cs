using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("0529e909-b42f-43c9-80a6-91d8d054a84a")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.ProxyServer")]
#else

[assembly: AssemblyDescription("HTTP(S) proxy server plugin based on Titanium.Web.Proxy")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2018-2024")]
#endif

/*if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe /targetplatform:v4  "/out:$(ProjectDir)..\bin\$(TargetFileName)" "$(TargetDir)$(TargetFileName)" "$(TargetDir)Titanium.Web.Proxy.dll" "$(TargetDir)BouncyCastle.Crypto.dll" "$(TargetDir)StreamExtended.dll" "/lib:..\..\..\SAL\bin"
)*/