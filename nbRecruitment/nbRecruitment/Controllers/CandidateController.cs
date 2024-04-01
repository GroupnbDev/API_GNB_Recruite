using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
//using NReco.PdfGenerator;
using System.ComponentModel;
using System.Net.Mime;
using System.Reflection.Emit;
using DinkToPdf;
using DinkToPdf.Contracts;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Internal;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Security.Policy;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;

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

                return Ok(_context.Jobtypes.Where(x => x.Status.Equals(1) && x.IsDelete.Equals(0)).Select(x => new
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
            public string type { get; set; }
            public string jobCode { get; set; }
            public string filterBy { get; set; }
            public int page { get; set; }
            public int size { get; set; }
        }

        [HttpPost("jobCandidates")]
        public async Task<ActionResult> jobCandidates(jobCandidatesClass jobCandidatesClass)
        {
            try
            {


                var candidates = jobCandidatesClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).
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
                    }).
                     Skip(jobCandidatesClass.page * jobCandidatesClass.size).
                     Take(jobCandidatesClass.size).ToList() :
                    _context.Candidates.Where(x => x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).
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
                    }).
                     Skip(jobCandidatesClass.page * jobCandidatesClass.size).
                     Take(jobCandidatesClass.size).ToList();

                int candidatesCount = jobCandidatesClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).Count() :
                    _context.Candidates.Where(x => x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                ) : x.StatusDescription == jobCandidatesClass.filterBy)
                ).Count();


                if (candidates.Count > 0)
                {
                    return Ok(new { candidates, candidatesCount });
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
                var candidates = jobCandidatesClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
               (x.StatusDescription == "Progress" ||
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
                    x.Num,
                    x.NumCode,
                    x.Country,
                    x.CurrentCountry,
                    CountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                    CurrentCountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                    x.IsViewed
                }).
                     Skip(jobCandidatesClass.page * jobCandidatesClass.size).
                     Take(jobCandidatesClass.size).
                    ToList() :
                    _context.Candidates.Where(x => x.JobCode == jobCandidatesClass.jobCode &&
                (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
               (x.StatusDescription == "Progress" ||
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
                    x.Num,
                    x.NumCode,
                    x.Country,
                    x.CurrentCountry,
                    CountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                    CurrentCountryName = _context.Countries.Where(i => i.Code == x.Country).Select(i => i.Name).FirstOrDefault(),
                    x.IsViewed
                }).
                     Skip(jobCandidatesClass.page * jobCandidatesClass.size).
                     Take(jobCandidatesClass.size).
                    ToList();

                int candidatesCount = jobCandidatesClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesClass.userId) && x.JobCode == jobCandidatesClass.jobCode &&
               (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
              (x.StatusDescription == "Progress" ||
               x.StatusDescription == "Selected" ||
               x.StatusDescription == "Not Selected")
               ) : x.StatusDescription == jobCandidatesClass.filterBy)
               ).Count() :
                   _context.Candidates.Where(x => x.JobCode == jobCandidatesClass.jobCode &&
               (jobCandidatesClass.filterBy == "Select All" ? (x.StatusDescription.Contains("") &&
              (x.StatusDescription == "Progress" ||
               x.StatusDescription == "Selected" ||
               x.StatusDescription == "Not Selected")
               ) : x.StatusDescription == jobCandidatesClass.filterBy)
               ).Count();

                if (candidates.Count > 0)
                {
                    return Ok(new { candidates, candidatesCount });
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
        public partial class jobCandidatesDialogClass
        {
            public int userId { get; set; }
            public string type { get; set; }
            public string jobCode { get; set; }
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

        [HttpPost("jobCandidatesDialog")]
        public async Task<ActionResult> jobCandidatesDialog(jobCandidatesDialogClass jobCandidatesDialogClass)
        {
            try
            {
                string secret = "GroupNBEncry2024";

                var candidates = jobCandidatesDialogClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesDialogClass.userId) && x.JobCode == jobCandidatesDialogClass.jobCode &&
               
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                 ).
                    Select(x => new
                    {
                      
                        jobTypeName = _context.Jobtypes.Where(i => i.Code == x.JobCode).Select(i => i.Name).FirstOrDefault(),
                        Check = false,
                        Fullname = $"{x.Lastname}, {x.Firstname} {x.Middlename}",
                        Link = "http://localhost:5173/canada/upload?details=" + Encrypt($"{x.Id}-{x.Lastname}, {x.Firstname} {x.Middlename}"),
                        x.Email,
                        x.StatusDescription,
                    }).ToList() :
                    _context.Candidates.Where(x => x.JobCode == jobCandidatesDialogClass.jobCode &&
              
                x.StatusDescription != "Progress" &&
                x.StatusDescription != "Selected" &&
                x.StatusDescription != "Not Selected"
                
                ).
                    Select(x => new
                    {
                        jobTypeName = _context.Jobtypes.Where(i => i.Code == x.JobCode).Select(i => i.Name).FirstOrDefault(),
                        Check = false,
                        Fullname = $"{x.Lastname}, {x.Firstname} {x.Middlename}",
                        Link = "http://apps.groupnb.com.ph/recruitmentapplication/canada/upload?details=" + Encrypt($"{x.Id}-{x.Lastname}, {x.Firstname} {x.Middlename}"),
                        x.Email,
                        x.StatusDescription,
                    }).ToList();

            


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

        [HttpPost("jobCandidatesDialog2")]
        public async Task<ActionResult> jobCandidatesDialog2(jobCandidatesDialogClass jobCandidatesDialogClass)
        {
            try
            {
                string secret = "GroupNBEncry2024";

                var candidates = jobCandidatesDialogClass.type == "User" ? _context.Candidates.Where(x => x.AsignTo.Equals(jobCandidatesDialogClass.userId) && x.JobCode == jobCandidatesDialogClass.jobCode &&

                (x.StatusDescription == "Progress" ||
                x.StatusDescription == "Selected" ||
                x.StatusDescription == "Not Selected")
                 ).
                    Select(x => new
                    {

                        jobTypeName = _context.Jobtypes.Where(i => i.Code == x.JobCode).Select(i => i.Name).FirstOrDefault(),
                        Check = false,
                        Fullname = $"{x.Lastname}, {x.Firstname} {x.Middlename}",
                        Link = "http://localhost:5173/canada/upload?details=" + Encrypt($"{x.Id}-{x.Lastname}, {x.Firstname} {x.Middlename}"),
                        x.Email,
                        x.StatusDescription,
                    }).ToList() :
                    _context.Candidates.Where(x => x.JobCode == jobCandidatesDialogClass.jobCode &&

                (x.StatusDescription == "Progress" ||
                x.StatusDescription == "Selected" ||
                x.StatusDescription == "Not Selected")

                ).
                    Select(x => new
                    {
                        jobTypeName = _context.Jobtypes.Where(i => i.Code == x.JobCode).Select(i => i.Name).FirstOrDefault(),
                        Check = false,
                        Fullname = $"{x.Lastname}, {x.Firstname} {x.Middlename}",
                        Link = "http://apps.groupnb.com.ph/recruitmentapplication/canada/upload?details=" + Encrypt($"{x.Id}-{x.Lastname}, {x.Firstname} {x.Middlename}"),
                        x.Email,
                        x.StatusDescription,
                    }).ToList();




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
            public int modifiedBy { get; set; }
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
                candidate.ModifiedBy = saveJobCandidateClass.modifiedBy;
                candidate.ModifiedDate = DateTime.Now;

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
     

        [HttpGet("getAdmissibility")]
        public IActionResult getResumeAndAdmissibility(string candidateId)
        {
            try
            {

                Admissibility admissibility = _context.Admissibilities.Where(x => x.CandidateId.Equals(int.Parse(candidateId))).FirstOrDefault()!;
                Candidate candidate = _context.Candidates.Where(x => x.Id.Equals(int.Parse(candidateId))).FirstOrDefault()!;

                var currDir = Directory.GetCurrentDirectory();
                Console.WriteLine(currDir);
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
               // var newHtmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                /* var tempFooter = Path.Combine(currDir, Path.Combine("PDF", "footers.html"));
                   var footer = System.IO.File.ReadAllText(tempFooter);

                   var tempHeader = Path.Combine(currDir, Path.Combine("PDF", "headers.html"));
                   var header = System.IO.File.ReadAllText(tempHeader);

                   newHtmlToPdf.PageHeaderHtml = header;
                   newHtmlToPdf.PageFooterHtml = footer;*/

                // newHtmlToPdf.Margins = new PageMargins { Top = 30, Bottom = 20, Left = 0, Right = 0 };
                content = content.Replace("{{fullname}}", $"{candidate.Lastname} {candidate.Firstname} {candidate.Middlename}");
                content = content.Replace("{{birthDate}}", candidate.DateOfBirth.ToString());


                content = content.Replace("{{1a}}", admissibility._1a == 0 ? "" : "Checked");
                content = content.Replace("{{1aa}}", admissibility._1a == 1 ? "" : "Checked");
                content = content.Replace("{{1b}}", admissibility._1b == 0 ? "" : "Checked");
                content = content.Replace("{{1bb}}", admissibility._1b == 1 ? "" : "Checked");
                content = content.Replace("{{1c}}", admissibility._1c);

                content = content.Replace("{{2a}}", admissibility._2a == 0 ? "" : "Checked");
                content = content.Replace("{{2aa}}", admissibility._2a == 1 ? "" : "Checked");
                content = content.Replace("{{2b}}", admissibility._2b == 0 ? "" : "Checked");
                content = content.Replace("{{2bb}}", admissibility._2b == 1 ? "" : "Checked");
                content = content.Replace("{{2c}}", admissibility._2c == 0 ? "" : "Checked");
                content = content.Replace("{{2cc}}", admissibility._2c == 1 ? "" : "Checked");
                content = content.Replace("{{2d}}", admissibility._2d);

                content = content.Replace("{{3a}}", admissibility._3a == 0 ? "" : "Checked");
                content = content.Replace("{{3aa}}", admissibility._3a == 1 ? "" : "Checked");
                content = content.Replace("{{3b}}", admissibility._3b);

                content = content.Replace("{{4a}}", admissibility._4a == 0 ? "" : "Checked");
                content = content.Replace("{{4aa}}", admissibility._4a == 1 ? "" : "Checked");
                content = content.Replace("{{4b}}", admissibility._4b);

                content = content.Replace("{{5a}}", admissibility._5a == 0 ? "" : "Checked");
                content = content.Replace("{{5aa}}", admissibility._5a == 1 ? "" : "Checked");

                content = content.Replace("{{6a}}", admissibility._6a == 0 ? "" : "Checked");
                content = content.Replace("{{6aa}}", admissibility._6a == 1 ? "" : "Checked");
                content = content.Replace("{{6b}}", admissibility._6b);
                content = content.Replace("{{6c}}", admissibility._6c == 0 ? "" : "Checked");
                content = content.Replace("{{6cc}}", admissibility._6c == 1 ? "" : "Checked");

                var converter = new BasicConverter(new PdfTools());

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                          ColorMode = ColorMode.Color,
                          Orientation = Orientation.Portrait,
                          PaperSize = PaperKind.Letter,
                  },
                    Objects = {
                          new ObjectSettings() {
                              //PagesCount = true,
                              HtmlContent = content,


                WebSettings = { DefaultEncoding = "utf-8" },
             // HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
          }
      }
                };

                byte[] pdf = Dummy.converter.Convert(doc);

                // newHtmlToPdf.Size = NReco.PdfGenerator.PageSize.Letter;
                // var newPdfBytes = newHtmlToPdf.GeneratePdf(content);

                if (candidate.IsViewed == 0)
                {
                    candidate.IsViewed = 1;
                    _context.SaveChanges();
                }


                return File(pdf, contentType);
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public sealed class Dummy
        {
            public static readonly IConverter converter = new SynchronizedConverter(new PdfTools());

            private Dummy() { }

        }
        [HttpGet("getResume")]
        public async Task<ActionResult> getResume (int candidateId)
        {
            try
            {
                 //string resumePath = "C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite//" + candidateId + ".pdf";
                string resumePath = "/home/groupnb/go/src/github.com/dafalo/AppNBRecruitment/GNBRecruitementFiles/" + candidateId + "/Resume.pdf";

                if (!System.IO.File.Exists(resumePath))
                {
                    return StatusCode(202, "File not found ");
                }

                var resumeeBytes = System.IO.File.ReadAllBytes(resumePath);
                var resumeExtension = Path.GetExtension(resumePath).ToLower();

                string resumeType;
                switch (resumeExtension)
                {
                    case ".jpg":
                    case ".jpeg":
                        resumeType = "image/jpeg";
                        break;
                    case ".png":
                        resumeType = "image/png";
                        break;
                    case ".gif":
                        resumeType = "image/gif";
                        break;
                    case ".pdf":
                        resumeType = "application/pdf";
                        break;
                    default:
                        return BadRequest("Unsupported file type");
                }

                  return File(resumeeBytes, resumeType);

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

        [HttpPost("transfer")]
        public async Task<ActionResult> transfer(int userId, int candidateId, int transferTo)
        {
            try
            {

                Candidate candidate = _context.Candidates.Where(x => x.Id.Equals(candidateId)).FirstOrDefault();

                if (candidate == null)
                {
                    return StatusCode(202, "Candidate is Null");
                }
                else
                {
                    candidate.AsignTo = transferTo;
                    candidate.IsViewed = 0;
                    candidate.ModifiedBy = userId;
                    candidate.ModifiedDate = DateTime.Now;

                    _context.SaveChanges();
                }

                return Ok("Transferred Succesfuly!");

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


        [HttpGet("availableTransfer")]
        public async Task<ActionResult> availableTransfer()
        {
            try
            {
                var availableTransfer = _context.Users.Where(x=>  x.Status == 1 && x.IsDelete == 0).
                    Select(x => new
                    {
                        x.Id,
                       x.Fullname
                    }).
                    ToList().OrderBy(x => x.Fullname);

                return Ok(availableTransfer);

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

        [HttpPost("emailCandidate")]
        public async Task<ActionResult> emailCandidate(string candidateEmail, string JobType, string link)
        {
            try
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

                var logoImagePath = Path.Combine(currDir, "PDF","images", "Logo.png");
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

                return Ok("Successfully Sent!");

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

        public partial class multiSendJObject
        {
            public string jobTypeName { get; set; }
            public string Link { get; set; }
            public string Email { get; set; }
            public string Fullname { get; set; }
            public bool Check { get; set; }
            public string StatusDescription { get; set; }
        }

        [HttpPost("multipleEmailCandidate")]
        public async Task<ActionResult> multipleEmailCandidate(string details)
        {
            try
            {
                List<multiSendJObject> candidateList = JsonConvert.DeserializeObject<List<multiSendJObject>>(details)!;


                foreach (multiSendJObject candidate in candidateList)
                {
                    var currDir = Directory.GetCurrentDirectory();
                    var mjmlPath = Path.Combine(currDir, Path.Combine("PDF", "emailTemplate.html"));
                    var content = System.IO.File.ReadAllText(mjmlPath);
                    // Replace placeholders in the HTML content with actual values

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("GroupNB-noreply@groupnb.ca", "groupnb23@gmail.com"));
                    message.To.Add(new MailboxAddress("Recipient Name", candidate.Email));
                    message.Subject = "Requirements GroupNB";

                    var bodyBuilder = new BodyBuilder();
                    content = content.Replace("{{link}}", candidate.Link);
                    content = content.Replace("{{jobType}}", candidate.jobTypeName);
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

                

                return Ok("Successfully Sent!");

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

        [HttpPost("fileList")]
        public async Task<ActionResult> fileList(string candidateId)
        {
            try
            {
                string directoryPath = "/home/groupnb/go/src/github.com/dafalo/AppNBRecruitment/GNBRecruitementFiles/" + candidateId;
                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Get all PDF files in the directory
                    var pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");

                    if (pdfFiles.Any())
                    {
                        var filesList = pdfFiles.Select(pdfFile =>
                        {
                            // Read the file into a byte array
                            byte[] fileBytes = System.IO.File.ReadAllBytes(pdfFile);

                            // Get the file name from the full path
                            var fileName = Path.GetFileName(pdfFile);

                            return fileName;
                        }).ToList();

                        return Ok(filesList);
                    }
                    else
                    {
                        // No PDF files found, return an appropriate response
                        return Ok("No PDF files found in the directory.");
                    }
                }
                else
                {
                    // Directory does not exist, return 404 Not Found response
                    return Ok("Directory not found.");
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


        //string resumePath = "/home/groupnb/go/src/github.com/dafalo/GNBRecruitementFiles/Resume/" + candidateId + ".pdf";
        // string directoryPath = "C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite\\" + candidateId+ "//" + fileType;
        //string directoryPath = "C:\\Users\\MarkJearoneFAlvarez\\Pictures\\gnbRecruite//" + candidateId + ".pdf";

        [HttpGet("fileContent")]
        public async Task<ActionResult> fileContent(int candidateId,string fileType)
        {
            try
            {
                string directoryPath = "/home/groupnb/go/src/github.com/dafalo/AppNBRecruitment/GNBRecruitementFiles/" + candidateId + "/" + fileType;

                if (!System.IO.File.Exists(directoryPath))
                {
                    return NotFound();
                }

                var resumeeBytes = System.IO.File.ReadAllBytes(directoryPath);
               // var resumeExtension = Path.GetExtension(directoryPath).ToLower();

                

                return File(resumeeBytes, "application/pdf");

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
