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

namespace MyStoryMaker.Controllers
{
    public class StoryBlockController : Controller
    {
        private StoryReposPath pathModels = new StoryReposPath();
        private StoryBlockRepository blockRepos;
       // private FileUpDownHelper fileUp = new FileUpDownHelper("http://localhost:50417/");

        // GET: /StoryBlock/
        public ActionResult Index(int id)
        {
            blockRepos = new StoryBlockRepository(id);
            IEnumerable<BlockModels> allBlockList = blockRepos.GetStoryBlocks();
            TempData["storyId"] = id; 
            return View(allBlockList.ToList());
        }

        // GET: /StoryBlock/Details/5
        public ActionResult Details(int id,int storyId)
        {
            blockRepos = new StoryBlockRepository(storyId);
            BlockModels block = blockRepos.GetBlockById(id);
            if (block == null)
            {
                return HttpNotFound();
            }
            return View(block);
        }

        // GET: /StoryBlock/Create
        [Authorize(Users = "devel,super")]
        public ActionResult Create(int storyId)
        {
            TempData["storyId"] = storyId; 
            return View();
        }

        // POST: /StoryBlock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "caption,text")] BlockModels block, HttpPostedFileBase file,int storyId)
        {
          
                try
                {
                    ImgRepository imgRepos = new ImgRepository(storyId);
                    StoryBlockRepository blockRepos = new StoryBlockRepository(storyId);

                    string imgName = "";
                    ImagesModels im = new ImagesModels();
              

                    if (file != null)
                    {
                        imgName = Path.GetFileName(file.FileName);
                        if (imgName != "")
                        {
                            var physicalPath = pathModels.reposPath + storyId.ToString() + "/" + file.FileName;
                            im.name = imgName;
                            im.link = storyId.ToString() + "/" + file.FileName;
                            imgRepos.InsertImgModel(im);
                            file.SaveAs(physicalPath);
                        }
                    }
                    block.imgLink = pathModels.relativePath + storyId.ToString() + "/" + file.FileName;
                    block.storyId = storyId;
                    blockRepos.InsertBlockModel(block);
                    //textRepos.InsertImgModel(story.text);
                    return RedirectToAction("Index", new { id = storyId });
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            

            return View(block);
        }

        // GET: /StoryBlock/Edit/5
        // [Authorize(Users = "devel,super")]
        public ActionResult Edit(int id,int storyId)
        {
            try
            {
                blockRepos = new StoryBlockRepository(storyId);
                BlockModels block = blockRepos.GetBlockById(id);
                //TempData["storyId"] = id;
                if (block == null)
                {
                    return HttpNotFound();
                }
                return View(block);
            }
            catch(Exception)
            {
                throw;
            }
        }

        // POST: /StoryBlock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,imgLink,storyId,caption,text")] BlockModels block, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int storyId = block.storyId;
                    ImgRepository imgRepos = new ImgRepository(storyId);
                    blockRepos = new StoryBlockRepository(storyId);
                    string imgName = "";
                    ImagesModels im = new ImagesModels();
                    if (file != null)
                    {
                        imgName = Path.GetFileName(file.FileName);
                        if (imgName != "")
                        {
                            var physicalPath = pathModels.reposPath + storyId.ToString() + "/" + file.FileName;
                            im.name = imgName;
                            im.link = pathModels.relativePath + storyId.ToString() + "/" + file.FileName;
                            imgRepos.InsertImgModel(im);
                            file.SaveAs(physicalPath);
                        }
                        block.imgLink = pathModels.relativePath + storyId.ToString() + "/" + file.FileName;
                    }
                    block.storyId = storyId;
                    blockRepos.EditBlockModel(block);
                    return RedirectToAction("Index", new { id = storyId });
                }


                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
              return View(block);
        }

        // GET: /StoryBlock/Delete/5
       //  [Authorize(Users = "devel,super")]
        public ActionResult Delete(int id, int storyId)
        {
            blockRepos = new StoryBlockRepository(storyId);
            BlockModels block = blockRepos.GetBlockById(id);
            if (block == null)
            {
                return HttpNotFound();
            }
            return View(block);
        }

        // POST: /StoryBlock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int storyId)
        {
            blockRepos = new StoryBlockRepository(storyId);
            blockRepos.DeleteImgModel(id);
            return RedirectToAction("Index", new { id = storyId });
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
