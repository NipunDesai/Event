using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Autofac;
using EventDemoProject.Migrations;
using EventDemoProject.Models.DataContext;

namespace EventDemoProject.App_Start
{
    public class DatabaseConfig
    {
        public static void Initialize(IComponentContext componentContext)
        {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EventDbContext, Configuration>("EventDbContext"));
            using (var eventDbContext = componentContext.Resolve<DbContext>())
            {
                eventDbContext.Database.Initialize(false);

            }

            
        }
    
    }
}