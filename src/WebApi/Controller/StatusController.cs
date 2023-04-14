using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 系统状态API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// 获取系统状态
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok();
        }
        
        /// <summary>
        /// 获取游戏服务器状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GameSrv")]
        public IActionResult GameSrv()
        {
            return Ok();
        }

        /// <summary>
        /// 获取数据库服务器状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DBSrv")]
        public IActionResult DBSrv()
        {
            return Ok();
        }

        /// <summary>
        /// 获取登录服务器状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoginSrv")]
        public IActionResult LoginSrv()
        {
            return Ok();
        }
    }
}