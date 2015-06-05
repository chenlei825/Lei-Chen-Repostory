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
    public class CollageRepository
    {
        private List<CollagesModels> allcollList;
        private XDocument xDoc;
        private string collageXml;
        private StoryReposPath pathModels = new StoryReposPath();
        private StoryReposPath reposPath= new StoryReposPath();
        private XmlTextWriter tw = null;

        public CollageRepository(int storyId)
        {
            try
            {
                collageXml = pathModels.reposPath + storyId.ToString() + "\\" + "Collages.xml";
                if (!File.Exists(collageXml))
                {
                    tw = new XmlTextWriter(collageXml, null);
                    tw.Formatting = Formatting.Indented;
                    tw.WriteStartDocument();
                    tw.WriteStartElement("collages");
                    tw.WriteFullEndElement();
                    tw.WriteEndDocument();
                    tw.Flush();
                    tw.Close();
                }

                xDoc = XDocument.Load(collageXml);
                allcollList = new List<CollagesModels>();

                var q = from x in xDoc.Elements("collages")
                            .Elements("collage")
                        select x;
                foreach (var eachColl in q)
                {
                    CollagesModels cm = new CollagesModels();
                    cm.id = (int)eachColl.Element("id");
                    cm.storyId = (int)eachColl.Element("storyId");
                    cm.name = eachColl.Element("name").Value;
                    cm.caption = eachColl.Element("caption").Value;
                    var block = from c in eachColl.Element("blocks")
                                .Elements("block")
                                where c.Value!=""
                                 select c;
                    

                    foreach (var b in block)
                    {
                        if (b.IsEmpty)
                        {
                            BlockModels bm = new BlockModels();
                            bm.storyId = (int)b.Element("storyId");
                            bm.id = (int)b.Element("id");
                            cm.blocksList.Add(bm);
                        }
                    }
                    allcollList.Add(cm);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<CollagesModels> GetCollList()
        {
            return allcollList;
        }

        public CollagesModels GetCollById(int id)
        {
            return allcollList.Find(item => item.id == id);
        }

        public List<BlockModels> GetCollBlockList(int id)
        {
            XElement node = xDoc.Root.Elements("collage").Where(i => (int)i.Element("id") == id).FirstOrDefault();
            List<BlockModels> usedBlockList = new List<BlockModels>();
            var block = from c in node.Element("blocks")
                             .Elements("block")
                        where c.Value != ""
                        select c;


            foreach (var b in block)
            {
                if (!b.IsEmpty)
                {
                    BlockModels bm = new BlockModels();
                    bm.storyId = (int)b.Element("storyId");
                    bm.id = (int)b.Element("id");
                    usedBlockList.Add(bm);
                }
            }
            return usedBlockList;
        }

        public CollagesModels EditCollModel(CollagesModels coll)
        {
            try
            {
                XElement node = xDoc.Root.Elements("collage").Where(i => (int)i.Element("id") == coll.id).FirstOrDefault();
                node.SetElementValue("id", coll.id);
                node.SetElementValue("name", coll.name);
                node.SetElementValue("caption", coll.caption);

                ////Remove all the blocks child nodes
                //node.Elements("blocks").Elements().ToList().Remove();

                //List<BlockModels> blockList = coll.blocksList;

                //foreach (BlockModels bm in blockList)
                //{
                //    XElement block = new XElement("block");
                //    block.Add(new XElement("id", bm.id),
                //        new XElement("storyId", bm.storyId));
                //    node.Element("blocks").Add(block);
                //}

                xDoc.Save(collageXml);
                return coll;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
 

        public CollagesModels CreateCollModel(CollagesModels coll)
        {
            coll.id = (int)(from S in xDoc.Descendants("collage") 
                             orderby (int)S.Element("id") 
                             descending select (int)S.Element("id")).FirstOrDefault() + 1;

            //List<BlockModels> blockList = coll.blocksList;
            //XElement block = new XElement("block");
            //foreach (BlockModels bm in blockList)
            //{
            //    block.Add(new XElement("id", bm.id),
            //        new XElement("storyId", bm.storyId));
            //}
                xDoc.Root.Add(new XElement("collage",
                 new XElement("id", coll.id),
                 new XElement("storyId", coll.storyId),
                 new XElement("name", coll.name),
                 new XElement("caption", coll.caption),
                 new XElement("blocks",null)));
                xDoc.Save(collageXml);
                return coll;
        }

        public void DeleteCollModel(int id)
        {
            try
            {
                xDoc.Root.Elements("collage").Where(i => (int)i.Element("id") == id).Remove();
                xDoc.Save(collageXml);
            }

            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateBlockList(int id,List<BlockModels> usedBlockList)
        {
            try
            {
                XElement node = xDoc.Root.Elements("collage").Where(i => (int)i.Element("id") == id).FirstOrDefault();

                //Remove all the blocks child nodes
                node.Elements("blocks").Elements().ToList().Remove();

               // List<BlockModels> blockList=coll.blocksList;

                foreach (BlockModels bm in usedBlockList)
                {
                  XElement block= new XElement("block");
                  block.Add(new XElement("id",bm.id),
                      new XElement("storyId",bm.storyId));
                   node.Element("blocks").Add(block);
              }

                xDoc.Save(collageXml);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}