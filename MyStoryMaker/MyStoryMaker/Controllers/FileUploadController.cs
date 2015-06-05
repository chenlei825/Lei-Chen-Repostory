using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace MyStoryMaker.Controllers
{
    public class UploadFileController : ApiController
    {   
        // GET: /UploadFile/
        public string storyDir {get;set;}
        public async Task<List<string>> PostAsync()
        {

            if (Request.Content.IsMimeMultipartContent())
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/Content/StoryRepos");
                
                MyStreamProvider streamProvider = new MyStreamProvider(uploadPath);
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                List<string> messages = new List<string>();
                uploadPath = uploadPath + "\\" + DateTime.Now.ToString("yyyymmddhhmm");
                storyDir = uploadPath;
                string storyName = "";

                foreach (var key in streamProvider.FormData.AllKeys)
                {
                    storyName = key.ToString();
                    storyDir = uploadPath + "_" + storyName + "\\";
                    Directory.CreateDirectory(storyDir);
                    foreach (var val in streamProvider.FormData.GetValues(key))
                    {
                        string textPath = storyDir + storyName + ".txt";
                        using (StreamWriter _testData = new StreamWriter(textPath, true))
                        {
                            _testData.WriteLine(val);
                            messages.Add("Story Text created successfully");
                        }
                    }
                }

                foreach (var file in streamProvider.FileData)
                {
                    
                    string imgPath = storyDir + file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    FileInfo fi = new FileInfo(file.LocalFileName);
                    fi.CopyTo(imgPath);
                    fi.Delete();
                }

                return messages;
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                throw new HttpResponseException(response);
            }
        }

        public class MyStreamProvider : MultipartFormDataStreamProvider
        {
            public string storyName="";
            public MyStreamProvider(string uploadPath)
                : base(uploadPath)
            {

            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {    
                string fileName = headers.ContentDisposition.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = Guid.NewGuid().ToString() + ".data";
                }
                storyName = fileName.Replace("\"", string.Empty);
                return storyName;
            }
        }

    }
}