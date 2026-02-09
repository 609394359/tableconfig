/*
* ==============================================================================
*
* FileName: BaserQuery.cs
* Created: 2026/1/1 16:43:47
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models
{
    public class PageParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Display(Name = "当前页")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        [Display(Name = "每页总条数")]
        public int PageSize { get; set; } = 20;


        /// <summary>
        /// 排序方式
        /// </summary>
        [Display(Name = "排序方式-(倒叙：desc/descending  正序：asc)")]
        public List<PageOrderByParm> PageOrderBy { get; set; }
    }

    public class PageOrderByParm
    {

        /// <summary>
        /// 排序字段
        /// </summary>
        [Display(Name = "排序字段")]
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        [Display(Name = "排序方式-(倒叙：desc/descending  正序：asc)")]
        public string Sort { get; set; }
    }
}
