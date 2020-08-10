using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftClassDeobfuscator
{
    public partial class Mainfrm : Form
    {
        public Mainfrm()
        {
            InitializeComponent();
        }
        List<string> classes = new List<string>();
        List<string> methods = System.IO.File.ReadLines("method.txt").ToList();
        List<string> fields = System.IO.File.ReadLines("field.txt").ToList();
        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    classes = DirSearch(fbd.SelectedPath);
                    label3.Text = "Path: " + fbd.SelectedPath;
                    foreach (string classe in classes)
                    {
                        if(Path.GetExtension(classe) == ".java")
                            classList.Items.Add(classe.Replace(fbd.SelectedPath, ""));
                    }
                    label2.Text = "Number of classes: " + classes.Count();
                }
            }
        }
        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string classe in classes)
            {
                if (Path.GetExtension(classe) == ".java")
                {
                    string str = File.ReadAllText(classe);
                    foreach (string method in methods)
                    {
                        string[] use = method.Split(';');
                        str = str.Replace(use[0], use[1]);
                    }
                    foreach (string field in fields)
                    {
                        string[] use = field.Split(';');
                        str = str.Replace(use[0], use[1]);
                    }
                    File.WriteAllText(classe, str);
                }
            }
            MessageBox.Show("Done !");
        }
    }
}
