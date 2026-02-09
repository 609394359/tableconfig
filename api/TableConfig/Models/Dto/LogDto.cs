/*
* ==============================================================================
*
* FileName: LogDto.cs
* Created: 2026/1/3 11:18:30
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Dto
{
    public class LogQueryDto: PageParm
    {
        [Required(ErrorMessage ="日志配置id不能为空")]
        public string ConfigId { get; set; }
        public List<LogQueryItem> Searchs { get; set; }
    }

    public class LogQueryItem
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
    }

    public class LogDelDto
    {
        [Required(ErrorMessage = "日志配置id不能为空")]
        public string ConfigId { get; set; }
        public List<string> Ids { get; set; }
    }

    public class LogImportDto
    {
        [Required(ErrorMessage = "日志配置id不能为空")]
        public string ConfigId { get; set; }

        [Display(Name = "file")]
        public IFormFile File { get; set; } // 文件
    }
}
