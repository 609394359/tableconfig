using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TableConfig.Attributes;
using TableConfig.IServices.Sys;
using TableConfig.Models;
using TableConfig.Models.Req;
using TableConfig.Models.Views;
using TableConfig.Services;
using TableConfig.Services.Sys;

namespace TableConfig.Controllers.Sys
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly TokenManager _tokenManager;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, TokenManager tokenManager)
        {
            _logger = logger;
            _authService = authService;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Login([FromBody] LoginReq req)
        {
            var info = await _authService.Login(req.UserCode, req.Password);


            var tokenInfo = new UserTokenVM
            {
                Id = info.Id,
                Code = info.UserCode,
                Name = info.UserName,
                Token = Guid.NewGuid().ToString(),
                LoginTime = DateTime.Now
            };

            var token = _tokenManager.SetToken(tokenInfo);

            return ToResult(token);
        }

        [HttpPost]
        [Authorization]
        public ApiResult LoginOut()
        {
            _tokenManager.Remove();

            return new ApiResult(200, "ok");
        }

        [HttpGet]
        [Authorization]
        public ApiResult<UserTokenVM> Get()
        {
            var token = _tokenManager.TokenInfo;

            return new ApiResult<UserTokenVM>(token);
        }

    }
}
