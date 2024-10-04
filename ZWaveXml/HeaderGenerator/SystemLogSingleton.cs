/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections;
using ZWave.Xml.Properties;

namespace ZWave.Xml.HeaderGenerator
{
    /// <summary>
    /// Singleton for providing system log text. Decoupled from UI using delegates.
    /// </summary>
    public class SystemLogSingleton
    {
        /// <summary>
        /// Category of the system log entry
        /// </summary>
        public enum Category
        {

            /// <summary>
            /// Error Entry, highest severity
            /// </summary>
            Error,
            /// <summary>
            /// Warning Entry
            /// </summary>
            Warning,
            /// <summary>
            /// Information Entry, lowest severity
            /// </summary>
            Info
        }
        /// <summary>
        /// System Log Actions. Defines from which functionality the log entry originates.
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// General code generation
            /// </summary>
            CodeGeneration,
            /// <summary>
            /// C Sharp code generation
            /// </summary>
            CSharpCodeGeneration,
            /// <summary>
            /// C header code generation
            /// </summary>
            CHeaderCodeGeneration,
            /// <summary>
            /// XML validation (Reading XML)
            /// </summary>
            XmlValidation,
            /// <summary>
            /// File New action
            /// </summary>
            FileNew,
            /// <summary>
            ///  Open file action
            /// </summary>
            FileOpen,
            /// <summary>
            /// Save File action
            /// </summary>
            FileSave,
            /// <summary>
            /// Add Basic Device
            /// </summary>
            AddBas,
            /// <summary>
            /// Add Generic Device
            /// </summary>
            AddGen,
            /// <summary>
            /// Add Specific Device
            /// </summary>
            AddSpec,
            /// <summary>
            /// Add Command Class
            /// </summary>
            AddCmdClass,
            /// <summary>
            /// Add Command
            /// </summary>
            AddCmd,
            /// <summary>
            /// Add Parameter
            /// </summary>
            AddParm,
            /// <summary>
            /// Edit Basic Device
            /// </summary>
            EditBas,
            /// <summary>
            /// Edit Generic Device
            /// </summary>
            EditGen,
            /// <summary>
            /// Edit Specific Device
            /// </summary>
            EditSpec,
            /// <summary>
            /// Edit Command Class
            /// </summary>
            EditCmdClass,
            /// <summary>
            /// Edit Command
            /// </summary>
            EditCmd,
            /// <summary>
            /// Edit Parameter
            /// </summary>
            EditParm,
            /// <summary>
            /// Remove Basic Device
            /// </summary>
            RemoveBas,
            /// <summary>
            /// Remove Generic Device
            /// </summary>
            RemoveGen,
            /// <summary>
            /// Remove Specific Device
            /// </summary>
            RemoveSpec,
            /// <summary>
            /// Remove Command Class
            /// </summary>
            RemoveCmdClass,
            /// <summary>
            /// Remove Command
            /// </summary>
            RemoveCmd,
            /// <summary>
            /// Remove Parameter
            /// </summary>
            RemoveParm,
            /// <summary>
            /// Add new Command Class version
            /// </summary>
            AddNewVersion,
            /// <summary>
            /// Edit all versions of Command Class
            /// </summary>
            EditAllVersionsCmdClass,
            /// <summary>
            /// Edit commands in all versions of Command Class
            /// </summary>
            EditAllVersionsCmd,
            /// <summary>
            /// Set the read only attribute
            /// </summary>
            SetReadOnly,
            /// <summary>
            /// Reset the read only attribute
            /// </summary>
            ResetReadOnly,
            /// <summary>
            /// Add variant group
            /// </summary>
            AddVg
        }

        private readonly SortedList _categoryStr;

        private readonly SortedList _actionStr;

        /// <summary>
        /// AddLogLine delegate declaration
        /// </summary>
        /// <param name="message"></param>
        public delegate void AddLogLine(string message);
        /// <summary>
        /// ClearLog delegate declaration
        /// </summary>
        public delegate void ClearLog();

        /// <summary>
        /// Setup delegates for system log singleton
        /// </summary>
        /// <param name="x">AddLogLine function</param>
        /// <param name="y">ClearLog function</param>
        public void Setup(AddLogLine x, ClearLog y)
        {
            _addLogLine = x;
            _clearLog = y;
        }

        /// <summary>
        /// Adds a log line by calling the delegate
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="act"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DoAddLogLine(Category cat, Action act, string message)
        {
            if (_addLogLine != null)
            {
                string newMessage;
                if (!_categoryStr.ContainsKey(cat))
                {
                    newMessage = "SystemLog_Unknown_Category";
                }
                else
                {
                    newMessage = (string)_categoryStr[cat];
                }
                newMessage += " ";

                if (!_actionStr.ContainsKey(act))
                {
                    newMessage += "SystemLog_Unknown_Action";
                }
                else
                {
                    newMessage += (string)_actionStr[act];
                }
                newMessage += ": " + message;

                _addLogLine(newMessage);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Clear the log by calling the delegate
        /// </summary>
        /// <returns>Returns true if a delegate is assigned otherwise false</returns>
        public bool DoClearLog()
        {
            if (_clearLog != null)
            {
                _clearLog();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create an instance if not already created
        /// The method is not thread safe.
        /// </summary>
        public static SystemLogSingleton Instance
        {
            get { return _instance ?? (_instance = new SystemLogSingleton()); }
        }

        /// <summary>
        /// The instance of the singleton
        /// </summary>
        private static SystemLogSingleton _instance;
        /// <summary>
        /// AddLogLine delegate instance
        /// </summary>
        private AddLogLine _addLogLine;
        /// <summary>
        /// Clear log line delegate instance
        /// </summary>
        private ClearLog _clearLog;

        /// <summary>
        /// Constructor. Called by the Instance property
        /// </summary>
        private SystemLogSingleton()
        {
            _categoryStr = new SortedList
            {
                {Category.Error, Resources.SystemLog_Category_Error},
                {Category.Warning, Resources.SystemLog_Category_Warning},
                {Category.Info, Resources.SystemLog_Category_Info}
            };

            _actionStr = new SortedList
            {
                {Action.CodeGeneration, Resources.SystemLog_Action_CodeGeneration},
                {Action.CSharpCodeGeneration, Resources.SystemLog_Action_CSharpCodeGeneration},
                {Action.CHeaderCodeGeneration, Resources.SystemLog_Action_CHeaderCodeGeneration},
                {Action.XmlValidation, Resources.SystemLog_Action_XMLValidation},
                {Action.FileNew, Resources.SystemLog_Action_FileNew},
                {Action.FileOpen, Resources.SystemLog_Action_FileOpen},
                {Action.FileSave, Resources.SystemLog_Action_FileSave},
                {Action.AddBas, Resources.SystemLog_Action_AddBas},
                {Action.AddGen, Resources.SystemLog_Action_AddGen},
                {Action.AddSpec, Resources.SystemLog_Action_AddSpec},
                {Action.AddCmdClass, Resources.SystemLog_Action_AddCC},
                {Action.AddCmd, Resources.SystemLog_Action_AddCmd},
                {Action.AddParm, Resources.SystemLog_Action_AddParm},
                {Action.EditBas, Resources.SystemLog_Action_EditBas},
                {Action.EditGen, Resources.SystemLog_Action_EditGen},
                {Action.EditSpec, Resources.SystemLog_Action_EditSpec},
                {Action.EditCmdClass, Resources.SystemLog_Action_EditCC},
                {Action.EditCmd, Resources.SystemLog_Action_EditCmd},
                {Action.EditParm, Resources.SystemLog_Action_EditParm},
                {Action.RemoveBas, Resources.SystemLog_Action_RemoveBas},
                {Action.RemoveGen, Resources.SystemLog_Action_RemoveGen},
                {Action.RemoveSpec, Resources.SystemLog_Action_RemoveSpec},
                {Action.RemoveCmdClass, Resources.SystemLog_Action_RemoveCC},
                {Action.RemoveCmd, Resources.SystemLog_Action_RemoveCmd},
                {Action.RemoveParm, Resources.SystemLog_Action_RemoveParm},
                {Action.AddNewVersion, Resources.SystemLog_Action_AddNewVersion},
                {Action.EditAllVersionsCmdClass, Resources.SystemLog_Action_EditAllVersionsCC},
                {Action.EditAllVersionsCmd, Resources.SystemLog_Action_EditAllVersionsCmd},
                {Action.SetReadOnly, Resources.SystemLog_Action_SetReadOnly},
                {Action.ResetReadOnly, Resources.SystemLog_Action_ResetReadOnly}
            };
        }
    }
}
