using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class UserRepository : DapperRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }

        public Task<int> UserGetListCountPageAsync(UserGetListPageDto userGetListDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> UserGetListPageAsync(UserGetListPageDto userGetListDto)
        {
            throw new NotImplementedException();
        }
    }
}
