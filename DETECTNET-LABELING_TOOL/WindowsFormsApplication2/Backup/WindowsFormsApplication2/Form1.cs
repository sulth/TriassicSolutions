using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// ///////////////////////////
        /// </summary>

        Rectangle rect = new Rectangle(125, 125, 50, 50);
        private int x1, y1, x2, y2;
        bool isMouseDown = false;
/// <summary>
/// ///////////////
/// </summary>
        private int i=0;
        private float m_x, m_y = 0.0F;
        private string iFilename;
        private string strfile;
        public Form1()
        {
            InitializeComponent();
            strfile = ConfigurationManager.AppSettings["files"];
            comboBox1.Items.Add("DontCare");
            comboBox1.Items.Add("Car");
            comboBox1.Select();
            iFilename = i.ToString("000000");
            this.pictureBox1.Load(strfile + iFilename + ".png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = 1;
            for (index = 1; index < 7444; index++)
            {

                if (index > 7443) return;
                string strFilename = index.ToString("000000");
                System.IO.StreamReader myfile = new System.IO.StreamReader(@"E:\test2\filesready\" + strFilename + ".txt");
                string fromFile;

                do
                {
                    fromFile = myfile.ReadLine();
                    if (fromFile != null)
                    {
                        string[] payInfo = fromFile.Split(' ');
                        string strDontCare = payInfo[0];
                        if(!String.Equals(strDontCare,"0"))
                        {
                            string text = "DontCare " + fromFile;
                            System.IO.File.WriteAllText(@"E:\test2\filesready\" + strFilename + ".txt", text);
                        }

                    }
                } while (fromFile != null);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {   
            /*
            if( i > 7443 ) return;
            Drawbox(e.X - 100, e.Y - 90);
            m_x = e.X;
            m_y = e.Y;
            */


        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            textBox1.Text = e.X.ToString();
            textBox2.Text = e.Y.ToString();
        }

        private void Drawbox(int x, int y)
        {
        


        // We first cast the "Image" property of the pbImage picture box control
        // into a Bitmap object.
        Bitmap pbImageBitmap = (Bitmap)(pictureBox1.Image);
        // Obtain a Graphics object from the Bitmap object.
        Graphics graphics = Graphics.FromImage((Image)pbImageBitmap);

        Pen whitePen = new Pen(Color.White, 1);
        // Show the coordinates of the mouse click on the label, label1.
       
        Rectangle rect = new Rectangle(x, y, 200, 180);
        
        // Draw the rectangle, starting with the given coordinates, on the picture box.
        graphics.DrawRectangle(whitePen, rect);

        // Refresh the picture box control in order that
        // our graphics operation can be rendered.
        pictureBox1.Refresh();

        // Calling Dispose() is like calling the destructor of the respective object.
        // Dispose() clears all resources associated with the object, but the object still remains in memory
        // until the system garbage-collects it.
        graphics.Dispose();
        }

        private void Renamefiles(int def,int start,int end)
        {
            FileRenamer(@"D:\C++ Beginner's Guide", "Module", "PDF");

        }
        static void FileRenamer(string source, string search, string replace)
        {
            string[] files = System.IO.Directory.GetFiles(source);

            foreach (string filepath in files)
            {
                int fileindex = filepath.LastIndexOf('\\');
                string filename = filepath.Substring(fileindex+1);

                int startIndex = filename.IndexOf(search);
                if (startIndex != 0)
                    continue;
                int endIndex = startIndex + search.Length;


                System.IO.File.Move(filepath, source + @"\" + replace +@"\"+ filename);
            }

        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            int lastcount = 6037;
            for (int j = 81; j < 83;j++ )
            {
                //move to each folder 11 to 84
                //keep folder name 
                string strfolder = j.ToString();
                //get number of files
                var list = System.IO.Directory.GetFiles(@"E:\test2\" + strfolder, "*.png");
                int nfilecount = list.Length;
                int i;
                for ( i = 1; i < nfilecount + 1; i++)
                {
                    string source = @"E:\test2\" + strfolder + @"\" + strfolder + " (" + i.ToString() + ").png";
                    System.IO.File.Move(source, @"E:\test2\" + (i + lastcount).ToString("000000") + ".png");
                }
                lastcount = (i+lastcount-1);

            }
 
              
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            string text = "Car 0.0 0 0.0 " + (m_x - 100).ToString("0.0") + " " + (m_y - 90).ToString("0.0") + " " + (m_x + 100).ToString("0.0") + " " + (m_y + 90).ToString("0.0") + " 0.0 0.0 0.0 0.0 0.0 0.0 0.0";
                  */
            string text = "Car 0.0 0 0.0 " + x1.ToString("0.0") + " " + y1.ToString("0.0") + " " + (x2-x1).ToString("0.0") + " " + (y2-y1).ToString("0.0") + " 0.0 0.0 0.0 0.0 0.0 0.0 0.0";

            
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(strfile + iFilename + ".txt", text);
            i++;
            if (i > 7443) return;
            iFilename = i.ToString("000000");
            pictureBox1.Invalidate();
            textBox3.Text = strfile + iFilename + ".png";

            this.pictureBox1.Load(strfile + iFilename + ".png");

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!isMouseDown)
            {
                Refresh(); 
                return;
            }
            e.Graphics.DrawRectangle(new Pen(Color.RoyalBlue), rect);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            x1 = e.X;
            y1 = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                x2 = e.X;
                y2 = e.Y;
                rect.X = x1;// pictureBox1.Width - rect.Width;
                rect.Y = y1;
                rect.Width = x2 - x1;
                rect.Height = y2 - y1;
             
                Refresh();
            }
        }


    }
}
