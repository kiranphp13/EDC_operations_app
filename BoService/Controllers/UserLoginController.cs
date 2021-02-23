using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IConfiguration _config;
        public BoAppDB Db { get; }
        public UserLoginController(BoAppDB db, IConfiguration config)
        {
            Db = db;
            _config = config;
        }
        // POST api/blog
        [HttpPost]
        public Dictionary<string, object> Post([FromBody] BoService.Models.Users value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;
                string username = value.User_Name;
                string password = value.User_Password;
                BoService.Models.Users objUsers = new BoService.Models.Users(Db);
                objUsers.User_Name = value.User_Name;
                objUsers.User_Password = value.User_Password;
                bIsValidUser = objUsers.IsUserRegistered();
                if(bIsValidUser == true)
                {
                    var jwt = new BoService.Authentication.JwtService(_config);
                    var token = jwt.GenerateSecurityToken(value.User_Name);
                    response.Add("status", "success");
                    response.Add("User Status", "Valid User Credential...");
                    response.Add("User Token", token);
                }
                else
                {
                    response.Add("status", "Error");
                    response.Add("message", "Invalid User Credentials...");
                }
            }
            catch(Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("message", Ex.Message);
            }
            return response;
        }
     }
}
