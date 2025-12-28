using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;
using Quiz.Repository;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        #region Injection
    public class ExamResultController : ControllerBase
    {
        IExamResultRepository examResultRepository;

        public ExamResultController(IExamResultRepository examResultRepository)
        {
            this.examResultRepository = examResultRepository;
        }
        #endregion


        #region Get All ExamResults To Creator

        [HttpGet("{ExamID:int}")]
        [Authorize]
        public IActionResult GetAll(int ExamID)

        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            List<ResultDTO> examResults = examResultRepository.GetAllResultsForExam()
                                                              .Where(e => e.ExamID == ExamID && e.CreatorID == UserId)
                                                              .ToList();

            return Ok(examResults);


        }
        #endregion


        #region Get Results To User

        [HttpGet("User-Results")]
        [Authorize]
        public IActionResult GetResultsByUserId()
        {
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            List<ResultDTO> result = examResultRepository.GetResultByUserId().Where(e => e.userid == UserID).ToList();

            return Ok(result);
        }

        #endregion


        #region Get One Result To User
        [HttpGet("OneResult/{ExamID}")]
        [Authorize]
        public IActionResult GetResultByUser(int ExamID)
        {
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            ResultDTO result = examResultRepository.GetOneResultByUserID(UserID, ExamID);
            return Ok(result);
        }

        #endregion


        #region Add Result
        [HttpPost]
        [Authorize]
        public IActionResult AddResult([FromBody] ResultAddDto NewResult)
        {
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            ExamResult examResult = new ExamResult
            {
                ExamID = NewResult.ExamID,
                UserID =UserID,
                Date = NewResult.date,


            };
            examResultRepository.Add(examResult);
            examResultRepository.Save();

            return Ok(examResult);

        }
        #endregion


        #region Update Result
        [HttpPut("{id:int}")]
        [Authorize]
        public IActionResult UpdateResult(int id, [FromBody] ResultUpdateDTO NewResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = examResultRepository.GetById(id);
            if (result == null)
            {
                return NotFound("Result Not Found");
            }
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (User == null || User.Identity == null || result.Exam.CreatedBy != UserID) 
                {
                return Unauthorized("Not allowed"); 
            }

            result.Score = NewResult.score;
            result.FeedBack = NewResult.FeedBack;



            examResultRepository.Update(id, result);
            examResultRepository.Save();

            return Ok(result);
        }
        #endregion


        #region Delete Result

        [HttpDelete("{id:int}")]
        public IActionResult DeleteResult(int id)
        {
            var result = examResultRepository.GetById(id);

            if (result == null)
            {
                return NotFound($"Result with ID {id} not found.");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            bool isUser = result.UserID == currentUserId;

            bool isExamCreator =result.Exam.CreatedBy== currentUserId;

            if (!isUser && !isExamCreator)
            {
                return Unauthorized("Not allowed");
            }

            examResultRepository.Remove(id);
            examResultRepository.Save();

            return NoContent();
        }

#endregion




    }
}
