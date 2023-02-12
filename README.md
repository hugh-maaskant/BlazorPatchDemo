# BlazorPatchDemo

A demo App to show how to use HTTP PATCH in ASP.NET Core with a hosted Blazor WASM application.

The demo provides a simple UI to manipulate (a List of) Items that are maintained in a MongoDB database on the server.
The definition of the Items, and large parts of the Server's Controler logic, are re-used from Julio Casal's excellent "Building Microservices With .NET" course.
They mimic an inventory of Game attributes, with a Name, Description, and a Price as the main user-relevant properties.
See https://dotnetmicroservices.com/ for details on the course.

At this stage, the Blazor UI provides a simple CRUD capabilty, respectively using using HTTP POST, GET, PUT, and DELETE operations. 
For now, the Create and Update operations use some "canned" logic to change the Items' properties.

To simulate connection and server errors, and also to test Client error handling, 
the Server will randomly (p = 20%) return an error in stead of performing the requested operation.

The next steps include:

* adding Edit Forms to the UI so that we can really add and edit Items to the Inventory collection;
* using PATCH in stead of PUT for partial updates.
