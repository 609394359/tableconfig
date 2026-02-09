/*
* ==============================================================================
*
* FileName: AuthDto.cs
* Created: 2026/1/8 9:23:09
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Dto
{
    public class LoginDto
    {
        [Display(Name = "登录账号")]
        [Required(ErrorMessage = "登录账号不能为空")]
        public string Code { get; set;}

        [Display(Name = "登录密码")]
        [Required(ErrorMessage = "登录密码不能为空")]
        public string Password { get; set; }
    }
}
