using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyStoryMaker.Models
{
    
        /// <summary>
        /// A custom initializer that will seed the data in.
        /// 
        /// There are 3 types of Initializers..
        ///     DropCreateDatabaseIfModelChanges
        ///     CreateDatabaseIfNotExists
        ///     DropCreateDatabaseAlways
        /// </summary>
    public class StoryDBInitializer : DropCreateDatabaseIfModelChanges<StoryContext>
    {

        //protected override void Seed(StoryContext context)
        //{
        //    ImagesModels cin_cover = new ImagesModels()
        //    {
        //        Id = 0,
        //        ImgName = "cover.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img1 = new ImagesModels()
        //    {
        //        Id = 1,
        //        ImgName = "1.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img2 = new ImagesModels()
        //    {
        //        Id = 2,
        //        ImgName = "2.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img3 = new ImagesModels()
        //    {
        //        Id = 3,
        //        ImgName = "3.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img4 = new ImagesModels()
        //    {
        //        Id = 4,
        //        ImgName = "4.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img5 = new ImagesModels()
        //    {
        //        Id = 5,
        //        ImgName = "5.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img6 = new ImagesModels()
        //    {
        //        Id = 6,
        //        ImgName = "6.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img7 = new ImagesModels()
        //    {
        //        Id = 7,
        //        ImgName = "7.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img8 = new ImagesModels()
        //    {
        //        Id = 8,
        //        ImgName = "8.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img9 = new ImagesModels()
        //    {
        //        Id = 9,
        //        ImgName = "9.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img10 = new ImagesModels()
        //    {
        //        Id = 10,
        //        ImgName = "10.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img11 = new ImagesModels()
        //    {
        //        Id = 11,
        //        ImgName = "11.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img12 = new ImagesModels()
        //    {
        //        Id = 12,
        //        ImgName = "12.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img13 = new ImagesModels()
        //    {
        //        Id = 13,
        //        ImgName = "13.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img14 = new ImagesModels()
        //    {
        //        Id = 14,
        //        ImgName = "14.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img15 = new ImagesModels()
        //    {
        //        Id = 15,
        //        ImgName = "15.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img16 = new ImagesModels()
        //    {
        //        Id = 16,
        //        ImgName = "16.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_img17 = new ImagesModels()
        //    {
        //        Id = 17,
        //        ImgName = "17.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    ImagesModels cin_end = new ImagesModels()
        //    {
        //        Id = 18,
        //        ImgName = "end.jpg",
        //        ImgLink = "~\\Content\\StoryRepos\\Cinderella"
        //    };

        //    StoryModels cinderella = new StoryModels()
        //    {
        //        Id=1,
        //        Text="../Content/StoryRepos/Cinderella/Ciderella.xml",
        //        Title = "cinderella",
        //        Author = "Charles Perrault",
        //        Time=new DateTime(1697),
        //        Description="This story is initialled by seed.",
        //        Genre="Farytale",
        //        Images = new List<ImagesModels>()
        //        {
        //            cin_img17,cin_img16,cin_img15,cin_img14,cin_img13,cin_img12,
        //            cin_img11,cin_img10,cin_img9,cin_img8,cin_img7,cin_img6,
        //            cin_img5,cin_img4,cin_img3,cin_img2,cin_img1,cin_cover,cin_end
        //        }
        //    };
           

            

        //    // The order is important, since we are setting up references

        //    context.Images.Add(cin_cover);
        //    context.Images.Add(cin_img17);
        //    context.Images.Add(cin_img16);
        //    context.Images.Add(cin_img15);
        //    context.Images.Add(cin_img14);
        //    context.Images.Add(cin_img13);
        //    context.Images.Add(cin_img12);
        //    context.Images.Add(cin_img11);
        //    context.Images.Add(cin_img10);
        //    context.Images.Add(cin_img9);
        //    context.Images.Add(cin_img8);
        //    context.Images.Add(cin_img7);
        //    context.Images.Add(cin_img6);
        //    context.Images.Add(cin_img5);
        //    context.Images.Add(cin_img4);
        //    context.Images.Add(cin_img3);
        //    context.Images.Add(cin_img2);
        //    context.Images.Add(cin_img1);
        //    context.Images.Add(cin_end);

        //    context.Stories.Add(cinderella);

        //    // letting the base method do anything it needs to get done
        //    base.Seed(context);

        //    // Save the changes you made, when adding the data above
        //    context.SaveChanges();
        //    }
        }
    }
