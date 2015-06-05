///////////////////////////////////////////////////////////////////////////
// FileController.cs - Demonstrates how to start a file handling service //
//                                                                       //
// Jim Fawcett, CSE686 - Internet Programming, Spring 2014               //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyStoryMaker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyStoryMaker.Controllers
{
    public class FileController : ApiController
    {
        //----< GET api/File - get list of available files >---------------

        private StoryReposPath pathRepos = new StoryReposPath();
        private StoryRepository storyRepos = new StoryRepository();
        private StoryReposPath pathModels = new StoryReposPath();
        private StoryBlockRepository blockRepos;
        private UserManager<ApplicationUser> check = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public IEnumerable<string> Get()
        {
            List<StoryModels> allStoryList = storyRepos.GetStoryList().ToList();
            List<string> allNames = new List<string>();
            foreach(StoryModels story in allStoryList)
            {
                allNames.Add(story.id.ToString() + "_" + story.title);
            }
            return allNames;
        }

        //----< GET api/File?fileName=foobar.txt&open=true >---------------
        //----< attempt to open or close FileStream >----------------------

        public HttpResponseMessage Get(string fileName, string open)
        {
            string sessionId;
            var response = new HttpResponseMessage();
            Models.Session session = new Models.Session();

            CookieHeaderValue cookie = Request.Headers.GetCookies("session-id").FirstOrDefault();
            if (cookie == null)
            {
                sessionId = session.incrSessionId();
                cookie = new CookieHeaderValue("session-id", sessionId);
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
            }
            else
            {
                sessionId = cookie["session-id"].Value;
            }
            try
            {
                FileStream fs;
                IEnumerable<StoryModels> allStoryList = storyRepos.GetStoryList();
                HtmlBuilder htmlBuilder = new HtmlBuilder();
                
                string[] fileInfo= fileName.Split('_');
                
                int storyId = Convert.ToInt32(fileInfo[0]);
                string storyTitile = fileInfo[1];


                if (open == "download")  // attempt to open requested fileName
                {
                    string dateNow = fileInfo[2];
                    StoryModels story = new StoryModels();
                    blockRepos = new StoryBlockRepository(storyId);
                    story = allStoryList.ToList().Find(x => x.id == storyId);
                    string storyDir = pathModels.reposPath + story.id.ToString();
                    List<BlockModels> blockList = blockRepos.GetStoryBlocks().ToList();
                    htmlBuilder.Builder(story, blockList);
                    string currentFileSpec = htmlBuilder.zipBuilder(storyDir,fileName);

                    fs = new FileStream(currentFileSpec, FileMode.Open);
                    session.saveStream(fs, sessionId);
                }
                
                else  // close FileStream
                {
                    fs = session.getStream(sessionId);
                    session.removeStream(sessionId);
                    fs.Close();
                }
                response.StatusCode = (HttpStatusCode)200;
            }
            catch
            {
                response.StatusCode = (HttpStatusCode)400;
            }
          finally  // return cookie to save current sessionId
            {
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            }
            return response;
        }


        public HttpResponseMessage Get(string fileName, string caption, string storyText, string storyName, string open)
        {
            string sessionId;
            var response = new HttpResponseMessage();
            Models.Session session = new Models.Session();

            CookieHeaderValue cookie = Request.Headers.GetCookies("session-id").FirstOrDefault();
            if (cookie == null)
            {
                sessionId = session.incrSessionId();
                cookie = new CookieHeaderValue("session-id", sessionId);
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
            }
            else
            {
                sessionId = cookie["session-id"].Value;
            }
            try
            {
                FileStream fs;
                IEnumerable<StoryModels> allStoryList = storyRepos.GetStoryList();
                HtmlBuilder htmlBuilder = new HtmlBuilder();

                string[] fileInfo = storyName.Split('_');

                int storyId = Convert.ToInt32(fileInfo[0]);
                string storyTitile = fileInfo[1];
                if (open == "upload")
                {
                    ImgRepository imgRepos = new ImgRepository(storyId);
                    StoryBlockRepository blockRepos = new StoryBlockRepository(storyId);
                    BlockModels block = new BlockModels();
                    ImagesModels im = new ImagesModels();
                    //path = path + "UpLoad";
                    string currentFileSpec = pathModels.reposPath + storyId.ToString() + "//" + fileName;
                    block.imgLink = pathModels.relativePath + storyId.ToString() + "//" + fileName;
                    block.storyId = storyId;
                    block.caption = caption;
                    block.text = storyText;
                    blockRepos.InsertBlockModel(block);

                    fs = new FileStream(currentFileSpec, FileMode.OpenOrCreate);
                    session.saveStream(fs, sessionId);
                }
                else  // close FileStream
                {
                    fs = session.getStream(sessionId);
                    session.removeStream(sessionId);
                    fs.Close();
                }
                response.StatusCode = (HttpStatusCode)200;
            }
            catch
            {
                response.StatusCode = (HttpStatusCode)400;
            }
          finally  // return cookie to save current sessionId
            {
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            }
            return response;
        }

        //----< GET api/File?blockSize=2048 - get a block of bytes >-------

        public HttpResponseMessage Get(int blockSize)
        {
            // get FileStream and read block

            Models.Session session = new Models.Session();
            CookieHeaderValue cookie = Request.Headers.GetCookies("session-id").FirstOrDefault();
            string sessionId = cookie["session-id"].Value;
            FileStream down = session.getStream(sessionId);
            byte[] Block = new byte[blockSize];
            int bytesRead = down.Read(Block, 0, blockSize);
            if (bytesRead < blockSize)  // compress block
            {
                byte[] returnBlock = new byte[bytesRead];
                for (int i = 0; i < bytesRead; ++i)
                    returnBlock[i] = Block[i];
                Block = returnBlock;
            }
            // make response message containing block and cookie

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            message.Content = new ByteArrayContent(Block);
            return message;
        }

        // POST api/file
        public HttpResponseMessage Post(int blockSize)
        {
            Task<byte[]> task = Request.Content.ReadAsByteArrayAsync();
            byte[] Block = task.Result;
            Models.Session session = new Models.Session();
            CookieHeaderValue cookie = Request.Headers.GetCookies("session-id").FirstOrDefault();
            string sessionId = cookie["session-id"].Value;
            FileStream up = session.getStream(sessionId);
            up.Write(Block, 0, Block.Count());
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return message;
        }

        // PUT api/file/5
        public HttpResponseMessage Put(string username, string password)
        {
            ApplicationUser user = check.Find(username, password);
            var response = new HttpResponseMessage();
            var store = new UserStore<MyStoryMaker.Models.ApplicationUser>(new ApplicationDbContext());
            var userMananger = new UserManager<ApplicationUser>(store);

            if (user != null)
            {
                if ((user.UserName=="devel") ||(user.UserName=="super"))
                {
                    response.StatusCode = (HttpStatusCode)200;
                }
                else
                {
                    response.StatusCode = (HttpStatusCode)400;
                }
            }
            else
            {
                response.StatusCode = (HttpStatusCode)400;
            }
            return response;
        }

        //// PUT api/file/5
        //public void Put(int id, [FromBody]string value)
        //{
        //    string debug = "debug";
        //}

        //// DELETE api/file/5
        //public void Delete(int id)
        //{
        //    string debug = "debug";
        //}
	}
}