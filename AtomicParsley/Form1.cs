using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HundredMilesSoftware.UltraID3Lib;
using System.Drawing.Imaging;
using Transitions;
namespace AtomicParsley
{
    public partial class Form1 : Form
    {
        //Width : 667 : added
        public Form1()
        {
            InitializeComponent();
        }

        //~~~~~~~~~~~Globals
        string[] fileTypes;
        string lastSelected;

        //~~~~~~~~~~~~~~~~~~~~DRAG MOVE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    private void frmDrag_Paint(object sender, PaintEventArgs e)
        {
            //Draws a border to make the Form stand out
            //Just done for appearance, not necessary

            Pen p = new Pen(Color.Gray, 3);
            e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            p.Dispose();
        }

        Point lastClick; //Holds where the Form was clicked

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Point newLocation = new Point(e.X - lastE.X, e.Y - lastE.Y);
            if (e.Button == MouseButtons.Left) //Only when mouse is clicked
            {
                //Move the Form the same difference the mouse cursor moved;
                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = new Point(e.X, e.Y); //We'll need this for when the Form starts to move
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            //Point newLocation = new Point(e.X - lastE.X, e.Y - lastE.Y);
            if (e.Button == MouseButtons.Left) //Only when mouse is clicked
            {
                //Move the Form the same difference the mouse cursor moved;
                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = new Point(e.X, e.Y); //We'll need this for when the Form starts to move
        }
        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            //Point newLocation = new Point(e.X - lastE.X, e.Y - lastE.Y);
            if (e.Button == MouseButtons.Left) //Only when mouse is clicked
            {
                //Move the Form the same difference the mouse cursor moved;
                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }
        }

        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = new Point(e.X, e.Y); //We'll need this for when the Form starts to move
        }


        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            //Point newLocation = new Point(e.X - lastE.X, e.Y - lastE.Y);
            if (e.Button == MouseButtons.Left) //Only when mouse is clicked
            {
                //Move the Form the same difference the mouse cursor moved;
                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = new Point(e.X, e.Y); //We'll need this for when the Form starts to move
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            panel4.Location = new Point(-100, 61);
            label5.Location = new Point(300, 65);
            listBox1.Location = new Point(listBox1.Location.X , listBox1.Location.Y -20);
            Transition t1 = new Transition(new TransitionType_EaseInEaseOut(400));
            t1.add(panel4, "Left", 123);

            Transition t2 = new Transition(new TransitionType_EaseInEaseOut(400));
            t2.add(label5, "Left", 124);

            Transition t3 = new Transition(new TransitionType_EaseInEaseOut(600));
            t3.add(listBox1, "Top", listBox1.Location.Y + 20);
            t3.run();

            Transition.runChain(t1, t2);

            //GET valid types
            StreamReader reader = new StreamReader("types.ini");
            string temp = reader.ReadToEnd();
            fileTypes = temp.Split('\n');
            reader.Close();
            
            
            //Analyze();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex > -1)
            {
                label1.Text = listBox2.Items[listBox1.SelectedIndex].ToString();
                if (Path.GetExtension(label1.Text).Equals(".m4a"))
                {
                    //string tempFile = Path.GetPathRoot(label1.Text) + "test" + Path.GetExtension(label1.Text);
                    //if (File.Exists(tempFile))
                        //File.Delete(tempFile);

                    //File.Move(label1.Text, tempFile);

                    //StreamWriter test = new StreamWriter("read.bat");
                    //test.Write("ap.exe \"" + tempFile + "\" -t -E > read.txt");
                    //test.Close();

                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = "ap.exe";
                    p.StartInfo.Arguments = "\"" + label1.Text + "\"" + " -t -E > read.txt";
                    p.Start();
                    p.WaitForExit();
                   
                        StreamReader streamReader = new StreamReader("read.txt");
                        richTextBox1.Text = streamReader.ReadToEnd();
                        streamReader.Close();
                    

                    //File.Move(tempFile, label1.Text);

                    Analyze();
                }
                else
                {
                    timer2.Start();
                    UltraID3 ID3 = new UltraID3();
                    ID3.Read(label1.Text);
                    infoTitle.Text = ID3.Title;
                    infoArtist.Text = ID3.Artist;
                    infoAlbum.Text = ID3.Album;
                    infoGenre.Text = ID3.Genre;
                    infoTrackNo.Text = ID3.TrackNum.ToString();
                    infoTrackNoTotal.Text = ID3.TrackCount.ToString();
                }

                lastSelected = label1.Text;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
           // MessageBox.Show(listBox1.SelectedItem.ToString());
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                //listBox1.Items.Add(file);
                string path = file;
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    MessageBox.Show("{0} is not a valid file or directory.", path);
                }

            }
        }


        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~USER FUNCTION~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void readFile()
        {
            if (File.Exists("read.txt"))
            {
                StreamReader streamReader = new StreamReader("read.txt");
                richTextBox1.Text = streamReader.ReadToEnd();
                streamReader.Close();
            }
        }
        public void Analyze()

        {
            string[] lines = richTextBox1.Text.Split('\n');
            infoTitle.Text = "";
            infoArtist.Text = "";
            infoAlbum.Text = "";
            infoGenre.Text = "";
            infoAlbumArtist.Text = "";
            infoTrackNo.Text = "";
            infoTrackNoTotal.Text = "";
            infoDiscNo.Text = "";
            infoDiscNoTotal.Text = "";

            foreach (string line in lines)
            {
                string[] splitted = line.Split(':');
                if(line.Contains("©nam"))
                {
                    for(int c=1; c<splitted.Count(); c++)
                    infoTitle.AppendText(splitted[c]);
                }

                if (line.Contains("©ART"))
                {
                    for (int c = 1; c < splitted.Count(); c++)
                        infoArtist.AppendText(splitted[c]);
                }

                if (line.Contains("©alb"))
                {
                    for (int c = 1; c < splitted.Count(); c++)
                        infoAlbum.AppendText(splitted[c]);
                }


                if (line.Contains("©gen"))
                {
                    for (int c = 1; c < splitted.Count(); c++)
                        infoGenre.AppendText(splitted[c]);
                }

                if (line.Contains("aART"))
                {
                    for (int c = 1; c < splitted.Count(); c++)
                        infoAlbumArtist.AppendText(splitted[c]);
                }
                if (line.Contains("disk"))
                {
                    string temp = splitted[1].Remove(0,1);
                    string[] tracks = temp.Split(' ');
                 
                        infoDiscNo.Text = tracks[0];
                        if (tracks.Count() > 2)
                        infoDiscNoTotal.Text = tracks[2];
                }



            }

            if(infoTitle.Text.Length>0)
            infoTitle.Text = infoTitle.Text.Remove(0,1);
            if (infoArtist.Text.Length > 0)
            infoArtist.Text = infoArtist.Text.Remove(0, 1);
            if (infoAlbum.Text.Length > 0)
            infoAlbum.Text = infoAlbum.Text.Remove(0, 1);
            if (infoGenre.Text.Length > 0)
            infoGenre.Text = infoGenre.Text.Remove(0, 1);
            if (infoAlbumArtist.Text.Length > 0)
            infoAlbumArtist.Text = infoAlbumArtist.Text.Remove(0, 1);


            if (infoTitle.Text.Length > 0)
                infoTitle.Text = infoTitle.Text.Remove(infoTitle.Text.Length-1, 1);
            if (infoArtist.Text.Length > 0)
                infoArtist.Text = infoArtist.Text.Remove(infoArtist.Text.Length-1, 1);
            if (infoAlbum.Text.Length > 0)
                infoAlbum.Text = infoAlbum.Text.Remove(infoAlbum.Text.Length-1, 1);
            if (infoGenre.Text.Length > 0)
                infoGenre.Text = infoGenre.Text.Remove(infoGenre.Text.Length-1, 1);
            if (infoAlbumArtist.Text.Length > 0)
                infoAlbumArtist.Text = infoAlbumArtist.Text.Remove(infoAlbumArtist.Text.Length-1, 1);

            if (smartScan.Checked)
            {
                for (int c = 0; c < listBox3.Items.Count; c++)
                {
                    string track = listBox3.Items[c].ToString().Remove(0, 1);
                    track = track.Remove(track.Length - 1, 1);
                    if (listBox1.SelectedItem.ToString().Contains(track))
                    {
                        //MessageBox.Show(c.ToString());
                        string title = listBox4.Items[c].ToString().Remove(0, 1);
                        title = title.Remove(title.Length - 1, 1);
                        infoTitle.Text = title;
                        break;
                    }
                }
            }

            if (SAlbumAppend.Checked)
            {
                infoSAlbum.Text = textSAlbumAppend.Text + infoAlbum.Text;

            }

            string imageURL = Path.GetPathRoot(label1.Text) + "test_artwork_1.jpg";
            if (File.Exists(imageURL))
            {
                FileStream stream = new FileStream(imageURL, FileMode.Open, FileAccess.Read);
                //pictureBox1.Image = Image.FromStream(stream);
                if (Path.GetDirectoryName(lastSelected) != Path.GetDirectoryName(label1.Text))
                {
                    timer2.Start();
                    albumArt = Image.FromStream(stream);
                    opacity = 0;
                    timer1.Start();
                }
                stream.Close();
                File.Delete(imageURL);
            }

           
            
        }

        Image albumArt;
        //~~~~~~~~~~~~~~~~~~~~~~~PRE DEFINED FUNCTION~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here. 
        public void ProcessFile(string path)
        {
            bool valid = false;
            foreach (string type in fileTypes)
            {
                string type2 = type.Replace("\r", "");
                //MessageBox.Show(":"+ Path.GetExtension(path)+":" + (type2) +":");
                if (Path.GetExtension(path).Equals(type2))
                {
                    valid = true;
                    break;
                }
            }

            foreach (string s in listBox2.Items)
            {
                if (path.Equals(s))
                    valid = false;
            }

            if (valid == true)
            {
                if (listBox1.Items[0].Equals("Drag Files Here"))
                    listBox1.Items.RemoveAt(0);
                Transition.run(this, "Width", 667, new TransitionType_EaseInEaseOut(1000));
                button4.Visible = true;
                listBox1.Items.Add(Path.GetFileName(path)); 
                listBox2.Items.Add(path); 
            }

        }

   

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix { Matrix33 = opacityvalue };
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }
        float opacity;
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = ChangeOpacity(albumArt, opacity);
            opacity += .02F;
            if (opacity >= 1)
                timer1.Enabled = false; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(label1.Text).Equals(".m4a"))
            {
                string tempFile = Path.GetPathRoot(label1.Text) + "test" + Path.GetExtension(label1.Text);

                string argument = "\"" + tempFile + "\"";
                if (cTitle.Checked == true)
                    argument = argument + " --title \"" + infoTitle.Text + "\"";
                if (cArtist.Checked == true)
                    argument = argument + " --artist \"" + infoArtist.Text + "\"";
                if (cAlbum.Checked == true)
                    argument = argument + " --album \"" + infoAlbum.Text + "\"";
                if (cGenre.Checked == true)
                    argument = argument + " --genre \"" + infoGenre.Text + "\"";
                if (cAlbumArtist.Checked == true)
                    argument = argument + " --albumArtist \"" + infoAlbumArtist.Text + "\"";
                if (cSAlbum.Checked == true)
                    argument = argument + " --sortOrder album \"" + infoSAlbum.Text + "\"";
                if (cTrackNo.Checked == true)
                    if (infoTrackNoTotal.Text != "")
                        argument = argument + " --tracknum \"" + infoTrackNo.Text + "/" + infoTrackNoTotal.Text + "\"";
                    else
                        argument = argument + " --tracknum \"" + infoTrackNo.Text + "\"";
                if (cDiscNo.Checked == true)
                    if (infoDiscNoTotal.Text != "")
                        argument = argument + " --tracknum \"" + infoDiscNo.Text + "/" + infoDiscNoTotal.Text + "\"";
                    else
                        argument = argument + " --tracknum \"" + infoDiscNo.Text + "\"";
                argument = argument + " --overWrite";


                if (File.Exists(tempFile))
                    File.Delete(tempFile);

                File.Move(label1.Text, tempFile);



                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "ap.exe";
                p.StartInfo.Arguments = argument;
                p.Start();
                p.WaitForExit();

                File.Move(tempFile, label1.Text);
            }
            else
            {
                UltraID3 ID3 = new UltraID3();
                ID3.Read(label1.Text);
                if (cTitle.Checked == true)
                ID3.Title = infoTitle.Text;
                if (cArtist.Checked == true)
                ID3.Artist = infoArtist.Text;
                if (cAlbum.Checked == true)
                ID3.Album = infoAlbum.Text;
                //ID3.Genre = infoGenre.Text;
                if (cTrackNo.Checked == true)
                ID3.TrackNum = Int16.Parse(infoTrackNo.Text);
                ID3.Write();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            pictureBox1.Image = ChangeOpacity(pictureBox1.Image, opacity);
            opacity -= .02F;
            if (opacity <= 0)
                timer2.Enabled = false; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            string[] splitted = richTextBox2.Text.Split('\n');
            foreach (string s in splitted)
            {
                Int16 temp;
                if(Int16.TryParse(textBox1.Text,out temp))
                if (s.Length > Int16.Parse(textBox1.Text))
                {

                    listBox3.Items.Add("\"" + s.Substring(0, Int16.Parse(textBox1.Text)) + "\"");
                    string temp2 = s.Remove(0, Int16.Parse(textBox1.Text) + 1);
                    temp2 = temp2.Substring(0, temp2.Length - Int16.Parse(textBox2.Text));
                    listBox4.Items.Add("\"" + temp2 + "\"");

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox2.Items.Count>0)
            {
                for (int c = 0; c < listBox2.Items.Count; c++)
                {
                    listBox1.SetSelected(c, true);
                    button1.PerformClick();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int index = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(index);
                listBox2.Items.RemoveAt(index);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_MouseEnter(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.Silver;
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            linkLabel1.LinkColor = Color.White;
        }

        private void linkLabel2_MouseEnter(object sender, EventArgs e)
        {
            linkLabel2.LinkColor = Color.Silver;
        }

        private void linkLabel2_MouseLeave(object sender, EventArgs e)
        {
            linkLabel2.LinkColor = Color.White;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Transition t3 = new Transition(new TransitionType_EaseInEaseOut(600));
            t3.add(this, "Width", 1200);
            t3.run();
        }

    
        

    
       

    }
}
