using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.DTO;
using Quiz.Models;
using Quiz.Repository;
using System;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamQuestionsController : ControllerBase
    {
        private readonly ExamQuestionsRepository examQuestionRepository;
        private readonly ExamRepository examRepository;
        private readonly ILogger<ExamQuestionsController> _logger;
        public ExamQuestionsController(ExamQuestionsRepository examQuestionRepository, ExamRepository examRepository, ILogger<ExamQuestionsController> logger)
        {
            this.examQuestionRepository = examQuestionRepository;
            this.examRepository = examRepository;
            _logger = logger;
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var examQuestions = examQuestionRepository.GetAll();
            return Ok(examQuestions);
        }


        [HttpGet("{examId:int}")]
        public IActionResult Get(int examId) // Get all Exam's Questions by Exam ID
        {
            var examQuestions = examQuestionRepository.GetAllByExamId(examId);
            if (examQuestions == null)
            {
                return NotFound();
            }
            return Ok(examQuestions);
        }

        [HttpPost("{examId:int}")]
        public IActionResult Post(int examId, [FromBody] AssignQuestionsToExamRequest request) // Assign multiple Questions to the Exam
        {
            if (request == null)
            {
                return BadRequest("ExamQuestionsDTO is null");
            }

            if (examId != request.ExamID)
            {
                return BadRequest("Exam ID in route does not match request body.");
            }


            if (request.Questions == null || !request.Questions.Any())
            {
                return BadRequest("At least one question is required.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var duplicateQuestionIds = request.Questions
            .GroupBy(q => q.QuestionID)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            if (duplicateQuestionIds.Any()) // Check for duplicate Question IDs
            {
                return BadRequest($"Duplicate QuestionIds found: {string.Join(", ", duplicateQuestionIds)}");
            }


            bool examExists = (examRepository.GetById(examId) != null);
            if (!examExists) // Check if the Exam exists
                return NotFound($"Exam with ID {examId} not found.");


            try
            {
                var examQuestions = request.Questions
                    .Select(q => new ExamQuestion
                    {
                        ExamID = examId,
                        QuestionID = q.QuestionID,
                        Points = q.Points
                    })
                    .ToList();
                foreach (var question in examQuestions)
                {
                    examQuestionRepository.Add(new ExamQuestion
                    {
                        ExamID = question.ExamID,
                        QuestionID = question.QuestionID,
                        Points = question.Points
                    });
                }

                examQuestionRepository.Save();

                // 5. Return success response
                return Ok(new
                {
                    ExamId = examId,
                    TotalAssigned = examQuestions.Count,
                    AssignedQuestionIds = examQuestions.Select(q => q.QuestionID)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign questions to exam {ExamId}", examId);
                return StatusCode(500, "An error occurred while assigning questions.");
            }
        }

        [HttpDelete("{examId:int}/{questionId:int}")]
        public IActionResult Delete(int examId, int questionId) // Remove a Question from the Exam
        {
            ExamQuestion? examQuestion = examQuestionRepository.GetAllByExamId(examId)
                .FirstOrDefault(eq => eq.QuestionID == questionId);
            if (examQuestion == null)
            {
                return NotFound($"ExamQuestion with Exam ID {examId} and Question ID {questionId} not found.");
            }
            examQuestionRepository.Remove(examQuestion.Id);
            examQuestionRepository.Save();
            return NoContent();
        }

        [HttpPut("{examId:int}")]
        public IActionResult Update(int examId, [FromBody] AssignQuestionsToExamRequest request) // Update the Exam's Questions
        {
            if (request == null)
            {
                return BadRequest("ExamQuestionsDTO is null");
            }
            if (examId != request.ExamID)
            {
                return BadRequest("Exam ID in route does not match request body.");
            }

            if (request.Questions == null || !request.Questions.Any())
            {
                return BadRequest("At least one question is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var duplicateQuestionIds = request.Questions
            .GroupBy(q => q.QuestionID)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            if (duplicateQuestionIds.Any()) // Check for duplicate Question IDs
            {
                return BadRequest($"Duplicate QuestionIds found: {string.Join(", ", duplicateQuestionIds)}");
            }

            bool examExists = (examRepository.GetById(examId) != null);
            if (!examExists) // Check if the Exam exists
                return NotFound($"Exam with ID {examId} not found.");



            try
            {
                // Get current questions in this exam
                var currentQuestions = examQuestionRepository.GetAllByExamId(examId);

                // Process each question in the request
                foreach (var requestedQuestion in request.Questions)
                {
                    var existing = currentQuestions.FirstOrDefault(cq => cq.QuestionID == requestedQuestion.QuestionID);

                    if (existing != null)
                    {
                        // Update points if question already exists in exam
                        existing.Points = requestedQuestion.Points;
                        examQuestionRepository.Update(existing.Id, existing);
                    }
                    else
                    {
                        // Add new question to exam
                        var newExamQuestion = new ExamQuestion
                        {
                            ExamID = examId,
                            QuestionID = requestedQuestion.QuestionID,
                            Points = requestedQuestion.Points
                        };
                        examQuestionRepository.Add(newExamQuestion);
                    }
                }

                examQuestionRepository.Save();

                return Ok(new
                {
                    ExamId = examId,
                    UpdatedCount = request.Questions.Count,
                    Message = "Exam questions updated"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to assign questions to exam {ExamId}", examId);
                return StatusCode(500, "An error occurred while assigning questions.");
            }
        }
    }
}
