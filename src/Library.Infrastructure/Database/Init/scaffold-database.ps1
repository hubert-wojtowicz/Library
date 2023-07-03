cd .\src\Library.Infrastructure

# added db models
dotnet ef dbcontext scaffold "Data Source=.;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer --output-dir .\Database\ --context LibraryDbContext -p .\Library.Infrastructure.csproj -s ..\Library.Api\Library.Api.csproj

# added migration for view
dotnet ef migrations add AddSearchView -p .\Library.Infrastructure.csproj -s ..\Library.Api\Library.Api.csproj -o .\Database\Migrations\

# update db
dotnet ef database update -p .\Library.Infrastructure.csproj -s ..\Library.Api\Library.Api.csproj
