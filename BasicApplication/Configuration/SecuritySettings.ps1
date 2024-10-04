# SPDX-License-Identifier: BSD-3-Clause
# SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
&("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe") SecuritySettings.xsd /c /l:CS /edb /nologo /n:ZWave.Configuration 
$file = gci "SecuritySettings.cs"
foreach ($str in $file) 
{
$cont = get-content -path $str
$cont | foreach {$_ -replace "using System.Xml.Serialization;", "using System;`r`n    using System.ComponentModel;`r`n    using System.Xml.Serialization;`r`n    using System.Collections.ObjectModel;"} | set-content $str

$cont = get-content -path $str
$cont | where {$_ -notmatch "System.ComponentModel.DesignerCategoryAttribute"} | set-content $str

$cont = get-content -path $str
$cont | where {$_ -notmatch "System.Diagnostics.DebuggerStepThroughAttribute"} | set-content $str

$cont = get-content -path $str
$cont | where {$_ -notmatch "System.CodeDom.Compiler.GeneratedCodeAttribute"} | set-content $str

$cont = get-content -path $str
$cont | where {$_ -notmatch "/// <remarks/>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "\[System.Xml.Serialization.", "["} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "\[System.ComponentModel.", "["} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "\[System.SerializableAttribute\(\)\]", "[Serializable]"} | set-content $str

#-----------------------------------------------------------------------------------------------

$cont = get-content -path $str
$cont | foreach {$_ -replace "SecurityExtension\[\]", "Collection<SecurityExtension>"} | set-content $str
}
