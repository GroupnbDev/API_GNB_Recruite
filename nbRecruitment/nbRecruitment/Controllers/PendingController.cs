using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using nbRecruitment.Models;
using nbRecruitment.ModelsERP;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User = nbRecruitment.Models.User;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly GnberpContext _contextP;
        private readonly IConfiguration _configuration;


        public PendingController(NbRecruitmentContext context, GnberpContext contextP, IConfiguration configuration)
        {
            _context = context;
            _contextP = contextP;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }

        [HttpPost("postingList")]
        public async Task<ActionResult> postingList( string? search, int page, int size)
        {
            try
            {

                var postingList = _context.Postings.
                     Where(x => x.IsPending == 1 && x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).
                     OrderByDescending(x => x.Id).
                     Skip(page * size).
                     Take(size).
                     Select(
                    x => new
                    {
                        x.Id,
                        x.JobCode,
                        x.JobType,
                        x.PositionCount,
                        x.Location,
                        x.Currency,
                        x.Per,
                        x.Salary,
                        x.ClientId,
                        x.ClientName,
                        x.Responsibility,
                        x.Description,
                        x.Requirements,
                        x.Type,
                        x.Status,
                        x.CreatedDate,
                        Asign = _context.AsignUsers.Where(i => i.PostingId.Equals(x.Id) && i.Status.Equals(1)).Select(
                            i => new
                            {
                                i.UserId,
                                fullname = _context.Users.Where(b => b.Id == i.UserId).Select(b => b.Fullname).FirstOrDefault()
                            }
                            ).ToList(),
                    }
                    ).
                ToList();

                int postingCount = _context.Postings.
                    Where(x => x.IsPending == 1 && x.IsDelete == 0 && (search == null ? x.JobType.Contains("") : x.JobType.Contains(search))).Count();


                return Ok(new { postingList, postingCount });

            }
            catch (Exception ex)
            {
                return StatusCode(202, ex.Message);
            }
        }


        [HttpPost("approved")]
        public async Task<ActionResult> approved(int postingId, sbyte isApproved, int approver)
        {
            try
            {

                Posting posting = _context.Postings.Where(x => x.Id == postingId).FirstOrDefault();
                posting.Approver = approver;
                posting.IsPending = isApproved;

                _context.SaveChanges();

                return Ok("Successfully changed!");
           

            }
            catch (Exception ex)
            {
                return StatusCode(202, ex.Message);
            }
        }

    }
}
