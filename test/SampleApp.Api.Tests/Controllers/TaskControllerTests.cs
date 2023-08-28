using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SampleApp.Api.Dto;
using SampleApp.Api.Models;
using SampleApp.Domain;
using Newtonsoft.Json;
using Xunit;

namespace SampleApp.Api.Tests.Controllers
{
    public class TaskControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private HttpClient Client { get; }

        public TaskControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTasksTest()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/api/v1.0/tasks");
            response.EnsureSuccessStatusCode();
            var result =
                JsonConvert.DeserializeObject<Response<IEnumerable<TaskResponseModel>>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetTasksByIdTest(int taskId)
        {
            // Arrange & Act
            var response = await Client.GetAsync($"/api/v1.0/tasks/{taskId}");
            response.EnsureSuccessStatusCode();
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task CreateTaskTest()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("Task 6", "Task 6 Description", DateTime.Now.AddDays(5),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(3), Enums.Priority.Middle, Enums.Status.New);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            response.EnsureSuccessStatusCode();
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.NotNull(result.Data);
            Assert.NotEqual(0, result.Data.Id);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task UpdateTaskTest()
        {
            // Arrange
            var taskCreate = new TaskUpdateRequest(1, "Task 1 Name Update", "Task 1 Description Update",
                DateTime.Now.AddDays(5),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(4), Enums.Priority.Low, Enums.Status.New);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // & Act
            var response = await Client.PutAsync("/api/v1.0/tasks", data);
            response.EnsureSuccessStatusCode();
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Task 1 Name Update", result.Data.Name);
            Assert.Equal("Task 1 Description Update", result.Data.Description);
            Assert.Equal(Enums.Priority.Low, result.Data.Priority);
            Assert.Equal(Enums.Status.New, result.Data.Status);
            Assert.Null(result.Errors);
        }

        [Theory]
        [InlineData(5)]
        public async Task DeleteTaskTest(int taskId)
        {
            // Arrange & Act
            var response = await Client.DeleteAsync($"/api/v1.0/tasks/{taskId}");
            response.EnsureSuccessStatusCode();
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(result is { Succeeded: true });
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task CreateTaskModelValidationTest()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("", "", default,
                default,
                default, default, default);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Name is a required field.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Description is a required field.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid Due Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than today.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid Start Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid End Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Priority is a required field.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Status is a required field.")));
        }

        [Fact]
        public async Task UpdateTaskModelValidationTest()
        {
            // Arrange
            var taskCreate = new TaskUpdateRequest(default, "", "", default, default, default, default, default);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // & Act
            var response = await Client.PutAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                    await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Id is a required field.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Name is a required field.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Description is a required field.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid Due Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than today.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid Start Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x =>
                x.Message.Contains("Valid End Date is required. Default Date value is not allowed.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Priority is a required field.")));
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Status is a required field.")));
        }

        [Fact]
        public async Task CreateTaskDueDateShouldNotBeInPastTest()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("Task 7", "Task 7 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than today.")));
        }

        [Fact]
        public async Task CreateTaskDueDateShouldNotBeLessThanStartDate()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("Task 7", "Task 7 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than Start date.")));
        }

        [Fact]
        public async Task CreateTaskDueDateShouldNotBeLessThanEndDate()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("Task 7", "Task 7 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than End date.")));
        }

        [Fact]
        public async Task UpdateTaskDueDateShouldNotBeInPastTest()
        {
            // Arrange
            var taskCreate = new TaskUpdateRequest(2, "Task 2", "Task 2 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than today.")));
        }

        [Fact]
        public async Task UpdateTaskDueDateShouldNotBeLessThanStartDate()
        {
            // Arrange
            var taskCreate = new TaskUpdateRequest(2, "Task 2", "Task 2 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than Start date.")));
        }

        [Fact]
        public async Task UpdateTaskDueDateShouldNotBeLessThanEndDate()
        {
            // Arrange
            var taskCreate = new TaskUpdateRequest(2, "Task 2", "Task 2 Description", DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(result.Errors.First(x => x.Message.Contains("Due Date shouldn't be less than End date.")));
        }

        [Fact]
        public async Task TaskStartDateShouldBeLessThanEndDateTest()
        {
            // Arrange
            var taskCreate = new TaskCreateRequest("Task 6", "Task 6 Description", DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2),
                DateTime.Now.AddDays(1), Enums.Priority.High, Enums.Status.Finished);

            var json = JsonConvert.SerializeObject(taskCreate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1.0/tasks", data);
            var result =
                JsonConvert.DeserializeObject<Response<TaskResponseModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(result is { Succeeded: false });
            Assert.Equal("Model Validation Failed", result.Message);
            Assert.NotNull(
                result.Errors.First(x => x.Message.Contains("Start Date shouldn't be greater than End date.")));
        }

        [Fact]
        public async Task SystemShouldNotHaveMoreThan100NotFinishedHighPriorityTaskTest()
        {
            // Arrange
            for (var i = 0; i <= 105; i++)
            {
                var taskCreate = new TaskCreateRequest($"Task {i}", $"Task {i} Description", DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(4), Enums.Priority.High, Enums.Status.New);

                var json = JsonConvert.SerializeObject(taskCreate);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await Client.PostAsync("/api/v1.0/tasks", data);

                if (response.StatusCode == HttpStatusCode.OK) continue;

                var result =
                    JsonConvert.DeserializeObject<Response<TaskResponseModel>>(
                        await response.Content.ReadAsStringAsync());

                // Assert
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.True(result is { Succeeded: false });
                Assert.NotNull(result.Errors.First(x =>
                    x.Message.Contains(
                        $"More than 100 high priority tasks with due date ({taskCreate.DueDate.ToShortDateString()}) are not finished yet hence new high priority task with same due date cannot be created.")));
                break;
            }
        }
    }
}