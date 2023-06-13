using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 名人榜API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CelebrityController : ControllerBase
    {
        /// <summary>
        /// 获取名人榜
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCelebrity")]
        public IActionResult GetCelebrity()
        {
            return Ok();
        }

        /// <summary>
        /// 获取战士名人榜
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Warriors")]
        public IActionResult Warriors()
        {
            return Ok();
        }

        /// <summary>
        /// 获取法师名人榜
        /// </summary>
        [HttpGet]
        [Route("Wizards")]
        public IActionResult Wizards()
        {
            return Ok();
        }

        /// <summary>
        /// 获取道士名人榜
        /// </summary>
        [HttpGet]
        [Route("Taoists")]
        public IActionResult Taoists()
        {
            return Ok();
        }
    }
}