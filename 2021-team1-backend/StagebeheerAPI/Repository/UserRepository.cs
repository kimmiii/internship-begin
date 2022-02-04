using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace StagebeheerAPI.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) 
        {           

        }

        public string returnRoleCode(int userId)
        {
            var user = FindByCondition(us => us.UserId == userId)
                .Include(item => item.Role)
                .FirstOrDefault();
            return user.Role.Code;     
        }

        public string ReturnUserName(int userId)
        {
            var user = FindByCondition(us => us.UserId == userId)
                .Include(r => r.Role)
                .FirstOrDefault();

            if (user.Role.Code == "COM")
            {
                MailAddress addr = new MailAddress(user.UserEmailAddress);
                return addr.User;
            } else
            {
                var userName = user.UserFirstName + " " + user.UserSurname;
                return userName;
            }     
        }

        public string CheckReviewerHeader(List<string> headers)
        {
            string headerError = null; 
            headerError =
                           (!headers.Exists(x => x == "voornaam")) ? "voornaam veld niet aanwezig in invoerbestand." :
                           (!headers.Exists(x => x == "naam")) ? "naam veld niet aanwezig in invoerbestand." :
                           (!headers.Exists(x => x == "e-mailadres")) ? "mailadres veld niet aanwezig in invoerbestand." :
                           null;
            return headerError;
        }

        public string CheckStudentHeader(List<string> headers)
        {
            string headerError = null; 
            headerError =
                            (!headers.Exists(x => x == "voornaam")) ? "voornaam veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "naam")) ? "naam veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "straat")) ? "straat veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "huisnr")) ? "huisnr veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "bus")) ? "bus veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "pc")) ? "pc veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "gemeente")) ? "gemeente veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "gsmnummer")) ? "gsmnummer veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "e-mailadres")) ? "mailadres veld niet aanwezig in invoerbestand." :
                            (!headers.Exists(x => x == "afstudeerrichting")) ? "afstudeerrichting veld niet aanwezig in invoerbestand." :
                            null;
            return headerError;
        }


        public string CheckReviewerData(string[] rows, int commas)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            string dataError = null; 
            dataError =
                                (!(commas == 2)) ? "invoerveld bevat ';'" :
                                (rows[0].ToString().Count() <= 0) ? "invoerveld 'Naam' is leeg" :
                                (rows[1].ToString().Count() <= 0) ? "invoerveld 'Voornaam' is leeg" :
                                (rows[2].ToString().Count() <= 0) ? "invoerveld 'e-mailadres' is leeg" :
                                (!(Regex.IsMatch((rows[2].ToString().ToLower()), pattern))) ? "e-mailadres opmaak is niet correct" :
                                null;
            return dataError;
        }

        public string CheckStudentData(string[] rows,int commas)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            string dataError = null; 
            dataError =
                                (!(commas == 9)) ? "invoerveld bevat ';'" :
                                (rows[0].ToString().Count() <= 0) ? "invoerveld 'Naam' is leeg" :
                                (rows[1].ToString().Count() <= 0) ? "invoerveld 'Voornaam' is leeg" :
                                (rows[8].ToString().Count() <= 0) ? "invoerveld 'e-mailadres' is leeg" :
                                (!(Regex.IsMatch((rows[8].ToString().ToLower()), pattern))) ? "e-mailadres opmaak is niet correct" :
                                null;
            return dataError;
        }


      
    }
}
