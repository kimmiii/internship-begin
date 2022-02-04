using AutoMapper;
using EventAPI.BLL;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using EventAPI.Tests.Builders;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using EventAPI.Services;
using Environment = EventAPI.Domain.Models.Environment;
using Specialisation = EventAPI.Domain.Models.Specialisation;

namespace EventAPI.Tests.BLL
{
    class EventBllTests
    {
        private DefaultHttpContext _context;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<IEventRepository> _eventRepository;
        private Mock<IEventCompanyRepository> _eventCompanyRepository;
        private Mock<IAcademicYearBll> _academicYearBll;
        private Mock<IInternshipBll> _internshipBll;
        private Mock<ICompanyBll> _companyBll;
        private EventBll _eventBll;
        private Mock<IAttendeeBLL> _attendeeBll;
        private Mock<IStudentBLL> _studentBll;
        private Mock<IAppointmentBll> _appointmentBll;
        private Mock<IFileStorageService> _fileStorageService;
        private Mock<IContactRepository> _contactRepository;

        [SetUp]
        public void Setup()
        {
            _context = new DefaultHttpContext();
            _mapper = new Mock<IMapper>();
            _httpContextAccessor= new Mock<IHttpContextAccessor>();
            _eventRepository = new Mock<IEventRepository>();
            _eventCompanyRepository = new Mock<IEventCompanyRepository>();
            _academicYearBll = new Mock<IAcademicYearBll>();
            _internshipBll = new Mock<IInternshipBll>();
            _companyBll = new Mock<ICompanyBll>();
            _appointmentBll = new Mock<IAppointmentBll>();
            _studentBll = new Mock<IStudentBLL>();
            _attendeeBll = new Mock<IAttendeeBLL>();
            _fileStorageService = new Mock<IFileStorageService>();
            _contactRepository = new Mock<IContactRepository>();
            _eventBll = new EventBll(
                _mapper.Object, 
                _httpContextAccessor.Object, 
                _eventRepository.Object, 
                _eventCompanyRepository.Object, 
                _academicYearBll.Object, 
                _internshipBll.Object, 
                _companyBll.Object,
                _appointmentBll.Object,
                _attendeeBll.Object,
                _studentBll.Object,
                _fileStorageService.Object);
        }

        [Test]
        public void GetAsync_ReturnsAllEvents()
        {
            // Arrange
            IList<Event> events = new List<Event>();
            _eventRepository.Setup(b => b.GetAsync()).ReturnsAsync(events);

            // Act
            var result = _eventBll.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(events));
            _eventRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetAsyncVm_ReturnsAllEventsVm()
        {
            // Arrange
            IList<Event> events = new List<Event>();
            IList<EventVM> eventVms = new List<EventVM>();
            _mapper.Setup(m => m.Map<IList<EventVM>>(events)).Returns(eventVms);

            // Act
            var result = _eventBll.GetAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(eventVms));
            _mapper.Verify(r => r.Map<IList<EventVM>>(It.IsAny<IList<Event>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_ReturnsEventDetail()
        {
            // Arrange
            var e = new EventBuilder().WithId.Build;
            _eventRepository.Setup(b => b.GetByIdAsync(e.Id)).ReturnsAsync(e);

            // Act
            var result = _eventBll.GetByIdAsync(e.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(e));
            _eventRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetByIdAsyncVm_ReturnsEventDetail()
        {
            // Arrange
            var e = new EventBuilder().WithId.Build;
            var eventVm = new EventVmBuilder().FromEvent(e).Build;
            _eventRepository.Setup(b => b.GetByIdAsync(e.Id)).ReturnsAsync(e);
            _mapper.Setup(m => m.Map<EventVM>(e)).Returns(eventVm);

            // Act
            var result = _eventBll.GetByIdAsyncVm(e.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventVm));
            _eventRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mapper.Verify(r => r.Map<EventVM>(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void GetActiveAsync_ReturnsActiveEventDetail()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);

            // Act
            var result = _eventBll.GetActiveAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(e));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
        }

        [Test]
        public void GetActiveAsyncVm_ReturnsActiveEventDetailVm()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            var eventVm = new EventVmBuilder().FromEvent(e).Build;
            _mapper.Setup(m => m.Map<EventVM>(e)).Returns(eventVm);

            // Act
            var result = _eventBll.GetActiveAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventVm));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _mapper.Verify(r => r.Map<EventVM>(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void CreateAsync_CreatesValidEvent()
        {
            // Arrange
            var e = new EventBuilder().Build;
            _eventRepository.Setup(b => b.CreateAsync(e)).ReturnsAsync(e);

            // Act
            var result = _eventBll.CreateAsync(e).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(e));
            _eventRepository.Verify(r => r.CreateAsync(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void CreateAsyncVm_CreatesNewEventVm()
        {
            // Arrange
            var e = new EventBuilder().Build;
            var eventVm = new EventVmBuilder().FromEvent(e).Build;
            _mapper.Setup(m => m.Map<Event>(eventVm)).Returns(e);
            _eventRepository.Setup(b => b.CreateAsync(e)).ReturnsAsync(e);
            _mapper.Setup(m => m.Map<EventVM>(e)).Returns(eventVm);

            // Act
            var result = _eventBll.CreateAsyncVm(eventVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventVm));
            _mapper.Verify(r => r.Map<EventVM>(It.IsAny<Event>()), Times.Once);
            _mapper.Verify(r => r.Map<Event>(It.IsAny<EventVM>()), Times.Once);
            _eventRepository.Verify(r => r.CreateAsync(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void UpdateAsync_UpdatesValidEvent()
        {
            // Arrange
            var e = new EventBuilder().WithId.Build;
            var eventToUpdate = e;
            _eventRepository.Setup(b => b.GetByIdAsync(e.Id)).ReturnsAsync(eventToUpdate);
            _mapper.Setup(m => m.Map(e, eventToUpdate)).Returns(eventToUpdate);
            _eventRepository.Setup(b => b.UpdateAsync(eventToUpdate));

            // Act
            var result = _eventBll.UpdateAsync(e).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventToUpdate));
            _eventRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(2));
            _mapper.Verify(r => r.Map(It.IsAny<Event>(), It.IsAny<Event>()), Times.Once);
            _eventRepository.Verify(r => r.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void UpdateAsyncVm_UpdatesEventVm()
        {
            // Arrange
            var e = new EventBuilder().WithId.Build;
            var eventVm = new EventVmBuilder().FromEvent(e).Build;
            _mapper.Setup(m => m.Map<Event>(eventVm)).Returns(e);
            _eventRepository.Setup(b => b.GetByIdAsync(e.Id)).ReturnsAsync(e);
            _mapper.Setup(m => m.Map<EventVM>(e)).Returns(eventVm);

            // Act
            var result = _eventBll.UpdateAsyncVm(eventVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventVm));
            _eventRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(2));
            _mapper.Verify(r => r.Map<EventVM>(It.IsAny<Event>()), Times.Once);
            _mapper.Verify(r => r.Map<Event>(It.IsAny<EventVM>()), Times.Once);
        }


        [Test]
        public void GetCompanyFromActiveEventAsync_ReturnsEventCompany()
        {
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var eventCompany = new EventCompanyBuilder().WithEventId(e.Id).WithCompanyId(companyId).Build;

            var allClaims = new[]
            {
                new Claim("companyId", companyId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventAndCompanyAsync(e.Id, companyId)).ReturnsAsync(eventCompany);

            // Act
            var result = _eventBll.GetCompanyFromActiveEventAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompany));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventAndCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetCompanyFromActiveEventByIdAsync_ReturnsEventCompany()
        {
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var eventCompany = new EventCompanyBuilder().WithEventId(e.Id).WithCompanyId(companyId).Build;
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventAndCompanyAsync(e.Id, companyId)).ReturnsAsync(eventCompany);

            // Act
            var result = _eventBll.GetCompanyFromActiveEventByIdAsync(companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompany));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventAndCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        }


        [Test]
        public void GetCompanyFromActiveEventAsyncVm_MapsReturnedEventCompanyToVM()
        {
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var eventCompany = new EventCompanyBuilder().WithEventId(e.Id).WithCompanyId(companyId).Build;
            var eventCompanyVm = new EventCompanyVMBuilder().FromEventCompany(eventCompany).Build;

            var allClaims = new[]
            {
                new Claim("companyId", companyId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventAndCompanyAsync(e.Id, companyId)).ReturnsAsync(eventCompany);
            _mapper.Setup(m => m.Map<EventCompanyVM>(eventCompany)).Returns(eventCompanyVm);

            // Act
            var result = _eventBll.GetCompanyFromActiveEventAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompanyVm));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventAndCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(m => m.Map<EventCompanyVM>(It.IsAny<EventCompany>()), Times.Once);
        }

        

        [Test]
        public void GetCompanyFromActiveEventByIdAsyncVm_MapsReturnedEventCompanyToVM()
        {
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var eventCompany = new EventCompanyBuilder().WithEventId(e.Id).WithCompanyId(companyId).Build;
            var eventCompanyVm = new EventCompanyVMBuilder().FromEventCompany(eventCompany).Build;

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventAndCompanyAsync(e.Id, companyId)).ReturnsAsync(eventCompany);
            _mapper.Setup(m => m.Map<EventCompanyVM>(eventCompany)).Returns(eventCompanyVm);

            // Act
            var result = _eventBll.GetCompanyFromActiveEventByIdAsyncVm(companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompanyVm));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventAndCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(m => m.Map<EventCompanyVM>(It.IsAny<EventCompany>()), Times.Once);
        }

        [Test]
        public void GetCompaniesFromActiveEventAsync_ReturnsAllCompaniesFromEvent()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var eventCompanies = new EventCompanyListBuilder().WithEventId(e.Id).Build;
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventIdAsync(e.Id)).ReturnsAsync(eventCompanies);

            // Act
            var result = _eventBll.GetCompaniesFromActiveEventAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompanies));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetCompaniesFromActiveEventAsyncVm_MapsReturnedToVm()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var eventCompanies = new EventCompanyListBuilder().WithEventId(e.Id).Build;
            var eventCompanyVms = new List<EventCompanyVM>();
            eventCompanies.ForEach(ev =>
            {
                var eventCompany = new EventCompanyVMBuilder().FromEventCompany(ev).Build;
                _mapper.Setup(m => m.Map<EventCompanyVM>(ev)).Returns(eventCompany);
                eventCompanyVms.Add(eventCompany);
            });

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _eventCompanyRepository.Setup(b => b.GetByEventIdAsync(e.Id)).ReturnsAsync(eventCompanies);
            var count = eventCompanies.Count;

            // Act
            var result = _eventBll.GetCompaniesFromActiveEventAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(eventCompanyVms));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _eventCompanyRepository.Verify(r => r.GetByEventIdAsync(It.IsAny<Guid>()), Times.Once);
            _mapper.Verify(m => m.Map<EventCompanyVM>(It.IsAny<EventCompany>()), Times.Exactly(count));
        }

        [Test]
        public void CreateEventCompanyAsync_CreateValidEventCompany()
        {
            // Arrange
            var eventCompany = new EventCompanyBuilder().Build;
            _eventCompanyRepository.Setup(b => b.CreateAsync(eventCompany)).ReturnsAsync(eventCompany);

            // Act
            var result = _eventBll.CreateEventCompanyAsync(eventCompany).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompany));
            _eventCompanyRepository.Verify(r => r.CreateAsync(It.IsAny<EventCompany>()), Times.Once);
        }

        [Test]
        public void CreateEventCompanyAsyncVm_MapsToVm()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var eventCompany = new EventCompanyBuilder().WithEventId(e.Id).WithCompanyId(companyId).Build;

            var allClaims = new[]
            {
                new Claim("companyId", companyId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);

            var eventCompanyVm = new EventCompanyVMBuilder().FromEventCompany(eventCompany).Build;
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _mapper.Setup(m => m.Map<EventCompany>(eventCompanyVm)).Returns(eventCompany);
            _eventCompanyRepository.Setup(b => b.CreateAsync(eventCompany)).ReturnsAsync(eventCompany);
            _mapper.Setup(m => m.Map<EventCompanyVM>(eventCompany)).Returns(eventCompanyVm);

            // Act
            var result = _eventBll.CreateEventCompanyAsyncVm(eventCompanyVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(eventCompanyVm));
            _mapper.Verify(r => r.Map<EventCompany>(It.IsAny<EventCompanyVM>()), Times.Once);
            _mapper.Verify(r => r.Map<EventCompanyVM>(It.IsAny<EventCompany>()), Times.Once);
            _eventCompanyRepository.Verify(r => r.CreateAsync(It.IsAny<EventCompany>()), Times.Once);
        }

        [Test]
        public void GetInternshipsFromActiveEventAsyncVm_MapsReturnedInternshipToVm()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var e = new EventBuilder().WithId.IsActivated.WithAcademicYear(academicYear.Id).Build;
            var contacts = new List<Contact>(); 
            var environments = new List<Environment>(); 
            var specializations = new List<Specialisation>(); 
            var internshipVms = new InternshipVMListBuilder().WithAcademicYear(academicYear.Description).Build;
           
            internshipVms.ForEach(i => i.InternshipEnvironment = new List<EnvironmentVM>());
            internshipVms.ForEach(i => i.InternshipSpecialisation = new List<SpecialisationVM>());
            internshipVms.ForEach(a => _companyBll.Setup(b => b.GetByIdAsync(a.CompanyId)).ReturnsAsync(It.IsAny<Company>()));
            
            var count = internshipVms.Count;

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _academicYearBll.Setup(b => b.GetByIdAsync(academicYear.Id)).ReturnsAsync(academicYear);
            _internshipBll.Setup(b => b.GetByAcademicYearAsyncVm(academicYear.Description)).ReturnsAsync(internshipVms);
            _internshipBll.Setup(b => b.GetContactsAsync()).ReturnsAsync(contacts);
            _internshipBll.Setup(b => b.GetEnvironmentsForFilterAsync()).ReturnsAsync(environments);
            _internshipBll.Setup(b => b.GetSpecializationsForFilterAsync()).ReturnsAsync(specializations);
            

            // Act
            var result = _eventBll.GetInternshipsFromActiveEventAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _academicYearBll.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _internshipBll.Verify(r => r.GetByAcademicYearAsyncVm(It.IsAny<string>()), Times.Once);
            _internshipBll.Verify(r => r.GetContactsAsync(), Times.Once);
            _internshipBll.Verify(r => r.GetEnvironmentsForFilterAsync(), Times.Once);
            _internshipBll.Verify(r => r.GetSpecializationsForFilterAsync(), Times.Once);
            _companyBll.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(count));

        }

        [Test]
        public void GetInternshipsByCompanyFromActiveEventAsyncVm_MapsReturnedEventCompanyToVM()
        {
            var academicYear = new AcademicYear();
            var e = new EventBuilder().WithId.IsActivated.WithAcademicYear(academicYear.Id).Build;
            var company = new Company();
            company.Name = Guid.NewGuid().ToString();
            var internshipVms = new InternshipVMListBuilder().WithCompanyId(company.CompanyId).WithAcademicYear(academicYear.Description).Build;
            internshipVms.ForEach(a => _companyBll.Setup(b => b.GetByIdAsync(a.CompanyId)).ReturnsAsync(It.IsAny<Company>()));

            var allClaims = new[]
            {
                new Claim("companyId", company.CompanyId.ToString(), ClaimValueTypes.Integer32),
                new Claim("companyName", company.Name)}.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _academicYearBll.Setup(b => b.GetByIdAsync(academicYear.Id)).ReturnsAsync(academicYear);
            _internshipBll.Setup(b => b.GetInternshipsFromEventByCompanyAsyncVm(academicYear.Description, company.CompanyId)).ReturnsAsync(internshipVms);

            // Act
            var result = _eventBll.GetInternshipsByCompanyFromActiveEventAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _academicYearBll.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _internshipBll.Verify(r => r.GetInternshipsFromEventByCompanyAsyncVm(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }


        [Test]
        public void GetAppointmentsFromCompanyVM_ReturnsAppointmentsFromCompany()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var appointments = new AppointmentListBuilder().WithCompanyIdAndEventId(companyId, e.Id).Build;
            var appointmentVMs = new List<AppointmentVM>();
            appointments.ForEach(a => appointmentVMs.Add(new AppointmentVmBuilder().FromAppointment(a).Build));
            var allClaims = new[]
            {
                new Claim("companyId", companyId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);


            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _appointmentBll.Setup(b => b.GetByCompanyAsync(e.Id, companyId)).ReturnsAsync(appointments);
            _mapper.Setup(m => m.Map<List<AppointmentVM>>(appointments)).Returns(appointmentVMs);

            // Act
            var result = _eventBll.GetAppointmentsFromCompanyAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(appointmentVMs));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _appointmentBll.Verify(r => r.GetByCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<List<AppointmentVM>>(It.IsAny<List<Appointment>>()), Times.Once);

        }

        [Test]
        public void GetAppointmentsByIdAsyncVm()
        {
            // Arrange
            var appointment = new Appointment();
            var appointmentVm = new AppointmentVmBuilder().FromAppointment(appointment).Build;

            _appointmentBll.Setup(b => b.GetById(appointment.Id)).ReturnsAsync(appointment);
            _mapper.Setup(m => m.Map<AppointmentVM>(appointment)).Returns(appointmentVm);

            // Act
            var result = _eventBll.GetAppointmentsByIdAsyncVm(appointment.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointmentVm));
            _appointmentBll.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            _mapper.Verify(r => r.Map<AppointmentVM>(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public void GetAppointmentsFromStudentVM_ReturnsAppointmentsFromStudent()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var studentId = new Random().Next();
            var appointments = new AppointmentListBuilder().WithStudentIdAndEventId(studentId, e.Id).Build;
            var appointmentVMs = new List<AppointmentVM>();
            appointments.ForEach(a => appointmentVMs.Add(new AppointmentVmBuilder().FromAppointment(a).Build));
            var allClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, studentId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);


            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _appointmentBll.Setup(b => b.GetByStudentAsync(e.Id, studentId)).ReturnsAsync(appointments);
            _mapper.Setup(m => m.Map<List<AppointmentVM>>(appointments)).Returns(appointmentVMs);

            // Act
            var result = _eventBll.GetAppointmentsFromStudentAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(appointmentVMs));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _appointmentBll.Verify(r => r.GetByStudentAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<List<AppointmentVM>>(It.IsAny<List<Appointment>>()), Times.Once);
        }

        [Test]
        public void GetAppointmentsByCompanyVM_ReturnsAppointmentsByCompany()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var companyId = new Random().Next();
            var appointments = new AppointmentListBuilder().WithCompanyIdAndEventId(companyId, e.Id).Build;
            var appointmentVMs = new List<AppointmentWithoutStudentDataVM>();
            appointments.ForEach(a => appointmentVMs.Add(new AppointmentWithoutStudentDataVMBuilder().FromAppointment(a).Build));
            

            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _appointmentBll.Setup(b => b.GetByCompanyAsync(e.Id, companyId)).ReturnsAsync(appointments);
            _mapper.Setup(m => m.Map<List<AppointmentWithoutStudentDataVM>>(appointments)).Returns(appointmentVMs);

            // Act
            var result = _eventBll.GetAppointmentsByCompanyAsyncVm(companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(appointmentVMs));
            _eventRepository.Verify(r => r.GetActiveAsync(), Times.Once);
            _appointmentBll.Verify(r => r.GetByCompanyAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<List<AppointmentWithoutStudentDataVM>>(It.IsAny<List<Appointment>>()), Times.Once);
        }


        [Test]
        public void CreateAppointmentAsyncVm_MapsCreatedAppointmentToVM()
        {
            // Arrange
            var e = new EventBuilder().WithId.IsActivated.Build;
            var studentId = new Random().Next();
            var appointment = new AppointmentBuilder().WithEventId(e.Id) .Build;
            var appointmentVm = new AppointmentVmBuilder().FromAppointment(appointment).Build;

            var allClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, studentId.ToString(), ClaimValueTypes.Integer32),
            }.ToList();

            var token = MockJwtTokens.GenerateJwtToken(allClaims);
            _context.Request.Headers["Authorization"] = token;
            _httpContextAccessor.Setup(o => o.HttpContext.Request.Headers["Authorization"]).Returns(token);
            _eventRepository.Setup(b => b.GetActiveAsync()).ReturnsAsync(e);
            _appointmentBll.Setup(b => b.CreateAppointmentAsync(appointment)).ReturnsAsync(appointment);
            _mapper.Setup(m => m.Map<AppointmentVM>(appointment)).Returns(appointmentVm);
            _mapper.Setup(m => m.Map<Appointment>(appointmentVm)).Returns(appointment);

            // Act
            var result = _eventBll.CreateAppointmentAsyncVm(appointmentVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointmentVm));
            _appointmentBll.Verify(r => r.CreateAppointmentAsync(It.IsAny<Appointment>()), Times.Once);
            _mapper.Verify(r => r.Map<AppointmentVM>(It.IsAny<Appointment>()), Times.Once);
            _mapper.Verify(r => r.Map<Appointment>(It.IsAny<AppointmentVM>()), Times.Once);
        }

        [Test]
        public void UpdateAppointmentAsyncVm_MapsUpdatedAppointmentToVM()
        {
            // Arrange
            var appointment = new AppointmentBuilder().Build;
            var appointmentVm = new AppointmentVmBuilder().FromAppointment(appointment).Build;
            var appointmentToUpdate = appointment;

            _appointmentBll.Setup(b => b.GetById(appointment.Id)).ReturnsAsync(appointmentToUpdate);
            _mapper.Setup(m => m.Map(appointment, appointmentToUpdate)).Returns(appointmentToUpdate);
            _appointmentBll.Setup(b => b.UpdateAppointmentAsync(appointmentToUpdate));
            _mapper.Setup(m => m.Map<AppointmentVM>(appointmentToUpdate)).Returns(appointmentVm);

            // Act
            var result = _eventBll.UpdateAppointmentAsyncVm(appointmentVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointmentVm));
            _appointmentBll.Verify(r => r.UpdateAppointmentAsync(It.IsAny<Appointment>()), Times.Once);
            _mapper.Verify(r => r.Map<AppointmentVM>(It.IsAny<Appointment>()), Times.Once);
        }

        //[Test]
        //public void UploadFilesAsync()
        //{
        //    // Arrange
        //    var appointment = new AppointmentBuilder().WithId.Build;

        //    var form = new Mock<IFormFile>();
        //    var path = Guid.NewGuid().ToString();
        //    var stream = new Mock<MemoryStream>();

        //    var filestorage = new FileStorage();

        //    _appointmentBll.Setup(b => b.GetById(appointment.Id)).ReturnsAsync(appointment);
        //    _fileStorageService.Setup(b => b.UploadFilesAsync(path, stream.Object, form.Object.ContentType)).ReturnsAsync(filestorage);

        //    // Act
        //    var result = _eventBll.UploadLogoAsync(appointment.Id, form.Object).Result;

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result, Is.SameAs(filestorage));
        //    _appointmentBll.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
        //    _fileStorageService.Verify(r => r.UploadFilesAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
        //}
    }
}
