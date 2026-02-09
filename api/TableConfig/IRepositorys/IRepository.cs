/*
* ==============================================================================
*
* FileName: IRepository.cs
* Created: 2026/1/20 12:25:48
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

namespace TableConfig.IRepositorys
{
    public interface IRepository<T> where T : class, new()
    {
        #region 查询

        Task<T> GetByIdAsync(object pkValue);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> where);

        Task<T> GetFirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType, int topNum);

        Task<List<T>> GetAllAsync();

        Task<int> GetCountAsync(Expression<Func<T, bool>> where);

        Task<PageInfo<T>> GetListAsync(PageParm parm);

        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> maxExpression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        #endregion

        #region 保存

        Task<int> InsertAsync(T entity);
        Task<int> InsertAsync(List<T> entitys);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateAsync(List<T> entitys);

        /// <summary>
        /// 只更新某列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns">指定更新的列</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, Expression<Func<T, object>> columns);

        /// <summary>
        /// 只更新某列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns">指定更新的列</param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> entitys, Expression<Func<T, object>> columns);

        /// <summary>
        /// 根据表达式更新
        /// </summary>
        /// <param name="where"></param>
        /// <param name="columns">必须设置更新的值</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> columns);


        /// <summary>
        /// 无主键添加或更新数据
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<(int InsertCount, int UpdateCount)> StorageableExecuteCommandAsync(T entity, Expression<Func<T, object>> whereColumns);

        /// <summary>
        /// 无主键添加或更新数据
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<(int InsertCount, int UpdateCount)> StorageableExecuteCommandAsync(List<T> entitys, Expression<Func<T, object>> whereColumns);

        #endregion

        #region 删除
        Task<int> DeleteAsync(object pkValue);
        Task<int> DeleteAsync(object[] pkValues);
        Task<int> DeleteAsync(Expression<Func<T, bool>> where);

        #endregion
    }
}
