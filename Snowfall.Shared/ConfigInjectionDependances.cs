using Microsoft.Extensions.DependencyInjection;
using Snowfall.Application.Services;
using Snowfall.Data.Context;
using Snowfall.Data.Repositories;

namespace Snowfall.Shared;

public static class ConfigInjectionDependances
{
    public static IServiceCollection EnregistrerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        
        // Data
        services.AddSingleton<DapperContext>();
        services.AddScoped<IEvenementRepository, EvenementRepository>();
        services.AddScoped<IVilleRepository, VilleRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ICommentaireRepository, CommentaireRepository>();
        //services.AddScoped<IAchatRepository, AchatRepository>();
        
        // Application
        services.AddScoped<IEvenementService, EvenementService>();
        services.AddScoped<IVilleService, VilleService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ICommentaireService, CommentaireService>();
        //services.AddScoped<IAchatService, AchatService>();
        //services.AddScoped<IPrixService, PrixService>();

        return services;
    }
}