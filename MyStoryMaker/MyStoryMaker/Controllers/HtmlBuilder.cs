using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MyStoryMaker.Models;
using System.Web.UI;

namespace MyStoryMaker.Controllers
{
    public class HtmlBuilder
    {
        private StoryReposPath pathModels = new StoryReposPath();
        //StringWriter stringWriter = null;


        public void Builder(StoryModels story, List<BlockModels> storyBlockList)
        {
            try
            {
                string htmlPath = pathModels.reposPath + story.id.ToString() + "\\" + story.title + ".html";

                if (File.Exists(htmlPath))
                {
                    File.Delete(htmlPath);
                }

                using (FileStream fs = new FileStream(htmlPath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("<H1>" + story.title + "</H1>");
                        string hStr = HTMLStrBuilder(storyBlockList);
                        w.WriteLine(hStr);
                        w.Close();
                    }
                }

            }
            catch (Exception)
            {
                ;
            }
        }



        public String HTMLStrBuilder(List<BlockModels> storyBlockList)
        {
            //Create a new StringBuilder object
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>  ");
            sb.AppendLine("    <head>");
            sb.AppendLine("        <title>Cinderella's Story</title>    ");
            sb.AppendLine(" <style>");
            sb.AppendLine("        .center {");
            sb.AppendLine("            margin-left: auto;");
            sb.AppendLine("            margin-right: auto;");
            sb.AppendLine("            width: 70%;");
            sb.AppendLine("        }");
            sb.AppendLine("        </style>");
            sb.AppendLine("    </head>");
            sb.AppendLine("    <body>");
            sb.AppendLine("        <div>");

            foreach (BlockModels block in storyBlockList)
            {
                string imgPath = block.imgLink;
                string imgName = imgPath.Substring(imgPath.LastIndexOf('/') + 1);
                sb.AppendLine("             <div>");
                sb.AppendLine("                <img src=\"" + imgName + "\">");
                sb.AppendLine("                <h4>" + block.text + "</h4>");
                sb.AppendLine("                </div>");
            }

            sb.AppendLine("            </div>");
            sb.AppendLine("    </body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }



        public string zipBuilder(string dir, string zipName)
        {
            string startPath = @dir;
            // string zipPath = @"c:\example\result.zip";
            string zipPath = pathModels.zipResultPath + zipName;

            ZipFile.CreateFromDirectory(startPath, zipPath);

            return zipPath;
        }

        public void copyDirForZip(string target,string source){

            if (!System.IO.Directory.Exists(target))
            {
                System.IO.Directory.CreateDirectory(target);
            }

          
            string[] files = System.IO.Directory.GetFiles(source);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                string fileName = System.IO.Path.GetFileName(s);
                string destFile = System.IO.Path.Combine(target, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
            
        }

    }
}