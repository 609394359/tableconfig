/*
* ==============================================================================
*
* FileName: UserDto.cs
* Created: 2026/1/5 16:34:18
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TableConfig.Attributes;
using TableConfig.Converters;

namespace TableConfig.Models.Dto
{
    public class UserQueryDto : PageParm
    {

        [Display(Name = "关键字查询 账号/密码")]
        public string QueryText { get; set; }
    }
    public class UserSaveDto
    {
        public string Id { get; set; }

        [Display(Name = "登录账号")]
        [Required(ErrorMessage = "登录账号不能为空")]
        public string Code { get; set; }

        [Display(Name = "登录密码")]
        public string Password { get; set; }

        [Display(Name = "用户名称")]
        [Required(ErrorMessage = "用户名称不能为空")]
        public string Name { get; set; }
    }
    public class UserDelDto
    {
        [MinCollectionCount(1, ErrorMessage = "id不能为空")]
        public List<string> Ids { get; set; }
    }
}
