/*
* ==============================================================================
*
* FileName: LoginReq.cs
* Created: 2026/1/20 14:44:52
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Req
{
    public class LoginReq
    {

        [Display(Name = "账号")]
        [Required(ErrorMessage = "请输入账号")]
        public string UserCode { get; set; }

        [Display(Name = "登录密码")]
        [Required(ErrorMessage = "请输入登录密码")]
        public string Password { get; set; }
    }
}
