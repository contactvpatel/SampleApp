# SampleApp Api

SampleApp Api is a RESTFul application that demonstrates the ability to manage Tasks (CRUD operation) & call Multipl APIs.

## Business Requirements
#### Multi API ####
- Calls should occur from the front-end WebAPI to the 2 back-end APIs in an Asynchronous manner
- Artificial delays can be introduced to mimic a longer running operation in API2 (TargetService) and API3 (AmazonService)

#### Task API ####
- When creating a Task, the Due Date cannot be in the past 
- The system should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time 
- Domain should include the following: 
    - Id 
    - Name 
    - Description 
    - Due Date 
    - Start Date 
    - End Date 
    - Priority (High, Middle, Low) 
    - Status (New, In Progress, Finished)

## Concepts covered
- Test Driven Development (TDD) 
- Dependency Injection (DI) 
- Domain Driven Design (DDD) 
- Clean Architecture

## Additional Topics
SampleApp Api project covered additional advanced topics

- Api Versioning supporting multiple versioning scheme
- Custom Swagger implementation to support JWT Authentication
- Bearer Token Authorization implementation through CustomAuthorization filter (Intercept all Api call to perform token validation)
- FluentValidation for Api Model Validation
- External Service Implementation (RedisCache)
- Health Checks with customized response
- Custom Response Model for all Api's response

```json
{
  "succeeded": true,
  "data": [
    {
      "id": 1,
      "name": "Task 1",
      "description": "Task 1 Description",
      "dueDate": "2022-05-13T10:42:23.5393606-04:00",
      "startDate": "2022-05-10T10:42:23.5417528-04:00",
      "endDate": "2022-05-11T10:42:23.5417542-04:00",
      "priority": 1,
      "status": 3
    }
  ],
  "message": "Found 1 task",
  "errors": null
}
```
- Global Exception Handler Middleware to hide underlying exception details 
```json
{
  "succeeded": false,
  "data": null,
  "message": "",
  "errors": [
    {
      "errorId": "3bd75b0e-a385-408f-9162-87fe06441a22",
      "statusCode": 500,
      "message": "Error occurred in the API. Please use the ErrorId [3bd75b0e-a385-408f-9162-87fe06441a22] and contact support team if the problem persists."
    }
  ]
}
```

## Tools & Technology

- Visual Studio 2022
- .NET 7
- EF Core 7

## Database

- EF Core In-Memory Database used for simplicity.
- If you choose to use SQL Server then set UseInMemoryDatabase as false and provide server and credential details in appsettings.json and then apply ef core migration to database.

```json
  "DbConnectionSettings": {
    "UseInMemoryDatabase": true,
    "Host": "(local)",
    "Port": "",
    "DatabaseName": "SampleApp",
    "IntegratedSecurity": false,
    "UserName": "test",
    "Password": "test"
  }
```
