# Implementations instructions

## Server environment variables:

### Development:
- ASPNETCORE_DBCON
Data Source=localhost;Database=ALT;User id=sa;Password=yourStrong(!)Password; TrustServerCertificate=true

### Staging:
- ASPNETCORE_DBCON
Data Source=[Server];User ID=[user];Password=[password];Connect Timeout=30;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Database=ALT

### Production:
- ASPNETCORE_DBCON

## Github environment variables:
- RENDER_API_KEY = [your key]
- RENDER_SERVICE_ID_STAGING = [your service id for staging]
- RENDER_SERVICE_ID_PRODUCTION = [your service id for production]

## AWS configuration:
Apply the correct inbound rules