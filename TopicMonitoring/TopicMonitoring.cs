using Structurizr;
using Structurizr.Api;

namespace StudentMonitoring;

public static class StudentMonitoring
{
    public static Workspace AddTopicMonitoring(this StructurizrClient client)
    {
        var workspace = client.GetWorkspace(BaseSystem.WorkspaceId);

        if (workspace is null)
        {
            workspace = BaseSystem.CreateBase();
        }

        var apiSystem = workspace.Model.SoftwareSystems.First();
        var containerView = workspace.Views.ContainerViews.First();

        var topicMonitoring = apiSystem.Containers.FirstOrDefault(container => container.Name == "TopicMonitoring");

        if (topicMonitoring is null)
        {
            topicMonitoring = apiSystem.AddContainer("TopicMonitoring", "JSON/HTTPS API.", "C#");
            topicMonitoring.AddTags(HdpTags.Microservice);
        }

        var database = apiSystem.Containers.FirstOrDefault(container => container.Name == "TopicMonitoring_Database");

        if (database is null)
        {
            database = apiSystem.AddContainer("TopicMonitoring_Database", "Stores study group topic information.", "MongoDB");
            database.AddTags(HdpTags.Database);
        }

        topicMonitoring.Uses(database, "Reads from and writes to", "MongoDb driver");
        var gateway = workspace.GetGateway();
        gateway.Uses(topicMonitoring, "Reads from and writes to", "REST");

        containerView.AddAllContainers();

        client.PutWorkspace(1, workspace);

        return workspace;
    }
}