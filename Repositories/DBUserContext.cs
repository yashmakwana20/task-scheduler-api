using ServiceStack.OrmLite;
using System.Data;
using TaskManagement.Models;
using TaskManagement.Services.Common;

namespace TaskManagement.Repositories
{
    public class DBUserContext
    {
        private readonly IConfiguration _config;

        private readonly OrmLiteConnectionFactory _dbFactory;

        public DBUserContext(IConfiguration config)
        {
            _config = config;
            _dbFactory = new OrmLiteConnectionFactory(_config.GetConnectionString("DefaultConnection"), MySqlDialect.Provider);
        }

        public DataTable GetUserData()
        {
            DataTable dt = new DataTable();
            List<Users> lstTaksItem = new();

            using (var db = _dbFactory.Open())
            {
                lstTaksItem = db.Select<Users>(item => item.UserRole == "User");
            }

            return lstTaksItem.ToDataTable();
        }
    }
}
