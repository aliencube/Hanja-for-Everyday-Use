﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5466
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace Aliencube.Utilities.KoreanHanjaExtractor.Services {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://aliencube.org/schemas/2013/06/hanja-for-everyday-use")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://aliencube.org/schemas/2013/06/hanja-for-everyday-use", IsNullable=false)]
    public partial class HanjaCollection {
        
        private Hanja[] hanjaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Hanja")]
        public Hanja[] Hanja {
            get {
                return this.hanjaField;
            }
            set {
                this.hanjaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://aliencube.org/schemas/2013/06/hanja-for-everyday-use")]
    public partial class Hanja {
        
        private string characterField;
        
        private string meaningField;
        
        private string pronunciationField;
        
        private string phoneticCodeField;
        
        /// <remarks/>
        public string Character {
            get {
                return this.characterField;
            }
            set {
                this.characterField = value;
            }
        }
        
        /// <remarks/>
        public string Meaning {
            get {
                return this.meaningField;
            }
            set {
                this.meaningField = value;
            }
        }
        
        /// <remarks/>
        public string Pronunciation {
            get {
                return this.pronunciationField;
            }
            set {
                this.pronunciationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PhoneticCode {
            get {
                return this.phoneticCodeField;
            }
            set {
                this.phoneticCodeField = value;
            }
        }
    }
}
