namespace Structurizr;

public static class BaseSystem
{
    public const long WorkspaceId = 1;

    public static Workspace CreateBase()
    {
        var workspace = new Workspace("HDP", "This is an HDP system.");
        var model = workspace.Model;
        model.ImpliedRelationshipsStrategy = new CreateImpliedRelationshipsUnlessAnyRelationshipExistsStrategy();
        var views = workspace.Views;

        model.Enterprise = new Enterprise("HDP");

        // people and software systems
        var apiSystem = model.AddSoftwareSystem(Location.Internal, "Api System", "Allows user to view information about their groups, classes, topics, tasks, academic performance etc.");
        var user = model.AddPerson(Location.External, "User", "A user of Horoschool.");
        user.Uses(apiSystem, "Views groups, classes, topics, tasks, academic performance etc.");

        var webApplication = apiSystem.AddContainer("Web Application", "Base UI of users.", "React Native");
       
        var gateway = apiSystem.AddContainer("Gateway", "JSON/HTTPS API.", "C#");
        gateway.AddTags(HdpTags.Microservice);

        var identity = apiSystem.AddContainer("Identity", "JSON/HTTPS API.", "C#");
        identity.AddTags(HdpTags.Microservice);
        var database = apiSystem.AddContainer("Identity_Database", "Stores user registration information, hashed authentication credentials, access logs, etc.", "PostgreSQL");
        database.AddTags(HdpTags.Database);

        user.Uses(webApplication, "Visits horodigital", "HTTPS");
        webApplication.Uses(gateway, "Proxy service", "REST");
        gateway.Uses(identity, "Reads from and writes to", "REST");
        identity.Uses(database, "Reads from and writes to", "PostgreSQL odbc driver");

        var containerView = views.CreateContainerView(apiSystem, "Containers", "The container diagram for the Internet Banking System.");
        containerView.PaperSize = PaperSize.A1_Landscape;
        containerView.Add(user);

        containerView.AddAllContainers();

        var styles = views.Configuration.Styles;
        styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
        styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5", Color = "#ffffff" });
        styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#000000" });
        styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person, FontSize = 22 });
        styles.Add(new ElementStyle(HdpTags.ExistingSystem) { Background = "#999999", Color = "#ffffff" });
        styles.Add(new ElementStyle(HdpTags.WebBrowser) { Shape = Shape.WebBrowser });
        styles.Add(new ElementStyle(HdpTags.Database) { Shape = Shape.Cylinder });
        styles.Add(new ElementStyle(HdpTags.Microservice) { Shape = Shape.Hexagon });

        return workspace;
    }

    public static Container GetGateway(this Workspace workspace)
    {
        return workspace.Model.SoftwareSystems.First().Containers.First(container => container.Name == "Gateway");
    }
}