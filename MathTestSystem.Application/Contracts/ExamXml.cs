using System.Xml.Serialization;

namespace MathTestSystem.Shared.Contracts
{
    public class ExamXml
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("Task")]
        public List<TaskXml> Tasks { get; set; } = new();
    }
}
