using API.DTOs;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;
using API.Param.Enums;

namespace API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAccountRepository _accountRepository;

        public TaskService(ITaskRepository taskRepository, IAccountRepository accountRepository)
        {
            _taskRepository = taskRepository;
            _accountRepository = accountRepository;
        }

        public async Task CreateTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountCreateId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {accountCreateId} not found.");
            }

            AuthorizeAdminRole(account.RoleId);

            var isValidAccountAssigned = await _taskRepository.CheckAccountAssignedValid(taskDto.AccountAssignedId);
            if (!isValidAccountAssigned)
            {
                throw new AccountAssignedTaskException($"Account assigned with id {taskDto.AccountAssignedId} is invalid");
            }

            await _taskRepository.CreateTaskAsync(taskDto, accountCreateId);
        }

        public async Task EditTaskAsync(CreateUpdateTaskDto taskDto, int accountCreateId, int taskId)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountCreateId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {accountCreateId} not found.");
            }

            AuthorizeAdminRole(account.RoleId);

            var task = await _taskRepository.GetTaskDetailAsync(accountCreateId, taskId, "ADMIN");
            if (task == null)
            {
                throw new BaseNotFoundException($"Task with ID {taskId} not found.");
            }

            var isValidAccountAssigned = await _taskRepository.CheckAccountAssignedValid(taskDto.AccountAssignedId);
            if (!isValidAccountAssigned)
            {
                throw new AccountAssignedTaskException($"Account assigned with id {taskDto.AccountAssignedId} is invalid");
            }

            await _taskRepository.EditTaskAsync(taskDto, accountCreateId, taskId);
        }

        public async Task UpdateTaskStatus(int accountId, int taskId, int taskStatus, string role)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {accountId} not found.");
            }

            TaskDto task = null;
            if (role.Equals("ADMIN"))
            {
                AuthorizeAdminRole(account.RoleId);
                task = await _taskRepository.GetTaskDetailAsync(accountId, taskId, "ADMIN");
            }

            if (role.Equals("STAFF"))
            {
                AuthorizeStaffRole(account.RoleId);
                task = await _taskRepository.GetTaskDetailAsync(accountId, taskId, "STAFF");
            }

            if (task == null)
            {
                throw new BaseNotFoundException($"Task with ID {taskId} not found.");
            }

            await _taskRepository.UpdateTaskStatus(taskId, taskStatus);
        }

        public async Task<TaskDetailDto> GetTaskAsync(int accountId, int taskId, string role)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {accountId} not found.");
            }

            TaskDetailDto taskDto = null; 
            if (role.Equals("ADMIN"))
            {
                AuthorizeAdminRole(account.RoleId);

                taskDto =  await _taskRepository.GetTaskDetailAsync(accountId, taskId, "ADMIN");
            }
            else if (role.Equals("STAFF"))
            {
                AuthorizeStaffRole(account.RoleId);

                taskDto = await _taskRepository.GetTaskDetailAsync(accountId, taskId, "STAFF");
            }
            
            if (taskDto == null)
            {
                throw new BaseNotFoundException($"Task with ID {taskId} not found.");
            }

            return taskDto;
        }

        public async Task<PageList<TaskDto>> GetTasksAsync(PaginationParams paginationParams,TaskRequest taskParam, int accountId, string role)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {accountId} not found.");
            }
            
            if (role.Equals("ADMIN"))
            {
                AuthorizeAdminRole(account.RoleId);

                return await _taskRepository.GetTasksAsync(paginationParams, taskParam, accountId, "ADMIN");
            }

            if (role.Equals("STAFF"))
            {
                AuthorizeStaffRole(account.RoleId);
                
                return await _taskRepository.GetTasksAsync(paginationParams, taskParam, accountId, "STAFF");
            }

            return null;
        }

        private void AuthorizeAdminRole(int role)
        {
            if (role != (int)RoleEnum.Admin)
            {
                throw new UnauthorizedAccessException("Only admin role can access this service");
            }
        }

        private void AuthorizeStaffRole(int role)
        {
            if (role != (int)RoleEnum.Staff)
            {
                throw new UnauthorizedAccessException("Only staff role can access this service");
            }
        }
    }
}
