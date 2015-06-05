using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;

//using System.Windows.Forms;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Client.Client tc = new Client.Client("http://localhost:50417/Api/File");

        public MainWindow()
        {
            InitializeComponent();
           // logout.IsEnabled = false;
            //getStory.IsEnabled = false;
            //download.IsEnabled = false;
            //browse.IsEnabled = false;
            //upload.IsEnabled = false;
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.InitialDirectory = Directory.GetCurrentDirectory();
            op.RestoreDirectory = true;
            op.Filter = "JPG Files(*.jpg)|*.jpg|All Files(*.*)|*.*";

            // Show open file dialog box 
            Nullable<bool> result = op.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = System.IO.Path.GetFileName(op.FileName);
                filespec.Text = op.FileName;
            }
        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            string path = filespec.Text;
            string ImgCaption = caption.Text;
            string blockText = bText.Text;
            string storyName = uploadStoryId.SelectedItem.ToString();
            if (storyName != null)
            {
                Thread thread = new Thread(() => uploadFiles(path, ImgCaption, blockText, storyName));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Please Select Story");
            }
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
           string fileName = listOfStory.SelectedItem.ToString();
           string zipName = fileName +  "_" +DateTime.Now.ToString("yyyymmddhhmm") + ".zip";
           Thread thread = new Thread(() => downloadFiles(zipName));
           thread.Start();
        }

        private void getStory_Click(object sender, RoutedEventArgs e)
        {
            listOfStory.Items.Clear();
            uploadStoryId.Items.Clear();
            Thread thread = new Thread(() => getServerFiles());
            thread.Start();
        }

        void getServerFiles()
        {
            string[] files = null;
            for (int i = 0; i < 10; ++i)
            {
                
                Thread.Sleep(100);
                try
                {
                    files = tc.getAvailableStories();

                    if (!Dispatcher.CheckAccess())
                    {
                        Dispatcher.Invoke(() =>
                        {
                            foreach (string file in files)
                            {
                                listOfStory.Items.Add(file);
                                uploadStoryId.Items.Add(file);
                            }
                        }
                        );
                    }
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        public void uploadFiles(string imgPath, string ImgCaption, string blockText,string storyName)
        {

             string fileName = System.IO.Path.GetFileName(imgPath);

            //tc.openServerUpLoadFile(imgPath, ImgCaption, blockText);
             tc.openServerUpLoadFile(fileName, ImgCaption, blockText, storyName);
             FileStream up = tc.openClientUpLoadFile(imgPath);

            const int upBlockSize = 512;
            byte[] upBlock = new byte[upBlockSize];
            int bytesRead = upBlockSize;
            while (bytesRead == upBlockSize)
            {
                bytesRead = up.Read(upBlock, 0, upBlockSize);
                if (bytesRead < upBlockSize)
                {
                    byte[] temp = new byte[bytesRead];
                    for (int i = 0; i < bytesRead; ++i)
                        temp[i] = upBlock[i];
                    upBlock = temp;
                }
               
                tc.putBlock(upBlock);

            }
            tc.closeServerFile();
            up.Close();
            MessageBox.Show("The story block has been uploading successfully");
            //filespec.Clear();
            //caption.Clear();
            //bText.Clear();
        }

        public void downloadFiles(string fileName)
        {
            try
            {
                FileStream down;
                int status = tc.openServerDownLoadFile(fileName);
                if (status >= 400) return;
                down = tc.openClientDownLoadFile(fileName);
                while (true)
                {
                    int blockSize = 512;
                    byte[] Block = tc.getFileBlock(down, blockSize);
                    if (Block.Length == 0 || blockSize <= 0)
                        break;
                    down.Write(Block, 0, Block.Length);
                    if (Block.Length < blockSize)    // last block
                        break;
                }
                tc.closeServerFile();
                down.Close();
                MessageBox.Show("Download File" + fileName + "Successfully");
            }
            catch(IOException)
            {
               MessageBox.Show("IOException happens");
            }
            
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_username.Text.Length > 0 && textbox_password.Text.Length > 0)
            {
                string username = textbox_username.Text;
                string password = textbox_password.Text;
                Thread thread = new Thread(() => trylogin(username, password));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Please enter username and password first!");
            }
        }

        private void trylogin(string username, string password)
        {
            int status = tc.logIn(username, password);
            if (status == 200)
            {
                MessageBox.Show("Logged in as administrator!");
                Dispatcher.Invoke(() =>
                {
                    getStory.IsEnabled = true;
                    download.IsEnabled = true;
                    browse.IsEnabled = true;
                    upload.IsEnabled = true;
                    logout.IsEnabled = true;
                    login.IsEnabled = false;
                }
                );
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            logout.IsEnabled = false;
            getStory.IsEnabled = false;
            download.IsEnabled = false;
            browse.IsEnabled = false;
            upload.IsEnabled = false;
            login.IsEnabled = true;
        }
    }
}
