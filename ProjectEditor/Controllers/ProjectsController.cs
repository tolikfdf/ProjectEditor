using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ProjectEditor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public ProjectsController(ILoggerManager logger, IRepositoryManager repository,
            IMapper mapper)
        {
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProjects([FromQuery]ProjectRequestParameters projectRequestParameters)
        {
            var projects = repository.Project.GetAllProjects(projectRequestParameters, trackChanges: false);
            var projectsDto = mapper.Map<IEnumerable<ProjectDto>>(projects);
            return Ok(projectsDto);
        }

        [HttpGet("{id}", Name = "ProjectById")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = repository.Project.GetProjectById(id, trackChanges: false);

            if (project == null)
            {
                logger.LogInfo($"Project with id: { id} doesn't exist in the database.");
                return NotFound($"Project with id: { id} doesn't exist in the database.");
            }

            var projectsDto = mapper.Map<ProjectDto>(project);
            return Ok(projectsDto);
        }



        [HttpPost]
        public IActionResult CreateProject([FromBody]ProjectForCreationDto project)
        {
            if (project == null)
            {
                logger.LogError("ProjectForCreationDto object sent from client is null.");

                return BadRequest("ProjectForCreationDto object is null.");
            }

            var projectEntity = mapper.Map<Project>(project);
            repository.Project.CreateProject(projectEntity);
            repository.Save();

            var projectToReturn = mapper.Map<ProjectDto>(projectEntity);
            return CreatedAtRoute("ProjectById", new { id = projectToReturn.Id },
                projectToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            var project = repository.Project.GetProjectById(id, trackChanges: false);

            if (project == null)
            {
                logger.LogInfo($"Project with id: { id} doesn't exist in the database.");
                return NotFound($"Project with id: { id} doesn't exist in the database.");
            }

            repository.Project.DeleteProject(project);
            repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, [FromBody]ProjectForUpdateDto project)
        {
            if (project == null)
            {
                logger.LogError("ProjectForUpdateDto object sent from client is null.");
                return BadRequest("ProjectForUpdateDto object is null");
            }

            var projectEntity = repository.Project.GetProjectById(id, trackChanges: true);
            if (projectEntity == null)
            {
                logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound($"Project with id: {id} doesn't exist in the database.");
            }

            mapper.Map(project, projectEntity);

            repository.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateForProject(Guid id,
            [FromBody] JsonPatchDocument<ProjectForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var project = repository.Project.GetProjectById(id, trackChanges: true);
            if (project == null)
            {
                logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound($"Project with id: {id} doesn't exist in the database.");
            }

            var projectForPatch = mapper.Map<ProjectForUpdateDto>(project);

            patchDoc.ApplyTo(projectForPatch);

            mapper.Map(projectForPatch, project);

            repository.Save();

            return NoContent();
        }
    }
}
