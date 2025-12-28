using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankRepository questionBankRepo;

        public QuestionBankController(IQuestionBankRepository questionBankRepo)
        {
            this.questionBankRepo = questionBankRepo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var allQuestionBanks = questionBankRepo.GetAll();

            var publicBanks = allQuestionBanks.Where(q => !q.IsPrivate).ToList();
            var privateBanks = allQuestionBanks
                .Where(q => q.IsPrivate && q.UserID == currentUserId)
                .ToList();

            publicBanks.AddRange(privateBanks);

            return Ok(publicBanks);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            QuestionBankWithQuestionsDTO questionBank = questionBankRepo.GetById(id);
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (questionBank != null && currentUserId!=null)
            {
                if(questionBank.IsPrivate)
                {
                    if(questionBank.UserID==currentUserId)
                    {
                        return Ok(questionBank);
                    }
                }
                else
                {
                    return Ok(questionBank);
                }
            }
            return BadRequest("This QB Not Found");
        }

        [HttpGet("{name:regex(^[[a-zA-Z]][[a-zA-Z0-9]]*$)}")]
        public IActionResult GetByName(string name)
        {
            QuestionBankWithQuestionsDTO questionBank = questionBankRepo.GetByName(name);
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (questionBank != null && currentUserId != null)
            {
                if (questionBank.IsPrivate)
                {
                    if (questionBank.UserID == currentUserId)
                    {
                        return Ok(questionBank);
                    }
                }
                else
                {
                    return Ok(questionBank);
                }
            }
            return BadRequest("This QB Not Found");
        }

        //[Authorize]
        [HttpPost]
        public IActionResult Add(QuestionBankAddDTO questionBank)
        {
            string? currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User is not authenticated.");

            QuestionBank newBank = new QuestionBank
            {
                Name = questionBank.Name,
                Category = questionBank.Category,
                IsPrivate = questionBank.IsPrivate,
                UserID = currentUserId
            };

            questionBankRepo.Add(newBank);
            questionBankRepo.Save();

            return Ok(newBank);
        }


        //[Authorize]
        //[HttpGet("claims")]
        //public IActionResult GetClaims()
        //{
        //    var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //    return Ok(claims);
        //}


        [HttpPut("{id:int}")]
        public IActionResult Edit(int id, QuestionBankAddDTO questionBank)
        {
            if (ModelState.IsValid)
            {
                QuestionBankWithQuestionsDTO bankFromDb = questionBankRepo.GetById(id);
                if(bankFromDb !=null)
                {
                    QuestionBankAddDTO qb = new QuestionBankAddDTO
                    {
                        Name = questionBank.Name,
                       // UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                        Category = questionBank.Category,
                        IsPrivate = questionBank.IsPrivate,
                        Questions = questionBank.Questions
                    };

                    questionBankRepo.Update(id, qb);
                    questionBankRepo.Save();
                    return Ok(qb);
                }
                return BadRequest("QB Not Found");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult delete(int id)
        {
            QuestionBankWithQuestionsDTO bankFromDb = questionBankRepo.GetById(id);
            if (bankFromDb != null)
            {
                questionBankRepo.Remove(id);
                questionBankRepo.Save();
                return NoContent();
            }
            return BadRequest("QB Not Found");
        }
    }
}
