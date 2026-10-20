// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Silicon Laboratories Inc. <https://www.silabs.com>
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.Reflection;
using System.Runtime.CompilerServices;
using Utils;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Utils")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ToolsCoreVersionInfo.COMPANY_NAME)]
[assembly: AssemblyProduct(ToolsCoreVersionInfo.PRODUCT)]
[assembly: AssemblyCopyright(ToolsCoreVersionInfo.COPYRIGHT)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion(ToolsCoreVersionInfo.VERSION)]
[assembly: AssemblyFileVersion(ToolsCoreVersionInfo.VERSION)]

// The J-Link board locator internals are unit tested from UtilsTests.
[assembly: InternalsVisibleTo("UtilsTests")]
