using System;
using System.ComponentModel.DataAnnotations;
using AmitTextile.ValidationAttributes;

namespace AmitTextile.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Вы должны ввести ваш адрес")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        
        [Fio(ErrorMessage = "Введите фамилию имя и отчество через пробелы")]
        public string Fio { get; set; }
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 20 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Вы должны подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}