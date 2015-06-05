using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStoryMaker.Models;
using System.Xml.Linq;
using System.Xml;

namespace MyStoryMaker.Controllers
{
    public class StoryBlockRepository
    {
        private List<BlockModels> StoryBlockList;
        private XDocument xDoc;
        private StoryReposPath pathModels = new StoryReposPath();
        private string eachStoryXml;
        private XmlTextWriter tw = null;

        public StoryBlockRepository(int storyId)
        {
            try
            {
                eachStoryXml = pathModels.reposPath + storyId.ToString() + "\\" + "Block.xml";
                if (!File.Exists(eachStoryXml))
                {
                    tw = new XmlTextWriter(eachStoryXml, null);
                    tw.Formatting = Formatting.Indented;
                    tw.WriteStartDocument();
                    tw.WriteStartElement("story");
                    tw.WriteFullEndElement();
                    tw.WriteEndDocument();
                    tw.Flush();
                    tw.Close();
                }
                xDoc = XDocument.Load(eachStoryXml);
                StoryBlockList = new List<BlockModels>();

                var q = from x in xDoc.Elements("story")
                        .Elements("block")
                        select x;
                foreach (var block in q)
                {
                    BlockModels sb = new BlockModels();
                    sb.id = (int)block.Element("id");
                    sb.caption = block.Element("caption").Value;
                    sb.imgLink =block.Element("image").Value;
                    sb.text = block.Element("text").Value;
                    sb.storyId = (int)block.Element("storyId");
                    StoryBlockList.Add(sb);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }


        public IEnumerable<BlockModels> GetStoryBlocks()
        {
            return StoryBlockList;
        }

        public BlockModels GetBlockById(int id)
        {
            return StoryBlockList.Find(item => item.id == id);
        }


        public void InsertBlockModel(BlockModels block)
        {
            block.id = (int)(from S in xDoc.Descendants("block")
                             orderby (int)S.Element("id")
                             descending
                             select (int)S.Element("id")).FirstOrDefault() + 1;

            xDoc.Root.Add(new XElement("block",
                 new XElement("id", block.id),
                 new XElement("storyId", block.storyId),
                 new XElement("caption", block.caption),
                 new XElement("image", block.imgLink),
                 new XElement("text", block.text)));
            xDoc.Save(eachStoryXml);
        }

        public void EditBlockModel(BlockModels block)
        {
            try
            {
                XElement node = xDoc.Root.Elements("block").Where(i => (int)i.Element("id") == block.id).FirstOrDefault();

                node.SetElementValue("caption", block.caption);
                node.SetElementValue("image", block.imgLink);
                node.SetElementValue("text", block.text);
                node.SetElementValue("storyId", block.storyId);
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
                xDoc.Root.Elements("block").Where(i => (int)i.Element("id") == id).Remove();
                xDoc.Save(eachStoryXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}