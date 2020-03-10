using System.ComponentModel.DataAnnotations;

namespace AmitTextile.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Вы должны ввести ваш адрес")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 20 символов")]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}