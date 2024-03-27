using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Utils;
using MimeKit;
using nbRecruitment.Models;
using Newtonsoft.Json;
using static nbRecruitment.Controllers.CandidateController;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;

        public UploadController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }
        [HttpPost("uploadFilesCandidates")]
        public async Task<ActionResult> uploadFilesCandidates([FromForm] IFormFile file, [FromForm] string fileType, [FromForm] string candidateId)
        {
            try
            {
                string fileextention = Path.GetExtension(file.FileName);
               
              

                if(fileextention != ".pdf")
                {
                    return StatusCode(202, "notPDF");

                }
                // string fileName = file.FileName + ".pdf";
                string uploadpath = "/home/groupnb/go/src/github.com/dafalo/GNBRecruitementFiles/" + candidateId + "/";
                //string uploadpath = Path.Combine("C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\"+ candidateId+"//");
                //string uploadpath = Path.Combine("C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\resume");
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                using (FileStream fs = System.IO.File.Create(uploadpath + fileType + fileextention))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                return Ok();
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

        [HttpPost("checkFilesCandidates")]
        public async Task<ActionResult> checkFilesCandidates(string fileType, string candidateId)
        {
            try
            {
                string uploadpath = Path.Combine("/home/groupnb/go/src/github.com/dafalo/GNBRecruitementFiles/" + candidateId + "/" + fileType + ".pdf");
                //string uploadpath = Path.Combine("C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\" + candidateId + "//"+ fileType + ".pdf");
                //string uploadpath = Path.Combine("C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\resume");
                if (System.IO.File.Exists(uploadpath))
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
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

    }
}
