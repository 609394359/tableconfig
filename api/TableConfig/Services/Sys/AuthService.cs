/*
* ==============================================================================
*
* FileName: AuthService.cs
* Created: 2026/1/20 14:38:55
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Utils;

namespace TableConfig.Services.Sys
{
    public class AuthService: IAuthService
    {
        private readonly ISysUsersRepository _userRepository;

        public AuthService(ISysUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        public async Task<SysUsers> Login(string userCode, string password)
        {
            var info = await _userRepository.GetFirstAsync(m => m.UserCode == userCode);
            if (info == null)
                throw new Tip(StatusCodeType.Error, "账号不存在");

            if (!PasswordUtil.ComparePasswords(info.Id, info.Password, password))
                throw new Tip(StatusCodeType.Error, "密码错误");

            return info;
        }
    }
}
