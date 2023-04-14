using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 游戏道具API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        /// <summary>
        /// 获取所有游戏道具数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult Items()
        {
            return Ok();
        }

        /// <summary>
        /// 获取游戏道具详情
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