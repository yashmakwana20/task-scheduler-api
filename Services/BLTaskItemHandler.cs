using System.Data;
using System.Security.Cryptography.X509Certificates;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class BLTaskItemHandler
    {
        private readonly DBTaskItemContext _dbContext;
        public int _userId;
        TaskItems _objTask = new TaskItems();
        Response response = new Response();

        public BLTaskItemHandler(DBTaskItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response GetTaskData(int userId)
        {
            DataTable dt = _dbContext.GetTaskData(userId);

            if (dt.Rows.Count > 0)
                response.Data = dt;
            else
            {
                response.IsError = true;
                response.Message = "No Record Found";
            }

            return response;
        }

        public Response SaveTaskItem(TaskItems objTaskItem)
        {
            _objTask = objTaskItem;
            _objTask.CreatedDate = DateTime.Now;

            response = _dbContext.Save(_objTask);
            response.Message = "Record Added Successfully";

            return response;
        }

        public Response UpdateTaskItem(TaskItems objTaskItem)
        {
            response = _dbContext.UpdateTaskItem(objTaskItem);
            response.Message = "Record Updated Successfully";

            return response;
        }

        public Response AssignTasks(TaskAssign objTaskAssign)
        {
            response = _dbContext.AssignTasks(objTaskAssign);
            response.Message = "Tasks Assigned Successfully";

            return response;
        }

        public Response DeleteTaskItem(int Id)
        {
            _dbContext.DeleteTaskItem(Id);
            response.Message = "Record Deleted Successfully";

            return response;
        }
    }
}
