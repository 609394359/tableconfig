using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;

namespace TableConfig.IRepositorys.Sys
{
    public interface ISysTableGroupsRepository : IRepository<SysTableGroups>
    {
        Task<Dictionary<long, int>> GetProjectGroupCount();
    }
}
