using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface ITaskService
    {
        Task<PageList<TaskDto>> GetTasksAsync(PaginationParams paginationParams, TaskRequest taskRequest, int accountId, string role);
        Task CreateTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId);
        Task EditTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId, int taskId);
        Task<TaskDetailDto> GetTaskAsync(int accountId, int taskId, string role);
        Task UpdateTaskStatus(int accountId, int taskId, int taskStatus, string role);
    }
}
