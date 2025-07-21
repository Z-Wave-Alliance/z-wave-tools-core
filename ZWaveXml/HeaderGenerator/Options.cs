/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.Xml.HeaderGenerator
{
    public class Options
    {
        /// <summary> 
        /// ctor, initializes hardcoded values
        /// values are overwritten by LoadOptions method
        /// </summary>
        public Options()
        {
            CsOutputFileLocation = string.Empty;
            CsDefaultFileName = "ZCmdClass.cs";

            ChTemplateFile = "\\zw_cheadertemplate.h";
            ChOutputFileLocation = string.Empty;
            ChDefaultFileName = "ZW_classcmd.h";

            XmlSchemaFile = "\\sys_cmd_classes.xsd";

            FileInitialDirectory = string.Empty;

            ChCommandClassPrefix = "COMMAND_CLASS_";
            ChCommandClassVersionPostfix = "_VERSION";
            ChGenericDeviceNamePrefix = "GENERIC_TYPE_";
            ChMaskPostfix = "_MASK";
            ChShiftPostfix = "_SHIFT";
            ChBitPostfix = "_BIT";
            ChEncapsulatedCommandIdentifier = "_ENCAP";

            CsSpecificTypeEnumComment = "is specified by the following enums";
            CsSpecificTypeEnumNotUsedComment = "SPECIFIC_TYPE_NOT_USED = 0x00";
            CsSpecificTypeEnumNotUsed = "SPECIFIC_TYPE_NOT_USED = 0x00";
            CsGenericDeviceNamePrefix = "GENERIC_TYPE_";
            CsCommandClassComment = "Command Class";
            CsCommandClassVersionComment = "Command class version";
            CsCommandClassVersionConstPostfix = "_VERSION";
            CsCommandEnumListComment = "Valid commands for this command class";
            CsCommandClassPrefix = "COMMAND_CLASS_";
            CsCommandClassNamePrefix = "cmd";
        }

        /// <summary> 
        /// Location of generated C# class
        /// </summary>
        public string CsOutputFileLocation { get; set; }

        /// <summary> 
        /// Default file name of generated C header
        /// </summary>
        public string CsDefaultFileName { get; set; }

        /// <summary> 
        /// C header template file (path included)
        /// </summary>
        public string ChTemplateFile { get; set; }

        /// <summary> 
        /// Location of generated C header file
        /// </summary>
        public string ChOutputFileLocation { get; set; }

        /// <summary> 
        /// Default file name of generated C header
        /// </summary>
        public string ChDefaultFileName { get; set; }

        /// <summary>
        /// Path and file name of XML Schema use for validation of the current XML
        /// </summary>
        public string XmlSchemaFile { get; set; }

        /// <summary>
        /// Initial directory for the file menu
        /// </summary>
        public string FileInitialDirectory { get; set; }

        /// <summary>
        /// Command Class prefix.
        /// </summary>
        public string ChCommandClassPrefix { get; set; }

        /// <summary>
        /// Command Class version postfix.
        /// </summary>
        public string ChCommandClassVersionPostfix { get; set; }

        /// <summary>
        /// Generic Device Name Prefix.
        /// </summary>
        public string ChGenericDeviceNamePrefix { get; set; }

        /// <summary>
        /// Mask Postfix.
        /// </summary>
        public string ChMaskPostfix { get; set; }

        /// <summary>
        /// Shift Postfix.
        /// </summary>
        public string ChShiftPostfix { get; set; }

        /// <summary>
        /// Bit postfix.
        /// </summary>
        public string ChBitPostfix { get; set; }

        /// <summary>
        /// Encapsulated Command Identifier.
        /// </summary>
        public string ChEncapsulatedCommandIdentifier { get; set; }

        /// <summary>
        /// Specific type enum comment
        /// </summary>
        public string CsSpecificTypeEnumComment { get; set; }

        /// <summary>
        /// Specific type enum not used comment
        /// </summary>
        public string CsSpecificTypeEnumNotUsedComment { get; set; }

        /// <summary>
        /// Specific type enum not used
        /// </summary>
        public string CsSpecificTypeEnumNotUsed { get; set; }

        /// <summary>
        /// Generic Device Name Prefix
        /// </summary>
        public string CsGenericDeviceNamePrefix { get; set; }

        /// <summary>
        /// Command Class Comment
        /// </summary>
        public string CsCommandClassComment { get; set; }

        /// <summary>
        /// Command Class Version Comment
        /// </summary>
        public string CsCommandClassVersionComment { get; set; }

        /// <summary>
        /// Command Class Version Const Postfix
        /// </summary>
        public string CsCommandClassVersionConstPostfix { get; set; }

        /// <summary>
        /// Command Enum List comment
        /// </summary>
        public string CsCommandEnumListComment { get; set; }

        /// <summary>
        /// Command Class Prefix
        /// </summary>
        public string CsCommandClassPrefix { get; set; }

        /// <summary>
        /// Command Class Name Prefix
        /// </summary>
        public string CsCommandClassNamePrefix { get; set; }
    }
}