using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {


        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public PostController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpGet("post")]
        public async Task<ActionResult> post( string? search, int page, int size)
        {
            try
            {

                var postingList = _context.Postings.
                    Where(x => x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).
                    OrderByDescending(x => x.Id).
                    Skip(page * size).
                    Take(size).
                   
                    ToList();

                int postingCount = _context.Postings.
                    Where(x =>  x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).Count();

                return Ok(new { postingList , postingCount});

            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

    }
}
