using System.Data;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class BLUserHandler
    {
        private readonly DBUserContext _dbContext;
        public int _userId;
        Users _objTask = new Users();
        Response response = new Response();

        public BLUserHandler(DBUserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response GetUserData()
        {
            DataTable dt = _dbContext.GetUserData();

            if (dt.Rows.Count > 0)
                response.Data = dt;
            else
            {
                response.IsError = true;
                response.Message = "No Record Found";
            }

            return response;
        }
    }
}
