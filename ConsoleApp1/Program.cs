// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServerSide.Demo;
using ServerSide.Models;
using ServerSide.Modle;
using ServerSide.Repositories;
using ServerSide.Services;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using ServerSide.Support;
using SharedObjects.DTOs;
using SharedObjects.FileSystemCommunication;
using SharedObjects.RequestsAndResponses;
using SharedObjects.TockenHandling;
using System.Collections;
using System.Data.SqlTypes;
using System.Reflection;
using static ServerSide.Support.RequestHandler;

//await CreateUserTest.Test(); 
//await AssignManagerTest.Test();
//await AssignTaskByManagerTest.Test(); 
//await CompleteTaskTest.Test(); 
//await CancelTaskTest.Test(); 
//await FilterUsersByRoleTest.Test(); 
//await GetAllSubordinatesTest.Test();  

//SetUpSystem();


void SetUpSystem()
{
    StartWatching();

}
void StartWatching()
{
    string Path = @"C:\Assignment\Requests";

    //turn on watching 
    Watcher watcher = new Watcher();
    watcher.WatchingEvent += GetRequest;
    watcher.watch(Path);
}

static async void GetRequest(object sender, WatchingEventArg e)
{

    await RequestHandler.HandleRequest(e.FileAsAsJason);

}








