using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LabelConversionTool
{
    public partial class ConversionTool : Form
    {
        StringBuilder sbResult = null;
        StringBuilder sbFileNames = null;
        private string fileName, ipRootPath, ipRootFolder, opPath, opPathAnnotations, opPathImageSets, opPathJPEGImages, className, truncated, difficult;
        private string[] classTypes = { "Car", "DontCare", "Pedestrian", "Truck" };

        // input folders should have the same name
        private string[] ipAcceptFol = { "Images", "Labels", "Test", "Train", "Val" };

        public ConversionTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Choose input folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;

            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtRootFolder.Text = txtOutputRootFolder.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        /// <summary>
        /// Choose output folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseOpFol_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;

            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputRootFolder.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        /// <summary>
        /// Start conversion process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConverter_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // validating the input fields
                if (!ValidateFields())
                    return;

                sbResult = new StringBuilder();
                ipRootPath = txtRootFolder.Text.Trim();

                // Create all output directory structure
                CreateOpDirectory();

                // create annotations to XML file and generating the image sets
                StartConversion();

                Cursor.Current = Cursors.Default;

                MessageBox.Show("Successfully converted..!", "Alert");

                OpenFolder(opPath);
            }
            catch (Exception ex)
            {
                txtOutput.Text = sbResult.AppendLine($"{Environment.NewLine}Error occurred : {ex.Message} | {ex.InnerException} | {ex.StackTrace}").ToString();
                MessageBox.Show($"An error occurred while processing file '{fileName}'", "Alert");
            }
        }

        /// <summary>
        /// Creating train val text file
        /// </summary>
        private void CreateTrainVal()
        {
            List<string> class_trainData = null, class_valData = null, class_trainVal = null, txtFiles = null;
            List<string> trainData = null, valData = null, trainVal = null;

            txtOutput.Text = sbResult.AppendLine($"{Environment.NewLine}Creating trainval text file..!").ToString();

            // getting all image set text file
            txtFiles = Directory.EnumerateFiles(opPathImageSets).Where(file => file.ToLower().EndsWith(".txt")).ToList();

            foreach (string cls in classTypes)
            {
                foreach (string file in txtFiles)
                {
                    //string fNameCls = file.Substring(file.LastIndexOf('\\') + 1).ToLower();
                    var fNameSplit = Path.GetFileNameWithoutExtension(file).ToLower().Split('_');

                    if (fNameSplit[0].Equals(cls.ToLower()))
                    {
                        if (fNameSplit[1].Equals(ipAcceptFol[3].ToLower()))
                        {
                            class_trainData = File.ReadAllLines(file).ToList();
                        }
                        else if (fNameSplit[1].Equals(ipAcceptFol[4].ToLower()))
                        {
                            class_valData = File.ReadAllLines(file).ToList();
                        }
                    }

                    if (fNameSplit[0].Equals(ipAcceptFol[3].ToLower()))
                    {
                        trainData = File.ReadAllLines(file).ToList();
                    }
                    else if (fNameSplit[0].Equals(ipAcceptFol[4].ToLower()))
                    {
                        valData = File.ReadAllLines(file).ToList();
                    }
                }

                class_trainVal = new List<string>();
                class_trainVal.AddRange(class_trainData ?? new List<string>());
                class_trainVal.AddRange(class_valData ?? new List<string>());

                //Random rnd = new Random();
                //class_trainVal = class_trainVal.OrderBy(p => rnd.Next()).ToList();

                class_trainVal = class_trainVal.OrderBy(p => p.Substring(0, p.Count() - 3).Trim()).ToList();

                string class_trainValFile = Path.Combine(opPathImageSets, $"{cls.ToLower()}_{ipAcceptFol[3].ToLower()}{ipAcceptFol[4].ToLower()}.txt");
                File.WriteAllLines(class_trainValFile, class_trainVal);
            }

            trainVal = new List<string>();
            trainVal.AddRange(trainData ?? new List<string>());
            trainVal.AddRange(valData ?? new List<string>());

            trainVal = trainVal.OrderBy(p => p.Substring(0, p.Count() - 3).Trim()).ToList();

            string trainValFile = Path.Combine(opPathImageSets, $"{ipAcceptFol[3].ToLower()}{ipAcceptFol[4].ToLower()}.txt");
            File.WriteAllLines(trainValFile, trainVal);

            txtOutput.Text = sbResult.AppendLine($"Success..!").ToString();
        }

        /// <summary>
        /// Starting conversion process
        /// </summary>
        private void StartConversion()
        {
            List<string> ipFolders = Directory.GetDirectories(ipRootPath).ToList();

            foreach (string ipFolder in ipFolders)
            {
                string itemFol = ipFolder.Substring(ipFolder.LastIndexOf('\\') + 1).ToLower();

                if (itemFol == ipAcceptFol[2].ToLower() || itemFol == ipAcceptFol[3].ToLower() || itemFol == ipAcceptFol[4].ToLower())
                    CreateAnnotations(ipFolder);
            }

            // creating train val text file
            CreateTrainVal();
        }

        /// <summary>
        /// Create annotations to XML file
        /// </summary>
        private void CreateAnnotations(string ipPath)
        {
            sbFileNames = new StringBuilder();
            ipRootFolder = ipPath.Substring(ipPath.LastIndexOf('\\') + 1);
            List<string> inputFolders = Directory.GetDirectories(ipPath).ToList();

            for (int i = 0; i < inputFolders.Count; i++)
            {
                string itemFol = inputFolders[i].Substring(inputFolders[i].LastIndexOf('\\') + 1).ToLower();
                if (itemFol != ipAcceptFol[0].ToLower() && itemFol != ipAcceptFol[1].ToLower())
                    inputFolders.RemoveAt(i);
            }

            foreach (string type in classTypes)
            {
                string imgSetFileName = $"{type}_{ipRootFolder}.txt";
                string imgSetFPath = Path.Combine(opPathImageSets, imgSetFileName.ToLower());

                // Delete the file if it exists.
                if (File.Exists(imgSetFPath))
                {
                    File.Delete(imgSetFPath);
                }

                // Create the file.
                using (FileStream fs = File.Create(imgSetFPath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(string.Empty);

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

            foreach (string item in inputFolders)
            {
                if (item.Substring(item.LastIndexOf('\\') + 1).ToLower() == ipAcceptFol[1].ToLower())
                {
                    List<string> txtFiles = Directory.EnumerateFiles(item).Where(file => file.ToLower().EndsWith(".txt")).ToList();

                    string imageFolPath = item.Replace(item.Substring(item.LastIndexOf('\\') + 1), ipAcceptFol[0]);
                    List<string> imageFiles = Directory.EnumerateFiles(imageFolPath).Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".png")).ToList();

                    txtOutput.Text = sbResult.AppendLine($"{Environment.NewLine}{txtFiles.Count().ToString()} labeled file(s) found in {ipRootFolder} folder, Starting conversion...").ToString();

                    foreach (string file in txtFiles)
                    {
                        fileName = Path.GetFileNameWithoutExtension(file);
                        string imageFile = imageFiles.Where(p => p.ToLower().Contains(fileName)).FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(imageFile))
                        {
                            txtOutput.Text = sbResult.AppendLine($"{Environment.NewLine}Converting {Path.GetFileName(file)}").ToString();

                            // to write to the text file in image set folder
                            sbFileNames.AppendLine(fileName);

                            // copying image file to JPEGImages folder
                            File.Copy(imageFile, Path.Combine(opPathJPEGImages, $"{fileName}{Path.GetExtension(imageFile)}"), true);

                            Image img = Image.FromFile(imageFile);

                            using (XmlWriter writer = XmlWriter.Create(Path.Combine(opPathAnnotations, $"{fileName}.xml")))
                            {
                                writer.WriteStartElement("annotation");

                                writer.WriteElementString("folder", txtFolderName.Text.Trim());
                                writer.WriteElementString("filename", Path.GetFileName(imageFile));

                                writer.WriteStartElement("source");
                                writer.WriteElementString("database", "The VOC2007 Database");
                                writer.WriteElementString("annotation", "PASCAL VOC2007");
                                writer.WriteElementString("image", "None");
                                writer.WriteElementString("flickrid", "None");
                                writer.WriteEndElement();

                                writer.WriteStartElement("owner");
                                writer.WriteElementString("flickrid", "None");
                                writer.WriteElementString("name", "None");
                                writer.WriteEndElement();

                                writer.WriteStartElement("size");
                                writer.WriteElementString("width", img.Width.ToString());
                                writer.WriteElementString("height", img.Height.ToString());
                                writer.WriteElementString("depth", "3");
                                writer.WriteEndElement();

                                writer.WriteElementString("segmented", "0");

                                List<string> txtImgSetFiles = Directory.EnumerateFiles(opPathImageSets).Where(p => p.ToLower().EndsWith(".txt")
                                                                                           && p.ToLower().Contains(ipRootFolder.ToLower())).ToList();

                                foreach (string txtImgSetFile in txtImgSetFiles)
                                {
                                    File.AppendAllText(txtImgSetFile, $"{fileName} -1");
                                }

                                // creating each object tag for each class
                                string[] textEntries = File.ReadAllLines(file);

                                foreach (string text in textEntries)
                                {
                                    List<dynamic> coordinates = GetTextData(text);

                                    writer.WriteStartElement("object");
                                    writer.WriteElementString("name", className);
                                    writer.WriteElementString("pose", "Frontal");
                                    writer.WriteElementString("truncated", truncated);
                                    writer.WriteElementString("difficult", difficult);

                                    writer.WriteStartElement("bndbox");
                                    writer.WriteElementString("xmin", coordinates[0].ToString());
                                    writer.WriteElementString("ymin", coordinates[1].ToString());
                                    writer.WriteElementString("xmax", coordinates[2].ToString());
                                    writer.WriteElementString("ymax", coordinates[3].ToString());
                                    writer.WriteEndElement();

                                    // object tag end
                                    writer.WriteEndElement();

                                    foreach (string txtImgSetFile in txtImgSetFiles)
                                    {
                                        string fNameToAppend = string.Empty;
                                        string clsName = txtImgSetFile.Substring(txtImgSetFile.LastIndexOf('\\') + 1).Split('_')[0];

                                        if (clsName.ToLower() == className.ToLower())
                                        {
                                            if (Convert.ToDouble(truncated) != 0 || Convert.ToDouble(difficult) != 0)
                                                fNameToAppend = $"{fileName}  0";
                                            else
                                                fNameToAppend = $"{fileName}  1";
                                        }

                                        List<string> allLines = File.ReadAllLines(txtImgSetFile).ToList();

                                        for (int i = 0; i < allLines.Count(); i++)
                                        {
                                            if (!string.IsNullOrWhiteSpace(allLines[i]) && !string.IsNullOrWhiteSpace(fNameToAppend)
                                                && allLines[i].ToLower().Substring(0, allLines[i].Count() - 3).Trim() == fileName.ToLower().Trim())
                                                allLines[i] = fNameToAppend;
                                        }

                                        File.WriteAllLines(txtImgSetFile, allLines);
                                    }
                                }

                                // annotation tag end
                                writer.WriteEndElement();

                                writer.Flush();
                            }

                            txtOutput.Text = sbResult.AppendLine($"Success..!").ToString();
                        }
                    }
                }
            }

            // for creating test.txt/train.txt/val.txt/trainval.txt files
            string imgSetOwnFileName = Path.Combine(opPathImageSets, $"{ipRootFolder.ToLower()}.txt");
            File.WriteAllText(imgSetOwnFileName, sbFileNames.ToString());
        }

        /// <summary>
        /// Getting the text data to process
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<dynamic> GetTextData(string text)
        {
            List<dynamic> coordinates;
            string[] values = text.Split(' ');

            className = values[0].Trim().ToLower();
            truncated = values[1].Trim() == "0.0" ? "0" : "1";
            difficult = "0";

            coordinates = new List<dynamic>();

            for (int i = 0; i < values.Count(); i++)
            {
                // getting coordinates only(index from 4 to 7) and skipping truncation and occlusion
                if (i >= 4 && i <= 7)
                {
                    double coordinate;
                    double.TryParse(values[i], out coordinate);

                    //if (coordinate != 0)
                    //{
                    coordinates.Add(coordinate);
                    //}
                }
            }

            return coordinates;
        }

        /// <summary>
        /// Creating output directories
        /// </summary>
        private void CreateOpDirectory()
        {
            opPath = Path.Combine(txtOutputRootFolder.Text.Trim(), txtFolderName.Text.Trim());

            if (Directory.Exists(opPath))
                Directory.Delete(opPath, true);

            if (!Directory.Exists(opPath))
                Directory.CreateDirectory(opPath);

            opPathAnnotations = Path.Combine(opPath, "Annotations");

            if (!Directory.Exists(opPathAnnotations))
                Directory.CreateDirectory(opPathAnnotations);

            opPathImageSets = Path.Combine(opPath, "ImageSets", "Main");

            if (!Directory.Exists(opPathImageSets))
                Directory.CreateDirectory(opPathImageSets);

            opPathJPEGImages = Path.Combine(opPath, "JPEGImages");

            if (!Directory.Exists(opPathJPEGImages))
                Directory.CreateDirectory(opPathJPEGImages);


        }

        /// <summary>
        /// Validating input fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtRootFolder.Text)
                || string.IsNullOrWhiteSpace(txtOutputRootFolder.Text)
                || string.IsNullOrWhiteSpace(txtFolderName.Text))
            {
                if (string.IsNullOrWhiteSpace(txtRootFolder.Text))
                {
                    MessageBox.Show("Please select input root folder..!", "Alert");
                }
                else if (string.IsNullOrWhiteSpace(txtOutputRootFolder.Text))
                {
                    MessageBox.Show("Please select output root folder..!", "Alert");
                }
                else if (string.IsNullOrWhiteSpace(txtFolderName.Text))
                {
                    MessageBox.Show("Please enter output folder name..!", "Alert");
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Opening the output folder
        /// </summary>
        /// <param name="outputPath"></param>
        private static void OpenFolder(string outputPath)
        {
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            Process folderOpenProc = new Process();
            folderOpenProc.StartInfo.FileName = windir + @"\explorer.exe";
            folderOpenProc.StartInfo.Arguments = outputPath;
            folderOpenProc.Start();
        }

        /// <summary>
        /// Maintain scroll for output text-box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            txtOutput.SelectionStart = txtOutput.Text.Length;

            // scroll it automatically
            txtOutput.ScrollToCaret();
        }
    }
}
