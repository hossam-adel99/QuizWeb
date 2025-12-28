using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Models
{
    public class Option
    {
        public int Id { get; set; }

        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        public string OptionText { get; set; }

        [JsonIgnore]
        public virtual Question? Question { get; set; }
    }
}
