/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils.UI.Interfaces
{
    public interface IAboutModel
    {
        string AppCompany { get; set; }
        string AppCopyright { get; set; }
        string AppDescription { get; set; }
        string AppProduct { get; set; }
        string AppVersion { get; set; }
        int VersionMajor { get; set; }
        int VersionMinor { get; set; }
        int VersionPatch { get; set; }
    }
}
