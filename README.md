# AspNetCore.Samples
Sample apps with ASP.NET Core.

Solution contains sample applications build with ASP.NET Core.

## AspNetCore.Sample/Redis
An ASP.NET Core application using Redis cache to store users.

Users are persisted as single entities, tracked by their key. Additionally, two sorted sets contain information regarding the date the user was created and points, respectively. These sorted sets will be used to create tow separate leaderboards.

### Usage
Redis cache with ASP.NET Core, Core dependency injection, ViewComponents, Routing, Static files.

# Download
Clone repository `https://github.com/gdyrrahitis/AspNetCore.Samples.git` on your desktop.

Make sure you have downloaded Redis for windows. If not, go [here](https://github.com/MSOpenTech/redis) and download redis. To run a redis server, go to `%programfiles%/Redis` and run the `redis-cli.exe`.

Then run the application.
