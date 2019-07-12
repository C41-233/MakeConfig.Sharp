import os
import subprocess

csc_path = r'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe'
if not os.path.exists(csc_path):
	csc_path = r'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
if not os.path.exists(csc_path):
	csc_path = r'C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe'

option = r' /nostdlib /noconfig /warnaserror /r:C:\Windows\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll /r:C:\Windows\Microsoft.NET\Framework\v2.0.50727\System.dll /r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll" /nologo /debug- /o /target:library /out:ConfigData.dll /langversion:7.2 *.cs'

cmd = '"' + csc_path + '"' + option

subprocess.call(cmd, shell=True)

print(csc_path)