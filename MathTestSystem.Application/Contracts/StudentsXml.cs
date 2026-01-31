using System.Xml.Serialization;

namespace MathTestSystem.Application.Contracts
{
    public class StudentsXml
    {
        [XmlElement("Student")]
        public List<StudentXml> Items { get; set; } = new();
    }
}
