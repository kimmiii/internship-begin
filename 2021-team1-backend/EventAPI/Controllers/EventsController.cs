using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.BLL;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Authorize]
    [Route("api/events")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IEventBll _eventBll;

        public EventsController(IEventBll eventBll)
        {
            _eventBll = eventBll;
        }

        /// <summary>
        /// Returns all events
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEventsAsync()
        {
            var events = await _eventBll.GetAsyncVm();
            return Ok(events);
        }

        /// <summary>
        ///  Creates event
        /// </summary>
        [Authorize(Policy = "CoordinatorOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventVM eventVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (eventVm == null) return BadRequest();
            eventVm = await _eventBll.CreateAsyncVm(eventVm);
            return Ok(eventVm);
        }

        /// <summary>
        /// Updates event
        /// </summary>
        [Authorize(Policy = "CoordinatorOnly")]
        [HttpPut]
        public async Task<IActionResult> UpdateEventAsync([FromBody] EventVM eventVm)
        {
            if (eventVm == null) return BadRequest();
            eventVm = await _eventBll.UpdateAsyncVm(eventVm);
            return Ok(eventVm);
        }

        /// <summary>
        /// Get active event
        /// </summary>
        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> GetActiveEventAsync()
        {
            var eventVm = await _eventBll.GetActiveAsyncVm();
            if (eventVm == null) return NoContent();
            return Ok(eventVm);
        }
      
        /// <summary>
        /// Returns EventCompany (token - active event)
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpGet]
        [Route("company")]
        public async Task<IActionResult> GetCompanyFromActiveEventAsync()
        {
            var eventCompany = await _eventBll.GetCompanyFromActiveEventAsyncVm();
            return Ok(eventCompany);
        }

        /// <summary>
        ///  Create eventCompany (token - active event)
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpPost]
        [Route("company")]
        public async Task<IActionResult> CreateEventCompany([FromBody] EventCompanyVM eventCompanyVm)
        {
            if (eventCompanyVm == null) return BadRequest();

            eventCompanyVm = await _eventBll.CreateEventCompanyAsyncVm(eventCompanyVm);
            return Ok(eventCompanyVm);
        }

        /// <summary>
        /// returns eventCompany by companyId
        /// </summary>
        [HttpGet]
        [Route("company/{id}")]
        public async Task<IActionResult> GetCompanyFromActiveEventByIdAsync([FromRoute] int id)
        {
            var eventCompany = await _eventBll.GetCompanyFromActiveEventByIdAsyncVm(id);
            return Ok(eventCompany);
        }

        /// <summary>
        /// Returns all eventCompanies from active event
        /// </summary>
        [HttpGet]
        [Route("company/all")]
        public async Task<IActionResult> GetCompaniesFromActiveEventAsync()
        {
            var eventCompanies = await _eventBll.GetCompaniesFromActiveEventAsyncVm();
            return Ok(eventCompanies);
        }

        /// <summary>
        ///  Creates attendee
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpPost]
        [Route("company/attendee")]
        public async Task<IActionResult> CreateAttendeeAsync([FromBody] AttendeeVM attendeeVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (attendeeVm == null) return BadRequest();
            attendeeVm = await _eventBll.CreateAttendeeAsyncVm(attendeeVm);
            return Ok(attendeeVm);
        }

        /// <summary>
        ///  Upload logo 
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpPost]
        [Route("company/logo")]
        public async Task<IActionResult> UploadLogoAsync([FromForm] IFormFile file)
        {

            var logo = await _eventBll.UploadLogoAsync(file);
            return Ok(logo);
        }

        /// <summary>
        ///  Get logo 
        /// </summary>
        [HttpGet]
        [Route("company/{id}/logo")]
        public async Task<IActionResult> DownloadLogoAsync([FromRoute] int id)
        {
            var logo = await _eventBll.GetLogoAsync(id);
            return Ok(logo);
        }
        /// <summary>
        /// Returns all internships from active event (approved and showInEvent)
        /// </summary>
        [HttpGet]
        [Route("internships")]
        public async Task<IActionResult> GetInternshipsByEventIdAsync()
        {
            var internshipsVm = await _eventBll.GetInternshipsFromActiveEventAsyncVm();
            return Ok(internshipsVm);
        }

        /// <summary>
        /// Returns internship by id
        /// </summary>
        [HttpGet]
        [Route("internships/{id}")]
        public async Task<IActionResult> GetInternshipsByIdAsync([FromRoute] int id)
        {
            var internshipVm = await _eventBll.GetInternshipByIdAsyncVm(id);
            return Ok(internshipVm);
        }

        /// <summary>
        /// Returns all internships (approved) from company(token) from active event
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpGet]
        [Route("internships/company")]
        public async Task<IActionResult> GetInternshipsByCompanyFromActiveEventAsync()
        {
            var internshipsVm = await _eventBll.GetInternshipsByCompanyFromActiveEventAsyncVm();
            return Ok(internshipsVm);
        }

        /// <summary>
        /// Returns all internships (approved) by companyId from active event
        /// </summary>
        [HttpGet]
        [Route("internships/company/{id}")]
        public async Task<IActionResult> GetInternshipsByCompanyIdFromActiveEventAsync([FromRoute] int id)
        {
            var internshipsVm = await _eventBll.GetInternshipsByCompanyIdFromActiveEventAsyncVm(id);
            return Ok(internshipsVm);
        }

        /// <summary>
        /// Create appointment in active event for student (token)
        /// </summary>
        [Authorize(Policy = "StudentOnly")]
        [HttpPost]
        [Route("appointments")]
        public async Task<IActionResult> CreateAppointmentAsync([FromBody]  AppointmentVM appointmentVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (appointmentVm == null) return BadRequest();
            appointmentVm = await _eventBll.CreateAppointmentAsyncVm(appointmentVm);
            return Ok(appointmentVm);
        }

        /// <summary>
        /// Update appointment
        /// </summary>
        [HttpPut]
        [Route("appointments")] 
        public async Task<IActionResult> UpdateAppointmentAsync([FromBody] AppointmentVM appointmentVm)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (appointmentVm == null) return BadRequest();
            appointmentVm = await _eventBll.UpdateAppointmentAsyncVm(appointmentVm);
            return Ok(appointmentVm);
        }

        /// <summary>
        /// Returns details appointment 
        /// </summary>
        [HttpGet]
        [Route("appointments/{id}")]
        public async Task<IActionResult> GetAppointmentsByIdAsync([FromRoute] Guid id)
        {
            var appointment = await _eventBll.GetAppointmentsByIdAsyncVm(id);
            return Ok(appointment);
        }

        /// <summary>
        ///  Delete appointment
        /// </summary>
        [HttpDelete]
        [Route("appointments/{id}")]
        public async Task<IActionResult> DeleteAppointmentByIdAsync([FromRoute] Guid id)
        {
            var appointment = await _eventBll.DeleteAppointmentByIdAsyncVm(id);
            return Ok(appointment);
        }


        /// <summary>
        ///  Get files from appointment
        /// </summary>
        [HttpGet]
        [Route("appointments/{id}/files")]
        public async Task<IActionResult> GetFilesAsync([FromRoute] Guid id)
        {
            var files = await _eventBll.GetFilesAsync(id);
            return Ok(files);
        }

        /// <summary>
        ///  Get single file from appointment
        /// </summary>
        [HttpGet]
        [Route("appointments/{id}/files/{fileName}")]
        public async Task<IActionResult> GetFileAsync([FromRoute] Guid id, [FromRoute] string fileName)
        {
            var file = await _eventBll.GetFileAsync(id, fileName);
            return Ok(file);
        }

        /// <summary>
        ///  Upload file  
        /// </summary>
        [HttpPost]
        [Route("appointments/{id}/files")]
        public async Task<IActionResult> UploadFilesAsync([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            var files = await _eventBll.UploadFileAsync(id, file);
            return Ok(files);
        }

        /// <summary>
        /// Get all appointment from company(token) from active event
        /// </summary>
        [Authorize(Policy = "CompanyOnly")]
        [HttpGet]
        [Route("appointments/company")]
        public async Task<IActionResult> GetAppointmentsFromCompanyAsync([FromQuery] string status)
        {
            List<AppointmentVM> appointments = null;
            if (status == null)
            {
                appointments = await _eventBll.GetAppointmentsFromCompanyAsyncVm();
                return Ok(appointments);
            }
            var allStatus = status.Split(',').ToList();
            appointments = await _eventBll.GetAllAppointmentsFromCompanyByStatusAsyncVm(allStatus);
            return Ok(appointments);
        }

        /// <summary>
        /// Get appointment by id from company from active event
        /// </summary>
        [HttpGet]
        [Route("appointments/company/{id}")]
        public async Task<IActionResult> GetAppointmentsByCompanyAsync([FromRoute] int id, [FromQuery] string status)
        {
            List<AppointmentWithoutStudentDataVM> appointments = null;
            if (status == null)
            {
                appointments = await _eventBll.GetAppointmentsByCompanyAsyncVm(id);
                return Ok(appointments);
            }
            var allStatus = status.Split(',').ToList();
            appointments = await _eventBll.GetAppointmentsByCompanyByStatusAsyncVm(id, allStatus);
            return Ok(appointments);
        }

        /// <summary>
        /// Get all appointments from student(token) from active event
        /// </summary>
        [Authorize(Policy = "StudentOnly")]
        [HttpGet]
        [Route("appointments/student")]
        public async Task<IActionResult> GetAppointmentsFromStudentAsync([FromQuery] string status)
        {
            List<AppointmentVM> appointments = null;
            if (status == null)
            {
                appointments = await _eventBll.GetAppointmentsFromStudentAsyncVm();
                return Ok(appointments);
            }
            var allStatus = status.Split(',').ToList();
            appointments = await _eventBll.GetAllAppointmentsForStudentByStatusAsyncVm(allStatus);
            return Ok(appointments);
        }

        /// <summary>
        /// Get all appointments by status from active event
        /// </summary>
        [Authorize(Policy = "CoordinatorOnly")]
        [HttpGet]
        [Route("appointments")]
        public async Task<IActionResult> GetAllAppointmentsByStatus([FromQuery] string status)
        {
            List<AppointmentVM> appointments = null;
            if (status == null)
            {
                appointments = await _eventBll.GetAllAppointmentsAsyncVm();
                return Ok(appointments);
            }
            var allStatus = status.Split(',').ToList();
            appointments = await _eventBll.GetAllAppointmentsByStatusAsyncVm(allStatus);
            return Ok(appointments);
        }
    }
}