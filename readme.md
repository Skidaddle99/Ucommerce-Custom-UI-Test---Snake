# About
Contains a template for easy setup of a Ucommerce project using the in-process library.

# Key Features
- Installs all necessary packages for an in-process Ucommerce experience.
- Includes basic setup and configuration of the application.

# How To
## Generate the project
To use the In-Process template:
- Make sure to have Docker installed.
- Open a terminal in the folder where you wish to create the project.
- Execute the command:
`dotnet new uc-mvc`
## Add the project to the solution
`dotnet sln add "<ProjectName>"`

## Run SQL and Elasticserver in Docker

The template generates a docker setup with an Azure SQL and an Elasticsearch server with the
necessary configuration added to just run Ucommerce.

`docker compose -f docker/docker-compose.yml up -d`

## Run the project
To run the project, just call `dotnet watch run` in the project directory.

# Template options
For a full list of options and commands for the template, you can execute:
`dotnet new uc-mvc -h`



# Links
- [Documentation](https://dev.ucommerce.net/)