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
    public class AchiveRepository
    {
        private List<AchiveModels> achiveList;
        private XDocument xDoc;
        private StoryReposPath reposPath= new StoryReposPath();

        public AchiveRepository()
        {
            try
            {
                xDoc = XDocument.Load(reposPath.AllAchiveXml);
                achiveList = new List<AchiveModels>();

                var q = from x in xDoc.Elements("achives")
                            .Elements("story")
                        select x;
                foreach (var eachAchive in q)
                {
                    AchiveModels achive = new AchiveModels();
                    achive.id = (int)eachAchive.Element("id");
                    achive.storyId = (int)eachAchive.Element("storyId");
                    achive.isAchive = (bool)eachAchive.Element("isAchive");
                    achiveList.Add(achive);
                }
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public IEnumerable<AchiveModels> GetAchiveList()
        {
            return achiveList;
        }

        public AchiveModels GetAchiveById(int storyId)
        {
            return achiveList.Find(item => item.storyId == storyId);
        }

        public AchiveModels InsertAchiveModel(AchiveModels achive)
        {
            achive.id = (int)(from S in xDoc.Descendants("story") 
                             orderby (int)S.Element("id") 
                             descending select (int)S.Element("id")).FirstOrDefault() + 1;

                xDoc.Root.Add(new XElement("story",
                 new XElement("id", achive.id),
                 new XElement("storyId", achive.storyId),
                 new XElement("isAchive", achive.isAchive)));
                xDoc.Save(reposPath.AllAchiveXml);
                return achive;
        }

        public void EditAchiveModel(AchiveModels story)
        {
            try
            {
                XElement node = xDoc.Root.Elements("story").Where(i => (int)i.Element("id") == story.id).FirstOrDefault();
                node.SetElementValue("storyId", story.storyId);
                node.SetElementValue("isAchive", story.isAchive);
                xDoc.Save(reposPath.AllAchiveXml);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteAchiveModel(int storyId)
        {
            try
            {
                xDoc.Root.Elements("story").Where(i => (int)i.Element("storyId") == storyId).Remove();
                xDoc.Save(reposPath.AllAchiveXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteAllAchiveModel()
        {
            try
            {
                xDoc.Root.RemoveAll();
                xDoc.Save(reposPath.AllAchiveXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}