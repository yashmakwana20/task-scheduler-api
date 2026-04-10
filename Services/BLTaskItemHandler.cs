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
        DataTable dtTaskItem;
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
            dtTaskItem = new DataTable();
            dtTaskItem = _dbContext.ValidateBeforeAssignTask(objTaskAssign.taskIds);

            if (dtTaskItem.Rows.Count > 0)
            {
                response = _dbContext.AssignTasks(objTaskAssign);
                response.Message = "Tasks Assigned Successfully";
            }
            else
            {
                response.IsError = true;
                response.Message = "No Tasks Found.";
            }

            return response;
        }

        public string PrepareNotificationMessage()
        {
            string message = String.Empty;

            message = string.Join(", ", dtTaskItem.AsEnumerable()
                                           .Select(row => row["Title"].ToString())
                                           .Where(msg => !string.IsNullOrEmpty(msg)));

            message = dtTaskItem.Rows.Count > 1 ? $"{message} have" : $"{message} has";

            return message;
        }

        public Response DeleteTaskItem(int Id)
        {
            _dbContext.DeleteTaskItem(Id);
            response.Message = "Record Deleted Successfully";

            return response;
        }
    }
}
