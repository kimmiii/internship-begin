using AutoMapper;
using EventAPI.BLL;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Tests.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace EventAPI.Tests.BLL
{
    class AppointmentBllTests
    {
        private Mock<IMapper> _mapper;
        private Mock<IAppointmentRepository> _appointmentRepository;
        private AppointmentBll _appointmentBll;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _appointmentBll = new AppointmentBll(_mapper.Object, _appointmentRepository.Object);
        }

        [Test]
        public void GetById_ReturnsAppointmentDetail()
        {
            // Arrange
            var appointment = new AppointmentBuilder().Build;
            _appointmentRepository.Setup(b => b.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);

            // Act
            var result = _appointmentBll.GetById(appointment.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointment));
            _appointmentRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetByCompanyAsync_ReturnsAllAppointmentsFromCompany()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var companyId = new Random().Next();
            var appointments = new AppointmentListBuilder().WithCompanyIdAndEventId(companyId, eventId).Build;
            _appointmentRepository.Setup(b => b.FindByCondition(It.IsAny<Expression<Func<Appointment, bool>>>())).ReturnsAsync(appointments);

            // Act
            var result = _appointmentBll.GetByCompanyAsync(eventId, companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(appointments));
            _appointmentRepository.Verify(r => r.FindByCondition(It.IsAny<Expression<Func<Appointment, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByStudentAsync_ReturnsAllAppointmentsFromStudent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = new Random().Next();
            var appointments = new AppointmentListBuilder().WithCompanyIdAndEventId(studentId, eventId).Build;
            _appointmentRepository.Setup(b => b.FindByCondition(It.IsAny<Expression<Func<Appointment, bool>>>())).ReturnsAsync(appointments);

            // Act
            var result = _appointmentBll.GetByStudentAsync(eventId, studentId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(appointments));
            _appointmentRepository.Verify(r => r.FindByCondition(It.IsAny<Expression<Func<Appointment, bool>>>()), Times.Once);
        }

        [Test]
        public void CreateAppointmentAsync_CreatesAppointment()
        {
            // Arrange
            var appointment = new AppointmentBuilder().Build;
            _appointmentRepository.Setup(b => b.CreateAsync(appointment)).ReturnsAsync(appointment);

            // Act
            var result = _appointmentBll.CreateAppointmentAsync(appointment).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointment));
            _appointmentRepository.Verify(r => r.CreateAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public void UpdateAppointmentAsync()
        {
            // Arrange
            var appointment = new AppointmentBuilder().Build;
            _appointmentRepository.Setup(b => b.UpdateAsync(appointment)).ReturnsAsync(appointment);

            // Act
            var result = _appointmentBll.UpdateAppointmentAsync(appointment).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(appointment));
            _appointmentRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
