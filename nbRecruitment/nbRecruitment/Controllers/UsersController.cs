using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using Newtonsoft.Json;
using static nbRecruitment.Controllers.PostingController;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }


        [HttpPost("userList")]
        public async Task<ActionResult> userList(int userId,string? search, int page, int size)
        {
            try
            {
                string[] searchSplitted = search == null ? null : search.Split(" ");
                var userList = _context.Users.
                     Where(x => x.Id != userId && x.IsDelete == 0 && (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))).
                     OrderByDescending(x => x.Firstname).OrderByDescending(x => x.Status).
                     Skip(page * size).
                     Take(size).
                     ToList();

                int userCount = _context.Users.
                    Where(x => x.Id != userId && x.IsDelete == 0 && (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))).Count();
               

                return Ok(new { userList, userCount });

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

        [HttpGet("laguageList")]
        public async Task<ActionResult> laguageList()
        {
            try
            {
                return Ok(_context.Languages.Where(x => x.IsDelete.Equals(0) && x.Status.Equals(1)).Select(x => new { x.Name, x.Code}).ToList());

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


        [HttpGet("laguageWAsignList")]
        public async Task<ActionResult> laguageWAsignList(int userId)
        {
            try
            {
                var languageList = _context.Languages.Where(x => x.IsDelete.Equals(0) && x.Status.Equals(1)).Select(x => new { x.Name, x.Code }).ToList();

                List<string> userLanguage = _context.Userlanguages.Where(x => x.UserId.Equals(userId)).Select(
                    x => x.LangCode).ToList();

                return Ok(new {languageList, userLanguage });

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

        [HttpPost("addUser")]
        public async Task<ActionResult> addUser(User user, string languages)
        {
            try
            {

                User checker = _context.Users.Where(x => x.IsDelete.Equals(0) && (x.Username == user.Username || x.Email == user.Email)).FirstOrDefault();

                if (checker != null)
                {
                    return StatusCode(202, "Email or Username already exist!");
                }
                else
                {
                    List<string> LanguageCodes = JsonConvert.DeserializeObject<List<string>>(languages)!;


                    _context.Users.Add(user);
                    _context.SaveChanges();


                    foreach (string LanguageCode in LanguageCodes)
                    {
                        _context.Userlanguages.Add(new Userlanguage
                        {
                            UserId = user.Id,
                            LangCode = LanguageCode,
                            Status = 1,
                            CreatedBy = user.CreatedBy,

                        });

                    }


                    if(user.Type == "User")
                    {
                 
                    }

                    _context.SaveChanges();

                    return Ok("Added Successfully!");
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

        [HttpPost("editUser")]
        public async Task<ActionResult> editUser(User user, string languages)
        {
            try
            {

                User editUser = _context.Users.Where(x => x.IsDelete.Equals(0) && x.Id.Equals(user.Id)).FirstOrDefault();

                if (editUser == null)
                {
                    return StatusCode(202, "User doesn't exist!");
                }
                else
                {
                    List<string> LanguageCodes = JsonConvert.DeserializeObject<List<string>>(languages)!;


                    editUser.Firstname = user.Firstname;
                    editUser.Lastname = user.Lastname;
                    editUser.Middlename = user.Middlename;
                    editUser.Fullname = user.Fullname;
                    editUser.Fullname2 = user.Fullname2;
                    editUser.Type = user.Type;
                    editUser.Status = user.Status;
                    editUser.ModifiedBy = user.ModifiedBy;
                    editUser.ModifiedDate = DateTime.Now;

                  List<Userlanguage> oldLanguage = _context.Userlanguages.Where(x => x.UserId.Equals(user.Id)).ToList();

                    foreach (var item in oldLanguage)
                    {
                        item.Status = 0;
                    }
                    

                    foreach (string LanguageCode in LanguageCodes)
                    {

                        Userlanguage userLanguage = _context.Userlanguages.Where(x => x.UserId.Equals(user.Id) && x.LangCode == LanguageCode).FirstOrDefault();

                        if (userLanguage != null)
                        {
                            userLanguage.Status = 1;
                        }
                        else
                        {
                            _context.Userlanguages.Add(new Userlanguage
                            {
                                UserId = user.Id,
                                LangCode = LanguageCode,
                                Status = 1,
                                CreatedBy = user.CreatedBy,

                            });

                        }

                     

                    }
                    _context.SaveChanges();

                    return Ok("Saved Successfully!");
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
