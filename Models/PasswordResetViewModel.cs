using System.ComponentModel.DataAnnotations;

namespace AmitTextile.Models
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 20 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Вы должны подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}