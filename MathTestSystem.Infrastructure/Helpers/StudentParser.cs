using MathTestSystem.Domain.Entities;
using MathTestSystem.Domain.Interfaces;
using System.Xml;

namespace MathTestSystem.Infrastructure.Helpers
{
    public class StudentParser : IStudentParser
    {
        public async Task<List<Student>> ParseStudentsAsync(string xml)
        {
            return await Task.Run(() =>
            {
                var students = new List<Student>();

                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var studentNodes = doc.SelectNodes("//Student");
                if (studentNodes == null) return students;

                foreach (XmlNode studentNode in studentNodes)
                {
                    var studentId = studentNode.Attributes?["ID"]?.Value;
                    if (string.IsNullOrEmpty(studentId)) continue;

                    var student = new Student(studentId);

                    var examNodes = studentNode.SelectNodes("Exam");
                    if (examNodes == null) continue;

                    foreach (XmlNode examNode in examNodes)
                    {
                        var examId = examNode.Attributes?["Id"]?.Value;
                        if (string.IsNullOrEmpty(examId)) continue;

                        var exam = new Exam(examId);

                        var taskNodes = examNode.SelectNodes("Task");
                        if (taskNodes == null) continue;

                        foreach (XmlNode taskNode in taskNodes)
                        {
                            var taskId = taskNode.Attributes?["id"]?.Value;
                            var content = taskNode.InnerText;

                            if (string.IsNullOrEmpty(taskId) || string.IsNullOrEmpty(content)) continue;

                            var parts = content.Split('=');
                            if (parts.Length != 2) continue;

                            var expr = parts[0].Trim();
                            var submitted = decimal.Parse(parts[1].Trim());

                            var task = new MathTask(taskId, expr, submitted);
                            exam.AddTask(task);
                        }

                        student.AddExam(exam);
                    }

                    students.Add(student);
                }

                return students;
            });
        }
    }
}
