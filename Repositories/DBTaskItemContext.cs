using Microsoft.Extensions.Configuration;
using ServiceStack.OrmLite;
using System.Data;
using TaskManagement.Models;
using TaskManagement.Services.Common;

namespace TaskManagement.Repositories
{
    public class DBTaskItemContext
    {
        private readonly IConfiguration _config;

        private readonly OrmLiteConnectionFactory _dbFactory;

        Response response = new Response();

        public DBTaskItemContext(IConfiguration config)
        {
            _config = config;
            _dbFactory = new OrmLiteConnectionFactory(_config.GetConnectionString("DefaultConnection"), MySqlDialect.Provider);
        }

        public DataTable GetTaskData(int userId)
        {
            DataTable dt = new DataTable();
            List<TaskItems> lstTaksItem = new();

            using (var db = _dbFactory.Open())
            {
                if (userId == 0)
                    lstTaksItem = db.Select<TaskItems>();
                else
                    lstTaksItem = db.Select<TaskItems>(item => item.UserId == userId);
            }

            return lstTaksItem.ToDataTable();
        }

        public Response Save(TaskItems objTask)
        {
            long Id = 0;
            using (var db = _dbFactory.Open())
            {
                Id = db.Insert(objTask, true);
            }

            if (Id > 0)
                response.masterId = Convert.ToInt32(Id);

            return response;
        }

        public Response UpdateTaskItem(TaskItems objTaskItem)
        {
            using (var db = _dbFactory.Open())
            {
                db.Update(objTaskItem);
                response.masterId = objTaskItem.Id;
            }

            return response;
        }

        public Response AssignTasks(TaskAssign objTaskAssign)
        {
            using (var db = _dbFactory.Open())
            {
                db.UpdateOnlyFields(new TaskItems { UserId = objTaskAssign.userId }, onlyFields: TI => TI.UserId, where: TI => Sql.In(TI.Id, objTaskAssign.taskIds));
            }

            return response;
        }

        public void DeleteTaskItem(int Id)
        {
            using (var db = _dbFactory.Open())
            {
                db.Delete<TaskItems>(TI => TI.Id == Id);
            }
        }
    }
}
