using System.ComponentModel.DataAnnotations;

namespace StagebeheerAPI.ViewModels
{
    public class Login
    {
        public string UserEmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
