using SqlSugar;

namespace TableConfig.IRepositorys
{
    public interface IUnitOfWork
    {
        void BeginTran();
        void CommitTran();
        void RollbackTran();

        SqlSugarClient CurrentDb();
    }
}
