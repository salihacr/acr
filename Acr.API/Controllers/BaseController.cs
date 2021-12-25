using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acr.API.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // yetki kontrolü yapılabilir burada
            base.OnActionExecuting(context);
        }

        [HttpGet("xxx")]
        public int GetUserId()
        {
            try
            {
                string token = Request.Headers["bearer"];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var jwtToken = jsonToken as JwtSecurityToken;

                string userId = jwtToken.Claims.First(claim => claim.Type == "userId").Value ?? "0";
                return Convert.ToInt32(userId);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}