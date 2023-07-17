# Database migrations  
via EntityFrameworkCore  

Open terminal:  
```
dotnet ef migrations --project ./ALTDeployTest/ALTDeployTest.csproj list  
dotnet ef migrations --project ./ALTDeployTest/ALTDeployTest.csproj add [migration name]
```

Migration naming conventions
[migration name] = [#github issue]_[name of GitHub issue]

Example:  
Github issue: Auto reply page #9  
Migration name: 9_Auto_reply_page
