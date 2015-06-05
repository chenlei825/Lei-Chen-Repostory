using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using MyStoryMaker.Models;
using System.Xml;
using System.Xml.Linq;

namespace MyStoryMaker.Controllers
{
    public class ImgRepository
    {
        private List<ImagesModels> StoryImgList;
        private XDocument xDoc;
        private StoryReposPath pathModels= new StoryReposPath();
        private string eachStoryXml;
        private XmlTextWriter tw = null;

        public ImgRepository(int storyId)
        {
            try
            {
                eachStoryXml = pathModels.reposPath + storyId.ToString() + "\\" + "Img.xml";
                if (!File.Exists(eachStoryXml))
                {
                    tw = new XmlTextWriter(eachStoryXml, null);
                    tw.Formatting = Formatting.Indented;
                    tw.WriteStartDocument();
                    tw.WriteStartElement("Images");
                    tw.WriteFullEndElement();
                    tw.WriteEndDocument();
                    tw.Flush();
                    tw.Close();
                }

                xDoc = XDocument.Load(eachStoryXml);
                StoryImgList = new List<ImagesModels>();

                var q = from x in xDoc.Elements("images")
                            .Elements("image")
                        select x;
                foreach (var eachSImg in q)
                {
                    ImagesModels im = new ImagesModels();
                    im.id = (int)eachSImg.Element("id");
                    im.name = eachSImg.Element("name").Value;
                    im.link = eachSImg.Element("link").Value;
                    im.description = eachSImg.Element("description").Value;
                    StoryImgList.Add(im);
                }
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }


        public IEnumerable<ImagesModels> GetImgList()
        {
            return StoryImgList;
        }

        public ImagesModels GetImgById(int id)
        {
            return StoryImgList.Find(item => item.id == id);
        }

        public void InsertImgModel(ImagesModels image)
        {
            image.id = (int)(from S in xDoc.Descendants("image") 
                             orderby (int)S.Element("id") 
                             descending select (int)S.Element("id")).FirstOrDefault() + 1;

            xDoc.Root.Add(new XElement("image",
                 new XElement("id", image.id),
                 new XElement("name", image.name),
                 new XElement("link", image.link),
                 new XElement("descriptions", image.description)));
            xDoc.Save(eachStoryXml);
        }

        public void EditImgModel(ImagesModels image)
        {
            try
            {
                XElement node = xDoc.Root.Elements("image").Where(i => (int)i.Element("id") == image.id).FirstOrDefault();
                node.SetElementValue("name", image.name);
                node.SetElementValue("link", image.link);
                node.SetElementValue("descriptions", image.description);
                xDoc.Save(eachStoryXml);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteImgModel(int id)
        {
            try
            {
                xDoc.Root.Elements("image").Where(i => (int)i.Element("id")== id).Remove();
                xDoc.Save(eachStoryXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}