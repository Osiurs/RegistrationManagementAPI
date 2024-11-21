using AutoMapper;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Registration, RegistrationDTO>().ReverseMap();
            CreateMap<Classroom, ClassroomDTO>().ReverseMap();
        }
    }
}
