using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbRecruitment.Models;
using static nbRecruitment.Controllers.LoginController;
using System.Text;
using static nbRecruitment.Controllers.ProfileController;

namespace nbRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly NbRecruitmentContext _context;
        private readonly IConfiguration _configuration;


        public ProfileController(NbRecruitmentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //    _pdfConverter = pdfConverter;
        }
        public class classProfileDetails
        {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string firstname { get; set; }
            public string middlename { get; set; }
            public string lastname { get; set; }
            public string email { get; set; }
        }

      string Encrypt(string password)
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
                Byte[] input = Encoding.UTF8.GetBytes(password);
                Byte[] output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                password = Convert.ToBase64String(output);
            }

            return password;
        }

        [HttpPost("ProfileDetails")]
        public async Task<ActionResult> ProfileDetails(classProfileDetails classProfileDetails)
        {
            try
            {
                User user = _context.Users.Where(x => x.Id == classProfileDetails.id).FirstOrDefault()!;


                 

                if ( user == null )
                {
                    return StatusCode(202, "Not Equal Id");
                }
                else
                {

                    if (Encrypt(classProfileDetails.password)  != user.Password)
                    {
                        return StatusCode(202, "Invalid Password!");
                    }

                    user.Username = classProfileDetails.username;
                    user.Firstname = classProfileDetails.firstname;
                    user.Middlename = classProfileDetails.middlename;
                    user.Lastname = classProfileDetails.lastname;
                    user.Email = classProfileDetails.email;
                    user.ModifiedBy = classProfileDetails.id;
                    user.ModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                }

                var newUser = _context.Users.Where(x => x.Id == classProfileDetails.id).Select(
                    x => new
                    {
                        x.Id,
                        x.Username,
                        x.Firstname,
                        x.Middlename,
                        x.Lastname,
                        x.Email,
                        x.CreatedBy,
                        x.CreatedDate,
                        x.Fullname,
                        x.Fullname2,
                        x.Type,
                        x.Sbucode
                    }
                    ).FirstOrDefault()!;

                return Ok(newUser);
              

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


        [HttpPost("changePassword")]
        public async Task<ActionResult> changePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {

               
                User user = _context.Users.Where(x => x.Id == userId && x.Password == Encrypt(currentPassword)).FirstOrDefault()!;

                if (user == null)
                {
                    return StatusCode(202, "Invalid Password!");
                }
                else
                {
                    user.Password = Encrypt(newPassword);


                    _context.SaveChanges();
                }

                return Ok("Success!");
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
