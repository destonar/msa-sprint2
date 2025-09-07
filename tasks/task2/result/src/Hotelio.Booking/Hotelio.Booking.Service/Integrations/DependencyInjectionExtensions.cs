using Hotelio.Booking.Service.Integrations.Hotels;
using Hotelio.Booking.Service.Integrations.PromoCodes;
using Hotelio.Booking.Service.Integrations.Reviews;
using Hotelio.Booking.Service.Integrations.Users;

namespace Hotelio.Booking.Service.Integrations;

public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder AddIntegrations(this WebApplicationBuilder builder)
    {
        var integrationSettings = new IntegrationOptions();
        builder.Configuration.GetSection("Integrations").Bind(integrationSettings);

        builder.Services.AddHttpClient<IHotelsClient, HotelsClient>(client =>
        {
            client.BaseAddress = new Uri(integrationSettings.HotelsUrl);
        }).ConfigurePrimaryHttpMessageHandler(_ =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        });

        builder.Services.AddHttpClient<IReviewsClient, ReviewsClient>(client =>
        {
            client.BaseAddress = new Uri(integrationSettings.ReviewsUrl);
        }).ConfigurePrimaryHttpMessageHandler(_ =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        });
        
        builder.Services.AddHttpClient<IUsersClient, UsersClient>(client =>
        {
            client.BaseAddress = new Uri(integrationSettings.UsersUrl);
        }).ConfigurePrimaryHttpMessageHandler(_ =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        });
        
        builder.Services.AddHttpClient<IPromoCodesClient, PromoCodesClient>(client =>
        {
            client.BaseAddress = new Uri(integrationSettings.PromoCodesUrl);
        }).ConfigurePrimaryHttpMessageHandler(_ =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
        });
        
        return builder;
    }
}