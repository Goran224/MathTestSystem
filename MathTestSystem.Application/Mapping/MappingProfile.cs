using AutoMapper;
using MathTestSystem.Application.DTOs;
using MathTestSystem.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Exams, opt => opt.MapFrom(src => src.Exams));

        CreateMap<Exam, ExamDto>()
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

        CreateMap<MathTask, TaskDto>();
        CreateMap<ExamResultDto, ExamResultDto>();
        CreateMap<TaskResultDto, TaskResultDto>();
    }
}