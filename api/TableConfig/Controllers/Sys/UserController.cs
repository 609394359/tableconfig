using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Models.Res;
using TableConfig.Models;
using TableConfig.Services;
using TableConfig.Services.Sys;
using TableConfig.Utils;
using TableConfig.Attributes;
using TableConfig.Models.Views;
using System.Linq;

namespace TableConfig.Controllers.Sys
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManager _tokenManager;
        private readonly CacheContext _cache;

        public UserController(ILogger<AuthController> logger, IUserService userService, TokenManager tokenManager, CacheContext cache)
        {
            _logger = logger;
            _userService = userService;
            _tokenManager = tokenManager;
            _cache = cache;
        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization]
        public async Task<ApiResult<List<SysUsers>>> List()
        {
            var list = await _userService.GetUserListAsync();

            return ToResult(list);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization]
        public async Task<ApiResult<List<UserTokenVM>>> Online()
        {
            var list = _cache.GetValues<UserTokenVM>(CacheContext.GroupToken).ToList();

            return await Task.FromResult(ToResult(list));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization]
        public async Task<ApiResult<SysUsers>> Get(long id)
        {
            var info = await _userService.GetInfoAsync(id);

            if (info == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            return ToResult(info);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization]
        public async Task<ApiResult<SysUsers>> Save([FromBody] SysUsersSaveReq req)
        {
            var info = await _userService.Save(req);

            return ToResult(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization]
        public async Task<ApiResult> Del([FromBody] BaseIdsReq req)
        {
            await _userService.Del(req.Ids);

            return ToResult("ok");
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Reset()
        {
            await _userService.Reset();

            return ToResult("ok");
        }
    }
}
