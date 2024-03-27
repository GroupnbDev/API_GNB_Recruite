using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using static nbRecruitment.Controllers.PostingController;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public DashboardController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpGet("candidatesProgression")]
        public async Task<ActionResult> candidatesProgression(int? userId, DateTime from, DateTime to)
        {
            try
            {

                List<Candidate> candidates = _context.Candidates.Where(x => (userId == null ? x.Firstname.Contains("") : x.AsignTo == userId) && x.ModifiedDate.Value.Date >= from.Date && x.ModifiedDate.Value.Date <= to).ToList();

                List<int> values = new();

                if(candidates.Count == 0)
                {
                    values.Add(0);
                    values.Add(0);
                    values.Add(0);
                }
                else
                {
                    values.Add(candidates.Where(x => x.StatusDescription == "Progress").Count());
                    values.Add(candidates.Where(x => x.StatusDescription == "Selected").Count());
                    values.Add(candidates.Where(x => x.StatusDescription == "Not Selected").Count());
                }

                return Ok(values);
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

        [HttpGet("regCandidates")]
        public async Task<ActionResult> regCandidates(int? userId, string Dates)
        {
            try
            {



                List<DateTime> dates = JsonConvert.DeserializeObject<List<DateTime>>(Dates)!;
                List<int> values = new();

                foreach (var date in dates)
                {

                    values.Add(_context.Candidates.Where(x => (userId == null ? x.Firstname.Contains("") : x.AsignTo == userId) &&  x.CreatedDate.Value.Month == date.Month && x.CreatedDate.Value.Year == date.Year).Count());
                    
                }

                return Ok(values);
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

        public class Goal
        {
            public string name { get; set; }
            public int value { get; set; }
            public int strokeWidth { get; set; }
            public int? strokeHeight { get; set; } // Nullable int to handle optional property
            public string strokeLineCap { get; set; } // Optional property
            public string strokeColor { get; set; }
        }

        public class DataPoint
        {
            public string X { get; set; }
            public int Y { get; set; }
            public List<Goal> Goals { get; set; }
        }


        [HttpGet("openPosition")]
        public async Task<ActionResult> openPosition(int? userId, string? PostingId)
        {
            try
            {
                List<Posting> openPosting = _context.Postings.Where(x => (userId == null ? x.JobType.Contains("") : x.CreatedBy == userId) && x.Status == 1 && x.IsDelete == 0).ToList();
                List< DataPoint > points = new List< DataPoint >();


                foreach (var posting in openPosting)
                {
                    int candidates = _context.Candidates.Where(x => (userId == null? x.Firstname.Contains("") : x.AsignTo == userId) && x.PostingId == posting.Id && x.StatusDescription == "Selected").Count();
                  
                    List<Goal> goals = new List<Goal>();

                    goals.Add(new Goal
                    {
                        name = "Expected",
                        value = posting.PositionCount,
                        strokeWidth = 10,
                        strokeHeight = 5,
                        strokeLineCap = "round",
                        strokeColor = "#775DD0"
                    });

                    points.Add(new DataPoint
                    {
                        X = posting.JobType,
                        Y = candidates,
                        Goals = goals
                    });
                   
                }



                return Ok(points);
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
