using API.Data;
using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class TaskRepository : BaseRepository<Entity.Task>, ITaskRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CheckAccountAssignedValid(int accountId)
        {
            return _context.Account.AnyAsync(c => c.AccountId.Equals(accountId) && c.RoleId.Equals((int)RoleEnum.Staff)); 
        }

        public async Task<TaskDto> CreateTaskAsync(CreateUpdateTaskDto taskDto, int adminId)
        {
            var accountCreate = _context.Account.SingleOrDefault(c => c.AccountId.Equals(adminId));
            var accountAssigned = _context.Account.SingleOrDefault(c => c.AccountId.Equals(taskDto.AccountAssignedId));

            var newTask = new Entity.Task();
            newTask.AccountAssignedName = accountAssigned.AccountName;
            newTask.AccountAssignedId = accountAssigned.AccountId;
            newTask.AccountCreateId = accountCreate.AccountId;
            newTask.AccountCreateName = accountCreate.AccountName;
            newTask.TaskTitle = taskDto.TaskTitle;
            newTask.TaskContent = taskDto.TaskContent;
            newTask.Status = taskDto.Status;
            newTask.TaskNotes = taskDto.TaskNotes;
            newTask.DateCreated = taskDto.DateCreated != null ? taskDto.DateCreated : DateTime.Now;

            _context.Set<Entity.Task>().Add(newTask);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(newTask);
        }

        public async Task<TaskDto> EditTaskAsync(CreateUpdateTaskDto taskDto, int adminId, int taskId)
        {
            var existingTask = await _context.Task.FindAsync(taskId);

            var accountCreate = _context.Account.SingleOrDefault(c => c.AccountId.Equals(adminId));
            var accountAssigned = _context.Account.SingleOrDefault(c => c.AccountId.Equals(taskDto.AccountAssignedId));

            existingTask.AccountAssignedName = accountAssigned.AccountName;
            existingTask.AccountAssignedId = accountAssigned.AccountId;
            existingTask.AccountCreateId = accountCreate.AccountId;
            existingTask.AccountCreateName = accountCreate.AccountName;
            existingTask.TaskTitle = taskDto.TaskTitle;
            existingTask.TaskContent = taskDto.TaskContent;
            existingTask.Status = taskDto.Status;
            existingTask.TaskNotes = taskDto.TaskNotes;
            existingTask.DateUpdated = DateTime.Now;

            _context.Entry(existingTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(existingTask);
        }

        public async Task<TaskDto?> GetTaskAdminRoleAsync(int adminId, int taskId)
        {
            var task = await _context.Task.SingleOrDefaultAsync(t => t.AccountCreateId == adminId &&
                                    t.TaskId == taskId);
            if (task != null)
            {
                return _mapper.Map<TaskDto>(task);
            }

            return null;
        }

        public async Task<PageList<TaskDto>> GetTasksAsync(TaskParam taskParam, int adminId)
        {
            var query = _context.Task.AsQueryable();
            query = query.Where(t => t.AccountCreateId == adminId);

            if (taskParam.Status != -1)
            {
                query = query.Where(t => t.Status == taskParam.Status);
            }

            if (!string.IsNullOrEmpty(taskParam.AccountAssignedName))
            {
                query = query.Where(t =>
                    t.AccountAssignedName.ToLower().Contains(taskParam.AccountAssignedName.ToLower()));
            }

            if (!string.IsNullOrEmpty(taskParam.AccountCreateName))
            {
                query = query.Where(t =>
                    t.AccountCreateName.ToLower().Contains(taskParam.AccountCreateName.ToLower()));
            }

            query = query.OrderByDescending(r => r.DateCreated);

            return await PageList<TaskDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<TaskDto>(_mapper.ConfigurationProvider),
            taskParam.PageNumber,
            taskParam.PageSize);
        }

        public async Task<PageList<TaskDto>> GetTasksStaffRoleAsync(TaskParam taskParam, int staffId)
        {
            var staff = await _context.Account.SingleOrDefaultAsync(a => a.AccountId.Equals(staffId));
            if (staff != null)
            {
                var query = _context.Task.AsQueryable();
                query = query.Where(t => t.AccountAssignedId.Equals(staffId));

                if (taskParam.Status != -1)
                {
                    query = query.Where(t => t.Status == taskParam.Status);
                }

                if (!string.IsNullOrEmpty(taskParam.AccountAssignedName))
                {
                    query = query.Where(t =>
                        t.AccountAssignedName.ToLower().Contains(taskParam.AccountAssignedName.ToLower()));
                }

                if (!string.IsNullOrEmpty(taskParam.AccountCreateName))
                {
                    query = query.Where(t =>
                        t.AccountCreateName.ToLower().Contains(taskParam.AccountCreateName.ToLower()));
                }

                query = query.OrderByDescending(r => r.DateCreated);

                return await PageList<TaskDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<TaskDto>(_mapper.ConfigurationProvider),
                taskParam.PageNumber,
                taskParam.PageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<TaskDto?> GetTaskStaffRoleAsync(int staffId, int taskId)
        {
            var task = await _context.Task.SingleOrDefaultAsync(t => t.TaskId.Equals(taskId) && 
            t.AccountAssignedId.Equals(staffId));
            if (task != null)
            {
                return _mapper.Map<TaskDto>(task);
            }

            return null;
        }

        public async Task<TaskDto> UpdateTaskStatus(int taskId, int taskStatus)
        {
            var existingTask = await _context.Task.FindAsync(taskId);
            if (existingTask != null)
            {
                existingTask.Status = taskStatus;
                _context.Entry(existingTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return _mapper.Map<TaskDto>(existingTask);
            }

            return null;
        }
    }
}
