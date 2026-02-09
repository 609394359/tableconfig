/*
* ==============================================================================
*
* FileName: UnitOfWork.cs
* Created: 2026/1/20 12:24:17
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using TableConfig.IRepositorys;

namespace TableConfig.Repositorys
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        public void BeginTran()
        {
            CurrentDb().BeginTran();
        }

        public void CommitTran()
        {
            CurrentDb().CommitTran();
        }


        public void RollbackTran()
        {
            CurrentDb().RollbackTran();
        }

        public SqlSugarClient CurrentDb()
        {
            return _sqlSugarClient as SqlSugarClient;
        }
    }
}
