using CoreApiTemplate.Application.Dtos;
using CoreApiTemplate.IntegrationTest.Extensions;
using CoreApiTemplate.IntegrationTest.Scenarios.ToDo;
using FluentAssertions;
using Newtonsoft.Json;
using SSS.AspNetCore.Extensions.Models;
using System.Threading.Tasks;
using Xunit;

namespace CoreApiTemplate.IntegrationTest.Scenarios.Todo
{
    public class ToDoScenario : IClassFixture<ToDoFixture>
    {
        private const string _baseUrl = "api/v1/todo";

        private readonly ToDoFixture _fixture;

        public ToDoScenario(ToDoFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Get_Todo_Item_Successfully()
        {
            var query = new BaseQuery().GetQueryString();

            var response = await _fixture.Client.GetAsync($"{_baseUrl}?{query}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_Todo_Item_Successfully()
        {
            var data = new CreateTodoItemDto
            {
                Title = "Test",
                Description = "Test"
            };

            var response = await _fixture.Client.PostAsync(_baseUrl, data.GetRequestContent());

            response.EnsureSuccessStatusCode();

            var strResult = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ToDoItemDto>(strResult);

            result?.Id.Should().NotBeNullOrEmpty();
        }
    }
}
