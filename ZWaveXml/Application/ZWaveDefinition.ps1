# SPDX-License-Identifier: BSD-3-Clause
# SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
&("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\xsd.exe") ZWaveDefinition.xsd /c /l:CS /edb /n:ZWave.Xml.Application

$content = get-content .\ZWaveDefinition.cs
$content `
| where-object {$_ -notmatch '//'} `
| where-object {$_ -notmatch 'System.Diagnostics.DebuggerStepThroughAttribute'} `
| where-object {$_ -notmatch 'System.ComponentModel.DesignerCategoryAttribute'} `
| where-object {$_ -notmatch 'System.CodeDom.Compiler.GeneratedCodeAttribute'} `
| set-content ZWaveDefinition.cs

$file = gci "ZWaveDefinition.cs"
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
$cont | foreach {$_ -replace "BasicDevice\[\]", "Collection<BasicDevice>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "SpecificDevice\[\]", "Collection<SpecificDevice>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "GenericDevice\[\]", "Collection<GenericDevice>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "SupportedCommandClass\[\]", "Collection<SupportedCommandClass>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "CommandClass\[\]", "Collection<CommandClass>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Command\[\]", "Collection<Command>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Param\[\]", "Collection<Param>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Define\[\]", "Collection<Define>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "DefineSet\[\]", "Collection<DefineSet>"} | set-content $str
$cont = get-content -path $str
$cont | foreach {$_ -replace "Mapping\[\]", "Collection<Mapping>"} | set-content $str
}
