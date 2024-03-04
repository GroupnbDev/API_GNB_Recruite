using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {

        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public CandidateController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpGet("jobTypeList")]
        public async Task<ActionResult> jobTypeList()
        {
            try
            {

                return Ok(_context.Jobtypes.Where(x => x.Status.Equals(1) && x.IsDelete.Equals(0)).Select( x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                }).ToList());

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

        [HttpGet("jobCandidates")]
        public async Task<ActionResult> jobCandidates(int userId, string jobCode)
        {
            try
            {
                var candidates = _context.Candidates.Where(x=> x.AsignTo.Equals(userId) && x.JobCode == jobCode).
                    Select( x=> new
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
                        x.Country,
                        x.CurrentCountry,
                        CountryName = _context.Countries.Where( i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                        CurrentCountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                        x.IsViewed
                    }).
                    ToList();

                if (candidates.Count > 0)
                {
                    return Ok(candidates);
                }
                else
                {
                    return StatusCode(202, "No Data");
                }

              

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


        [HttpGet("ImageViewer")]
        public IActionResult ImageViewer(string? candidateId)
        {
            try
            {
                string filePath = "C:\\Users\\MarkJearoneFAlvarez\\Pictures\\GNBRecruite\\asd.pdf";

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var fileExtension = Path.GetExtension(filePath).ToLower();

                string contentType;
                switch (fileExtension)
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    default:
                        return BadRequest("Unsupported file type");
                }

                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
