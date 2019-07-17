import os
import subprocess

search_path = [
	r'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\Roslyn\csc.exe',
	r'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csc.exe',
	r'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe',
	r'C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe',
]

csc_path = ""
for path in search_path:
	if os.path.exists(path):
		csc_path = path
		break

option = r' /nostdlib /noconfig /warnaserror /r:C:\Windows\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll /r:C:\Windows\Microsoft.NET\Framework\v2.0.50727\System.dll /r:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll" /r:../BaseType/bin/BaseType.dll /nologo /debug- /o /target:library /out:bin/ConfigData.dll /langversion:7.2 AutoGen/*.cs'

cmd = '"' + csc_path + '"' + option

print(cmd)

subprocess.call(cmd, shell=True)

os.system("pause")