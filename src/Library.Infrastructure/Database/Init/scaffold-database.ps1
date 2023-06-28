cd .\src\Library.Infrastructure

dotnet ef dbcontext scaffold "Data Source=.;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer --output-dir .\Database\ --context LibraryDbContext -p .\Library.Infrastructure.csproj -s ..\Library.Api\Library.Api.csproj
