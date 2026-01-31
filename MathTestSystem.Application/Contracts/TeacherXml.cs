using System.Xml.Serialization;

namespace MathTestSystem.Application.Contracts
{
    [XmlRoot("Teacher")]
    public class TeacherXml
    {
        [XmlAttribute("ID")]
        public string ID { get; set; } = string.Empty;

        [XmlArray("Students")]
        [XmlArrayItem("Student")]
        public List<StudentXml> Students { get; set; } = new();
    }
}
