using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public LanguageController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpPost("languageList")]
        public async Task<ActionResult> languageList(string? search, int page, int size)
        {
            try
            {

                var languageList = _context.Languages.
                     Where(x => x.IsDelete == 0 && (search == null ? x.Name.Contains("") : x.Name.Contains(search))).
                     OrderByDescending(x => x.Name).OrderByDescending(x => x.Status).
                     Skip(page * size).
                     Take(size).
                     ToList();

                int languageCount = _context.Languages.
                    Where(x => x.IsDelete == 0 && (search == null ? x.Name.Contains("") : x.Name.Contains(search))).Count();


                return Ok(new { languageList, languageCount });

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


        [HttpPost("addLanguage")]
        public async Task<ActionResult> addLanguage(Language language)
        {
            try
            {

                Language checker = _context.Languages.Where(x => x.Code.Equals(language.Code)).FirstOrDefault();

                if(checker != null)
                {
                    return StatusCode(202, "Code is already Exist!");
                }
                else
                {
                  
                    _context.Languages.Add(language);
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

        [HttpPost("editLanguage")]
        public async Task<ActionResult> editLanguage(Language language)
        {
            try
            {

                Language checker = _context.Languages.Where(x => x.Code.Equals(language.Code)).FirstOrDefault();

                if (checker != null)
                {

                    checker.Name = language.Name;
                    checker.Status = language.Status;
                    checker.ModifiedBy = language.ModifiedBy;
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
