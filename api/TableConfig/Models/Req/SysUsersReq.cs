/*
* ==============================================================================
*
* FileName: SysUsersReq.cs
* Created: 2026/1/21 16:33:45
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Req
{
    public class SysUsersSaveReq
    {

        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "账号")]
        [Required(ErrorMessage = "账号不能为空")]
        public string UserCode { get; set; }

        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不能为空")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}
