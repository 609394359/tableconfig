/*
* ==============================================================================
*
* FileName: LogConfigDto.cs
* Created: 2026/1/1 16:51:07
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using TableConfig.Converters;
using TableConfig.Attributes;

namespace TableConfig.Models.Dto
{
    public class LogConfigQueryDto
    {

        [Display(Name = "关键字查询")]
        public string QueryText { get; set; }

    }

    public class LogConfigSaveDto
    {
        public string Id { get; set; }

        [Display(Name = "日志名称")]
        [Required(ErrorMessage = "日志名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "段落分割")]
        [Required(ErrorMessage = "段落分割不能为空")]
        public string Pattern { get; set; }

        [Required(ErrorMessage = "排序字段不能为空")]
        public string Sort { get; set; }

        [Required(ErrorMessage = "排序规则不能为空")]
        [AllowedValues("asc","desc",ErrorMessage = "排序规则错误")]
        public string SortOrder { get; set; }

        [MinCollectionCount(1,ErrorMessage = "数据字段不能为空")]
        public List<LogFiledSaveDto> Fields { get; set; }
    }

    public class LogFiledSaveDto
    {
        [Display(Name = "数据字段")]
        [Required(ErrorMessage = "数据字段不能为空")]
        public string Name { get; set; }

        [Display(Name = "显示标题")]
        [Required(ErrorMessage = "显示标题不能为空")]
        public string Title { get; set; }

        [Display(Name = "数据类型")]
        [Required(ErrorMessage = "数据类型不能为空")]
        public string Type { get; set; }

        [Display(Name = "显示顺序")]
        public int ShowIndex { get; set; }

        [Display(Name = "显示列宽")]
        public string Width { get; set; }

        [Display(Name = "查询排序")]
        public int SearchIndex { get; set; }

        [Display(Name = "提取规则")]
        [Required(ErrorMessage = "匹配规则不能为空")]
        public string Pattern { get; set; }
    }
    public class LogConfigDelDto
    {

        [Required(ErrorMessage = "id不能为空")]
        public string Id { get; set; }
    }
}
