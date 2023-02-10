using Xunit;

namespace CoreApiTemplate.IntegrationTest.Scenarios.ToDo
{
    [CollectionDefinition(Constants.ToDoFixture)]
    public class ToDoCollectionDefinition : ICollectionFixture<ToDoFixture>
    {

    }
}
