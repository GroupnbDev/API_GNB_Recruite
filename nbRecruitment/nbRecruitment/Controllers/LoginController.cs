using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using System.Text;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public LoginController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }
        [HttpPost("insertEncryption")]
        public async Task<ActionResult> insertEncryption(string password, int id)
        {
            try
            {

                User user = _context.Users.Where(x => x.Id == id).FirstOrDefault()!;

                String keySTR = "GroupNBEncry2024"; //16 byte
                String ivSTR = "GroupNBEncry2024"; //16 byte

                using (System.Security.Cryptography.RijndaelManaged rjm =
                                       new System.Security.Cryptography.RijndaelManaged
                                       {
                                           KeySize = 128,
                                           BlockSize = 128,
                                           Key = ASCIIEncoding.ASCII.GetBytes(keySTR),
                                           IV = ASCIIEncoding.ASCII.GetBytes(ivSTR)
                                       }
                           )
                {
                    Byte[] input = Encoding.UTF8.GetBytes(password);
                    Byte[] output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                    user.Password = Convert.ToBase64String(output);
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

        public partial class loggingin
        {
            public string password { get; set; }
            public string username { get; set; }

        }

        [HttpPost("login")]
        public async Task<ActionResult> login(loggingin loggingin)
        {
            try
            {
                String keySTR = "GroupNBEncry2024"; //16 byte
                String ivSTR = "GroupNBEncry2024"; //16 byte

                using (System.Security.Cryptography.RijndaelManaged rjm =
                                       new System.Security.Cryptography.RijndaelManaged
                                       {
                                           KeySize = 128,
                                           BlockSize = 128,
                                           Key = ASCIIEncoding.ASCII.GetBytes(keySTR),
                                           IV = ASCIIEncoding.ASCII.GetBytes(ivSTR)
                                       }
                           )
                {
                    Byte[] input = Encoding.UTF8.GetBytes(loggingin.password);
                    Byte[] output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                    loggingin.password = Convert.ToBase64String(output);
                }


                User user = _context.Users.Where(x => x.Username == loggingin.username && x.Password == loggingin.password).FirstOrDefault();


                if (user != null)
                {
                    List<String> userMenuTemp1 = _context.UserMenus.Where(x => x.UserId == user.Id && x.Status == 1).Select(x => x.MenuId.ToString()).ToList();

                    List<Menu> menuListTemp1 = _context.Menus.Where(x => userMenuTemp1.Contains(x.Id.ToString())).ToList();

                    List<Menu> parentTemp1 = _context.Menus.Where(x => menuListTemp1.Select(s => s.ParentId.ToString()).Contains(x.Id.ToString())).ToList();


                    return Ok(new {user, menuListTemp1, parentTemp1 });
                }
                else
                {
                    return StatusCode(202, "Incorrect Username or Password");
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
