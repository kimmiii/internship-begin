using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System.Linq;

namespace StagebeheerAPI.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }


        public bool ContactValidEmail(string ContactEmail)
        {           
            Contact dbcontactEmail = FindByCondition(x => x.Email.ToLower().Equals(ContactEmail)).FirstOrDefault();
            return (dbcontactEmail == null ? true : false);
        }

        public bool ContactValidPhoneNumber(string contactPhoneNumber)
        {
            Contact dbcontactPhoneNumber = FindByCondition(x => x.PhoneNumber.ToLower().Equals(contactPhoneNumber)).FirstOrDefault();
            return (dbcontactPhoneNumber == null ? true : false);        
        }

        //public bool ContactExists(int id)
        //{
        //    return FindByCondition(x => x.ContactId == id).Any(e => e.ContactId == id);
        //}


    }
   
}
