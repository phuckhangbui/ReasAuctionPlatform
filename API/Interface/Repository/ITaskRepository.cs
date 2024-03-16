using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface ITaskRepository : IBaseRepository<Entity.Task>
    {
        Task<PageList<TaskDto>> GetTasksAsync(PaginationParams paginationParams,TaskRequest taskRequest, int account, string role);
        Task CreateTaskAsync(CreateUpdateTaskDto taskDto, int adminId);
        Task EditTaskAsync(CreateUpdateTaskDto taskDto, int adminId, int taskId);
        Task<bool> CheckAccountAssignedValid(int accountId);
        Task<TaskDetailDto> GetTaskDetailAsync(int adminId, int taskId, string role);
        Task UpdateTaskStatus(int taskId, int taskStatus);
    }
}
