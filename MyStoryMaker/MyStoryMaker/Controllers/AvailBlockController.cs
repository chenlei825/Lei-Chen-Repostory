using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStoryMaker.Models;

namespace MyStoryMaker.Controllers
{
    public class AvailBlockController : Controller
    {

        private StoryReposPath pathModels = new StoryReposPath();
        private StoryBlockRepository blockRepos;
        private CollageRepository collRepos;
        private AvailBlockModels availBlock = new AvailBlockModels();

        //
        // GET: /AvailBlock/
        public ActionResult Index(int id,int storyId)
        {
            TempData["collId"] = id;
            TempData["storyId"] = storyId;
            blockRepos = new StoryBlockRepository(storyId);
            collRepos = new CollageRepository(storyId);
            availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
            availBlock.usedBlockList = collRepos.GetCollBlockList(id);
            if (availBlock.usedBlockList == null)
            {
                availBlock.availBlockList = availBlock.totalBlockList;
            }
            else
            {
                for (int i = 0; i < availBlock.usedBlockList.Count;i++)
                {
                    BlockModels block = availBlock.usedBlockList[i];
                    block = availBlock.totalBlockList.Find(x => x.id == block.id);
                    availBlock.usedBlockList[i]=block;
                }
                var result = availBlock.totalBlockList.Where(p => !availBlock.usedBlockList.Any(p2 => p2.id == p.id));
                availBlock.availBlockList = result.ToList();
            }
            return View(availBlock);
        }

        //
        // GET: /AvailBlock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AvailBlock/Create
        //[Bind(Include = "title,author,description,genre")] StoryModels story)
        public ActionResult Create(int storyId,int collId)
        {
            TempData["storyId"] = storyId;
            TempData["collId"] = collId;
            blockRepos = new StoryBlockRepository(storyId);
            collRepos = new CollageRepository(storyId);

            availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
            availBlock.usedBlockList = collRepos.GetCollBlockList(collId);
            if (availBlock.usedBlockList == null)
            {
                availBlock.availBlockList = availBlock.totalBlockList;
            }
            else
            {
                for (int i = 0; i < availBlock.usedBlockList.Count; i++)
                {
                    BlockModels block = availBlock.usedBlockList[i];
                    block = availBlock.totalBlockList.Find(x => x.id == block.id);
                    availBlock.usedBlockList[i] = block;
                }
                var result = availBlock.totalBlockList.Where(p => !availBlock.usedBlockList.Any(p2 => p2.id == p.id));
                availBlock.availBlockList = result.ToList();
            }
            return View(availBlock);
        }

        //
        // POST: /AvailBlock/Create
        [HttpPost]
        public ActionResult Create(int[] AddedItems, int storyId, int collId)
        {
            try
            {
                // TODO: Add insert logic here
                blockRepos = new StoryBlockRepository(storyId);
                collRepos = new CollageRepository(storyId);
                availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
                availBlock.usedBlockList = collRepos.GetCollBlockList(collId);
                if (availBlock.usedBlockList == null)
                {
                    List<BlockModels> tempList = new List<BlockModels>();
                    availBlock.usedBlockList = tempList;
                }
                foreach(int blockId in AddedItems)
                {
                    BlockModels block = new BlockModels();
                    block = availBlock.totalBlockList.Find(x => x.id == blockId);
                    availBlock.usedBlockList.Add(block);
                }
                collRepos.UpdateBlockList(collId, availBlock.usedBlockList);
                return RedirectToAction("Index", new { id = collId,storyId=storyId });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AvailBlock/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /AvailBlock/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index",new { id = collId,storyId=storyId });
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /AvailBlock/Delete/5
        public ActionResult Delete(int collId,int storyId)
        {
            TempData["storyId"] = storyId;
            TempData["collId"] = collId;
            blockRepos = new StoryBlockRepository(storyId);
            collRepos = new CollageRepository(storyId);

            availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
            availBlock.usedBlockList = collRepos.GetCollBlockList(collId);
            if (availBlock.usedBlockList == null)
            {
                availBlock.availBlockList = availBlock.totalBlockList;
            }
            else
            {
                for (int i = 0; i < availBlock.usedBlockList.Count; i++)
                {
                    BlockModels block = availBlock.usedBlockList[i];
                    block = availBlock.totalBlockList.Find(x => x.id == block.id);
                    availBlock.usedBlockList[i] = block;
                }
            }
            return View(availBlock);
        }

        //
        // POST: /AvailBlock/Delete/5
        [HttpPost]
        public ActionResult Delete(int[] deletedItems,int collId,int storyId)
        {
            try
            {
                blockRepos = new StoryBlockRepository(storyId);
                collRepos = new CollageRepository(storyId);
                availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
                availBlock.usedBlockList = collRepos.GetCollBlockList(collId);
                if (availBlock.usedBlockList == null)
                {
                    List<BlockModels> tempList = new List<BlockModels>();
                    availBlock.usedBlockList = tempList;
                }
                else{
                    for (int i = 0; i < availBlock.usedBlockList.Count; i++)
                    {
                        BlockModels block = availBlock.usedBlockList[i];
                        block = availBlock.totalBlockList.Find(x => x.id == block.id);
                        availBlock.usedBlockList[i] = block;
                    }
                }
                foreach (int blockId in deletedItems)
                {
                    BlockModels block = new BlockModels();
                    block = availBlock.totalBlockList.Find(x => x.id == blockId);
                    availBlock.usedBlockList.Remove(block);
                }

                collRepos.UpdateBlockList(collId, availBlock.usedBlockList);
                return RedirectToAction("Index", new { id = collId, storyId = storyId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult TimeLineView(int storyId,int id)
        {

            // Get each collage available blocks
            
            TempData["storyId"] = storyId;
            blockRepos = new StoryBlockRepository(storyId);
            collRepos = new CollageRepository(storyId);
            availBlock.totalBlockList = blockRepos.GetStoryBlocks().ToList();
            availBlock.usedBlockList = collRepos.GetCollBlockList(id);
            if (availBlock.usedBlockList == null)
            {
                availBlock.availBlockList = availBlock.totalBlockList;
            }
            else
            {
                for (int i = 0; i < availBlock.usedBlockList.Count; i++)
                {
                    BlockModels block = availBlock.usedBlockList[i];
                    block = availBlock.totalBlockList.Find(x => x.id == block.id);
                    availBlock.usedBlockList[i] = block;
                }
                var result = availBlock.totalBlockList.Where(p => !availBlock.usedBlockList.Any(p2 => p2.id == p.id));
                availBlock.availBlockList = result.ToList();
            }

            //// Get each story's collages
            //List<CollagesModels> collList = new List<CollagesModels>();
            //collList = collRepos.GetCollList().ToList();
         
            //        TempData["id"] = id;
               
            

            return View(availBlock);
        }

    }
}
