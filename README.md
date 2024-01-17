## Getting started - local development

- **Software dependencies**
    - .NET
    - Node
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
