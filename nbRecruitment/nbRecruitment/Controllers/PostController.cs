using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Utils;
using MimeKit;
using nbRecruitment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using static nbRecruitment.Controllers.PostingController;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using System.Text;

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
                    Where(x => x.Status == 1 && x.IsPending == 0 && x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).
                    OrderByDescending(x => x.Id).
                    Skip(page * size).
                    Take(size).
                    ToList();

                int postingCount = _context.Postings.
                    Where(x => x.IsPending == 0 && x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).Count();

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

        [HttpGet("directPost")]
        public async Task<ActionResult> directPost(int Id)
        {
            try
            {

                Posting postingList = _context.Postings.
                    Where(x => x.Id == Id && x.Status == 1 && x.IsPending == 0 && x.IsDelete == 0).
                    FirstOrDefault();

                return Ok(postingList);

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

        [HttpGet("country")]
        public async Task<ActionResult> country(int postingId)
        {
            try
            {

                var country = _context.Countries.Where(x => x.Status.Equals(1) && x.IsDelete.Equals(0)).
                      Select(x => new
                      {
                          x.Code,
                          x.Name
                      }).
                      ToList();

              /*  var posting = _context.Postings.Where(x => x.Id.Equals(postingId)).Select(x => x.LanguageCodes).FirstOrDefault();

                List<string> langCode = JsonConvert.DeserializeObject<List<string>>(posting)!;

                List<Language> language = new List<Language>();

                foreach (string item in langCode)
                {
                    List<Language> vLanguage = _context.Languages.Where(x => x.Code == item).ToList();

                    foreach (var item1 in vLanguage)
                    {
                        language.Add(item1);
                    }


                }*/

                //var language = _context.Languages.Where(x => x.Status.Equals(1) && x.IsDelete.Equals(0)).ToList();

                return Ok(new { country });

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

        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = ASCIIEncoding.ASCII.GetBytes("GroupNBEncry2024");
                aesAlg.IV = ASCIIEncoding.ASCII.GetBytes("GroupNBEncry2024");

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        [HttpPost("addCandidate")]
        public async Task<IActionResult> addCandidate(
            [FromForm] IFormFile file,
            [FromForm] string admissibility,
            [FromForm] string posting
        )
        {
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(admissibility);
                Posting Selectedposting = JsonConvert.DeserializeObject<Posting>(posting)!;
                string language = obj.language;



                List<AsignUser> recruiter = _context.AsignUsers.Where(x => x.PostingId.Equals(Selectedposting.Id) ).OrderBy(x => x.Count).ToList();
                var test = obj.dob;
                DateOnly dob = DateOnly.Parse((obj.dob).ToString("yyyy-MM-dd"));
                string tempPolo = obj.current_residing_address;
                sbyte polo = tempPolo.ToLower() == "ph" || tempPolo.ToLower().Contains("philippine") ? sbyte.Parse("1") : sbyte.Parse("0");
                string numberCut = obj.contactNumber;
                string numberCodeCut = obj.countryCode;
                Candidate candidate = new Candidate() {

                    AsignTo = recruiter[0].UserId,
                    PostingId = Selectedposting.Id,
                    JobCode = Selectedposting.JobCode,
                    Firstname = obj.first_name,
                    Middlename = obj.middle_name,
                    Lastname = obj.last_name,
                    DateOfBirth = dob,
                    NumCode = obj.countryCode,
                    Nationality = obj.nationality,
                    CurrentResidingAddress = obj.current_residing_address,
                    Num = numberCut.Remove(0, numberCodeCut.Length),
                    Email = obj.email,
                    Polo = polo,
                    StatusDescription = "New",
                    LastStatusDescription = "New",
                    Status = 1
                };

                  _context.Candidates.Add(candidate);

                _context.SaveChanges();

                _context.Admissibilities.Add(new Admissibility { 
                
                CandidateId = candidate.Id,
                _1a = obj.question_1_a == "yes"?sbyte.Parse("1"): sbyte.Parse("0"),
                _1b = obj.question_1_b == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _1c = obj.question_1_c,
                _2a = obj.question_2_a == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _2b = obj.question_2_b == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _2c = obj.question_2_c == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _2d = obj.question_2_d,
                _3a = obj.question_3_a == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _3b = obj.question_3_b,
                _4a = obj.question_4_a == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _4b = obj.question_4_b,
                _5a = obj.question_5_a == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _6a = obj.question_6_a == "yes" ? sbyte.Parse("1") : sbyte.Parse("0"),
                _6b = obj.question_6_b,
                _6c = obj.ircc == "yes" ? sbyte.Parse("1") : sbyte.Parse("0")

                });

                recruiter[0].Count = recruiter[recruiter.Count()-1].Count + 1;




         

                string fileName = file.FileName + ".pdf";
                string fileextention = Path.GetExtension(fileName);
                 string uploadpath = Path.Combine("/home/groupnb/go/src/github.com/dafalo/AppNBRecruitment/GNBRecruitementFiles/" + candidate.Id +"/");
                //string uploadpath = Path.Combine("C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\resume");
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                using (FileStream fs = System.IO.File.Create(uploadpath + "Resume" + fileextention))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                _context.SaveChanges();

                string link = "http://apps.groupnb.com.ph/recruitmentapplication/canada/upload?details=" + Encrypt($"{candidate.Id}-{candidate.Lastname}, {candidate.Firstname} {candidate.Middlename}");

                 sendEmail(candidate.Email, Selectedposting.JobType, link);

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

        public static void sendEmail(string candidateEmail, string JobType, string link)
        {
            var currDir = Directory.GetCurrentDirectory();
            var mjmlPath = Path.Combine(currDir, Path.Combine("PDF", "emailTemplate.html"));
            var content = System.IO.File.ReadAllText(mjmlPath);

            // Replace placeholders in the HTML content with actual values
            content = content.Replace("{{jobType}}", JobType);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("GroupNB-noreply@groupnb.ca", "groupnb23@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient Name", candidateEmail));
            message.Subject = "Requirements GroupNB";

            var bodyBuilder = new BodyBuilder();
            content = content.Replace("{{link}}", link);
            content = content.Replace("{{jobType}}", JobType);
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
        }


        [HttpGet("getDir")]
        public async Task<IActionResult> getDir(string toCombine)
        {
           
            var currDir = Directory.GetCurrentDirectory();
            string uploadpath = Path.Combine(toCombine);
            string isTrue = "";

            if (System.IO.File.Exists(uploadpath))
            {
                isTrue = toCombine;
            }

            return Ok(new { dir = currDir , path = isTrue });
        }

    }
}
