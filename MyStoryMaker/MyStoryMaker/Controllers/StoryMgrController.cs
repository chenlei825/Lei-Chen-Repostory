using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyStoryMaker.Models;
using System.IO.Compression;

namespace MyStoryMaker.Controllers
{
   
    public class StoryMgrController : Controller
    {
        private StoryReposPath pathModels = new StoryReposPath();
        private StoryRepository storyRepos = new StoryRepository();
        private HtmlBuilder htmlBuilder = new HtmlBuilder();
        private AchiveRepository achiveRepos;

        //private FileUpDownHelper fileHelper = new FileUpDownHelper("http://localhost:50417/api/File");
        
        // GET: /StoriesMrg/
       [Authorize(Users="admin,devel,super")]
        public ActionResult Index()
        {
            achiveRepos = new AchiveRepository();
            List<StoryModels> allStoryList = storyRepos.GetStoryList().ToList();
            List<AchiveModels> allAchiveList = achiveRepos.GetAchiveList().ToList();
            List<StoryModels> nonAchiveLsit = new List<StoryModels>();
            if(allAchiveList.Count>0)
            {
                nonAchiveLsit = allStoryList.Where(p => !allAchiveList.Any(p2 => p2.storyId == p.id)).ToList();
            }

            else
            {
                nonAchiveLsit = allStoryList;
            }
            return View(nonAchiveLsit);
        }


        // GET: /StoriesMrg/Details/5
        public ActionResult Details(int id)
        {
            StoryModels story = storyRepos.GetStoryById(id);
            if (story == null)
            {
                return HttpNotFound();
            }

            return View(story);

        }

        // GET: /StoriesMrg/Create
        [HttpGet]
        [Authorize(Users="devel,super")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StoriesMrg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,author,description,genre")] StoryModels story)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    story = storyRepos.InsertStoryModel(story);
                    if (!Directory.Exists(pathModels.reposPath + story.id.ToString()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathModels.reposPath + story.id.ToString());
                    }

                    if (!Directory.Exists(pathModels.reposPath + story.id.ToString() + "\\Collage"))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathModels.reposPath + story.id.ToString() + "\\Collage");
                    }

                    ImgRepository imgRepos = new ImgRepository(story.id);
                    StoryBlockRepository blockRepos = new StoryBlockRepository(story.id);
                    return RedirectToAction("Index");
                }

                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }

            return View(story);
        }


        // GET: /StoriesMrg/Edit/5
        [HttpGet]
        [Authorize(Users = "devel,super")]
        public ActionResult Edit(int id)
        {

            try
            {
                StoryModels story = storyRepos.GetStoryById(id);
                if (story == null) { return HttpNotFound(); }
                return View(story);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: /StoriesMrg/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "id,title,author,description,genre")] StoryModels story)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    storyRepos.EditStoryModel(story);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    throw new NotImplementedException(); ;
                }
            }
            return View(story);

        }

        // GET: /StoriesMrg/Delete/5
        [Authorize(Users="admin,super")]
        public ActionResult Delete(int id)
        {
            StoryModels story = storyRepos.GetStoryById(id);
            if (story == null)
            {
                return RedirectToAction("Index");
            }
            return View(story);
        }

        // POST: /StoriesMrg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                storyRepos.DeleteStoryModel(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult FilePathDownload(int[] downItems)
        {
            IEnumerable<StoryModels> allStoryList = storyRepos.GetStoryList();
            StoryBlockRepository blockRepos;
            string target = pathModels.zipResultPath + "temp";
            if (downItems != null)
            {
                if (System.IO.Directory.Exists(target))
                {
                    System.IO.Directory.Delete(target,true); 
                }
                System.IO.Directory.CreateDirectory(target);
                foreach (int storyId in downItems)
                {
                    StoryModels story = new StoryModels();
                    blockRepos = new StoryBlockRepository(storyId);
                    story = allStoryList.ToList().Find(x => x.id == storyId);
                    string storyDir = pathModels.reposPath + story.id.ToString();
                    List<BlockModels> blockList = blockRepos.GetStoryBlocks().ToList();
                    htmlBuilder.Builder(story, blockList);
                    htmlBuilder.copyDirForZip(target+"\\"+story.title+"\\", storyDir);
                }

                string zipName = DateTime.Now.ToString("yyyymmddhhmm") + ".zip";
                string zipPath = htmlBuilder.zipBuilder(target, zipName);
                var name = Path.GetFileName(zipPath);
                return File(zipPath, "application/x-zip-compressed", name);
            }
            return RedirectToAction("Index");
        }


        // GET: /StoriesMrg/Delete/5
        [Authorize(Users = "admin,super")]
        public ActionResult Achive(int id)
        {
            StoryModels story = storyRepos.GetStoryById(id);
            if (story == null)
            {
                return RedirectToAction("Index");
            }
            return View(story);
        }

        // POST: /StoriesMrg/Delete/5
        [HttpPost, ActionName("Achive")]
        [ValidateAntiForgeryToken]
        public ActionResult AchiveConfirmed(int id)
        {
            try
            {
                achiveRepos = new AchiveRepository();
                AchiveModels achive = new AchiveModels();
                achive.storyId = id;
                achive.isAchive = true;
                achiveRepos.InsertAchiveModel(achive);
                string storyDir = pathModels.reposPath + id.ToString();
                FileInfo fInfo = new FileInfo(storyDir);
                string zipName = id.ToString() +  "_" + DateTime.Now.ToString("yyyymmddhhmm") + ".zip";
                ZipFile.CreateFromDirectory(@storyDir, pathModels.achivePath + zipName);
                //fInfo.MoveTo(pathModels.achivePath + zipName);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult AchiveRelief()
        {
            achiveRepos = new AchiveRepository();
            achiveRepos.DeleteAllAchiveModel();
            System.IO.DirectoryInfo achiveDirInfo = new DirectoryInfo(pathModels.achivePath);
            foreach (FileInfo file in achiveDirInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in achiveDirInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
