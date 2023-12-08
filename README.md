# User management solution for Callapp

- Run sql script `schema.sql` which will create a full database for the web application. Modify `ConnectionStrings:UserManagement` setting from `appsettings.json` accordingly to access your desired server.
- EF Core was used as an ORM for easy database manipulation. Migrations, Context and Entities are placed in `Callapp.UserManagement.Data`.
- We have business logic organised into `Callapp.UserManagement.Application`. It includes service interfaces and implementations along critical validations.
- `Callapp.UserManagement.Web` is an ASP.NET Core Web API where endpoins are created through Minimal APIs. It uses `Bearer Token Authorization` scheme.
- Once you lauch the solution, Swagger's Api Explorer will be shown in a browser.