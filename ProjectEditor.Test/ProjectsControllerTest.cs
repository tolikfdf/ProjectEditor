using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectEditor.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProjectEditor.Test
{
    public class ProjectsControllerTest
    {
        private Mock<IRepositoryManager> repository;
        private Mock<IMapper> mapper;
        private Mock<ILoggerManager> logger;

        public ProjectsControllerTest()
        {
            repository = new Mock<IRepositoryManager>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILoggerManager>();
        }

        #region Get all projects
        [Fact]
        public void GetAllProjects_ActionExecute_ReturnsOkObjectResult()
        {
            var projectRequestParameters = new ProjectRequestParameters();
            repository.Setup(x => x.Project.GetAllProjects(projectRequestParameters ,false)).
                Returns(new List<Project> { new Project(), new Project() });
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.GetAllProjects(projectRequestParameters);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllProjects_ActionExecute_ReturnsExactNumberOfProjects()
        {
            var projects = new List<Project> { 
                new Project(),
                new Project() 
            };
            var projectsDto = new List<ProjectDto> { new ProjectDto(), new ProjectDto() };
            var projectRequestParameters = new ProjectRequestParameters();
            repository.Setup(x => x.Project.GetAllProjects(projectRequestParameters, false)).
                Returns(projects);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);
            mapper.Setup(x => x.Map<IEnumerable<ProjectDto>>(projects)).
                Returns(projectsDto);

            var result = sut.GetAllProjects(projectRequestParameters);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var actualProjects = Assert.IsType<List<ProjectDto>>(viewResult.Value);
            Assert.Equal(2, projects.Count);
        }

        #endregion

        #region Get project by id
        [Fact]
        public void GetProjectById_ActionExecute_ReturnsOkObjectResult()
        {
            var guid = Guid.Parse("a20a19dc-2cc1-4c84-97b1-0e54b39474cc");
            var project = new Project();
            var projectDto = new ProjectDto();
            repository.Setup(x => x.Project.GetProjectById(guid, false)).
                Returns(project);
            mapper.Setup(x => x.Map<ProjectDto>(project)).
                Returns(projectDto);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.GetProjectById(guid);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetProjectById_ActionExecute_ReturnsNotFoundResult()
        {
            var guid = Guid.Parse("a20a19dc-2cc1-4c84-97b1-0e54b39474cc");
            var diffGuid = Guid.Parse("77de26da-bd50-4071-9362-2b86d17aafeb");
            var project = new Project();
            var projectDto = new ProjectDto();
            repository.Setup(x => x.Project.GetProjectById(diffGuid, false)).
                Returns(project);
            mapper.Setup(x => x.Map<ProjectDto>(project)).
                Returns(projectDto);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.GetProjectById(guid);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetProjectById_ActionExecute_ReturnsExactProjectId()
        {
            var guid = Guid.Parse("a20a19dc-2cc1-4c84-97b1-0e54b39474cc");
            var project = new Project { Id = guid };
            var projectDto = new ProjectDto { Id = guid };
            repository.Setup(x => x.Project.GetProjectById(guid, false)).
                Returns(project);
            mapper.Setup(x => x.Map<ProjectDto>(project)).
                Returns(projectDto);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.GetProjectById(guid);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var actualProject = Assert.IsType<ProjectDto>(viewResult.Value);
            Assert.Equal(guid, actualProject.Id);
        }

        #endregion

        #region Create a new project

        [Fact]
        public void CreateProject_ActionExecute_ReturnsBadRequestResult()
        {
            ProjectForCreationDto project = null;
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.CreateProject(project);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateProject_ActionExecute_Succeeds()
        {
            var project = new Project();
            var projectForCreationDto = new ProjectForCreationDto();
            var projectDto = new ProjectDto();
            repository.Setup(o => o.Project.CreateProject(project));
            mapper.Setup(m => m.Map<Project>(projectForCreationDto)).Returns(project);
            mapper.Setup(m => m.Map<ProjectDto>(project)).Returns(projectDto);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            sut.CreateProject(projectForCreationDto);

            repository.Verify(x=>x.Save(), Times.Once());
        }
        #endregion

        #region Delete a project

        [Fact]
        public void DeleteProject_ActionExecute_No_Project_With_ProjectId_ReturnsNotFoundResult()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffProjectId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var project = new Project();
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.DeleteProject(diffProjectId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTask_Succeeds_ActionExecute()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project();
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.DeleteProject(projectId);

            repository.Verify(x => x.Save(), Times.Once());
        }

        #endregion

        #region Update a project

        [Fact]
        public void UpdateProject_ActionExecute_ProjectForUpdateDto_Is_Null_ReturnsBadRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            ProjectForUpdateDto projectForUpdateDto = null;
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateProject(projectId, projectForUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateProject_ActionExecute_No_Project_With_Provided_ProjectId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            var diffProjectId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            var projectForUpdateDto = new ProjectForUpdateDto();
            repository.Setup(x => x.Project.GetProjectById(projectId, true)).Returns(project);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateProject(diffProjectId, projectForUpdateDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateProject_ActionExecute_Succeeds()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            var projectForUpdateDto = new ProjectForUpdateDto();
            repository.Setup(x => x.Project.GetProjectById(projectId, true)).Returns(project);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateProject(projectId, projectForUpdateDto);

            repository.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region Partially update a project

        [Fact]
        public void PartiallyUpdateForProject_ActionExecute_JsonPatchDocument_Is_Null_ReturnsBadRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            JsonPatchDocument<ProjectForUpdateDto> patchDoc = null;var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForProject(projectId, patchDoc);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PartiallyUpdateForProject_ActionExecute_No_Project_With_Provided_ProjectId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffProjectId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            JsonPatchDocument<ProjectForUpdateDto> patchDoc = new JsonPatchDocument<ProjectForUpdateDto>();
            var project = new Project { Id = projectId };
            repository.Setup(x => x.Project.GetProjectById(projectId, true)).Returns(project);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForProject(diffProjectId, patchDoc);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PartiallyUpdateForProject_ActionExecute_Succeeds()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            JsonPatchDocument<ProjectForUpdateDto> patchDoc = new JsonPatchDocument<ProjectForUpdateDto>();
            var project = new Project { Id = projectId };
            repository.Setup(x => x.Project.GetProjectById(projectId, true)).Returns(project);
            var projectForPatch = new ProjectForUpdateDto();
            mapper.Setup(x => x.Map<ProjectForUpdateDto>(project)).Returns(projectForPatch);
            var sut = new ProjectsController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForProject(projectId, patchDoc);

            repository.Verify(x => x.Save(), Times.Once);
        }

        #endregion
    }
}