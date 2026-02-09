using System.Threading.Tasks;
using TableConfig.Models.Entitys;

namespace TableConfig.IServices.Sys
{
    public interface IAuthService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        Task<SysUsers> Login(string userCode, string password);
    }
}
