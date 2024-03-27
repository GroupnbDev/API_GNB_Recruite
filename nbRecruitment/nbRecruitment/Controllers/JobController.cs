using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nbRecruitment.Models;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;

        public JobController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpPost("jobTypeList")]
        public async Task<ActionResult> jobTypeList( string? search, int page, int size)
        {
            try
            {

                var jobList = _context.Jobtypes.
                     Where(x => x.IsDelete == 0 && (search == null ? x.Name.Contains("") : x.Name.Contains(search))).
                     OrderByDescending(x => x.Name).OrderByDescending(x => x.Status).
                     Skip(page * size).
                     Take(size).
                     ToList();

                int jobCount = _context.Jobtypes.
                    Where(x => x.IsDelete == 0 && (search == null ? x.Name.Contains("") : x.Name.Contains(search))).Count();


                return Ok(new { jobList, jobCount });

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

        [HttpPost("addJobType")]
        public async Task<ActionResult> addJobType(Jobtype jobtype)
        {
            try
            {

                Jobtype checker = _context.Jobtypes.Where(x => x.Code.Equals(jobtype.Code)).FirstOrDefault();

                if(checker != null)
                {
                    return StatusCode(202, "Code is already Exist!");
                }
                else
                {
                  
                    _context.Jobtypes.Add(jobtype);
                    _context.SaveChanges();
                    return Ok("Added Succesfully!");

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

        [HttpPost("editJobType")]
        public async Task<ActionResult> editJobType(Jobtype jobtype)
        {
            try
            {

                Jobtype checker = _context.Jobtypes.Where(x => x.Code.Equals(jobtype.Code)).FirstOrDefault();

                if (checker != null)
                {

                    checker.Name = jobtype.Name;
                    checker.Status = jobtype.Status;
                    checker.ModifiedBy = jobtype.ModifiedBy;
                    checker.ModifiedDate = DateTime.Now; 
                    _context.SaveChanges();
                    return Ok("Saved Succesfully!");
                }
                else
                {
                    return StatusCode(202, "Doesn't Exist!");

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
