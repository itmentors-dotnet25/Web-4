namespace BookLibrary.Tests.IntegrationTests;

[CollectionDefinition("TestContainers", DisableParallelization = true)]
public class TestContainersCollection : ICollectionFixture<BookLibraryTestContainersFactory> { }


