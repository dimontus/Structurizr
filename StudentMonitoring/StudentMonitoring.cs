using Structurizr;
using Structurizr.Api;

namespace StudentMonitoring;

public static class StudentMonitoring
{
    public static Workspace AddStudentMonitoring(this StructurizrClient client)
    {
        var workspace = client.GetWorkspace(BaseSystem.WorkspaceId);

        if (workspace is null)
        {
            workspace = BaseSystem.CreateBase();
        }

        var apiSystem = workspace.Model.SoftwareSystems.First();
        var containerView = workspace.Views.ContainerViews.First();

        var studentMonitoring = apiSystem.Containers.FirstOrDefault(container => container.Name == "StudentMonitoring");

        if (studentMonitoring is null)
        {
            studentMonitoring = apiSystem.AddContainer("StudentMonitoring", "JSON/HTTPS API.", "C#");
            studentMonitoring.AddTags(HdpTags.Microservice);
        }

        var database = apiSystem.Containers.FirstOrDefault(container => container.Name == "StudentMonitoring_Database");

        if (database is null)
        {
            database = apiSystem.AddContainer("StudentMonitoring_Database", "Stores student information about their groups, topics, tasks, academic performance etc.", "MongoDB");
            database.AddTags(HdpTags.Database);
        }

        studentMonitoring.Uses(database, "Reads from and writes to", "MongoDb driver");
        var gateway = workspace.GetGateway();
        gateway.Uses(studentMonitoring, "Reads from and writes to", "REST");

        containerView.AddAllContainers();

        client.PutWorkspace(1, workspace);

        return workspace;
    }
}