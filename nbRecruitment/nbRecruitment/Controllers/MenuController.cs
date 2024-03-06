using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using Newtonsoft.Json;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;

        public MenuController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }

        [HttpPost("userList")]
        public async Task<ActionResult> userList(int userId, string? search, int page, int size)
        {
            try
            {
                string[] searchSplitted = search == null ? null : search.Split(" ");
                var userList = _context.Users.
                     Where(x => x.Id != userId && x.IsDelete == 0&& x.Status == 1 && (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))).
                     OrderByDescending(x => x.Firstname).OrderByDescending(x => x.Status).
                     Skip(page * size).
                     Take(size).
                     ToList();

                int userCount = _context.Users.
                    Where(x => x.Id != userId && x.IsDelete == 0 && x.Status == 1 && (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))).Count();


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



        [HttpPost("userMenu")]
        public async Task<ActionResult> userMenu(int userId)
        {
            try
            {

                List<UserMenu> userMenu = _context.UserMenus.Where(x => x.UserId.Equals(userId) && x.Status == 1).ToList();

                return Ok(userMenu);

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


        [HttpPost("userMenusSave")]
        public async Task<ActionResult> userMenusSave(
            int userId,
            int selectedUserId,
            string selected
            )
        {
            try
            {

                List<bool> item = JsonConvert.DeserializeObject<List<bool>>(selected)!;

                List<UserMenu> toInactive = _context.UserMenus.Where(x => x.UserId == selectedUserId).ToList();

                foreach (var set in toInactive)
                {
                    set.Status = 0;
                }
                _context.SaveChanges();

                if (item[0] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 2).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 2,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }


                if (item[1] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 3).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 3,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }
                if (item[2] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 4).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 4,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }

                if (item[3] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 6).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 6,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }

                if (item[4] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 7).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 7,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }

                if (item[5] == true)
                {
                    UserMenu userMenu = _context.UserMenus.Where(x => x.UserId == selectedUserId && x.MenuId == 8).FirstOrDefault();

                    if (userMenu != null)
                    {
                        userMenu.Status = 1;
                        userMenu.ModifiedBy = userId;
                        userMenu.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        _context.UserMenus.Add(new UserMenu
                        {
                            MenuId = 8,
                            UserId = selectedUserId,
                            Status = 1,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now

                        });
                    }
                }
                _context.SaveChanges();
                return Ok("Successfully Change!");

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
