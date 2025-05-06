# SPDX-License-Identifier: BSD-3-Clause
# SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
&("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe") FrameDefinition.xsd /c /l:CS /edb /n:ZWave.Xml.FrameHeader
clear-Host
$file = gci "FrameDefinition.cs"
$file
foreach ($str in $file) 
{
$cont = get-content -path $str
$cont | foreach {$_ -replace "using System.Xml.Serialization;", "using System.Xml.Serialization;
    using System.Collections.ObjectModel;"} | set-content $str


$cont = get-content -path $str
$cont | foreach {$_ -replace "BaseHeader\[\]", "Collection<BaseHeader>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "Header\[\]", "Collection<Header>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "Param\[\]", "Collection<Param>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "Validation\[\]", "Collection<Validation>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "Define\[\]", "Collection<Define>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "DefineSet\[\]", "Collection<DefineSet>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "HeaderFilter\[\]", "Collection<HeaderFilter>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "string\[\]", "Collection<string>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "RadioFrequency\[\]", "Collection<RadioFrequency>"} | set-content $str

$cont = get-content -path $str
$cont | foreach {$_ -replace "LongRangeConfig\[\]", "Collection<LongRangeConfig>"} | set-content $str

}
write-host "After `n"
$file