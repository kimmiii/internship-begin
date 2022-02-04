using StagebeheerAPI.Models;

namespace StagebeheerAPI.Contracts
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        public bool ContactValidEmail(string ContactEmail);
        public bool ContactValidPhoneNumber(string contactPhoneNumber);
    }
}
