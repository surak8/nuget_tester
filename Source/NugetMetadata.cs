
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlType("metadata")]
public class NugetMetadata {
    public string id { get; set; }
    public string version { get; set; }
    public string authors { get; set; }
    public string description { get; set; }
    public string projectUrl { get; set; }
    public string licenseUrl { get; set; }
    public string iconUrl { get; set; }
    public string language { get; set; }
    public string owners { get; set; }
    public string releaseNotes { get; set; }
    public string copyright { get; set; }
    public string tags { get; set; }
    public bool requireLicenseAcceptance { get; set; }

    public List<NugetDependency> dependencies { get; set; }
    public List<NugetFWAssembly> frameworkAssemblies { get; set; }
}


