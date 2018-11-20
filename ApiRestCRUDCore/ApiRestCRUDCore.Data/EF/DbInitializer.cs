using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiRestCRUDCore.Data.EF
{

    public class DbInitializer : DbContext
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {

            var ctx = serviceProvider.GetRequiredService<ApiRestCRUDCoreDataContext>();

            ctx.Database.EnsureCreated();

        }

    }

}