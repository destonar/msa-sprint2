var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Hotelio_Booking_Service>("booking")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Hotelio_Booking_Statistics>("booking-statistics")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
