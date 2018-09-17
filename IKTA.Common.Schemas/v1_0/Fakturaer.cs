namespace IKTA.Common.Schemas.v1_0 {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://IKTA.Common.Schemas.v1_0.Fakturering",@"Fakturering")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::IKTA.Common.Schemas.FraSystem), XPath = @"/*[local-name()='Fakturering' and namespace-uri()='http://IKTA.Common.Schemas.v1_0.Fakturering']/*[local-name()='FraSystem' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Fakturering"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"IKTA.Common.Schemas.PropertySchema", typeof(global::IKTA.Common.Schemas.PropertySchema))]
    public sealed class Fakturering : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://IKTA.Common.Schemas.v1_0.Fakturering"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://IKTA.Common.Schemas.PropertySchema"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://IKTA.Common.Schemas.v1_0.Fakturering"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://IKTA.Common.Schemas.PropertySchema"" location=""IKTA.Common.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Fakturering"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:FraSystem"" xpath=""/*[local-name()='Fakturering' and namespace-uri()='http://IKTA.Common.Schemas.v1_0.Fakturering']/*[local-name()='FraSystem' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""FraSystem"" type=""xs:string"" />
        <xs:element name=""AntallFakturaer"" type=""xs:int"" />
        <xs:element name=""TotalBeloep"" type=""xs:double"" />
        <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""Faktura"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""Firma"" type=""xs:string"" />
              <xs:element name=""TransaksjonsDato"" type=""xs:date"" />
              <xs:element name=""Ordre"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""EksterntOrdreNummer"" type=""xs:string"" />
                    <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""OrdreLinje"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""LinjeNummer"" type=""xs:int"" />
                          <xs:element name=""Produkt"" type=""xs:string"" />
                          <xs:element name=""Beskrivelse"" type=""xs:string"" />
                          <xs:element name=""Antall"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""Enhet"" type=""xs:string"" />
                          <xs:element name=""EnhetsPris"" type=""xs:double"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""RessursNummer"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Gbnr"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""TjenesteType"" type=""xs:string"" />
              <xs:element name=""FakturaMottaker"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""FNr_OrgNr"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo notes=""Kan enten være personnummer, Org.nummer eller D-nummer"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Navn"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Adresse"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""AdresseLinje"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""PostNummer"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""PostSted"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""Land"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""FaktureringsAdresse"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""AdresseLinje"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""PostNummer"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""PostSted"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""Land"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""FakturaBeskrivelse"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""ToppTekst1"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ToppTekst2"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BunnTekst1"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BunnTekst2"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Fakturering() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Fakturering";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
