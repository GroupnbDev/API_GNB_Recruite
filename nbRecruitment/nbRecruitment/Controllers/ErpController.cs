using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using nbRecruitment.ModelsERP;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User = nbRecruitment.Models.User;
using Candidate = nbRecruitment.Models.Candidate;
using MimeKit.Utils;
using MimeKit;
using MailKit.Net.Smtp;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErpController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly GnberpContext _contextP;
        private readonly IConfiguration _configuration;


        public ErpController(NbRecruitmentContext context, GnberpContext contextP, IConfiguration configuration)
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
            {//_contextP.Candidates.Where(i => i.CanId == x.Id.ToString()) == null

                var twoWeeksAgo = DateTime.Now.AddDays(-14);
                var today = DateTime.Now.Date;

                var candidate = _context.SelectedCandidateNotInErps.Select(
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
                        ClientId = _context.Postings.Where(i => i.Id == x.PostingId).Select(i => i.ClientId).FirstOrDefault(),
                        Client = _context.Postings.Where(i => i.Id == x.PostingId).Select(i => i.ClientName).FirstOrDefault(),
                        Number = x.Num,
                        x.Email,
                        ProcessedBy = _context.Users.Where(i => i.Id == x.AsignTo).
                        Select(i => i.Middlename == null ? $"{i.Firstname} {i.Lastname}" : $"{i.Firstname} {i.Middlename} {i.Lastname}").FirstOrDefault()


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

        [HttpPost("setStatus")]
        public async Task<ActionResult> setStatus(int candidateId)
        {
            try
            {

                Candidate candidate = _context.Candidates.Where(x => x.Id == candidateId).FirstOrDefault()!;

                if (candidate == null)
                {
                    return BadRequest();
                }
                candidate.StatusDescription = "Not Selected";

                string directoryPath = "/home/groupnb/go/src/github.com/dafalo/AppNBRecruitment/GNBRecruitementFiles/" + candidate.Id;
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    if (Path.GetFileName(file) != "Resume.pdf")
                    {
                        System.IO.File.Delete(file);
                    }
                }


                _context.SaveChanges();

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

        public class details
        {
            public string email { get; set; }
            public string stringContent { get; set; }
            public string link { get; set; }
            public string footer { get; set; }
        }

        [HttpPost("erpSendEmail")]
        public async Task<ActionResult> erpSendEmail(details details)
        {
            try
            {

                var currDir = Directory.GetCurrentDirectory();
                var mjmlPath = Path.Combine(currDir, Path.Combine("PDF", "emailERPTemplate.html"));
                var content = System.IO.File.ReadAllText(mjmlPath);

                // Replace placeholders in the HTML content with actual values
                content = content.Replace("{{jobType}}", details.stringContent);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("GroupNB-noreply@groupnb.ca", "groupnb23@gmail.com"));
                message.To.Add(new MailboxAddress("Recipient Name", details.email));
                message.Subject = "Requirements GroupNB";

                var bodyBuilder = new BodyBuilder();
                content = content.Replace("{{link}}", details.link);
                content = content.Replace("{{footer}}", details.footer);
                content = content.Replace("{{jobType}}", details.stringContent);
                bodyBuilder.HtmlBody = content;

                var logoImagePath = Path.Combine(currDir, "PDF", "images", "Logo.png");
                var logoImage = bodyBuilder.LinkedResources.Add(logoImagePath);
                logoImage.ContentId = MimeUtils.GenerateMessageId();

                var centerImgPath = Path.Combine(currDir, "PDF", "images", "bg.png");
                var centerImg = bodyBuilder.LinkedResources.Add(centerImgPath);
                centerImg.ContentId = MimeUtils.GenerateMessageId();


                bodyBuilder.HtmlBody = bodyBuilder.HtmlBody
                    .Replace("{{Logo}}", $"cid:{logoImage.ContentId}")
                     .Replace("{{centerImg}}", $"cid:{centerImg.ContentId}");

                message.Body = bodyBuilder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("groupnb23@gmail.com", "dwxlbnyzqaxzjtbx");
                    client.Send(message);
                    client.Disconnect(true);
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

    }
}
