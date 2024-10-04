/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

namespace ZWave.Xml.Application {
    using System.Xml.Serialization;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class zw_classes : object, INotifyPropertyChanged {
        
        private Collection<object> itemsField;
        
        private string versionField;
        
        [XmlElement("bas_dev", typeof(bas_dev))]
        [XmlElement("cmd_class", typeof(cmd_class))]
        [XmlElement("gen_dev", typeof(gen_dev))]
        public Collection<object> Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }
        
        [XmlAttribute]
        public string version {
            get {
                return versionField;
            }
            set {
                versionField = value;
                RaisePropertyChanged("version");
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
    public partial class bas_dev : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string nameField;
        
        private string helpField;
        
        private string commentField;
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string help {
            get {
                return helpField;
            }
            set {
                helpField = value;
                RaisePropertyChanged("help");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
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
    public partial class cmd_class : object, INotifyPropertyChanged {
        
        private Collection<cmd> cmdField;
        
        private string keyField;
        
        private string versionField;
        
        private string nameField;
        
        private string helpField;
        
        private string commentField;
        
        [XmlElement("cmd")]
        public Collection<cmd> cmd {
            get {
                return cmdField;
            }
            set {
                cmdField = value;
                RaisePropertyChanged("cmd");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string version {
            get {
                return versionField;
            }
            set {
                versionField = value;
                RaisePropertyChanged("version");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string help {
            get {
                return helpField;
            }
            set {
                helpField = value;
                RaisePropertyChanged("help");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
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
    public partial class cmd : object, INotifyPropertyChanged {
        
        private Collection<object> itemsField;
        
        private string keyField;
        
        private string nameField;
        
        private string helpField;
        
        private string cmd_maskField;
        
        private zwXmlSupportModes support_modeField;
        
        private string commentField;
        
        public cmd() {
            support_modeField = zwXmlSupportModes.APP;
        }
        
        [XmlElement("param", typeof(param))]
        [XmlElement("variant_group", typeof(variant_group))]
        public Collection<object> Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string help {
            get {
                return helpField;
            }
            set {
                helpField = value;
                RaisePropertyChanged("help");
            }
        }
        
        [XmlAttribute]
        public string cmd_mask {
            get {
                return cmd_maskField;
            }
            set {
                cmd_maskField = value;
                RaisePropertyChanged("cmd_mask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(zwXmlSupportModes.APP)]
        public zwXmlSupportModes support_mode {
            get {
                return support_modeField;
            }
            set {
                support_modeField = value;
                RaisePropertyChanged("support_mode");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
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
    public partial class param : object, INotifyPropertyChanged {
        
        private Collection<object> itemsField;
        
        private string keyField;
        
        private string nameField;
        
        private zwXmlParamType typeField;
        
        private string commentField;
        
        private zwXmlEncapType encaptypeField;
        
        private string optionaloffsField;
        
        private string optionalmaskField;
        
        private string primaryoffsField;
        
        private string primarymaskField;
        
        private byte primaryshiftField;
        
        private string cmd_maskField;
        
        private bool skipfieldField;
        
        public param() {
            encaptypeField = zwXmlEncapType.HEX;
            primaryshiftField = ((byte)(0));
            skipfieldField = false;
        }
        
        [XmlElement("arrayattrib", typeof(arrayattrib))]
        [XmlElement("bitfield", typeof(bitfield))]
        [XmlElement("bitflag", typeof(bitflag))]
        [XmlElement("bitmask", typeof(bitmask))]
        [XmlElement("const", typeof(@const))]
        [XmlElement("fieldenum", typeof(fieldenum))]
        [XmlElement("multi_array", typeof(multi_array))]
        [XmlElement("paramdescloc", typeof(paramdescloc))]
        [XmlElement("variant", typeof(variant))]
        public Collection<object> Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public zwXmlParamType type {
            get {
                return typeField;
            }
            set {
                typeField = value;
                RaisePropertyChanged("type");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(zwXmlEncapType.HEX)]
        public zwXmlEncapType encaptype {
            get {
                return encaptypeField;
            }
            set {
                encaptypeField = value;
                RaisePropertyChanged("encaptype");
            }
        }
        
        [XmlAttribute]
        public string optionaloffs {
            get {
                return optionaloffsField;
            }
            set {
                optionaloffsField = value;
                RaisePropertyChanged("optionaloffs");
            }
        }
        
        [XmlAttribute]
        public string optionalmask {
            get {
                return optionalmaskField;
            }
            set {
                optionalmaskField = value;
                RaisePropertyChanged("optionalmask");
            }
        }
        
        [XmlAttribute]
        public string primaryoffs {
            get {
                return primaryoffsField;
            }
            set {
                primaryoffsField = value;
                RaisePropertyChanged("primaryoffs");
            }
        }
        
        [XmlAttribute]
        public string primarymask {
            get {
                return primarymaskField;
            }
            set {
                primarymaskField = value;
                RaisePropertyChanged("primarymask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte primaryshift {
            get {
                return primaryshiftField;
            }
            set {
                primaryshiftField = value;
                RaisePropertyChanged("primaryshift");
            }
        }
        
        [XmlAttribute]
        public string cmd_mask {
            get {
                return cmd_maskField;
            }
            set {
                cmd_maskField = value;
                RaisePropertyChanged("cmd_mask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(false)]
        public bool skipfield {
            get {
                return skipfieldField;
            }
            set {
                skipfieldField = value;
                RaisePropertyChanged("skipfield");
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
    public partial class arrayattrib : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string lenField;
        
        private bool is_asciiField;
        
        public arrayattrib() {
            is_asciiField = false;
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string len {
            get {
                return lenField;
            }
            set {
                lenField = value;
                RaisePropertyChanged("len");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(false)]
        public bool is_ascii {
            get {
                return is_asciiField;
            }
            set {
                is_asciiField = value;
                RaisePropertyChanged("is_ascii");
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
    public partial class bitfield : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string fieldnameField;
        
        private string fieldmaskField;
        
        private byte shifterField;
        
        public bitfield() {
            shifterField = ((byte)(0));
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string fieldname {
            get {
                return fieldnameField;
            }
            set {
                fieldnameField = value;
                RaisePropertyChanged("fieldname");
            }
        }
        
        [XmlAttribute]
        public string fieldmask {
            get {
                return fieldmaskField;
            }
            set {
                fieldmaskField = value;
                RaisePropertyChanged("fieldmask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte shifter {
            get {
                return shifterField;
            }
            set {
                shifterField = value;
                RaisePropertyChanged("shifter");
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
    public partial class bitflag : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string flagnameField;
        
        private string flagmaskField;
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string flagname {
            get {
                return flagnameField;
            }
            set {
                flagnameField = value;
                RaisePropertyChanged("flagname");
            }
        }
        
        [XmlAttribute]
        public string flagmask {
            get {
                return flagmaskField;
            }
            set {
                flagmaskField = value;
                RaisePropertyChanged("flagmask");
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
    public partial class bitmask : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private byte paramoffsField;
        
        private string lenmaskField;
        
        private byte lenoffsField;
        
        private byte lenField;
        
        public bitmask() {
            lenoffsField = ((byte)(0));
            lenField = ((byte)(0));
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public byte paramoffs {
            get {
                return paramoffsField;
            }
            set {
                paramoffsField = value;
                RaisePropertyChanged("paramoffs");
            }
        }
        
        [XmlAttribute]
        public string lenmask {
            get {
                return lenmaskField;
            }
            set {
                lenmaskField = value;
                RaisePropertyChanged("lenmask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte lenoffs {
            get {
                return lenoffsField;
            }
            set {
                lenoffsField = value;
                RaisePropertyChanged("lenoffs");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte len {
            get {
                return lenField;
            }
            set {
                lenField = value;
                RaisePropertyChanged("len");
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
    public partial class @const : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string flagnameField;
        
        private string flagmaskField;
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string flagname {
            get {
                return flagnameField;
            }
            set {
                flagnameField = value;
                RaisePropertyChanged("flagname");
            }
        }
        
        [XmlAttribute]
        public string flagmask {
            get {
                return flagmaskField;
            }
            set {
                flagmaskField = value;
                RaisePropertyChanged("flagmask");
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
    public partial class fieldenum : object, INotifyPropertyChanged {
        
        private Collection<fieldenum> fieldenum1Field;
        
        private string keyField;
        
        private string valueField;
        
        private string fieldnameField;
        
        private string fieldmaskField;
        
        private byte shifterField;
        
        public fieldenum() {
            shifterField = ((byte)(0));
        }
        
        [XmlElement("fieldenum")]
        public Collection<fieldenum> fieldenum1 {
            get {
                return fieldenum1Field;
            }
            set {
                fieldenum1Field = value;
                RaisePropertyChanged("fieldenum1");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string value {
            get {
                return valueField;
            }
            set {
                valueField = value;
                RaisePropertyChanged("value");
            }
        }
        
        [XmlAttribute]
        public string fieldname {
            get {
                return fieldnameField;
            }
            set {
                fieldnameField = value;
                RaisePropertyChanged("fieldname");
            }
        }
        
        [XmlAttribute]
        public string fieldmask {
            get {
                return fieldmaskField;
            }
            set {
                fieldmaskField = value;
                RaisePropertyChanged("fieldmask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte shifter {
            get {
                return shifterField;
            }
            set {
                shifterField = value;
                RaisePropertyChanged("shifter");
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
    public partial class multi_array : object, INotifyPropertyChanged {
        
        private Collection<object> itemsField;
        
        [XmlElement("bitflag", typeof(bitflag))]
        [XmlElement("paramdescloc", typeof(paramdescloc))]
        public Collection<object> Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
                RaisePropertyChanged("Items");
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
    public partial class paramdescloc : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string paramField;
        
        private string paramdescField;
        
        private string paramstartField;
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string param {
            get {
                return paramField;
            }
            set {
                paramField = value;
                RaisePropertyChanged("param");
            }
        }
        
        [XmlAttribute]
        public string paramdesc {
            get {
                return paramdescField;
            }
            set {
                paramdescField = value;
                RaisePropertyChanged("paramdesc");
            }
        }
        
        [XmlAttribute]
        public string paramstart {
            get {
                return paramstartField;
            }
            set {
                paramstartField = value;
                RaisePropertyChanged("paramstart");
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
    public partial class variant : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private byte paramoffsField;
        
        private bool is_asciiField;
        
        private string sizemaskField;
        
        private byte sizeoffsField;
        
        private sbyte sizechangeField;
        
        public variant() {
            is_asciiField = false;
            sizeoffsField = ((byte)(0));
            sizechangeField = ((sbyte)(0));
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public byte paramoffs {
            get {
                return paramoffsField;
            }
            set {
                paramoffsField = value;
                RaisePropertyChanged("paramoffs");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(false)]
        public bool is_ascii {
            get {
                return is_asciiField;
            }
            set {
                is_asciiField = value;
                RaisePropertyChanged("is_ascii");
            }
        }
        
        [XmlAttribute]
        public string sizemask {
            get {
                return sizemaskField;
            }
            set {
                sizemaskField = value;
                RaisePropertyChanged("sizemask");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(byte), "0")]
        public byte sizeoffs {
            get {
                return sizeoffsField;
            }
            set {
                sizeoffsField = value;
                RaisePropertyChanged("sizeoffs");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(sbyte), "0")]
        public sbyte sizechange {
            get {
                return sizechangeField;
            }
            set {
                sizechangeField = value;
                RaisePropertyChanged("sizechange");
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
    public enum zwXmlParamType {
        
        ARRAY,
        
        BIT_24,
        
        BITMASK,
        
        BYTE,
        
        CONST,
        
        DWORD,
        
        MARKER,
        
        MULTI_ARRAY,
        
        STRUCT_BYTE,
        
        VARIANT,
        
        WORD,
    }
    
    [System.Serializable]
    public enum zwXmlEncapType {
        
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
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class variant_group : object, INotifyPropertyChanged {
        
        private Collection<object> itemsField;
        
        private string keyField;
        
        private string nameField;
        
        private string paramOffsField;
        
        private string sizemaskField;
        
        private string sizeoffsField;
        
        private sbyte sizechangeField;
        
        private string optionaloffsField;
        
        private string optionalmaskField;
        
        private string moretofollowoffsField;
        
        private string moretofollowmaskField;
        
        private string commentField;
        
        private bool skipfieldField;
        
        public variant_group() {
            sizechangeField = ((sbyte)(0));
            skipfieldField = false;
        }
        
        [XmlElement("param", typeof(param))]
        [XmlElement("variant_group", typeof(variant_group))]
        public Collection<object> Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string paramOffs {
            get {
                return paramOffsField;
            }
            set {
                paramOffsField = value;
                RaisePropertyChanged("paramOffs");
            }
        }
        
        [XmlAttribute]
        public string sizemask {
            get {
                return sizemaskField;
            }
            set {
                sizemaskField = value;
                RaisePropertyChanged("sizemask");
            }
        }
        
        [XmlAttribute]
        public string sizeoffs {
            get {
                return sizeoffsField;
            }
            set {
                sizeoffsField = value;
                RaisePropertyChanged("sizeoffs");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(typeof(sbyte), "0")]
        public sbyte sizechange {
            get {
                return sizechangeField;
            }
            set {
                sizechangeField = value;
                RaisePropertyChanged("sizechange");
            }
        }
        
        [XmlAttribute]
        public string optionaloffs {
            get {
                return optionaloffsField;
            }
            set {
                optionaloffsField = value;
                RaisePropertyChanged("optionaloffs");
            }
        }
        
        [XmlAttribute]
        public string optionalmask {
            get {
                return optionalmaskField;
            }
            set {
                optionalmaskField = value;
                RaisePropertyChanged("optionalmask");
            }
        }
        
        [XmlAttribute]
        public string moretofollowoffs {
            get {
                return moretofollowoffsField;
            }
            set {
                moretofollowoffsField = value;
                RaisePropertyChanged("moretofollowoffs");
            }
        }
        
        [XmlAttribute]
        public string moretofollowmask {
            get {
                return moretofollowmaskField;
            }
            set {
                moretofollowmaskField = value;
                RaisePropertyChanged("moretofollowmask");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
            }
        }
        
        [XmlAttribute]
        [DefaultValue(false)]
        public bool skipfield {
            get {
                return skipfieldField;
            }
            set {
                skipfieldField = value;
                RaisePropertyChanged("skipfield");
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
    public enum zwXmlSupportModes {
        
        APP,
        
        TX,
        
        RX,
        
        TX_RX,
    }
    
    [System.Serializable]
    [XmlType(AnonymousType=true)]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class gen_dev : object, INotifyPropertyChanged {
        
        private Collection<spec_dev> spec_devField;
        
        private string keyField;
        
        private string nameField;
        
        private string helpField;
        
        private string commentField;
        
        [XmlElement("spec_dev")]
        public Collection<spec_dev> spec_dev {
            get {
                return spec_devField;
            }
            set {
                spec_devField = value;
                RaisePropertyChanged("spec_dev");
            }
        }
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string help {
            get {
                return helpField;
            }
            set {
                helpField = value;
                RaisePropertyChanged("help");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
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
    public partial class spec_dev : object, INotifyPropertyChanged {
        
        private string keyField;
        
        private string nameField;
        
        private string helpField;
        
        private string commentField;
        
        [XmlAttribute]
        public string key {
            get {
                return keyField;
            }
            set {
                keyField = value;
                RaisePropertyChanged("key");
            }
        }
        
        [XmlAttribute]
        public string name {
            get {
                return nameField;
            }
            set {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }
        
        [XmlAttribute]
        public string help {
            get {
                return helpField;
            }
            set {
                helpField = value;
                RaisePropertyChanged("help");
            }
        }
        
        [XmlAttribute]
        public string comment {
            get {
                return commentField;
            }
            set {
                commentField = value;
                RaisePropertyChanged("comment");
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
}
