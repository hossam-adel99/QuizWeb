using Quiz.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DTO
{
    public class QuestionBankAddDTO
    {
        [Display(Name = "Name")]
        [MinLength(2, ErrorMessage = "Name must be greater than one charachters")]
        [Required(ErrorMessage = "You must enter a name")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }
        [Required]
        public bool IsPrivate { get; set; }

        public ICollection<Question>? Questions { get; set; }

    }
}
