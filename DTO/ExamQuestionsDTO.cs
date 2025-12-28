namespace Quiz.DTO
{
    public class AssignQuestionsToExamRequest
    {
        public int ExamID { get; set; }
        public List<ExamQuestionAssignmentDto> Questions { get; set; }
    }

    public class ExamQuestionAssignmentDto
    {
        public int QuestionID { get; set; }
        public int Points { get; set; }
    }
}
