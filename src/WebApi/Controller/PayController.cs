using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 充值API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PayController : ControllerBase
    {
        /// <summary>
        /// 获取充值记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            return Ok();
        }
        
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Pay")]
        public IActionResult Pay()
        {
            return Ok();
        }
    }
}