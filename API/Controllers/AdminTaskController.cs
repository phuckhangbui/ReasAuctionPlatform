using API.DTOs;
using API.Errors;
using API.Exceptions;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminTaskController : BaseApiController
    {
        private readonly ITaskService _taskService;

        public AdminTaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private const string BaseUri = "/api/admin/{adminId}/tasks";
        private const string DetailUri = BaseUri + "/{taskId}";
        private const string ChangeStatusUri = DetailUri + "/{status}";

        [HttpGet(BaseUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetTasks([FromQuery] TaskParam taskParam, int adminId)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(taskParam, adminId, "ADMIN");

                Response.AddPaginationHeader(new PaginationHeader(tasks.CurrentPage, tasks.PageSize,
                tasks.TotalCount, tasks.TotalPages));

                return Ok(tasks);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
        }

        [HttpGet(DetailUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetTaskDetail(int adminId, int taskId)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(adminId, taskId, "ADMIN");

                return Ok(task);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
        }

        [HttpPost(BaseUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> CreateTask(CreateUpdateTaskDto taskDto, int adminId)
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(taskDto, adminId);

                return Ok(task);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost(DetailUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> UpdateTask(CreateUpdateTaskDto taskDto, int adminId, int taskId)
        {
            try
            {
                var task = await _taskService.EditTaskAsync(taskDto, adminId, taskId);

                return Ok(task);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPut(ChangeStatusUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> UpdateTaskStatus(int adminId, int taskId, int status)
        {
            try
            {
                var task = await _taskService.UpdateTaskStatus(adminId, taskId, status, "ADMIN");

                return Ok(task);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}
