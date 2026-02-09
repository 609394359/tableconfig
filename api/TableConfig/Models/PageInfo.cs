/*
* ==============================================================================
*
* FileName: PageInfo.cs
* Created: 2026/1/1 16:43:57
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Mapster;
using System;
using System.Collections.Generic;

namespace TableConfig.Models
{
    public class PageInfo<T>
    {
        public PageInfo()
        {
        }

        public PageInfo(List<T> data, long totalCount, long pageIndex, long pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            TotalPage = (totalCount + pageSize - 1) / pageSize;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public List<T> Data { get; set; } = [];

        public long TotalCount { get; set; }

        public long TotalPage { get; set; }

        public long PageIndex { get; set; }

        public long PageSize { get; set; }


        /// <summary>
        /// PageInfo<T>根据Func<T, TReturn> PageInfo<TReturn>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="info"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PageInfo<TReturn> ToChange<TReturn>(Func<T, TReturn> func=null)
        {
            List<TReturn> list = new List<TReturn>();

            if (func == null)
            {
                list = this.Data.Adapt<List<TReturn>>();
            }
            else {

                foreach (var item in this.Data)
                {
                    list.Add(func(item));
                }
            }

            return new PageInfo<TReturn>()
            {
                Data = list,
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                TotalCount = this.TotalCount,
                TotalPage = this.TotalPage,
            };
        }
    }
}
