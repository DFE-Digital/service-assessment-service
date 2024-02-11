## Description

This project is a web application which supports the service assessment process within the Department for Education (DfE).

[//]: # (Note that formal service assessments and informal peer reviews are both to be supported, and their terms are used interchangeably except where explicitly indicated otherwise.)


[//]: # (- Service teams:)
[//]: # (  - requesting an assessment of their service)
[//]: # (  - viewing the status of their service assessment)
[//]: # (  - providing feedback for their service assessment)
[//]: # (- Service assessors:)
[//]: # (  - planning and conducting service assessments)
[//]: # (  - recording service feedback)
[//]: # (- Administrative teams:)
[//]: # (  - managing and coordinating the service assessment process)
[//]: # (  - recording service assessor activity)
[//]: # (- General:)
[//]: # (  - viewing service assessment reports)

A service assessment is a peer review of a transactional service to check it meets
the [14 points of the Service Standard](https://www.gov.uk/service-manual/service-standard).
Service assessments happen at the end of alpha, beta and live phases. If you are in the discovery phase, you can
have a discovery peer review.

Additional information about service assessments, and the service standards to which services are assessed against, can
be found at:

- https://www.gov.uk/service-manual
- https://apply-the-service-standard.education.gov.uk/

### Project status

Currently this online service is:
- in it's private beta phase, and
- under active development, therefore subject to change.

### Screenshots

Note that this service is under active development and will change based on ongoing user research.
This means screenshots below may include features/pages that are not yet implemented, and are subject to change based on ongoing feedback.

- Screenshot of the prototype website, showing the task-list of complete/optional/in-progress tasks for an in-progress "discovery peer review":
  - ![Screenshot showing a prototype website, with Department for Education blue branding and the page content showing the task-list of complete/optional/in-progress tasks for an in-progress "discovery peer review".](docs/images/prototype--assure-service--screenshot-of-in-progress-assessment--page-task-list--full.png)

## Documentation for _using_ the system

This documentation is for developers and contributors to the application.

For help _using_ the application (as opposed to _installing and running_ the application):

- please refer to [Service assessments - apply the service standard in DfE](https://apply-the-service-standard.education.gov.uk/service-assessments)
  and documentation on the application itself, then
- contact the service assessment team for any additional queries.

## Developers and contributors

### Local development environment

#### Software dependencies/tooling
- .NET
- NodeJS
- Local SQL database (optional, if hosted database is available)

#### Quick start
- Clone the repository
- Setup database
- Install node dependencies
- Build the compiled HTML/CSS/JS files
- Setup C# user secrets
- Compile the C# code
- Run the application

#### Detailed steps
- **Install node dependencies**
    - ```shell
      cd src/ServiceAssessmentService/ServiceAssessmentService.WebApp
      npm install
      ```
- **Build the HTML/CSS/JS files**
    - ```shell
      cd src/ServiceAssessmentService/ServiceAssessmentService.WebApp
      npm run buildDev:watch
      ```
- **Setup secrets**
    - ...
- **Compile the C# code**
    - ```shell
      cd src/ServiceAssessmentService/ServiceAssessmentService.WebApp
      dotnet build
      ```
- **Run the application**
    - ```shell
      cd src/ServiceAssessmentService/ServiceAssessmentService.WebApp
      dotnet watch
      ```
