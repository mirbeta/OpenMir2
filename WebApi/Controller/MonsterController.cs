using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 游戏怪物API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MonsterController : ControllerBase
    {
        /// <summary>
        /// 获取所有游戏怪物数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult Items()
        {
            return Ok();
        }

        /// <summary>
        /// 获取怪物道具详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Detail(int Id)
        {
            return Ok();
        }
    }
}