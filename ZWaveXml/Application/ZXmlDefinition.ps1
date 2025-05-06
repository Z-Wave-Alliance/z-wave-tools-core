# SPDX-License-Identifier: BSD-3-Clause
# SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
&("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe") ZXmlDefinition.xsd /c /l:CS /edb /n:ZWave.Xml.Application /nologo

$content = get-content .\ZXmlDefinition.cs
$content `
| where-object {$_ -notmatch '//'} `
| where-object {$_ -notmatch 'System.Diagnostics.DebuggerStepThroughAttribute'} `
| where-object {$_ -notmatch 'System.ComponentModel.DesignerCategoryAttribute'} `
| where-object {$_ -notmatch 'System.CodeDom.Compiler.GeneratedCodeAttribute'} `
| set-content ZXmlDefinition.cs

$file = gci "ZXmlDefinition.cs"
foreach ($str in $file) 
{
$cont = get-content -path $str
$cont | foreach {$_ -replace "using System.Xml.Serialization;", "using System.Xml.Serialization;
    using System.Collections.ObjectModel;
    using System.ComponentModel;"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "System.Xml.Serialization\.", ""} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "System.ComponentModel\.", ""} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Attribute\(\)", ""} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Attribute\(", "("} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "this\.", ""} | set-content $str


$cont = get-content -path $str
$cont | foreach {$_ -replace "object\[\]", "Collection<object>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "cmd\[\]", "Collection<cmd>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "fieldenum\[\]", "Collection<fieldenum>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "param\[\]", "Collection<param>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "spec_dev\[\]", "Collection<spec_dev>"} | set-content $str
}
