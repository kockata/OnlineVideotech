﻿namespace OnLineVideotech.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddDomainServices(this IServiceCollection services)
        //{
        //    Assembly
        //        .GetAssembly(typeof(IService))
        //        .GetTypes()
        //        .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == t.Name))
        //        .Select(t => new
        //        {
        //            Interface = t.GetInterface(t.Name),
        //            Implementation = t
        //        })
        //        .ToList()
        //        .ForEach(s => services.AddTransient(s.Interface, s.Implementation));

        //    return services;
        //}
    }
}