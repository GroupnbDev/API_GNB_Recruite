using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostingController : ControllerBase
    {


        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public PostingController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpGet("lanJob")]
        public async Task<ActionResult> lanJob()
        {
            try
            {


                var language = _context.Languages.Where(x=>x.Status == 1 && x.IsDelete == 0).Select(
                    x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                    }
                    ).ToList();

                var jobType = _context.Jobtypes.Where(x=>x.Status.Equals(1) && x.IsDelete.Equals(0)).Select( x=> new
                {
                    x.Id,
                    x.Code,
                    x.Name
                }).ToList();

                List<VLanguageRecruiter> userlanguages = _context.VLanguageRecruiters.Where(x => x.Status == 1).ToList();

                return Ok(new { language, jobType, userlanguages });

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


        [HttpGet("userLanguage")]
        public async Task<ActionResult> userLanguage()
        {
            try
            {
                List<VLanguageRecruiter> userlanguages = _context.VLanguageRecruiters.Where(x => x.Status == 1).ToList();


                return Ok(userlanguages);

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


        public partial class aePosting
        {
            public Posting posting { get; set; }
            public string tagUsers { get; set; }

        }


        [HttpPost("postingList")]
        public async Task<ActionResult> postingList(int employeeId,  string? search, int page,int size)
        {
            try
            {

               var postingList = _context.Postings.
                    Where(x => x.CreatedBy.Equals(employeeId) && x.IsDelete == 0 && (search == null? x.JobType.Contains("") : x.JobType.Contains(search))).
                    OrderByDescending(x => x.Id).
                    Skip(page * size).
                    Take(size).
                    Select(
                   x => new
                   {
                       x.Id,
                       x.JobCode,
                       x.JobType,
                       x.LanguageCodes,
                       x.PositionCount,
                       x.Location,
                       x.Currency,
                       x.Per,
                       x.Salary,
                       x.Description,
                       x.Requirements,
                       x.Type,
                       x.Status,
                       x.CreatedDate,
                       Asign = _context.AsignUsers.Where(i => i.PostingId.Equals(x.Id) && i.Status.Equals(1)).ToList(),
                   }
                   ).
                    ToList();

                int postingCount = _context.Postings.
                    Where(x => x.CreatedBy.Equals(employeeId) && x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).Count();


                return Ok(new { postingList , postingCount });

            }
            catch (Exception ex)
            {
                return StatusCode(202, ex.Message);
            }
        }

        [HttpPost("AddPosting")]
        public async Task<ActionResult> AddPosting(Posting posting,string tagUsers)
        {
            try
            {
          
                List<dynamic> test = JsonConvert.DeserializeObject<List<dynamic>>(tagUsers)!;
                _context.Postings.Add(posting);
                _context.SaveChanges();

                List<string> LanguageCodes = JsonConvert.DeserializeObject<List<string>>(posting.LanguageCodes)!;

                int theIndex = 0;

                foreach (var item in LanguageCodes)
                {

                 
                    int count = 1;
                    foreach (var Id in test[theIndex])
                    {
                        _context.AsignUsers.Add(new AsignUser
                        {
                            PostingId = posting.Id,
                            UserId = Id,
                            LanguageCode = item,
                            Count = count,
                            Status = 1
                        });
                        count++;
                    }

                    theIndex++;
                }
                _context.SaveChanges();

                return Ok("Successfully Added!");

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
        [HttpPost("editPosting")]
        public async Task<ActionResult> editPosting(Posting newPosting, string tagUsers)
        {
            try
            {

                Posting posting = _context.Postings.Where(x => x.Id == newPosting.Id).FirstOrDefault();

                posting.JobCode = newPosting.JobCode;
                posting.JobType = newPosting.JobType;
                posting.LanguageCodes = newPosting.LanguageCodes;
                posting.PositionCount = newPosting.PositionCount;
                posting.Location = newPosting.Location;
                posting.Currency = newPosting.Currency;
                posting.Per = newPosting.Per;
                posting.Salary = posting.Salary;
                posting.Description = newPosting.Description;
                posting.Requirements = newPosting.Requirements;
                posting.Type = newPosting.Type;
                posting.Status = newPosting.Status;
                posting.ModifiedBy = newPosting.ModifiedBy;
                posting.ModifiedDate = DateTime.Now;
                posting.Status = newPosting.Status;

                List<AsignUser> asignUsers = _context.AsignUsers.Where(x => x.PostingId.Equals(posting.Id)).ToList();

                foreach (var item in asignUsers)
                {
                    item.Status = 0;
                }
                _context.SaveChanges();

                List<dynamic> test = JsonConvert.DeserializeObject<List<dynamic>>(tagUsers)!;
                List<string> LanguageCodes = JsonConvert.DeserializeObject<List<string>>(posting.LanguageCodes)!;

                int theIndex = 0;

                foreach (var item in LanguageCodes)
                {


                  


                   
                        int count = 1;
                        foreach (int Id in test[theIndex])
                        {
                            AsignUser aUser = _context.AsignUsers.Where(x => x.UserId.Equals(Id) && x.LanguageCode == item).FirstOrDefault()!;

                        if (aUser == null)
                        {
                            _context.AsignUsers.Add(new AsignUser
                            {
                                PostingId = posting.Id,
                                UserId = Id,
                                LanguageCode = item,
                                Count = count,
                                Status = 1
                            });
                            count++;
                        }
                        else
                        {
                            aUser.Status = 1;
                        }
                       
                        
                    }
                 

                    theIndex++;
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

    }
}
