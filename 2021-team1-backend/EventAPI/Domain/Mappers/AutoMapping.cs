using AutoMapper;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Domain.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Event, EventVM>();
            CreateMap<EventVM, Event>();
            CreateMap<Event, Event>();

            CreateMap<Internship, InternshipVM>();
            CreateMap<InternshipVM, Internship>();
            CreateMap<Internship, Internship>();

            CreateMap<EventCompany, EventCompanyVM>();
            CreateMap<EventCompanyVM, EventCompany>();
            CreateMap<EventCompany, CompanyFilterVM>();
            CreateMap<CompanyFilterVM, EventCompany>();
            CreateMap<EventCompanyVM, CompanyFilterVM>();
            CreateMap<CompanyFilterVM, EventCompanyVM>();

            CreateMap<AcademicYear, AcademicYearVM>();
            CreateMap<AcademicYearVM, AcademicYear>();

            CreateMap<Company, CompanyVM>();
            CreateMap<CompanyVM, Company>();
            CreateMap<Company, CompanyFilterVM>();
            CreateMap<CompanyFilterVM, Company>();

            CreateMap<Attendee, AttendeeVM>();
            CreateMap<AttendeeVM, Attendee>();

            CreateMap<Internship, InternshipVM>();

            CreateMap<Specialisation, SpecialisationFilterVM>();
            CreateMap<SpecialisationFilterVM, Specialisation>();
            CreateMap<Specialisation, SpecialisationVM>();
            CreateMap<SpecialisationVM, Specialisation>();

            CreateMap<Environment, EnvironmentFilterVm>();
            CreateMap<EnvironmentFilterVm, Environment>();
            CreateMap<Environment, EnvironmentVM>();
            CreateMap<EnvironmentVM, Environment>();

            CreateMap<Location, LocationFilterVM>(); 
            CreateMap<LocationFilterVM, Location>();

            CreateMap<Appointment, AppointmentVM>()
                .ForMember(
                    dest => dest.AppointmentStatus,
                    opt => opt.MapFrom(src => src.AppointmentStatus.ToString()));
            CreateMap<AppointmentVM, Appointment>();
            CreateMap<Appointment, AppointmentWithoutStudentDataVM>()
                .ForMember(
                    dest => dest.AppointmentStatus,
                    opt => opt.MapFrom(src => src.AppointmentStatus.ToString()));
            CreateMap<AppointmentWithoutStudentDataVM, Appointment>();

            CreateMap<Contact, ContactVM>();
            CreateMap<ContactVM, Contact>();
        }
    }
}