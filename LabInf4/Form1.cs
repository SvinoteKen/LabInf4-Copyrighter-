using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabInf4
{
    public partial class Form1 : Form
    {
        List<string> images = new List<string>();
        Bitmap panelBitmap;
        Bitmap selcetedImage;
        int selectedPicture = 0;
        string copyrightText = "";
        string directoryForSave = "";
        Font font = new Font("Times New Roman", 100);
        Brush brush = Brushes.Red;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "image files(*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP)|*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = null;
                images.Clear();
                selectedPicture = 0;
                panelBitmap = new Bitmap(panel1.Width, 90 + 15);
                panel1.AutoScrollMinSize = new Size(0, 90 + 15);
                images.Add(openFile.FileName);
                Bitmap image = new Bitmap(openFile.FileName);
                Graphics graphics = Graphics.FromImage(panelBitmap);
                graphics.DrawImage(image, 10, 15, 90, 90);
                panel1.Refresh();
                if (images.Count > 0)
                {
                    selcetedImage = new Bitmap(images[0]);
                    pictureBox1.Image = selcetedImage;
                }
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Transform = new Matrix(1, 0, 0, 1, panel1.AutoScrollPosition.X, panel1.AutoScrollPosition.Y);
            if (panelBitmap != null)
            {
                e.Graphics.DrawImage(panelBitmap, 0,0);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int realY = e.Y - panel1.AutoScrollPosition.Y;
            int cgeck = (realY - 15) / 90;
            if (cgeck >= images.Count)
            {
                return;
            }
            selectedPicture = cgeck;
            Bitmap image = new Bitmap(images[selectedPicture]);
            pictureBox1.Image = image;
        }

        private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            images.Clear();
            pictureBox1.Image = null;

            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                int i = 0;

                foreach (string imagefile in Directory.EnumerateFiles(file.SelectedPath))
                {
                    try
                    {
                        Bitmap image = new Bitmap(imagefile);
                        images.Add(imagefile);
                        i++;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                panelBitmap = new Bitmap(panel1.Width, 90 * i + 15);
                panel1.AutoScrollMinSize = new Size(0, 90 * i + 15);
                i = 0;

                foreach (string files in Directory.EnumerateFiles(file.SelectedPath))
                {
                    try
                    {
                        Bitmap image = new Bitmap(files);

                        Graphics graphics = Graphics.FromImage(panelBitmap);
                        graphics.DrawImage(image , 10, 90 * i + 15, 90, 90);
                        i++;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                panel1.Refresh();
                if (images.Count > 0)
                {
                    selcetedImage = new Bitmap(images[0]);
                    pictureBox1.Image = selcetedImage;
                }

            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CopyrightDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Укажите директорию для сохранения картинок");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                directoryForSave = fbd.SelectedPath;
            }
        }

        private void CopyrightTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите текст копирайта в текст бокс");
            Form2 f2 = new Form2();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                copyrightText = f2.textBox1.Text;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (copyrightText == "")
            {
                MessageBox.Show("Укажите текст для копирайта в Settings - Copyright text");
                return;
            }
            if (images.Count == 0)
            {
                return;
            }
            selcetedImage = new Bitmap(images[selectedPicture]);
            Graphics graphics = Graphics.FromImage(selcetedImage);
            graphics.DrawString(copyrightText, font, brush, new Point(80, selcetedImage.Height - 350));
            pictureBox1.Image = selcetedImage;
            Bitmap image = new Bitmap(imageList1.Images[0]);
            Graphics graphics1 = Graphics.FromImage(panelBitmap);
            graphics1.DrawImage(image, 70, 90 * (selectedPicture + 1) - 15, 30, 30);
            panel1.Refresh();
            FileInfo file = new FileInfo(images[selectedPicture]);
            dataGridView1.Rows.Add(file.Name, selcetedImage.Width, selcetedImage.Height, copyrightText + $" [{DateTime.Now.ToShortTimeString()}]");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = ".jpg";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(sf.FileName);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (directoryForSave == "")
            {
                MessageBox.Show("Укажите деррикторию для сохранения картинок в Setting - Copyright directory");
                return;
            }
            if (copyrightText == "")
            {
                MessageBox.Show("Укажите текст для копирайта в Settings - Copyright text");
                return;
            }
            for (int j = 0; j < images.Count; j++)
            {
                selcetedImage = new Bitmap(images[j]);
                Graphics graphics = Graphics.FromImage(selcetedImage);
                graphics.DrawString(copyrightText, font, Brushes.Red, new Point(80, selcetedImage.Height - 350));
                selcetedImage.Save(directoryForSave + $"\\LabImage{j}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Bitmap image = new Bitmap(imageList1.Images[0]);
                Graphics ds = Graphics.FromImage(panelBitmap);
                ds.DrawImage(image, 70, 90 * (j + 1) - 15, 30, 30);
                panel1.Refresh();
                FileInfo info = new FileInfo(images[j]);
                dataGridView1.Rows.Add(info.Name, selcetedImage.Width, selcetedImage.Height, copyrightText + $" [{DateTime.Now.ToShortTimeString()}]");
                pictureBox1.Image = selcetedImage;
                selectedPicture = images.Count - 1;
            }
        }
        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            Font = fontDialog1.Font;
        }

        private void ColorToolStripMenuItem_Click(object sender, EventArgs e) //я хотел сделать что бы можно было выбрать цвет для копирайта но Color нельзя перенести в Brush
        {
            colorDialog1.ShowDialog();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = colorDialog1.Color;
                }
            }
        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение, позволяющее открывать указанное изображение или все изображения в указанной директории.Сохранять новую версию изображения с добавленным текстом копирайта.");
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selectedPicture < images.Count)
            {
                if (panelBitmap != null)
                {
                    images.RemoveAt(selectedPicture);
                    int i = images.Count;
                    panelBitmap = new Bitmap(panel1.Width, 90 * i + 15);
                    panel1.AutoScrollMinSize = new Size(0, 90 * i + 15);

                    i = 0;

                    foreach (string files in images)
                    {
                        try
                        {
                            Bitmap image = new Bitmap(files);

                            Graphics graphics = Graphics.FromImage(panelBitmap);
                            graphics.DrawImage(image, 10, 90 * i + 15, 90, 90);
                            i++;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    panel1.Refresh();
                    pictureBox1.Image = null;
                }
            }
        }
        
    }
}
