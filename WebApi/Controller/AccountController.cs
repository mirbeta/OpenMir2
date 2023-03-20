using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 认证授权API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetToken")]
        public IActionResult GetToken(string userName, string password)
        {
            return Ok();
        }

        /// <summary>
        /// 自助解封
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Unblocking")]
        public IActionResult Unblocking()
        {
            return Ok();
        }
    }
}