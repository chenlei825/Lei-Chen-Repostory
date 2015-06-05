using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MyStoryMaker.Models
{
    public class StoryModels
    {
        public StoryModels()
        {
            this.id = 0;
            this.title = null;
            this.author = null;
            this.createTime = (DateTime.Now).ToString("g"); //2/27/2009 12:12 PM
            this.description = null;
            this.genre = null;
        }

        public StoryModels(int id,string title,
            string author, string createTime, string description,string genre)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.createTime = createTime;
            this.description = description;
        }


        public int id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title Required")]
        public string title { get; set; }

        [Display(Name = "Author")]
        public string author { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Genre")]
        public string genre { get; set; }

        public string createTime { get; set; }
    }

    public class StoryReposPath
    {
        public string allDataPath { get; set; }
        public string allStoryXml { get; set; }
        public string reposPath { get; set; }
        public string relativePath { get; set; }
        public string allCollageXml { get; set; }
        public string zipResultPath { get; set; }
        public string achivePath { get; set; }
        public string AllAchiveXml { get; set; }

        public StoryReposPath(){
            allDataPath = HttpContext.Current.Server.MapPath("~/StoryDataBase/");
            allStoryXml = allDataPath + "AllStories.xml";
            allCollageXml = allDataPath + "AllCollages.xml";
            reposPath = allDataPath + "StoryRepos"+"\\";
            relativePath = "~/StoryDataBase/StoryRepos/";
            zipResultPath = allDataPath + "DownloadZone\\";
            achivePath = allDataPath + "AchiveZone\\";
            AllAchiveXml = allDataPath + "AllAchives.xml";
        }
    }

    public class BlockModels
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Caption Required")]
        public string caption { get; set; }
        [Required(ErrorMessage = "Please upload your image")]
        public string imgLink { get; set; }
        public string text { get; set; }
        public int storyId { get; set; }
    }

    public class ImagesModels
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string description { get; set; }
    }

    public class CollagesModels
    {
        public int id { get; set; }
        public string caption { get; set; }
        public string name { get; set; }
        public int storyId { get; set; }
        public List<BlockModels> blocksList { get; set; }
    }

    public class AvailBlockModels
    {
        public List<BlockModels> usedBlockList { get; set; }
        public List<BlockModels> totalBlockList { get; set; }
        public List<BlockModels> availBlockList { get; set; }
    }

    public class AchiveModels
    {
        public int id { get; set; }
        public int storyId { get; set; }
        public bool isAchive { get; set; }
    }
}