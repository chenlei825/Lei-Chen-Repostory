using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using MyStoryMaker.Models;
using System.Xml.Linq;

namespace MyStoryMaker.Controllers
{
    public class StoryRepository
    {
        private List<StoryModels> allStoryList;
        private XDocument xDoc;
        private StoryReposPath reposPath= new StoryReposPath();

        public StoryRepository()
        {
            try
            { 
                xDoc = XDocument.Load(reposPath.allStoryXml);
                allStoryList = new List<StoryModels>();

                var q = from x in xDoc.Elements("stories")
                            .Elements("story")
                        select x;
                foreach (var eachStroy in q)
                {
                    StoryModels sm = new StoryModels();
                    sm.id = (int)eachStroy.Element("id");
                    sm.title = eachStroy.Element("title").Value;
                    sm.author = eachStroy.Element("author").Value;
                    sm.createTime = eachStroy.Element("createTime").Value;
                    sm.genre = eachStroy.Element("genre").Value;
                    sm.description = eachStroy.Element("description").Value;
                    allStoryList.Add(sm);
                }
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public IEnumerable<StoryModels> GetStoryList()
        {
            return allStoryList;
        }

        public StoryModels GetStoryById(int id)
        {
            return allStoryList.Find(item => item.id == id);
        }

        public StoryModels InsertStoryModel(StoryModels story)
        {
                story.id = (int)(from S in xDoc.Descendants("story") 
                             orderby (int)S.Element("id") 
                             descending select (int)S.Element("id")).FirstOrDefault() + 1;

                xDoc.Root.Add(new XElement("story",
                 new XElement("id",story.id),
                 new XElement("title", story.title),
                 new XElement("author", story.author),
                 new XElement("createTime", story.createTime),
                 new XElement("genre", story.genre),
                 new XElement("description", story.description)));
                xDoc.Save(reposPath.allStoryXml);
                return story;
        }

        public void EditStoryModel(StoryModels story)
        {
            try
            {
                XElement node = xDoc.Root.Elements("story").Where(i => (int)i.Element("id") == story.id).FirstOrDefault();
                node.SetElementValue("title", story.title);
                node.SetElementValue("author", story.author);
                node.SetElementValue("createTime", story.createTime);
                node.SetElementValue("genre", story.genre);
                node.SetElementValue("description", story.description);
                xDoc.Save(reposPath.allStoryXml);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteStoryModel(int id)
        {
            try
            {
                xDoc.Root.Elements("story").Where(i => (int)i.Element("id") == id).Remove();
                xDoc.Save(reposPath.allStoryXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}
    
