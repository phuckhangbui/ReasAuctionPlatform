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
    public class StaffTaskController : BaseApiController
    {
        private readonly ITaskService _taskService;

        public StaffTaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private const string BaseUri = "/api/staff/{staffId}/tasks";
        private const string DetailUri = BaseUri + "/{taskId}";
        private const string ChangeStatusUri = DetailUri + "/{status}";

        [HttpPost(BaseUri)]
        [Authorize(policy: "Staff")]
        public async Task<IActionResult> GetTasks(
            [FromQuery] PaginationParams paginationParams, 
            [FromBody] TaskRequest taskParam, int staffId)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(paginationParams, taskParam, staffId, "STAFF");

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
        [Authorize(policy: "Staff")]
        public async Task<IActionResult> GetTaskDetail(int staffId, int taskId)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(staffId, taskId, "STAFF");

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

        [HttpPut(ChangeStatusUri)]
        [Authorize(policy: "Staff")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateTaskStatus(int staffId, int taskId, int status)
        {
            try
            {
                await _taskService.UpdateTaskStatus(staffId, taskId, status, "STAFF");

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
