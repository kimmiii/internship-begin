using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailService;
using Moq;
using NUnit.Framework;

namespace StagebeheerAPI.Tests.EmailService
{
    public class EmailServiceTests
    {
        //Mailservice refactor
        //Emailconfig door DI alvorens verder te kunnen testen

        [Test]
        public void EmailSender_SendEmail_Failed()
        {
            //Arrange

            EmailConfiguration emailConfiguration = new EmailConfiguration();
            emailConfiguration.SmtpServer = "";
            emailConfiguration.DebugEmailAddress = "";
            emailConfiguration.ErrorEmailAddress = "";
            emailConfiguration.From = "from";
            emailConfiguration.Password = "pw";
            emailConfiguration.Port = 111;
            emailConfiguration.UserName = "user";

            var emailSender = new EmailSender(emailConfiguration);

            List<string> mailTo = new List<string>();
            mailTo.Add("mailTo@mail.com");

            Message message = new Message(mailTo,"subject","content",null);

            //Act
            var returnValue = emailSender.SendEmail(message);

            //Assert
            Assert.IsFalse(returnValue);
        }


        [Test]
        public void EmailSenderAsync_SendEmail_Failed()
        {
            //Arrange

            EmailConfiguration emailConfiguration = new EmailConfiguration();
            emailConfiguration.SmtpServer = "";
            emailConfiguration.DebugEmailAddress = "";
            emailConfiguration.ErrorEmailAddress = "";
            emailConfiguration.From = "from";
            emailConfiguration.Password = "pw";
            emailConfiguration.Port = 111;
            emailConfiguration.UserName = "user";

            var emailSender = new EmailSender(emailConfiguration);

            List<string> mailTo = new List<string>();
            mailTo.Add("mailTo@mail.com");

            Message message = new Message(mailTo, "subject", "content", null);

            //Act
            var returnValue = emailSender.SendEmailAsync(message);

            //Assert
            Assert.IsTrue(returnValue.IsCompleted);
        }


    }
}
