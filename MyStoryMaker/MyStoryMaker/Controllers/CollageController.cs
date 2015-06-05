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
using System.Text;

namespace MyStoryMaker.Controllers
{
   
    public class CollageController : Controller
    {
        private StoryReposPath pathModels = new StoryReposPath();
        private CollageRepository collRepos;
        private StoryBlockRepository blockRepos;

        // GET: /Collage/
        //id:StoryId
        [Authorize(Users = "devel,super")]
        public ActionResult Index(int id)
        {
            collRepos = new CollageRepository(id);
            blockRepos = new StoryBlockRepository(id);
            IEnumerable<CollagesModels> allCollist = collRepos.GetCollList();
            TempData["storyId"] = id;
            return View(allCollist.ToList());
        }

        // GET: /Collage/Details/5
        //id:collageId
        public ActionResult Details(int? id, int storyId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["storyId"] = storyId; 
            collRepos = new CollageRepository(storyId);
            CollagesModels coll = collRepos.GetCollById(id.Value);
            if (coll == null)
            {
                return HttpNotFound();
            }

            return View(coll);
        }

        // GET: /Collage/Create
         [Authorize(Users = "devel,super")]
        public ActionResult Create(int storyId)
        {
            TempData["storyId"] = storyId; 
            return View();
        }

        // POST: /Collage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "devel,super")]
        public ActionResult Create([Bind(Include="name,caption")] CollagesModels collage)
        {
            try
            {
                int storyId = (TempData["storyId"] as int?).GetValueOrDefault();
                collRepos = new CollageRepository(storyId);
                collage.storyId = storyId;
                List<BlockModels> blockList = new List<BlockModels>();
                collage.blocksList = blockList;
                collage = collRepos.CreateCollModel(collage); //get the coll id back;
                //HtmlBuilder builder = new HtmlBuilder(collage.name, collage.storyId,collage.caption);
                return RedirectToAction("Index", new { id = storyId });
            }
            catch(Exception)
            {
                throw new NotImplementedException();
            }
        }

        // GET: /Collage/Edit/5
         [Authorize(Users = "devel,super")]
        public ActionResult Edit(int? id,int storyId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                TempData["storyId"] = storyId;
                collRepos = new CollageRepository(storyId);
                CollagesModels coll = collRepos.GetCollById(id.Value);
                if(coll==null)
                {
                    return HttpNotFound();
                }
                return View(coll);
            }

            catch
            {
                throw;
            }

            //AvailBlockModels availBlock = new AvailBlockModels();
            //collRepos = new CollageRepository(storyId);
            //blockRepos = new StoryBlockRepository(storyId);


            //availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
            //availBlock.usedBlockList = collRepos.GetCollById(id.Value).blocksList;
            //if (availBlock.usedBlockList == null)
            //{
            //    availBlock.availBlockList = availBlock.totalBlockList;
            //}
            //else
            //{
            //    var result = availBlock.totalBlockList.Where(p => !availBlock.usedBlockList.Any(p2 => p2.id == p.id));
            //    availBlock.availBlockList = result.ToList();
            //}

            //return View(availBlock);
        }

        // POST: /Collage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,storyId,name,caption")] CollagesModels collage)
        {
            
            if (ModelState.IsValid)
            {
                int storyId = collage.storyId;
                collRepos = new CollageRepository(storyId);
                collRepos.EditCollModel(collage);
                return RedirectToAction("Index", new { id = storyId });
            }
            return View(collage);
        }

        // GET: /Collage/Delete/5
         [Authorize(Users = "devel,super")]
        public ActionResult Delete(int? id,int storyId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collRepos = new CollageRepository(storyId);
            TempData["storyId"] = storyId;
            CollagesModels collage = collRepos.GetCollById(id.Value);
            if (collage == null)
            {
                return HttpNotFound();
            }
            return View(collage);
        }

        // POST: /Collage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int storyId)
        {
            collRepos = new CollageRepository(storyId);
            collRepos.DeleteCollModel(id);
            return RedirectToAction("Index", new { id=storyId});
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
