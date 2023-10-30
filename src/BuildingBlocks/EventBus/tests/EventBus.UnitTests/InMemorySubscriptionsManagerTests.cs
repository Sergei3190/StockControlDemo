using EventBus.UnitTests.TestsData;

namespace EventBus.UnitTests;

public class InMemorySubscriptionsManagerTests
{
    [Fact]
    public void After_Creation_Should_Be_Empty()
    {
        var manager = new InMemorySubscriptionsManager();
        Assert.True(manager.IsEmpty);
    }

    [Fact]
    public void After_One_Event_Subscription_Should_Contain_The_Event()
    {
        var manager = new InMemorySubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        Assert.True(manager.HasSubscriptionsForEvent<TestIntegrationEvent>());
    }

    [Fact]
    public void After_All_Subscriptions_Are_Deleted_Event_Should_No_Exists()
    {
        var manager = new InMemorySubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.RemoveSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        Assert.False(manager.HasSubscriptionsForEvent<TestIntegrationEvent>());
    }

    [Fact]
    public void Deleting_Last_Subscription_Should_Raise_On_Deleted_Event()
    {
        bool raised = false;
        var manager = new InMemorySubscriptionsManager();
        manager.OnEventRemoved += (o, e) => raised = true;
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.RemoveSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        Assert.True(raised);
    }

    [Fact]
    public void Get_Handlers_For_Event_Should_Return_All_Handlers()
    {
        var manager = new InMemorySubscriptionsManager();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationEventHandler>();
        manager.AddSubscription<TestIntegrationEvent, TestIntegrationOtherEventHandler>();
        var handlers = manager.GetHandlersForEvent<TestIntegrationEvent>();
        Assert.Equal(2, handlers.Count());
    }
}