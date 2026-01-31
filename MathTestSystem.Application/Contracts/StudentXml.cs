using System.Xml.Serialization;

namespace MathTestSystem.Application.Contracts
{
    public class StudentXml
    {
        [XmlAttribute("ID")]
        public string ID { get; set; } = string.Empty;

        [XmlElement("Exam")]
        public List<ExamXml> Exams { get; set; } = new();
    }
}
