using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using static nbRecruitment.Controllers.PostingController;

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
                sbyte polo = obj.current_residing_country == "PH" ? sbyte.Parse("1") : sbyte.Parse("0");
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
                    CurrentCountry = obj.current_residing_country,
                    Country = obj.residing_country,
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
                 string uploadpath = Path.Combine("/home/groupnb/go/src/github.com/dafalo/GNBRecruitementFiles/"+ candidate.Id +"/");
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
