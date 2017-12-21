using System.Xml.Serialization;

[XmlType("file")]
public class NugetFile {
    [XmlAttribute]
    public string src { get; set; }

    [XmlAttribute]
    public string target { get; set; }
}