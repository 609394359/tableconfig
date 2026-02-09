/*
* ==============================================================================
*
* FileName: SysUsers.cs
* Created: 2026/1/19 16:28:52
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Entitys
{
    [SugarTable("t_sys_users")]
    public class SysUsers : IDeleted
    {
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [Display(Name = "账号")]
        public string UserCode { get; set; }

        [Display(Name = "名称")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Newtonsoft.Json.JsonIgnore]
        public string Password { get; set; }


        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "创建用户Id")]
        public long CreateId { get; set; }

        [Display(Name = "创建用户名称")]
        public string CreateName { get; set; }

        [Display(Name = "修改时间")]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "修改用户Id")]
        public long UpdateId { get; set; }

        [Display(Name = "修改用户名称")]
        public string UpdateName { get; set; }

        [Display(Name = "是否删除", Order = 0)]
        public bool IsDeleted { get; set; }
    }
}
