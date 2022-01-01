using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ProjectEditor.Controllers
{
    [Route("api/projects/{projectId}/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public TasksController(
            ILoggerManager loggerManager,
            IRepositoryManager repository,
            IMapper mapper)
        {
            this.logger = loggerManager;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTasksForProject(Guid projectId, [FromQuery]
            TaskRequestParameters requestParameters)
        {
            var project = repository.Project.GetProjectById(projectId, trackChanges: false);

            if (project == null)
            {
                logger.LogInfo($"Project with id: {projectId} doesn't exist in the database.");
                return NotFound($"Project with id: {projectId} doesn't exist in the database.");
            }

            var tasks = repository.Task.GetTasksByProjectId(projectId, requestParameters, trackChanges: false);
            var tasksDto = mapper.Map<IEnumerable<TaskDto>>(tasks);
            return Ok(tasksDto);
        }

        [HttpGet("{id}", Name = "TaskById")]
        public IActionResult GetTaskById(Guid projectId, Guid id)
        {
            var project = repository.Project.GetProjectById(projectId, trackChanges: false);
            if (project == null)
            {
                logger.LogInfo($"Project with id: {projectId} doesn't exist in the database.");
                return NotFound($"Project with id: {projectId} doesn't exist in the database.");
            }

            var task = repository.Task.GetTaskById(projectId, id, trackChanges: false);
            if (task == null)
            {
                logger.LogInfo($"Task with id: {id} doesn't exist in the database.");
                return NotFound($"Task with id: {id} doesn't exist in the database.");
            }

            var taskDto = mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [HttpPost]
        public IActionResult CreateTask(Guid projectId, [FromBody] TaskForCreationDto taskForCreationDto)
        {
            if (taskForCreationDto == null)
            {
                logger.LogError("TaskForCreationDto object sent from client is null.");

                return BadRequest("TaskForCreationDto object is null");
            }

            var project = repository.Project.GetProjectById(projectId, trackChanges: false);

            if (project == null)
            {
                logger.LogInfo($"Project with id: {projectId} doesn't exist in the database.");
                return NotFound($"Project with id: {projectId} doesn't exist in the database.");
            }

            var taskEntity = mapper.Map<Entities.Models.Task>(taskForCreationDto);
            repository.Task.CreateTask(projectId, taskEntity);
            repository.Save();

            var taskToReturn = mapper.Map<TaskDto>(taskEntity);
            return CreatedAtRoute("TaskById", new { projectId, id = taskEntity.Id },
                taskToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid projectId, Guid id)
        {
            var project = repository.Project.GetProjectById(projectId, trackChanges: false);

            if (project == null)
            {
                logger.LogInfo($"Project with id: { id} doesn't exist in the database.");
                return NotFound($"Project with id: { id} doesn't exist in the database.");
            }

            var task = repository.Task.GetTaskById(projectId, id, trackChanges: false);
            if (task == null)
            {
                logger.LogInfo($"Task with id: {id} doesn't exist in the database.");
                return NotFound($"Task with id: {id} doesn't exist in the database.");
            }

            repository.Task.DeleteTask(task);
            repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid projectId, Guid id, [FromBody] TaskForUpdateDto task)
        {
            if (task == null)
            {
                logger.LogError("TaskForUpdateDto object sent from client is null.");
                return BadRequest("TaskForUpdateDto object is null");
            }

            var project = repository.Project.GetProjectById(projectId, trackChanges: false);
            if (project == null)
            {
                logger.LogInfo($"Project with id: {projectId} doesn't exist in the database.");
                return NotFound($"Project with id: {projectId} doesn't exist in the database.");
            }

            var taskEntity = repository.Task.GetTaskById(projectId, id, trackChanges: true);
            if (taskEntity == null)
            {
                logger.LogInfo($"Task with id: {id} doesn't exist in the database.");
                return NotFound($"Task with id: {id} doesn't exist in the database.");
            }

            mapper.Map(task, taskEntity);

            repository.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateForTask(Guid projectId, Guid id,
            [FromBody] JsonPatchDocument<TaskForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var project = repository.Project.GetProjectById(projectId, trackChanges: false);
            if (project == null)
            {
                logger.LogInfo($"Project with id: {projectId} doesn't exist in the database.");
                return NotFound($"Project with id: {projectId} doesn't exist in the database.");
            }

            var task = repository.Task.GetTaskById(projectId, id, trackChanges: true);
            if (task == null)
            {
                logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound($"Project with id: {id} doesn't exist in the database.");
            }

            var taskForPatch = mapper.Map<TaskForUpdateDto>(task);

            patchDoc.ApplyTo(taskForPatch);

            mapper.Map(taskForPatch, task);

            repository.Save();

            return NoContent();
        }
    }
}
