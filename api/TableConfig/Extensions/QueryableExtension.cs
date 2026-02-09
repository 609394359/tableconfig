/*
* ==============================================================================
*
* FileName: QueryableExtension.cs
* Created: 2026/1/20 12:28:48
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TableConfig.Models;

namespace TableConfig.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PageInfo<T>> ToPageAsync<T>(this ISugarQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = await source.CountAsync();

            if (total == 0)
                return new PageInfo<T>([], 0, 1, pageSize);

            var totalPage = (total + pageSize - 1) / pageSize;

            var items = await source.ToPageListAsync(pageIndex, pageSize);

            return new PageInfo<T>(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 读取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PageInfo<T>> ToPageAsync<T>(this ISugarQueryable<T> source, PageParm parm)
        {
            var total = await source.CountAsync();

            if (total == 0)
                return new PageInfo<T>([], 0, 1, parm.PageSize);

            var totalPage = (total + parm.PageSize - 1) / parm.PageSize;

            if (parm.PageOrderBy != null)
            {
                foreach (var orderBy in parm.PageOrderBy)
                {
                    source.OrderByIF(!string.IsNullOrEmpty(orderBy.Sort), $"{orderBy.OrderBy} {(orderBy.Sort == "descending" || orderBy.Sort == "desc" ? "desc" : "asc")}");
                }
            }

            var items = await source.ToPageListAsync(parm.PageIndex, parm.PageSize);

            return new PageInfo<T>(items, total, parm.PageIndex, parm.PageSize);
        }



        /// <summary>
        /// 设置排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderby"></param>
        /// <param name="sort">asc/desc</param>
        /// <returns></returns>
        public static PageParm SetOrderBy(this PageParm source, string orderby, string sort = "desc")
        {
            if (source.PageOrderBy == null) source.PageOrderBy = new List<PageOrderByParm>();
            source.PageOrderBy.Add(new PageOrderByParm { OrderBy = orderby, Sort = sort == "descending" || sort == "desc" ? "descending" : "asc" });

            return source;
        }


        /// <summary>
        /// 设置排序--推荐使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <param name="sort">asc/desc</param>
        /// <returns></returns>
        public static PageParm SetOrderBy<T>(this PageParm source, Expression<Func<T, object>> order, string sort = "desc") where T : class, new()
        {
            if (order == null) return source;

            if (source.PageOrderBy == null) source.PageOrderBy = new List<PageOrderByParm>();

            if (order.Body is MemberExpression)
            {
                source.SetOrderBy((order.Body as MemberExpression).Member.Name, sort);
            }
            else if (order.Body is NewExpression)
            {
                foreach (var item in (order.Body as NewExpression).Arguments)
                {
                    source.SetOrderBy((item as MemberExpression).Member.Name, sort);
                }
            }
            else if (order.Body is UnaryExpression)
            {
                var unary = order.Body as UnaryExpression;
                if (unary.Operand is MemberExpression)
                {
                    source.SetOrderBy((unary.Operand as MemberExpression).Member.Name, sort);
                }
            }
            return source;
        }


        /// <summary>
        /// 设置默认排序--推荐使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <param name="defaultSort">asc/desc</param>
        /// <returns></returns>
        public static PageParm SetDefaultOrderBy<T>(this PageParm source, Expression<Func<T, object>> order, string defaultSort = "desc") where T : class, new()
        {
            if (order == null) return source;

            if (source.PageOrderBy == null || source.PageOrderBy.Count <= 0 || (source.PageOrderBy.Count == 1 && string.IsNullOrWhiteSpace(source.PageOrderBy[0].OrderBy)))
            {
                source.PageOrderBy = [];
            }
            else
            {
                return source;
            }


            if (order.Body is MemberExpression)
            {
                source.SetOrderBy((order.Body as MemberExpression).Member.Name, defaultSort);
            }
            else if (order.Body is NewExpression)
            {
                foreach (var item in (order.Body as NewExpression).Arguments)
                {
                    source.SetOrderBy((item as MemberExpression).Member.Name, defaultSort);
                }
            }
            else if (order.Body is UnaryExpression)
            {
                var unary = order.Body as UnaryExpression;
                if (unary.Operand is MemberExpression)
                {
                    source.SetOrderBy((unary.Operand as MemberExpression).Member.Name, defaultSort);
                }
            }
            return source;
        }


        /// <summary>
        /// 设置默认排序，当PageOrderBy不存在时使用默认排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="defaultOrderBy"></param>
        /// <param name="defaultSort">asc/desc</param>
        /// <returns></returns>
        public static PageParm SetDefaultOrderBy(this PageParm source, string defaultOrderBy, string defaultSort = "desc")
        {
            if (source.PageOrderBy == null || source.PageOrderBy.Count <= 0 || (source.PageOrderBy.Count == 1 && string.IsNullOrWhiteSpace(source.PageOrderBy[0].OrderBy)))
            {
                source.PageOrderBy = [];
                source.SetOrderBy(defaultOrderBy, defaultSort);
            }
            return source;
        }
    }
}
