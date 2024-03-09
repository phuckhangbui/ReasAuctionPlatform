using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface ITaskRepository : IBaseRepository<Entity.Task>
    {
        //Admin
        Task<PageList<TaskDto>> GetTasksAsync(TaskParam taskParam, int adminId);
        Task<TaskDto> CreateTaskAsync(CreateUpdateTaskDto taskDto, int adminId);
        Task<TaskDto> EditTaskAsync(CreateUpdateTaskDto taskDto, int adminId, int taskId);
        Task<bool> CheckAccountAssignedValid(int accountId);
        Task<TaskDto?> GetTaskAdminRoleAsync(int adminId, int taskId);
        
        //Staff
        Task<PageList<TaskDto>> GetTasksStaffRoleAsync(TaskParam taskParam, int staffId);
        Task<TaskDto?> GetTaskStaffRoleAsync(int staffId, int taskId);

        //Admin and staff
        Task<TaskDto> UpdateTaskStatus(int taskId, int taskStatus);
    }
}
