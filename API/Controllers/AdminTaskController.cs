using API.DTOs;
using API.Errors;
using API.Exceptions;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
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

        [HttpPost(BaseUri)]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetTasks(
            [FromQuery] PaginationParams paginationParams,
            [FromBody] TaskRequest taskRequest, int adminId)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(paginationParams ,taskRequest, adminId, "ADMIN");

                Response.AddPaginationHeader(new PaginationHeader(tasks.CurrentPage, tasks.PageSize,
                tasks.TotalCount, tasks.TotalPages));

                return Ok(tasks);
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
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
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
        }

        [HttpPost(BaseUri + "/create")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<ApiResponseMessage>> CreateTask(CreateUpdateTaskDto taskDto, int adminId)
        {
            try
            {
                await _taskService.CreateTaskAsync(taskDto, adminId);

                return Ok(new ApiResponseMessage("MSG24"));
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost(DetailUri)]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateTask(CreateUpdateTaskDto taskDto, int adminId, int taskId)
        {
            try
            {
                await _taskService.EditTaskAsync(taskDto, adminId, taskId);

                return Ok(new ApiResponseMessage("MSG25"));
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPut(ChangeStatusUri)]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateTaskStatus(int adminId, int taskId, int status)
        {
            try
            {
                await _taskService.UpdateTaskStatus(adminId, taskId, status, "ADMIN");

                return Ok(new ApiResponseMessage("MSG25"));
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ApiResponse(403, ex.Message));
            }
            catch (AccountAssignedTaskException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }
    }
}
