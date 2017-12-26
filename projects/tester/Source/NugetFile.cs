using System.Xml.Serialization;

namespace NSTester {
    [XmlType("file")]
    public class NugetFile {

        #region ctors
        NugetFile() { }

        public NugetFile(string asrc) : this(asrc, null) { }

        public NugetFile(string asrc, string atarget) : this() {
            src = asrc;
            target = atarget;
        }
        #endregion

        #region properties
        [XmlAttribute]
        public string src { get; set; }

        [XmlAttribute]
        public string target { get; set; }

        #endregion
    }
}