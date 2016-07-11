using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using EventDemoProject.Models.Modules;
using EventDemoProject.Models.Modules.Global;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventDemoProject.Models.DataContext
{
    public class EventUser : IdentityUser
    {
         public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<EventUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class EventDbContext :IdentityDbContext<EventUser>
    {
        public EventDbContext()
            : base("EventDbContext")
        {


        
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EventJoin>()
                .HasRequired(n => n.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Event> Event { get; set; }
        public static EventDbContext Create()
        {
            return new EventDbContext();
        }
    }
}