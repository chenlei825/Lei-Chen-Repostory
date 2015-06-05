using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyStoryMaker.Models
{
    public class StoryContext : DbContext
    {

       public StoryContext()
            : base("StoryDatabase")
        {
            //Database.SetInitializer<StoryContext>(new StoryDBInitializer());
        }

       //public System.Data.Entity.DbSet<MyStoryMaker.Models.AdminRole> AdminRoles { get; set; }

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.StoryModels> Stories { get; set; }

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.ImagesModels> Images { get; set; }

        ///// <summary>
        ///// If you want to do anything custom, you can put it in here.. Otherwise, the method is a waste
        ///// </summary>
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.BlockModels> BlockModels { get; set; }

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.StoryModels> StoryModels { get; set; }

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.CollagesModels> storyCollages { get; set; }

        //public System.Data.Entity.DbSet<MyStoryMaker.Models.AvailBlockModels> storyCollages { get; set; }


    }
}