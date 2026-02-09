/*
* ==============================================================================
*
* FileName: Repository.cs
* Created: 2026/1/20 12:27:32
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
using TableConfig.Extensions;
using TableConfig.IRepositorys;

namespace TableConfig.Repositorys
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ISqlSugarClient _db;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.CurrentDb();
        }

        #region 查询

        public async Task<T> GetByIdAsync(object pkValue)
        {
            return await _db.Queryable<T>().InSingleAsync(pkValue);
        }
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Queryable<T>().Where(where).FirstAsync();
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType)
        {
            return await _db.Queryable<T>().Where(where).OrderBy(order, byType).FirstAsync();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Queryable<T>().Where(where).ToListAsync();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType)
        {
            return await _db.Queryable<T>().Where(where).OrderBy(order, byType).ToListAsync();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByType byType, int topNum)
        {
            return await _db.Queryable<T>().Where(where).OrderBy(order, byType).Take(topNum).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Queryable<T>().Where(where).CountAsync();
        }

        public async Task<PageInfo<T>> GetListAsync(PageParm parm)
        {
            return await _db.Queryable<T>().ToPageAsync(parm);
        }

        public async Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> maxExpression)
        {
            return await _db.Queryable<T>().Where(where).MaxAsync(maxExpression);
        }



        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Queryable<T>().AnyAsync(where);
        }

        #endregion

        #region 保存

        public async Task<int> InsertAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync();
        }
        public async Task<int> InsertAsync(List<T> entitys)
        {
            return await _db.Insertable(entitys).ExecuteCommandAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            return await _db.Updateable(entity).ExecuteCommandAsync();
        }
        public async Task<int> UpdateAsync(List<T> entitys)
        {
            return await _db.Updateable(entitys).ExecuteCommandAsync();
        }

        /// <summary>
        /// 只更新某列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns">指定更新的列</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity, Expression<Func<T, object>> columns)
        {
            return await _db.Updateable(entity).UpdateColumns(columns).ExecuteCommandAsync();
        }

        /// <summary>
        /// 只更新某列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns">指定更新的列</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<T> entitys, Expression<Func<T, object>> columns)
        {
            return await _db.Updateable(entitys).UpdateColumns(columns).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据表达式更新
        /// </summary>
        /// <param name="where"></param>
        /// <param name="columns">必须设置更新的值</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> setColumns)
        {
            return await _db.Updateable<T>().SetColumns(setColumns).Where(where).ExecuteCommandAsync();
        }

        /// <summary>
        /// 无主键添加或更新数据
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<(int InsertCount, int UpdateCount)> StorageableExecuteCommandAsync(T entity, Expression<Func<T, object>> whereColumns)
        {
            var storage = _db.Storageable(entity).WhereColumns(whereColumns).ToStorage();
            var insertCount = await storage.AsInsertable.ExecuteCommandAsync();//不存在插入
            var updateCount = await storage.AsUpdateable.ExecuteCommandAsync();//存在更新
            return (insertCount, updateCount);
        }

        /// <summary>
        /// 无主键添加或更新数据
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<(int InsertCount, int UpdateCount)> StorageableExecuteCommandAsync(List<T> entitys, Expression<Func<T, object>> whereColumns)
        {
            var storage = _db.Storageable(entitys).WhereColumns(whereColumns).ToStorage();
            var insertCount = await storage.AsInsertable.ExecuteCommandAsync();//不存在插入
            var updateCount = await storage.AsUpdateable.ExecuteCommandAsync();//存在更新
            return (insertCount, updateCount);
        }

        #endregion

        #region 删除
        public async Task<int> DeleteAsync(object pkValue)
        {
            return await _db.Deleteable<T>(pkValue).ExecuteCommandAsync();
        }
        public async Task<int> DeleteAsync(object[] pkValues)
        {
            return await _db.Deleteable<T>().In(pkValues).ExecuteCommandAsync();
        }
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Deleteable(where).ExecuteCommandAsync();
        }

        #endregion

    }
}
