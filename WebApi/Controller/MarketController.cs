using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{
    /// <summary>
    /// 寄售行API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MarketController : ControllerBase
    {
        /// <summary>
        /// 获取寄售行数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult Market()
        {
            return Ok();
        }
        
        /// <summary>
        /// 获取寄售行物品类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MarketType")]
        public IActionResult MarketType()
        {
            return Ok();
        }

        /// <summary>
        /// 购买寄售行物品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("BuyItem")]
        public IActionResult BuyItem()
        {
            return Ok();
        }

        /// <summary>
        /// 搜索寄售行物品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Search")]
        public IActionResult SearchItem()
        {
            return Ok();
        }

        /// <summary>
        /// 物品寄售
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SellItem")]
        public IActionResult SellItem()
        {
            return Ok();
        }
        
        /// <summary>
        /// 取消物品寄售
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CancelSell")]
        public IActionResult CancelSell()
        {
            return Ok();
        }
    }
}