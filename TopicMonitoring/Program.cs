using Structurizr.Api;
using StudentMonitoring;

var structurizrClient = new StructurizrClient(
    "http://localhost:8080/api",
    "4aacd087-8499-477b-b4d7-88a86beace35",
    "67ea8a99-5269-412e-b2af-cc2374378b5f");

structurizrClient.AddTopicMonitoring();