using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;
using Quiz.Repository;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        #region Injection
    public class UserAnswerController : ControllerBase
    {
       IUserAnswerRepository userAnswerRepository;
        IExamResultRepository examResultRepository;
        IExamQuestionsRepository questionRepository;
        IQuestionRepository question;

        public UserAnswerController(IUserAnswerRepository userAnswerRepository , IExamResultRepository examResultRepository , IExamQuestionsRepository questionRepository , IQuestionRepository question )
        {
            this.examResultRepository = examResultRepository;
            this.userAnswerRepository = userAnswerRepository;
            this.questionRepository = questionRepository;
            this.question = question;
        }

        #endregion

        #region Get Answers For Result

        [HttpGet("{ResultID}")]
        [Authorize]
        public IActionResult GetAnswers(int ResultID)
        {
            List<UserAnswerDTO> userAnswers = userAnswerRepository.GetAnswersForResult(ResultID);
            return Ok(userAnswers);
        }

        #endregion

        #region Add Answer
        [HttpPost]
        [Authorize]
        public IActionResult AddAnswers([FromBody] AnswerAddDTO NewAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            UserAnswer userAnswer = new UserAnswer
            {
               ResultID = NewAnswer.ResultID,
               UserAnswerText = NewAnswer.UserAnswerText,
               QuestionID = NewAnswer.QID,
               


            };
            userAnswerRepository.Add(userAnswer);
            userAnswerRepository.Save();

            return Ok(userAnswer);

        }
        #endregion

        #region Update Point & IsCorrect
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateAnswer(int id, [FromBody] AnswerUpdateDTO NewResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = userAnswerRepository.GetById(id);
            if (result == null)
            {
                return NotFound("Answer Not Found");
            }

            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (UserID != result.Result.Exam.CreatedBy)
            {
                return Unauthorized("You are not authorized to correct this question.");
            }

            result.IsCorrect = NewResult.IsCorrect;
            result.point = NewResult.point;

            userAnswerRepository.Update(id, result);
            userAnswerRepository.Save();

            return Ok(result);
        }
        #endregion


        #region Delete Answer
        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult DeleteAnswer(int id)
        {
            var result = userAnswerRepository.GetById(id);
            if (result == null)
            {
                return NotFound($"Answer with ID {id} not found.");
            }
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserID != result.Result.UserID)
            {
                return Unauthorized("You are not authorized to delete The Answer");
            }

            userAnswerRepository.Remove(id);
            userAnswerRepository.Save();

            return NoContent();
        }
        #endregion

        #region Calculate Score
        [HttpPut("CalculateScore/{resultId:int}")]
        [Authorize]
        public IActionResult CalculateScore(int resultId)
        {
            var answers = userAnswerRepository.GetAnswersForResult(resultId);
            if (answers == null || answers.Count == 0)
            {
                return NotFound("No answers found for this result.");
            }

           double totalPoints = (double)answers.Sum(a => a.point);

            var result = examResultRepository.GetById(resultId);
            if (result == null)
            {
                return BadRequest("Result not found.");
            }
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (UserID != result.Exam.CreatedBy)
            {
                return Unauthorized("You are not authorized to correct The Exam.");
            }

            result.Score = totalPoints;
            examResultRepository.Update(resultId, result);
            examResultRepository.Save();

            return Ok(new { Message = "Score calculated and updated.", TotalPoints = totalPoints });
        }

        #endregion

        #region Auto Correct 
        [HttpPut("AutoCorrect/{resultId:int}")]
        [Authorize]
        public IActionResult AutoCorrect(int resultId)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = examResultRepository.GetById(resultId);
            if (result == null || result.Exam == null)
            {
                return NotFound("Result or Exam not found.");
            }
            if (result.Exam.CreatedBy != userId)
            {
                return Unauthorized("Only the creator of the exam can perform auto-correction.");
            }
            var answers = userAnswerRepository.GetAnswersForResult(resultId);
            if (answers == null || answers.Count == 0)
            {
                return NotFound("No answers found for this result.");
            }

            int correctedCount = 0;

            foreach (var dto in answers)
            {
                var answer = userAnswerRepository.GetById(dto.id); 
                if (answer == null)
                {
                    BadRequest("Answer Not Fount");
                }

                var qu = question.GetById(answer.QuestionID);
                var quPoint = questionRepository.GetById(answer.QuestionID);

                if (qu == null)
                {
                    return BadRequest($"Question not found.");
                }

                string userAnswerText = answer.UserAnswerText?.Trim().ToLower();
                string correctAnswerText = qu?.CorrectAnswer.Trim().ToLower();

                if (userAnswerText == correctAnswerText)
                {
                    answer.IsCorrect = "true";
                    answer.point = quPoint.Points;
                    correctedCount++;
                }
                else
                {
                    answer.IsCorrect = "false";
                    answer.point = 0;
                }

                userAnswerRepository.Update(answer.Id, answer); 
                
            }

            userAnswerRepository.Save();


            return Ok(new
            {
                Message = "Correction Completed",
                TotalAnswers = answers.Count,
                CorrectAnswers = correctedCount,
            });
        }

        #endregion


    }
}
