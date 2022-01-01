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
    public class TasksControllerTest
    {

        private Mock<IRepositoryManager> repository;
        private Mock<IMapper> mapper;
        private Mock<ILoggerManager> logger;
        public TasksControllerTest()
        {
            repository = new Mock<IRepositoryManager>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILoggerManager>();
        }
        #region Get tasks by projectId
        [Fact]
        public void GetTasksForProject_ActionExecute_ReturnsOkObjectResult()
        {
            //Arrange
            var requestParameters = new TaskRequestParameters();
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            List<Task> tasks = new List<Task>
            {
                new Task(), new Task()
            };
            List<TaskDto> taskDtos = new List<TaskDto>
            {
                new TaskDto(), new TaskDto()
            };
            repository.Setup(x => x.Task.GetTasksByProjectId(projectId, requestParameters, false)).
                Returns(tasks);
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            mapper.Setup(x => x.Map<IEnumerable<TaskDto>>(tasks)).
                Returns(taskDtos);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTasksForProject(projectId, requestParameters);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTasksForProject_ActionExecute_ReturnsNotFoundResult()
        {
            //Arrange
            var requestParameters = new TaskRequestParameters();
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffprojectId = Guid.Parse("77de26da-bd50-4071-9362-2b86d17aafeb");
            var project = new Project { Id = projectId };
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTasksForProject(diffprojectId, requestParameters);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetTasksForProject_ActionExecute_ReturnsExactNumberOfTasks()
        {
            //Arrange
            var requestParameters = new TaskRequestParameters();
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id=projectId };
            List<Task> tasks = new List<Task> 
            { 
                new Task(), new Task()
            };
            List<TaskDto> taskDtos = new List<TaskDto> 
            {
                new TaskDto(), new TaskDto()
            };
            repository.Setup(x => x.Task.GetTasksByProjectId(projectId, requestParameters, false)).
                Returns(tasks);
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            mapper.Setup(x => x.Map<IEnumerable<TaskDto>>(tasks)).
                Returns(taskDtos);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTasksForProject(projectId, requestParameters);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var actualTasks = Assert.IsType<List<TaskDto>>(viewResult.Value);
            Assert.Equal(tasks.Count, actualTasks.Count);
        }

        #endregion

        #region Get task by project and task id
        [Fact]
        public void GetTaskById_ActionExecute_ReturnsOkObjectResult()
        {
            //Arrange
            var projectId = Guid.Parse("d8debf4e-6efe-4c73-93ff-d625fd69b020");
            var project = new Project { Id = projectId };
            var taskId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var task = new Task();
            var taskDto = new TaskDto();
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, false)).
                Returns(task);
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            mapper.Setup(x => x.Map<TaskDto>(task)).
                Returns(taskDto);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTaskById(projectId, taskId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTaskById_ActionExecute_ReturnsNotFoundResult()
        {
            //Arrange
            var projectId = Guid.Parse("d8debf4e-6efe-4c73-93ff-d625fd69b020");
            var diffprojectId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var project = new Project { Id = projectId };
            var taskId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var task = new Task();
            var taskDto = new TaskDto();
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, false)).
                Returns(task);
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            mapper.Setup(x => x.Map<TaskDto>(task)).
                Returns(taskDto);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTaskById(diffprojectId, taskId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetTaskById_ActionExecute_ReturnsTask()
        {
            //Arrange
            var projectId = Guid.Parse("d8debf4e-6efe-4c73-93ff-d625fd69b020");
            var project = new Project { Id = projectId };
            var taskId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var task = new Task { Id = taskId };
            var taskDto = new TaskDto { Id = taskId };
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, false)).
                Returns(task);
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).
                Returns(project);
            mapper.Setup(x => x.Map<TaskDto>(task)).
                Returns(taskDto);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            //Act
            var result = sut.GetTaskById(projectId, taskId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var actualTask = Assert.IsType<TaskDto>(viewResult.Value);
            Assert.True(taskDto.Id.Equals(actualTask.Id));
        }
        #endregion

        #region Create a new task

        [Fact]
        public void CreateTask_ActionExecute_TaskForCreationDto_Is_Null_ReturnsBadRequestResult()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            TaskForCreationDto taskForCreationDto = null;
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.CreateTask(projectId, taskForCreationDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateTask_ActionExecute_No_Project_With_Id_ReturnsNotFoundResult()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffProjectId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var taskForCreationDto = new TaskForCreationDto();
            var project = new Project { Id = projectId };
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.CreateTask(diffProjectId, taskForCreationDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CreateTask_ActionExecute_Succeeds()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var task = new Entities.Models.Task();
            var project = new Project { Id = projectId };
            var taskForCreationDto = new TaskForCreationDto();
            var taskDto = new TaskDto();
            repository.Setup(o => o.Task.CreateTask(projectId, task));
            repository.Setup(o => o.Project.GetProjectById(projectId, false)).Returns(project);
            mapper.Setup(m => m.Map<Entities.Models.Task>(taskForCreationDto)).Returns(task);
            mapper.Setup(m => m.Map<TaskDto>(task)).Returns(taskDto);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.CreateTask(projectId, taskForCreationDto);

            repository.Verify(x => x.Save(), Times.Once());
        }
        #endregion

        #region Delete a task

        [Fact]
        public void DeleteTask_ActionExecute_No_Project_With_ProjectId_ReturnsNotFoundResult()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffProjectId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var project = new Project();
            var task = new Task();
            var taskId = Guid.Parse("2c514072-20ba-4070-9e0f-1274718e96fa");
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.DeleteTask(diffProjectId, taskId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTask_ActionExecute_No_Task_With_Id_ReturnsNotFoundResult()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project();
            var task = new Task();
            var taskId = Guid.Parse("2c514072-20ba-4070-9e0f-1274718e96fa");
            var diffTaskId = Guid.Parse("66652026-a066-48e9-8191-844fc9b1367a");
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, false)).Returns(task);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.DeleteTask(projectId, diffTaskId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTask_Succeeds_ActionExecute()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project();
            var task = new Task();
            var taskId = Guid.Parse("2c514072-20ba-4070-9e0f-1274718e96fa");
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, false)).Returns(task);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.DeleteTask(projectId, taskId);

            repository.Verify(x => x.Save(), Times.Once());
        }

        #endregion

        #region Update a task

        [Fact]
        public void UpdateTask_ActionExecute_TaskForUpdateDto_Is_Null_ReturnsBadRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var taskId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            TaskForUpdateDto taskForUpdateDto = null;
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateTask(projectId, taskId, taskForUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateTask_ActionExecute_No_Project_With_Provided_ProjectId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            var diffProjectId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            var taskId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var taskForUpdateDto = new TaskForUpdateDto();
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateTask(diffProjectId, taskId, taskForUpdateDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateTask_ActionExecute_No_Task_With_Provided_TaskId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            var taskId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var diffTaskId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            var task = new Task { Id = taskId };
            var taskForUpdateDto = new TaskForUpdateDto();
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, true)).Returns(task);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateTask(projectId, diffTaskId, taskForUpdateDto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateTask_ActionExecute_Succeeds()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var project = new Project { Id = projectId };
            var taskId = Guid.Parse("6570fd52-606a-4790-a1ba-f3574d13c794");
            var task = new Task { Id = taskId };
            var taskForUpdateDto = new TaskForUpdateDto();
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, true)).Returns(task);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.UpdateTask(projectId, taskId, taskForUpdateDto);

            repository.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region Partially update a project

        [Fact]
        public void PartiallyUpdateForTask_ActionExecute_JsonPatchDocument_Is_Null_ReturnsBadRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var id = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            JsonPatchDocument<TaskForUpdateDto> patchDoc = null;
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForTask(projectId, id, patchDoc);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PartiallyUpdateForTask_ActionExecute_No_Project_With_Provided_ProjectId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var diffProjectId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            var taskId = Guid.Parse("36e0ef90-4072-4d1b-a022-1f4f913bd04c");
            var patchDoc = new JsonPatchDocument<TaskForUpdateDto>();
            var project = new Project { Id = projectId };
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForTask(diffProjectId, taskId, patchDoc);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PartiallyUpdateForTask_ActionExecute_No_Task_With_Provided_TaskId_ReturnsNotFoundRequest()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var taskId = Guid.Parse("36e0ef90-4072-4d1b-a022-1f4f913bd04c");
            var diffTaskId = Guid.Parse("30b85733-876a-484b-a967-3ee818cc4cb3");
            var patchDoc = new JsonPatchDocument<TaskForUpdateDto>();
            var project = new Project { Id = projectId };
            var task = new Task { Id = taskId };
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, true)).Returns(task);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForTask(projectId, diffTaskId, patchDoc);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PartiallyUpdateForTask_ActionExecute_Succeeds()
        {
            var projectId = Guid.Parse("d86bc0d0-4edf-45d0-b7bb-78e7068c80b9");
            var taskId = Guid.Parse("36e0ef90-4072-4d1b-a022-1f4f913bd04c");
            var patchDoc = new JsonPatchDocument<TaskForUpdateDto>();
            var project = new Project { Id = projectId };
            var task = new Task { Id = taskId };
            repository.Setup(x => x.Project.GetProjectById(projectId, false)).Returns(project);
            repository.Setup(x => x.Task.GetTaskById(projectId, taskId, true)).Returns(task);
            var taskForPatch = new TaskForUpdateDto();
            mapper.Setup(x => x.Map<TaskForUpdateDto>(task)).Returns(taskForPatch);
            var sut = new TasksController(logger.Object, repository.Object, mapper.Object);

            var result = sut.PartiallyUpdateForTask(projectId, taskId, patchDoc);

            repository.Verify(x => x.Save(), Times.Once);
        }

        #endregion
    }
}