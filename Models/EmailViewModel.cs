using System.ComponentModel.DataAnnotations;

namespace AmitTextile.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Вы должны ввести ваш адрес")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
    }
}