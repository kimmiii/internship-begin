using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using EventAPI.Helpers;
using EventAPI.Services;
using Microsoft.AspNetCore.Http;
using MimeTypes;

namespace EventAPI.BLL
{
    public interface IEventBll
    {

        #region Event

        Task<IList<Event>> GetAsync();
        Task<IList<EventVM>> GetAsyncVm();
        Task<Event> GetByIdAsync(Guid id);
        Task<EventVM> GetByIdAsyncVm(Guid id);
        Task<Event> GetActiveAsync();
        Task<EventVM> GetActiveAsyncVm();
        Task<Event> CreateAsync(Event e);
        Task<EventVM> CreateAsyncVm(EventVM eventVm);
        Task<Event> UpdateAsync(Event e);
        Task<EventVM> UpdateAsyncVm(EventVM eventVm);

        #endregion

        #region EventCompany

        Task<EventCompany> GetCompanyFromActiveEventAsync();
        Task<EventCompany> GetCompanyFromActiveEventByIdAsync(int id);
        Task<EventCompanyVM> GetCompanyFromActiveEventAsyncVm();
        Task<EventCompanyVM> GetCompanyFromActiveEventByIdAsyncVm(int id);

        Task<List<EventCompany>> GetCompaniesFromActiveEventAsync();
        Task<List<EventCompanyVM>> GetCompaniesFromActiveEventAsyncVm();
        Task<List<CompanyFilterVM>> GetCompaniesFromActiveEventForFilterAsyncVm();
        Task<EventCompany> CreateEventCompanyAsync(EventCompany eventCompany);
        Task<EventCompanyVM> CreateEventCompanyAsyncVm(EventCompanyVM eventCompanyVm);

        #endregion

        #region Internship

        Task<List<InternshipVM>> GetInternshipsFromActiveEventAsyncVm();
        Task<InternshipVM> GetInternshipByIdAsyncVm(int id);
        Task<List<InternshipVM>> GetInternshipsByCompanyFromActiveEventAsyncVm();
        Task<List<InternshipVM>> GetInternshipsByCompanyIdFromActiveEventAsyncVm(int id);

        #endregion

        #region Appointment
        Task<List<AppointmentVM>> GetAllAppointmentsAsyncVm();
        Task<List<AppointmentVM>> GetAllConfirmedAppointmentsAsyncVm();
        Task<List<AppointmentVM>> GetAllAppointmentsByStatusAsyncVm(List<string> allStatus);
        Task<AppointmentVM> GetAppointmentsByIdAsyncVm(Guid id);
        Task<List<AppointmentVM>> GetAppointmentsFromCompanyAsyncVm();
        Task<List<AppointmentVM>> GetAllAppointmentsFromCompanyByStatusAsyncVm(List<string> allStatus);
        Task<List<AppointmentWithoutStudentDataVM>> GetAppointmentsByCompanyByStatusAsyncVm(int id, List<string> allStatus);
        Task<List<AppointmentVM>> GetAppointmentsFromStudentAsyncVm();
        Task<List<AppointmentVM>> GetAllAppointmentsForStudentByStatusAsyncVm(List<string> allStatus);
        Task<List<AppointmentWithoutStudentDataVM>> GetAppointmentsByCompanyAsyncVm(int id);
        Task<AppointmentVM> CreateAppointmentAsyncVm(AppointmentVM appointmentVM);
        Task<AppointmentVM> UpdateAppointmentAsyncVm(AppointmentVM appointmentVm);
        Task<AppointmentVM> DeleteAppointmentByIdAsyncVm(Guid id);


        #endregion

        #region Files

        Task<List<FileStorage>> GetFilesAsync(Guid id);
        Task<FileStream> GetFileAsync(Guid id, string fileName);
        Task<FileStorage> UploadFileAsync(Guid id, IFormFile file);

        #endregion

        #region Logo
        Task<FileStorage> UploadLogoAsync(IFormFile file);
        Task<FileStream> GetLogoAsync(int id);
        
        #endregion

        Task<AttendeeVM> CreateAttendeeAsyncVm(AttendeeVM attendeeVm);
    }


    public class EventBll : IEventBll
    {
        private readonly IAcademicYearBll _academicYearBll;
        private readonly IAppointmentBll _appointmentBll;
        private readonly IAttendeeBLL _attendeeBll;
        private readonly IStudentBLL _studentBll;
        private readonly IFileStorageService _fileStorage;
        private readonly ICompanyBll _companyBll;
        private readonly IEventCompanyRepository _eventCompanyRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInternshipBll _internshipBll;
        private readonly IMapper _mapper;

        public EventBll(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IEventRepository eventRepository,
            IEventCompanyRepository eventCompanyRepository,
            IAcademicYearBll academicYearBll,
            IInternshipBll internshipBll,
            ICompanyBll companyBll,
            IAppointmentBll appointmentBll,
            IAttendeeBLL attendeeBll,
            IStudentBLL studentBll,
            IFileStorageService fileStorage
            )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _eventRepository = eventRepository;
            _eventCompanyRepository = eventCompanyRepository;
            _academicYearBll = academicYearBll;
            _internshipBll = internshipBll;
            _companyBll = companyBll;
            _appointmentBll = appointmentBll;
            _attendeeBll = attendeeBll;
            _studentBll = studentBll;
            _fileStorage = fileStorage;
        }

        #region Event

        public async Task<IList<Event>> GetAsync()
        {
            var events = await _eventRepository.GetAsync();
            return events.ToList();
        }

        public async Task<IList<EventVM>> GetAsyncVm()
        {
            var events = await GetAsync();
            return _mapper.Map<List<EventVM>>(events);
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            var e = await _eventRepository.GetByIdAsync(id);
            return e;
        }

        public async Task<EventVM> GetByIdAsyncVm(Guid id)
        {
            var e = await GetByIdAsync(id);
            return _mapper.Map<EventVM>(e);
        }

        public async Task<Event> GetActiveAsync()
        {
            var e = await _eventRepository.GetActiveAsync();
            return e;
        }

        public async Task<EventVM> GetActiveAsyncVm()
        {
            var e = await GetActiveAsync();
            return _mapper.Map<EventVM>(e);
        }

        public async Task<Event> CreateAsync(Event e)
        {
            e = await _eventRepository.CreateAsync(e);
            return e;
        }

        public async Task<EventVM> CreateAsyncVm(EventVM eventVM)
        {
            var e = _mapper.Map<Event>(eventVM);
            e = await CreateAsync(e);
            return _mapper.Map<EventVM>(e);
        }

        public async Task<Event> UpdateAsync(Event e)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(e.Id);
            eventToUpdate = _mapper.Map(e, eventToUpdate);
            await _eventRepository.UpdateAsync(eventToUpdate);
            eventToUpdate = await _eventRepository.GetByIdAsync(e.Id);
            return eventToUpdate;
        }

        public async Task<EventVM> UpdateAsyncVm(EventVM eventVM)
        {
            var e = _mapper.Map<Event>(eventVM);
            e = await UpdateAsync(e);
            return _mapper.Map<EventVM>(e);
        }

        #endregion

        #region EventCompany

        public async Task<EventCompany> GetCompanyFromActiveEventAsync()
        {
            var id = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            return await GetCompanyFromActiveEventByIdAsync(id);
        }

        public async Task<EventCompany> GetCompanyFromActiveEventByIdAsync(int id)
        {
            var e = await GetActiveAsync();
            var eventCompany = await _eventCompanyRepository.GetByEventAndCompanyAsync(e.Id, id);
            return eventCompany;
        }


        public async Task<EventCompanyVM> GetCompanyFromActiveEventAsyncVm()
        {
            var eventCompany = await GetCompanyFromActiveEventAsync();
            if (eventCompany != null)
            {
                var eventCompanyVm = _mapper.Map<EventCompanyVM>(eventCompany);
                eventCompanyVm = await FillInFieldEventCompanyVM(eventCompanyVm);
                return eventCompanyVm;
            }
            else
            {
                return null;
            }
        }

        public async Task<EventCompanyVM> GetCompanyFromActiveEventByIdAsyncVm(int id)
        {
            var eventCompany = await GetCompanyFromActiveEventByIdAsync(id);
            if (eventCompany != null)
            {
                var eventCompanyVm = _mapper.Map<EventCompanyVM>(eventCompany);
                eventCompanyVm = await FillInFieldEventCompanyVM(eventCompanyVm);
                return eventCompanyVm;
            } 
            else
            {
                return null;
            }
        }

        public async Task<List<EventCompany>> GetCompaniesFromActiveEventAsync()
        {
            var e = await GetActiveAsync();
            var eventCompany = await _eventCompanyRepository.GetByEventIdAsync(e.Id);
            return eventCompany;
        }

        public async Task<List<EventCompanyVM>> GetCompaniesFromActiveEventAsyncVm()
        {
            var eventCompanies = await GetCompaniesFromActiveEventAsync();
            var eventCompaniesVm = new List<EventCompanyVM>();

            foreach (var eventCompany in eventCompanies)
            {
                var eventCompanyVm = _mapper.Map<EventCompanyVM>(eventCompany);
                eventCompanyVm = await FillInFieldEventCompanyVM(eventCompanyVm);
                eventCompaniesVm.Add(eventCompanyVm);
            }
            return eventCompaniesVm;
        }

        public async Task<List<CompanyFilterVM>> GetCompaniesFromActiveEventForFilterAsyncVm()
        {
            var eventCompanies = await GetCompaniesFromActiveEventAsync();
            var eventCompaniesVm = new List<CompanyFilterVM>();

            foreach (var eventCompany in eventCompanies)
            {
                var eventCompanyVm = _mapper.Map<CompanyFilterVM>(eventCompany);
                var company = await _companyBll.GetByIdAsync(eventCompanyVm.CompanyId);
                if (company != null)
                {
                    eventCompanyVm.Name = company.Name;
                }
                eventCompaniesVm.Add(eventCompanyVm);
            }
            return eventCompaniesVm;
        }

        public async Task<EventCompany> CreateEventCompanyAsync(EventCompany eventCompany)
        {
            eventCompany = await _eventCompanyRepository.CreateAsync(eventCompany);
            return eventCompany;
        }

        public async Task<EventCompanyVM> CreateEventCompanyAsyncVm(EventCompanyVM eventCompanyVm)
        {
            var eventCompany = _mapper.Map<EventCompany>(eventCompanyVm);
            var e = await GetActiveAsync();
            eventCompany.EventId = e.Id;
            var companyId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            eventCompany.CompanyId = companyId;
            eventCompany = await CreateEventCompanyAsync(eventCompany);
            eventCompanyVm = _mapper.Map<EventCompanyVM>(eventCompany);
            eventCompanyVm = await FillInFieldEventCompanyVM(eventCompanyVm);
            return eventCompanyVm;
        }

        private async Task<EventCompanyVM> FillInFieldEventCompanyVM(EventCompanyVM eventCompanyVm)
        {
            var company = await _companyBll.GetByIdAsync(eventCompanyVm.CompanyId);
            if (company != null)
            {
                eventCompanyVm.CompanyName = company.Name;
                eventCompanyVm.Email = company.Email;
            }

            return eventCompanyVm;
        }

        #endregion

        #region Internship

        public async Task<List<InternshipVM>> GetInternshipsFromActiveEventAsyncVm()
        {
            var e = await GetActiveAsync();
            var academicYear = await _academicYearBll.GetByIdAsync(e.AcademicYearId);
            var internshipVms =
                (List<InternshipVM>) await _internshipBll.GetByAcademicYearAsyncVm(academicYear.Description);

            var specialisations = await _internshipBll.GetSpecializationsForFilterAsync();
            var environments = await _internshipBll.GetEnvironmentsForFilterAsync(); 
            var contacts = await _internshipBll.GetContactsAsync();
            foreach (var internshipVm in internshipVms)
            {
                var company = await _companyBll.GetByIdAsync(internshipVm.CompanyId);
                if (company != null) internshipVm.CompanyName = company.Name;

                var contact = contacts.Find(c => c.ContactId == internshipVm.ContactPersonId);
                if (contact != null) internshipVm.ContactPersonName = $"{contact.Firstname} {contact.Surname}";

                foreach (var specialisation in internshipVm.InternshipSpecialisation)
                {
                    var internshipSpecialisation =
                        specialisations.Find(s => s.SpecialisationId == specialisation.SpecialisationId);
                    if (internshipSpecialisation != null) specialisation.Description = internshipSpecialisation.Description;
                }

                foreach (var environment in internshipVm.InternshipEnvironment)
                {
                    var internshipEnvironment = environments.Find(e => e.EnvironmentId == environment.EnvironmentId);
                    if (internshipEnvironment != null) environment.Description = internshipEnvironment.Description;
                }
            }
            return internshipVms;
        }

        public async Task<InternshipVM> GetInternshipByIdAsyncVm(int id)
        {
            var internshipVm = await _internshipBll.GetByIdAsyncVm(id);
            var company = await _companyBll.GetByIdAsync(internshipVm.CompanyId);
            if (company != null)
            {
                internshipVm.CompanyName = company.Name;
                // internshipVm.Email = company.Email;
                var eventCompany = await GetCompanyFromActiveEventByIdAsync(company.CompanyId);
                internshipVm.Website = eventCompany?.Website;
            }
            
            var contact = (await _internshipBll.GetContactsAsyncVm()).FirstOrDefault(c => c.ContactId == internshipVm.ContactPersonId);
            if (contact != null)
            {
                internshipVm.ContactPersonName = $"{contact.Firstname} {contact.Surname}";
                internshipVm.Email = contact.Email;
            }

            var specialisations = await _internshipBll.GetSpecializationsForFilterAsync();
            foreach (var specialisation in internshipVm.InternshipSpecialisation)
            {
                var internshipSpecialisation =
                    specialisations.Find(s => s.SpecialisationId == specialisation.SpecialisationId);
                if (internshipSpecialisation != null) specialisation.Description = internshipSpecialisation.Description;
            }

            var environments = await _internshipBll.GetEnvironmentsForFilterAsync();
            foreach (var environment in internshipVm.InternshipEnvironment)
            {
                var internshipEnvironment = environments.Find(e => e.EnvironmentId == environment.EnvironmentId);
                if (internshipEnvironment != null) environment.Description = internshipEnvironment.Description;
            }

            return internshipVm;
        }

        public async Task<List<InternshipVM>> GetInternshipsByCompanyFromActiveEventAsyncVm()
        {
            var e = await GetActiveAsync();
            var academicYear = await _academicYearBll.GetByIdAsync(e.AcademicYearId);
            var companyId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            var companyName = Helper.GetClaimFromToken<string>(_httpContextAccessor, "companyName");
            var internshipVms =
                (List<InternshipVM>) await _internshipBll.GetInternshipsFromEventByCompanyAsyncVm(
                    academicYear.Description, companyId);

            if (companyName == null) return internshipVms;
            internshipVms.ForEach(i => i.CompanyName = companyName);
            return internshipVms;
        }

        public async Task<List<InternshipVM>> GetInternshipsByCompanyIdFromActiveEventAsyncVm(int id)
        {
            var e = await GetActiveAsync();
            var academicYear = await _academicYearBll.GetByIdAsync(e.AcademicYearId);
            var internshipVms =
                (List<InternshipVM> )await _internshipBll.GetInternshipsFromEventByCompanyAsyncVm(
                    academicYear.Description, id);

            internshipVms = internshipVms.Where(i => i.ShowInEvent).ToList();

            foreach (var internshipVm in internshipVms)
            {
                var company = await _companyBll.GetByIdAsync(internshipVm.CompanyId);
                if (company != null)
                {
                    internshipVm.CompanyName = company.Name;
                    internshipVm.Email = company.Email;
                    var eventCompany = await GetCompanyFromActiveEventByIdAsync(company.CompanyId);
                    internshipVm.Website = eventCompany?.Website;
                }
            }

            return internshipVms;
        }



        #endregion

        #region Appointment

        public async Task<List<AppointmentVM>> GetAllAppointmentsAsyncVm()
        {
            var e = await GetActiveAsync();
            var appointments = await _appointmentBll.GetAllAsync(e.Id);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
                var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
                if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
            }

            return appointmentsVms;
        }


        public async Task<List<AppointmentVM>> GetAllConfirmedAppointmentsAsyncVm()
        {
            var e = await GetActiveAsync();
            var appointments = await _appointmentBll.GetAllConfirmedAsync(e.Id);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
                var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
                if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
            }

            return appointmentsVms;
        }

        public async Task<List<AppointmentVM>> GetAllAppointmentsByStatusAsyncVm(List<string> allStatus)
        {
            var status = GetListAppointmentStatus(allStatus);
            var e = await GetActiveAsync();
            var appointments = await _appointmentBll.GetAllByStatusAsync(e.Id, status);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
                var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
                if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
            }

            return appointmentsVms;
        }

        public async Task<AppointmentVM> GetAppointmentsByIdAsyncVm(Guid id)
        {

            var appointment = await _appointmentBll.GetById(id);
            var appointmentVm = _mapper.Map<AppointmentVM>(appointment);

            var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
            if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
            var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
            if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
            var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
            if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";

            return appointmentVm;
        }

        public async Task<List<AppointmentVM>> GetAppointmentsFromCompanyAsyncVm()
        {
            var e = await GetActiveAsync();
            var companyId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            var appointments = await _appointmentBll.GetByCompanyAsync(e.Id, companyId);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
                var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
                if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
            }

            return appointmentsVms;
        }

        public async Task<List<AppointmentVM>> GetAllAppointmentsFromCompanyByStatusAsyncVm(List<string> allStatus)
        {
            var status = GetListAppointmentStatus(allStatus);
            var e = await GetActiveAsync();
            var companyId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            var appointments = await _appointmentBll.GetByCompanyAsyncByStatus(e.Id, companyId, status);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
                var student = await _studentBll.GetByIdAsync(appointmentVm.StudentId);
                if (student != null) appointmentVm.StudentName = $"{student.UserFirstName} {student.UserSurname}";
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
            }

            return appointmentsVms;
        }

        public async Task<List<AppointmentWithoutStudentDataVM>> GetAppointmentsByCompanyByStatusAsyncVm(int id, List<string> allStatus)
        {
            var status = GetListAppointmentStatus(allStatus);
            var e = await GetActiveAsync();
            var appointments = await _appointmentBll.GetByCompanyAsyncByStatus(e.Id, id, status);
            var appointmentsVms = _mapper.Map<List<AppointmentWithoutStudentDataVM>>(appointments);
            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
            }
            return appointmentsVms;
        }

        public async Task<List<AppointmentVM>> GetAppointmentsFromStudentAsyncVm()
        {
            var e = await GetActiveAsync();
            var studentId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "nameid");
            var appointments = await _appointmentBll.GetByStudentAsync(e.Id, studentId);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);
            
            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
            }

            return appointmentsVms;
        }

        public async Task<List<AppointmentVM>> GetAllAppointmentsForStudentByStatusAsyncVm(List<string> allStatus)
        {
            var e = await GetActiveAsync();
            var studentId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "nameid");
            var status = GetListAppointmentStatus(allStatus);
            var appointments = await _appointmentBll.GetByStudentAsyncByStatus(e.Id, studentId, status);
            var appointmentsVms = _mapper.Map<List<AppointmentVM>>(appointments);

            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
                var attendee = await _attendeeBll.GetByIdAsync(appointmentVm.AttendeeId);
                if (attendee != null) appointmentVm.AttendeeName = $"{attendee.FirstName} {attendee.LastName}";
                var internship = await _internshipBll.GetByIdAsync(appointmentVm.InternshipId);
                if (internship != null) appointmentVm.ResearchTopicTitle = internship.ResearchTopicTitle;
            }

            return appointmentsVms;
        }

        private List<AppointmentStatus> GetListAppointmentStatus(List<string> allStatus)
        {
            var status = new List<AppointmentStatus>();
            allStatus.ForEach(s =>
            {
                var statusReturned = (AppointmentStatus)System.Enum.Parse(typeof(AppointmentStatus), s.ToUpper());
                status.Add(statusReturned);
            });

            return status;
        }

        public async Task<List<AppointmentWithoutStudentDataVM>> GetAppointmentsByCompanyAsyncVm(int id)
        {
            var e = await GetActiveAsync();
            var appointments = await _appointmentBll.GetByCompanyAsync(e.Id, id);
            var appointmentsVms = _mapper.Map<List<AppointmentWithoutStudentDataVM>>(appointments);
            foreach (var appointmentVm in appointmentsVms)
            {
                var company = await _companyBll.GetByIdAsync(appointmentVm.CompanyId);
                if (company != null) appointmentVm.CompanyName = company.Name;
            }
            return appointmentsVms;
        }

        public async Task<AppointmentVM> CreateAppointmentAsyncVm(AppointmentVM appointmentVM)
        {
            var e = await GetActiveAsync();
            var alreadyExist = await _appointmentBll.CheckOnExistingAppointmentAsync(e.Id, appointmentVM.CompanyId, appointmentVM.AttendeeId, appointmentVM.BeginHour);
            if (alreadyExist != null) return null;
            var appointment = _mapper.Map<Appointment>(appointmentVM);
            var studentId = Helper.GetClaimFromToken<int>(_httpContextAccessor, "nameid");
            appointment.StudentId = studentId;
            appointment.EventId = e.Id;
            await _appointmentBll.CreateAppointmentAsync(appointment);

            return _mapper.Map<AppointmentVM>(appointment);
        }

        public async Task<AppointmentVM> UpdateAppointmentAsyncVm(AppointmentVM appointmentVm)
        {
            var appointmentToUpdate = await _appointmentBll.GetById(appointmentVm.Id);
            appointmentToUpdate = _mapper.Map(appointmentVm, appointmentToUpdate);
            await _appointmentBll.UpdateAppointmentAsync(appointmentToUpdate);
            appointmentToUpdate = await _appointmentBll.GetById(appointmentVm.Id);

            return _mapper.Map<AppointmentVM>(appointmentToUpdate);
        }

        public async Task<AppointmentVM> DeleteAppointmentByIdAsyncVm(Guid id)
        {
            var appointment = await _appointmentBll.Delete(id);
            return _mapper.Map<AppointmentVM>(appointment);
        }

        #endregion

        #region Files

        public async Task<List<FileStorage>> GetFilesAsync(Guid id)
        {
            var appointment = await _appointmentBll.GetById(id);
            var path = $"{appointment.Id}\\";

            try
            {
                var filesReturned = await _fileStorage.GetFilesAsync(path);
                return filesReturned;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FileStream> GetFileAsync(Guid id, string fileName)
        {
            var appointment = await _appointmentBll.GetById(id);
            var path = $"{appointment.Id}\\";

            try
            {
                var fileReturned = await _fileStorage.GetFileAsync(path, fileName);
                return fileReturned;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FileStorage> UploadFileAsync(Guid id, IFormFile file)
        {
            var appointment = await _appointmentBll.GetById(id);
            // TODO return if not exists ??
            var path = $"{appointment.Id}\\{file.FileName}";

            try
            {
                await using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileReturned = await _fileStorage.UploadFilesAsync(path, stream, file.ContentType);
                return fileReturned;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Logo

        public async Task<FileStorage> UploadLogoAsync(IFormFile file)
        {
            var id = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            var pathToUpload = $"logos\\{id}\\{file.FileName}";
            var pathToGetAll = $"logos\\{id}\\";

            try
            {
                var filesReturned = await _fileStorage.GetFilesAsync(pathToGetAll);
                foreach (var fileStorage in filesReturned)
                {
                    await _fileStorage.DeleteFileAsync(fileStorage.RelativePath);
                }

                await using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileReturned = await _fileStorage.UploadFilesAsync(pathToUpload, stream, file.ContentType);
                return fileReturned;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<FileStream> GetLogoAsync(int id)
        {
            var path = $"logos\\{id}\\";

            try
            {
                var filesReturned = await _fileStorage.GetFilesAsync(path);
                var fileReturned = filesReturned.FirstOrDefault();
                if (fileReturned == null)
                {
                    return null;
                }
                var logoReturned = await _fileStorage.GetFileAsync(path, fileReturned.FileName);
                
                return logoReturned;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<AttendeeVM> CreateAttendeeAsyncVm(AttendeeVM attendeeVm)
        {
            var attendee = _mapper.Map<Attendee>(attendeeVm);
            var id = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            var eventCompany = await GetCompanyFromActiveEventByIdAsync(id);
            attendee.EventCompanyId = eventCompany.Id; 
            await _attendeeBll.CreateAttendeeAsync(attendee);

            return _mapper.Map<AttendeeVM>(attendee);
        }

        #endregion
    }
}