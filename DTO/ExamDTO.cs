namespace Quiz.DTO
{
    public class ExamDTO
    {
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
