using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public NotificationController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpPost("NotificationList")]
        public async Task<ActionResult> NotificationList(int userId)
        {
            try
            {

                var candidate = _context.Candidates.Where(x => x.AsignTo.Equals(userId) && x.IsViewed == sbyte.Parse("0")).
                    Select(x => new
                    {
                        x.AsignTo,
                        x.PostingId,
                        x.Id,
                        x.JobCode,
                        jobTypeName = _context.Jobtypes.Where(i => i.Code == x.JobCode).Select(i => i.Name).FirstOrDefault(),
                        x.Firstname,
                        x.Middlename,
                        x.Lastname,
                        x.Email,
                        x.Status,
                        x.IsDelete,
                        x.StatusDescription,
                        x.LastStatusDescription,
                        x.Polo,
                        x.Num,
                        x.NumCode,
                        x.Country,
                        x.CurrentCountry,
                        CountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                        CurrentCountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                        x.IsViewed
                    }).ToList();

                if (candidate.Count() == 0)
                {
                    return Ok();
                }

                return Ok(candidate);
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
