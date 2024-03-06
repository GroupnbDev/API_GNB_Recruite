using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using NReco.PdfGenerator;
using System.Net.Mime;
using System.Reflection.Emit;

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


        public partial class jobCandidatesClass
        {
            public int userId { get; set; }
            public string jobCode { get; set; }
            public string filterBy { get; set; }

        }

        [HttpPost("jobCandidates")]
        public async Task<ActionResult> jobCandidates(jobCandidatesClass jobCandidatesClass)
        {
            try
            {
                var candidates = _context.Candidates.Where(x=> x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All"?( x.StatusDescription.Contains("")&&
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).
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

        [HttpPost("jobCandidates2")]
        public async Task<ActionResult> jobCandidates2(jobCandidatesClass jobCandidatesClass)
        {
            try
            {
                var candidates = _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
               ( x.StatusDescription == "Progress" ||
                x.StatusDescription == "Selected" ||
                x.StatusDescription == "Not Selected")
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).Select(x => new
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
                        CountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
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

        public partial class saveJobCandidateClass
        {
            public int id { get; set; }
            public string statusDescription { get; set; }
            public sbyte polo { get; set; }

        }


        [HttpPost("saveJobCandidate")]
        public async Task<ActionResult> saveJobCandidate(saveJobCandidateClass saveJobCandidateClass)
        {
            try
            {


                Candidate candidate = _context.Candidates.Where(x => x.Id.Equals(saveJobCandidateClass.id)).FirstOrDefault();

                candidate.LastStatusDescription = candidate.StatusDescription;
                candidate.StatusDescription = saveJobCandidateClass.statusDescription;
                candidate.Polo = saveJobCandidateClass.polo;

                _context.SaveChanges();

            return Ok("Saved Successfully!");

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


        [HttpGet("getResumeAndAdmissibility")]
        public IActionResult getResumeAndAdmissibility(string candidateId)
        {
            try
            {
                /* string filePath = "C:\\Users\\MarkJearoneFAlvarez\\Pictures\\GNBRecruite\\Resume//" + candidateId + ".pdf";

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

                 return File(fileBytes, contentType);*/

                var currDir = Directory.GetCurrentDirectory();
                var img = Path.Combine(currDir, Path.Combine("PDF", "protection.png"));

                if (!System.IO.File.Exists(img))
                {
                    return NotFound();
                }

                var fileBytes = System.IO.File.ReadAllBytes(img);
                var fileExtension = Path.GetExtension(img).ToLower();

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
                    default:
                        return BadRequest("Unsupported file type");
                }

                string base64String = Convert.ToBase64String(fileBytes);

                var mjmlPath = Path.Combine(currDir, Path.Combine("PDF", "admissibility.html"));
                var content = System.IO.File.ReadAllText(mjmlPath);
                content = content.Replace("@basesixfour", "data:image/png;base64,"+base64String);
                var newHtmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                /* var tempFooter = Path.Combine(currDir, Path.Combine("PDF", "footers.html"));
                   var footer = System.IO.File.ReadAllText(tempFooter);

                   var tempHeader = Path.Combine(currDir, Path.Combine("PDF", "headers.html"));
                   var header = System.IO.File.ReadAllText(tempHeader);

                   newHtmlToPdf.PageHeaderHtml = header;
                   newHtmlToPdf.PageFooterHtml = footer;*/

                // newHtmlToPdf.Margins = new PageMargins { Top = 30, Bottom = 20, Left = 0, Right = 0 };
                newHtmlToPdf.Size = NReco.PdfGenerator.PageSize.Letter;
                var newPdfBytes = newHtmlToPdf.GeneratePdf(content);
                return File(newPdfBytes, "application/pdf");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("test")]
        public async Task<ActionResult> saveJobCandidate()
        {
            try
            {
                var currDir = Directory.GetCurrentDirectory();
                var mjmlPath = Path.Combine(currDir, Path.Combine( "PDF", "admissibility.html"));
                var content = System.IO.File.ReadAllText(mjmlPath);
                var newHtmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                /* var tempFooter = Path.Combine(currDir, Path.Combine("PDF", "footers.html"));
                   var footer = System.IO.File.ReadAllText(tempFooter);

                   var tempHeader = Path.Combine(currDir, Path.Combine("PDF", "headers.html"));
                   var header = System.IO.File.ReadAllText(tempHeader);

                   newHtmlToPdf.PageHeaderHtml = header;
                   newHtmlToPdf.PageFooterHtml = footer;*/

                newHtmlToPdf.Margins = new PageMargins { Top = 30, Bottom = 0, Left = 0, Right = 0 };
                var newPdfBytes = newHtmlToPdf.GeneratePdf(content);
                return File(newPdfBytes, "application/pdf");

               // return Ok("Saved Successfully!");

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
