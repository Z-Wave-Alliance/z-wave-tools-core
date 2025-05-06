/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

namespace ZWave.Xml.Application {
    using System.Xml.Serialization;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class ZWaveDefinition : object, INotifyPropertyChanged {
        
        private Collection<BasicDevice> basicDevicesField;
        
        private Collection<GenericDevice> genericDevicesField;
        
        private Collection<CommandClass> commandClassesField;
        
        private string versionField;
        
        [XmlArrayItem("BasicDevice", IsNullable=false)]
        public Collection<BasicDevice> BasicDevices {
            get {
                return basicDevicesField;
            }
            set {
                basicDevicesField = value;
                RaisePropertyChanged("BasicDevices");
            }
        }
        
        [XmlArrayItem("GenericDevice", IsNullable=false)]
        public Collection<GenericDevice> GenericDevices {
            get {
                return genericDevicesField;
            }
            set {
                genericDevicesField = value;
                RaisePropertyChanged("GenericDevices");
            }
        }
        
        [XmlArrayItem("CommandClass", IsNullable=false)]
        public Collection<CommandClass> CommandClasses {
            get {
                return commandClassesField;
            }
            set {
                commandClassesField = value;
                RaisePropertyChanged("CommandClasses");
            }
        }
        
        [XmlAttribute]
        public string Version {
            get {
                return versionField;
            }
            set {
                versionField = value;
                RaisePropertyChanged("Version");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class BasicDevice : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string nameField;
        
        private string textField;
        
        private string commentField;
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute(DataType="ID")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class GenericDevice : object, INotifyPropertyChanged {
        
        private Collection<SpecificDevice> specificDeviceField;
        
        private string keyField;
        
        private string nameField;
        
        private string textField;
        
        private string commentField;
        
        [XmlElement("SpecificDevice")]
        public Collection<SpecificDevice> SpecificDevice {
            get {
                return specificDeviceField;
            }
            set {
                specificDeviceField = value;
                RaisePropertyChanged("SpecificDevice");
            }
        }
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute(DataType="ID")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class SpecificDevice : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string scopeKeyField;
        
        private string nameField;
        
        private string textField;
        
        private string commentField;
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute]
        public string ScopeKey {
            get {
                return scopeKeyField;
            }
            set {
                scopeKeyField = value;
                RaisePropertyChanged("ScopeKey");
            }
        }
        
        [XmlAttribute(DataType="ID")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class CommandClass : object, INotifyPropertyChanged {
        
        private Collection<Command> commandField;
        
        private Collection<DefineSet> defineSetField;
        
        private string keyField;
        
        private byte bitsField;
        
        private bool bitsFieldSpecified;
        
        private string nameField;
        
        private byte versionField;
        
        private string textField;
        
        private string commentField;
        
        [XmlElement("Command")]
        public Collection<Command> Command {
            get {
                return commandField;
            }
            set {
                commandField = value;
                RaisePropertyChanged("Command");
            }
        }
        
        [XmlElement("DefineSet")]
        public Collection<DefineSet> DefineSet {
            get {
                return defineSetField;
            }
            set {
                defineSetField = value;
                RaisePropertyChanged("DefineSet");
            }
        }
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute]
        public byte Bits {
            get {
                return bitsField;
            }
            set {
                bitsField = value;
                RaisePropertyChanged("Bits");
            }
        }
        
        [XmlIgnore]
        public bool BitsSpecified {
            get {
                return bitsFieldSpecified;
            }
            set {
                bitsFieldSpecified = value;
                RaisePropertyChanged("BitsSpecified");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public byte Version {
            get {
                return versionField;
            }
            set {
                versionField = value;
                RaisePropertyChanged("Version");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class Command : object, INotifyPropertyChanged {
        
        private Collection<Param> paramField;
        
        private string keyField;
        
        private byte bitsField;
        
        private bool bitsFieldSpecified;
        
        private string nameField;
        
        private string textField;
        
        private zwSupportModes supportModeField;
        
        private string commentField;
        
        public Command() {
            supportModeField = zwSupportModes.APP;
        }
        
        [XmlElement("Param")]
        public Collection<Param> Param {
            get {
                return paramField;
            }
            set {
                paramField = value;
                RaisePropertyChanged("Param");
            }
        }
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute]
        public byte Bits {
            get {
                return bitsField;
            }
            set {
                bitsField = value;
                RaisePropertyChanged("Bits");
            }
        }
        
        [XmlIgnore]
        public bool BitsSpecified {
            get {
                return bitsFieldSpecified;
            }
            set {
                bitsFieldSpecified = value;
                RaisePropertyChanged("BitsSpecified");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(zwSupportModes.APP)]
        public zwSupportModes SupportMode {
            get {
                return supportModeField;
            }
            set {
                supportModeField = value;
                RaisePropertyChanged("SupportMode");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class Param : object, INotifyPropertyChanged {
        
        private Collection<Param> param1Field;
        
        private byte orderField;
        
        private string nameField;
        
        private string textField;
        
        private zwParamType typeField;
        
        private string optionalReferenceField;
        
        private string primaryReferenceField;
        
        private string moreToFollowReferenceField;
        
        private byte sizeField;
        
        private bool sizeFieldSpecified;
        
        private sbyte sizeChangeField;
        
        private bool sizeChangeFieldSpecified;
        
        private string sizeReferenceField;
        
        private byte bitsField;
        
        private byte[] defaultValueField;
        
        private string commentField;
        
        private string definesField;
        
        private bool skipFieldField;
        
        private bool skipFieldFieldSpecified;
        
        [XmlElement("Param")]
        public Collection<Param> Param1 {
            get {
                return param1Field;
            }
            set {
                param1Field = value;
                RaisePropertyChanged("Param1");
            }
        }
        
        [XmlAttribute]
        public byte Order {
            get {
                return orderField;
            }
            set {
                orderField = value;
                RaisePropertyChanged("Order");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        [XmlAttribute]
        public zwParamType Type {
            get {
                return typeField;
            }
            set {
                typeField = value;
                RaisePropertyChanged("Type");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string OptionalReference {
            get {
                return optionalReferenceField;
            }
            set {
                optionalReferenceField = value;
                RaisePropertyChanged("OptionalReference");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string PrimaryReference {
            get {
                return primaryReferenceField;
            }
            set {
                primaryReferenceField = value;
                RaisePropertyChanged("PrimaryReference");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string MoreToFollowReference {
            get {
                return moreToFollowReferenceField;
            }
            set {
                moreToFollowReferenceField = value;
                RaisePropertyChanged("MoreToFollowReference");
            }
        }
        
        [XmlAttribute]
        public byte Size {
            get {
                return sizeField;
            }
            set {
                sizeField = value;
                RaisePropertyChanged("Size");
            }
        }
        
        [XmlIgnore]
        public bool SizeSpecified {
            get {
                return sizeFieldSpecified;
            }
            set {
                sizeFieldSpecified = value;
                RaisePropertyChanged("SizeSpecified");
            }
        }
        
        [XmlAttribute]
        public sbyte SizeChange {
            get {
                return sizeChangeField;
            }
            set {
                sizeChangeField = value;
                RaisePropertyChanged("SizeChange");
            }
        }
        
        [XmlIgnore]
        public bool SizeChangeSpecified {
            get {
                return sizeChangeFieldSpecified;
            }
            set {
                sizeChangeFieldSpecified = value;
                RaisePropertyChanged("SizeChangeSpecified");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string SizeReference {
            get {
                return sizeReferenceField;
            }
            set {
                sizeReferenceField = value;
                RaisePropertyChanged("SizeReference");
            }
        }
        
        [XmlAttribute]
        public byte Bits {
            get {
                return bitsField;
            }
            set {
                bitsField = value;
                RaisePropertyChanged("Bits");
            }
        }
        
        [XmlAttribute(DataType="hexBinary")]
        public byte[] DefaultValue {
            get {
                return defaultValueField;
            }
            set {
                defaultValueField = value;
                RaisePropertyChanged("DefaultValue");
            }
        }
        
        [XmlAttribute]
        public string Comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("Comment");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Defines {
            get {
                return definesField;
            }
            set {
                definesField = value;
                RaisePropertyChanged("Defines");
            }
        }
        
        [XmlAttribute]
        public bool SkipField {
            get {
                return skipFieldField;
            }
            set {
                skipFieldField = value;
                RaisePropertyChanged("SkipField");
            }
        }
        
        [XmlIgnore]
        public bool SkipFieldSpecified {
            get {
                return skipFieldFieldSpecified;
            }
            set {
                skipFieldFieldSpecified = value;
                RaisePropertyChanged("SkipFieldSpecified");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    public enum zwParamType {
        
        HEX,
        
        BOOLEAN,
        
        CHAR,
        
        NUMBER,
        
        NUMBER_SIGNED,
        
        NODE_NUMBER,
        
        BITMASK,
        
        MARKER,
        
        BAS_DEV_REF,
        
        GEN_DEV_REF,
        
        SPEC_DEV_REF,
        
        CMD_CLASS_REF,
        
        CMD_REF,
        
        CMD_DATA,
        
        CMD_ENCAP,
    }
    
    [System.Serializable]
    public enum zwSupportModes {
        
        APP,
        
        TX,
        
        RX,
        
        TX_RX,
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class DefineSet : object, INotifyPropertyChanged {
        
        private Collection<Define> defineField;
        
        private string nameField;
        
        private zwDefineSetType typeField;
        
        [XmlElement("Define")]
        public Collection<Define> Define {
            get {
                return defineField;
            }
            set {
                defineField = value;
                RaisePropertyChanged("Define");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public zwDefineSetType Type {
            get {
                return typeField;
            }
            set {
                typeField = value;
                RaisePropertyChanged("Type");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class Define : object, INotifyPropertyChanged {
        
        private Collection<Define> define1Field;
        
        private string keyField;
        
        private string nameField;
        
        private string textField;
        
        [XmlElement("Define")]
        public Collection<Define> Define1 {
            get {
                return define1Field;
            }
            set {
                define1Field = value;
                RaisePropertyChanged("Define1");
            }
        }
        
        [XmlAttribute]
        public string Key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("Key");
            }
        }
        
        [XmlAttribute(DataType="NCName")]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }
        
        [XmlAttribute]
        public string Text {
            get {
                return textField;
            }
            set {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Serializable]
    public enum zwDefineSetType {
        
        Unknown,
        
        Partial,
        
        Full,
    }
}
