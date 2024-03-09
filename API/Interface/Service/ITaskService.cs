using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface ITaskService
    {
        Task<PageList<TaskDto>> GetTasksAsync(TaskParam taskParam, int accountId, string role);
        Task<TaskDto> CreateTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId);
        Task<TaskDto> EditTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId, int taskId);
        Task<TaskDto?> GetTaskAsync(int accountId, int taskId, string role);
        Task<TaskDto> UpdateTaskStatus(int accountId, int taskId, int taskStatus, string role);
    }
}
