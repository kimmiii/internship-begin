using EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Configuration.Constants;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IEmailSender _emailSender;

        public ContactController(IRepositoryWrapper repoWrapper, IEmailSender emailSender)
        {
            _repoWrapper = repoWrapper;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> PostInternship(Contact contact)
        {
            //if (!_repoWrapper.Contact.ContactValidEmail(contact.Email))
            //{
            //    var result = new Result();
            //    result.Status = Status.Error;
            //    result.Message = "E-Mail is reeds in gebruik.";
            //    return BadRequest(result);
            //}
            //else if (!_repoWrapper.Contact.ContactValidPhoneNumber(contact.PhoneNumber))
            //{
            //    var result = new Result();
            //    result.Status = Status.Error;
            //    result.Message = "Telefoon Nummer is reeds in gebruik.";
            //    return BadRequest(result);
            //}
            //else
            //{
                try
                {
                    if (contact != null)
                    {
                        _repoWrapper.Contact.Create(contact);
                        await Task.Run(() => _repoWrapper.Save());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }

                return CreatedAtAction(
                    actionName: "GetContact",
                    routeValues: new { id = contact.ContactId },
                    value: contact);
            //}
        }

        // GET api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {

            var user = await _repoWrapper.Contact.FindByCondition(x => x.ContactId == id)
                //.Include(i => i.Internships)      TODO: nakijken? Waarom moeten er internships bij een contact zitten?
                .FirstOrDefaultAsync(i => i.ContactId == id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // GET api/Contacts/5/Company
        [HttpGet("{companyId}/getByCompany")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContactByCompany(int companyId)
        {
            var companyContacts = _repoWrapper.Contact.FindByCondition(x => x.CompanyId == companyId && x.Activated == true);
            return await companyContacts.ToListAsync();
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            var existingContact = _repoWrapper.Contact.FindByCondition(x => x.ContactId == id)
                //.Include(i => i.Internships)   TODO: nakijken? Waarom moeten er internships bij een contact zitten?
                .FirstOrDefault();

            existingContact = contact;

            if (!ContactExists(id))
            {
                return NotFound();
            }
            else

                try
                {
                    _repoWrapper.Contact.Update(existingContact);
                    await Task.Run(() => _repoWrapper.Save());
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }
            return NoContent();
        }

        // PUT: api/Contacts/5/Remove
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}/Remove")]
        public async Task<IActionResult> SoftDeleteContact(int id)
        {
            var existingContact = _repoWrapper.Contact.FindByCondition(x => x.ContactId == id)
                //.Include(i => i.Internships)   TODO: nakijken? Waarom moeten er internships bij een contact zitten?
                .FirstOrDefault();

            if (!ContactExists(id))
            {
                return NotFound();
            }

            try
            {
                existingContact.Activated = false;
                _repoWrapper.Contact.Update(existingContact);
                await Task.Run(() => _repoWrapper.Save());

            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            return NoContent();

        }

        [HttpGet("SendReminderEmail/{reviewerId}/{internshipId}")]
        public async Task<IActionResult> SendReminderEmail(int reviewerId, int internshipId)
        {
            var result = new Result();
            var toReviewer = _repoWrapper.User.FindByCondition(x => x.UserId == reviewerId).FirstOrDefault();
            var internship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();
            
            if (toReviewer == null || internship == null)
            {
                return NotFound();
            }

            List<string> mailTo = new List<string>();
            mailTo.Add(toReviewer.UserEmailAddress);

            try
            {
                var subject = EmailMessages.reminderSubject;
                var body = EmailMessages.reminderBody(internship.ResearchTopicTitle);
                var message = new Message(mailTo, subject, body, null);
                _emailSender.SendEmail(message);
            }
            catch (Exception e)
            {
                result.Status = Status.Error;
                result.Message = e.InnerException.ToString();
                return BadRequest(result);
            }

            return NoContent();
        }


        private bool ContactExists(int id)
        {
            return _repoWrapper.Contact.FindByCondition(x => x.ContactId == id).Any(e => e.ContactId == id);
        }
    }
}