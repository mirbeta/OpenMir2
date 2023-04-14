using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 游戏行会API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GuildController : ControllerBase
    {
        /// <summary>
        /// 获取所有游戏行会数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult Items()
        {
            return Ok();
        }

        /// <summary>
        /// 获取游戏行会详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id:int}")]
        public IActionResult Detail(int Id)
        {
            return Ok();
        }
    }
}