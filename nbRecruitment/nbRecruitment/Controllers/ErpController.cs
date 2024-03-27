using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using nbRecruitment.ModelsERP;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User = nbRecruitment.Models.User;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErpController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly GnbContext _contextP;
        private readonly IConfiguration _configuration;


        public ErpController(NbRecruitmentContext context, GnbContext contextP, IConfiguration configuration)
        {
            _context = context;
            _contextP = contextP;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }

        [HttpGet("candidates")]
        public async Task<ActionResult> candidates()
        {
            try
            {

                var candidate = _context.Candidates.Where(x => x.StatusDescription == "Selected").Select(
                    x => new
                    {
                        x.Id,
                        x.Firstname,
                        x.Middlename,
                        x.Lastname,
                        Fullname = $"{x.Firstname} {x.Middlename} {x.Lastname}",
                        x.DateOfBirth,
                        x.CurrentResidingAddress,
                        Position = _context.Postings.Where(i => i.Id == x.PostingId).Select(i => i.JobType).FirstOrDefault(),
                        x.Nationality,
                        Client = _context.Postings.Where(i => i.Id == x.PostingId).Select(i => i.ClientName).FirstOrDefault(),
                        Number = x.Num,
                        x.Email


                    }
                    ).ToList();


                return Ok( candidate );
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
