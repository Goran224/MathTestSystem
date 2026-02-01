using System.Xml.Serialization;

namespace MathTestSystem.Shared.Contracts
{
    public class StudentXml
    {
        [XmlAttribute("ID")]
        public string ID { get; set; } = string.Empty;

        [XmlElement("Exam")]
        public List<ExamXml> Exams { get; set; } = new();
    }
}
