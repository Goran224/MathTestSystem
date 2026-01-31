using AutoMapper;
using MathTestSystem.Application.Contracts;
using MathTestSystem.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentXml, Student>()
            .ConstructUsing(src => new Student(src.ID));

        CreateMap<ExamXml, Exam>()
            .ConstructUsing(src => new Exam(src.Id));

        CreateMap<TaskXml, MathTask>()
      .ConstructUsing(src =>
          new MathTask(
              src.Id,
              src.Value.Split('=')[0].Trim(),
              decimal.Parse(src.Value.Split('=')[1].Trim())
          ));
    }
}