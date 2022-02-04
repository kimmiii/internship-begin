using System.ComponentModel.DataAnnotations;

namespace StagebeheerAPI.ViewModels
{
    public class Register
    {
        [DataType(DataType.EmailAddress)]
        public string UserEmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string UserPass { get; set; }

        [Compare("UserPass")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
