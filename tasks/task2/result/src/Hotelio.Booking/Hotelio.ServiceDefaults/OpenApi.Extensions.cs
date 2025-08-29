using Asp.Versioning;
using Hotelio.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

public static partial class Extensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        app.MapOpenApi();

        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                // Disable default fonts to avoid download unnecessary fonts
                options.DefaultFonts = false;
            });
            app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = null)
    {
        var openApi = builder.Configuration.GetSection("OpenApi");
        var identitySection = builder.Configuration.GetSection("Identity");

        var scopes = identitySection.Exists()
            ? identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value)
            : new Dictionary<string, string?>();


        if (!openApi.Exists())
        {
            return builder;
        }

        if (apiVersioning is null)
        {
            return builder;
        }
        
        // the default format will just be ApiVersion.ToString(); for example, 1.0.
        // this will format the version as "'v'major[.minor][-status]"
        apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        builder.Services.AddOpenApi("v1", options =>
        {
            options.ApplyApiVersionInfo(openApi.GetRequiredValue("Document:Title"), openApi.GetRequiredValue("Document:Description"));
            options.ApplyAuthorizationChecks([.. scopes.Keys]);
            options.ApplyOperationDeprecatedStatus();
            options.ApplyApiVersionDescription();
            options.ApplySchemaNullableFalse();
            // Clear out the default servers so we can fallback to
            // whatever ports have been allocated for the service by Aspire
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Servers = [];
                return Task.CompletedTask;
            });
        });

        return builder;
    }
}
