# Database migrations  
via EntityFrameworkCore  

Open terminal:  
```
dotnet ef migrations --project ./AutoLegalTracker-API/AutoLegalTracker-API.csproj list  
dotnet ef migrations --project ./AutoLegalTracker-API/AutoLegalTracker-API.csproj add [migration name] -o 3_DataAccess/Migrations
dotnet ef database --project ./AutoLegalTracker-API/AutoLegalTracker-API.csproj update  
```

Migration naming conventions
[migration name] = [#github issue]_[name of GitHub issue]

Example:  
Github issue: Auto reply page #9  
Migration name: 9_Auto_reply_page


