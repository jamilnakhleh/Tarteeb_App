using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tarteeb_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string folderPath = "";

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void OrganizeFilesInFolder(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath);

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    string fileExtension = Path.GetExtension(filePath).ToLower();
                    string destinationFolder = Path.Combine(folderPath, fileExtension.TrimStart('.'));

                    // creating the destination folder if it does NOT exist
                    Directory.CreateDirectory(destinationFolder);

                    // moving the file to the des folder
                    string newFilePath = Path.Combine(destinationFolder, fileName);
                    File.Move(filePath, newFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error organizing files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PopulateTreeView(string directory, TreeNodeCollection parentNode)
        {
            // Enumerate files
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                TreeNode node = new TreeNode(Path.GetFileName(file));
                parentNode.Add(node);
            }

            // Enumerate subdirectories
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                TreeNode node = new TreeNode(Path.GetFileName(subDirectory));
                parentNode.Add(node);

                // Recursively populate subdirectories
                PopulateTreeView(subDirectory, node.Nodes);
            }
        }

        static string ShortenFolderPath(string folderPath, int maxLength)
        {
            string[] pathParts = folderPath.Split('\\');
            int partsCount = pathParts.Length;

            if (partsCount > 0)
            {
                for (int i = 0; i < partsCount; i++)
                {
                    if (pathParts[i].Length > maxLength)
                    {
                        // Abbreviate directory name to maxLength characters
                        pathParts[i] = pathParts[i].Substring(0, maxLength);
                    }
                }

                // Reconstruct the shortened path
                string shortenedPath = string.Join("\\", pathParts);
                return shortenedPath;
            }
            else
            {
                // If the path is empty, return it as is
                return folderPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
          
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

           
            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            
            DialogResult result = folderBrowserDialog.ShowDialog();

           
            if (result == DialogResult.OK)
            {
          
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
 
                textBox1.Text =  selectedFolderPath;
                treeView_before.Nodes.Clear();
                treeView_after.Nodes.Clear();

                PopulateTreeView(textBox1.Text, treeView_before.Nodes);
            }
            else
            {
                textBox1.Text = "Error selection";
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void shortPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
       

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shortPathToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            folderPath = textBox1.Text;

            if (Directory.Exists(folderPath))
            {
                OrganizeFilesInFolder(folderPath);
                MessageBox.Show("The files organized successfully!", "Tarteeb_App", MessageBoxButtons.OK, MessageBoxIcon.Information);

                PopulateTreeView(folderPath, treeView_after.Nodes);
            }
            else
            {
                MessageBox.Show("Folder does NOT exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}