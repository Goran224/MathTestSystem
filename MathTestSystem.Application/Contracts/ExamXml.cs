using System.Xml.Serialization;

namespace MathTestSystem.Application.Contracts
{
    public class ExamXml
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("Task")]
        public List<TaskXml> Tasks { get; set; } = new();
    }
}
