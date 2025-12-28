using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        IQuestionRepository questionRepo;
        IOptionRepository optionRepo;
        private readonly IQuestionBankRepository bankRepo;

        public QuestionController(IQuestionRepository questionRepository, IOptionRepository optionRepository, IQuestionBankRepository bankRepo)
        {
            questionRepo = questionRepository;
            optionRepo = optionRepository;
            this.bankRepo = bankRepo;
        }

        #region Get Question By BankID
        [HttpGet("{BankID:int}")]
        public IActionResult GetBankQuestions(int BankID)
        {
            List<GetBankQuestionDTO> Question = questionRepo.GetBankQuestion(BankID);

            if (Question != null && Question.Any())
            {
                NoContent();
            }
          
            return Ok(Question);
        }
        #endregion

        #region Filter Questions By type
        [HttpGet]
        [Route("{BankID:int}/{type:int}")]
        public IActionResult FilterByType(int BankID,int type)
        {
            List<GetBankQuestionDTO> Question = questionRepo.FilterQuestions(BankID, type);

            if (Question != null && Question.Any())
            {
                NoContent();
            }

            return Ok(Question);
        }
        #endregion

        #region Add Question
        [HttpPost]
        [Authorize]
        public IActionResult AddQuestion(AddOrEditQuestionDTO question)
        {
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            QuestionBankWithQuestionsDTO bankFromDb=null;
            if (question.BankID!=0)
            {
                 bankFromDb = bankRepo.GetById(question.BankID);
            }
            if (bankFromDb != null && currentUserId == bankFromDb.UserID)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var newQuestion = new Question
                        {
                            Text = question.QstText,
                            CorrectAnswer = question.CorAnswer,
                            Type = (QuestionType)question.QuestionType,
                            BankID = question.BankID,
                        };

                        questionRepo.Add(newQuestion);
                        questionRepo.Save();

                        if (question.QuestionType == 0)
                        {
                            var newOption = new List<Option>
                         {
                        new Option { QuestionID = newQuestion.Id, OptionText = question.Option1 },
                        new Option { QuestionID = newQuestion.Id, OptionText = question.Option2 },
                        new Option { QuestionID = newQuestion.Id, OptionText = question.Option3 },
                        new Option { QuestionID = newQuestion.Id, OptionText = question.Option4 }
                    };

                            optionRepo.AddOption(newOption);
                            optionRepo.Save();
                        }

                        return Ok("Added successfully");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("An error occurred: " + ex.InnerException?.Message ?? ex.Message);
                    }
                }
                return BadRequest(ModelState);
            }
            return Unauthorized("You are not authorized to access this resource.");
        }
        #endregion
        
        #region Delete
        [HttpDelete("{QuestionID:int}")]
        [Authorize]
        public IActionResult DeleteQuestion(int QuestionID)
        {
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Question questionFromDB = questionRepo.GetById(QuestionID);
            if (questionFromDB == null)
            {
                return BadRequest("Invalid ID");
            }

            QuestionBankWithQuestionsDTO bankFromDb = null;
            if (questionFromDB.BankID != 0)
            {
                bankFromDb = bankRepo.GetById(questionFromDB.BankID);
            }

            if (bankFromDb != null && currentUserId == bankFromDb.UserID)
            {
                questionRepo.Remove(QuestionID);
                questionRepo.Save();
                if (questionFromDB.Type.ToString() == "MCQ")
                {
                    optionRepo.Remove(QuestionID);
                    optionRepo.Save();
                }
                return Ok("Deleted Succcessfuly");
            }

            return Unauthorized("You are not authorized to access this resource.");

        }
        #endregion
    }
}
