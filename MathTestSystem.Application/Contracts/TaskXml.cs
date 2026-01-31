using System.Xml.Serialization;

namespace MathTestSystem.Application.Contracts
{

    public class TaskXml
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }
}
