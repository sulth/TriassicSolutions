using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ErrorDigits
{
    public partial class ErrorAnalysis : Form
    {
        StringBuilder sbResult = null;
        StringBuilder sbCsv = null;
        Dictionary<string, string> dicCsv = null;
        List<string> classNames = null;

        private static string opFolder = "Output";
        private static string className, csvFileName, csvHeader;
        private static short slNum = 0, truePositive = 0, falsePositive = 0, trueNegative = 0, falseNegative = 0;

        /// <summary>
        /// Invoke constructor
        /// </summary>
        public ErrorAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Choose image folder click event handler
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
                txtImageFolder.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        /// <summary>
        /// Analyze error check click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckError_Click(object sender, EventArgs e)
        {
            // validating the input fields
            if (!ValidateFields())
                return;

            // starting error analysis
            AnalyseError();
        }

        /// <summary>
        /// Start analyzing error
        /// </summary>
        private void AnalyseError()
        {
            Cursor.Current = Cursors.WaitCursor;

            slNum = 0;
            sbResult = new StringBuilder();
            sbCsv = new StringBuilder();
            dicCsv = new Dictionary<string, string>();
            classNames = new List<string>();

            csvHeader = $"Sl.No.,File Name,Class Name,TP,TN,FP,FN,Remark";
            sbCsv.AppendLine(csvHeader);

            if (!dicCsv.ContainsKey("Header"))
                dicCsv.Add("Header", csvHeader);

            string jobId = txtJobId.Text.Trim();
            string imageFolder = txtImageFolder.Text;

            // constructing output folder path
            string opFolderPath = Path.Combine(imageFolder, opFolder);

            // create if not exists
            if (!Directory.Exists(opFolderPath))
                Directory.CreateDirectory(opFolderPath);

            // getting all the file entries from input folder
            string[] files = Directory.GetFiles(imageFolder);

            // assuming each image file should have a text file
            txtOutput.Text = sbResult.AppendLine($"{(files.Count() / 2).ToString()} file(s) found, Starting analysis...").ToString();

            // processing each file
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExt = Path.GetExtension(file);

                if (fileExt.ToLower() == ".jpg" || fileExt.ToLower() == ".jpeg" || fileExt.ToLower() == ".png")
                {
                    truePositive = falsePositive = trueNegative = falseNegative = 0;
                    string imageFilePath = $"@{file}";
                    string txtFilePath = $"{imageFolder}/{fileName}.txt";

                    txtOutput.Text = sbResult.AppendLine(string.Empty).ToString();
                    txtOutput.Text = sbResult.AppendLine($"Processing {Path.GetFileName(file)} ..!").ToString();
                    txtOutput.Text = sbResult.AppendLine(ReadDigits(jobId, imageFilePath, txtFilePath)).ToString();
                }
            }

            // writing result to csv file
            WriteToCSV();

            // opening the output folder
            if (Directory.Exists(opFolderPath))
                OpenErrorFolder(opFolderPath);

            txtOutput.Text = sbResult.AppendLine(string.Empty).ToString();
            txtOutput.Text = sbResult.AppendLine("*****Analysis completed..!*****").ToString();
            txtOutput.Text = sbResult.AppendLine(string.Empty).ToString();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Processing the image file with the response data from API.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="imageFile"></param>
        /// <param name="txtFile"></param>
        /// <returns></returns>
        private string ReadDigits(string jobId, string imageFile, string txtFile)
        {
            double inputCentroid_x, inputCentroid_y, responseCentroid_x, responseCentroid_y, upperTol = 15, lowerTol = 15;
            string groundTruthLabel, result, retString;
            List<dynamic> coordinates = null;

            string command = $"curl {txtIPAddress.Text}/models/images/generic/infer_one.json -XPOST -F job_id={jobId} -F image_file={imageFile}";
            string fName = Path.GetFileName(imageFile);

            try
            {
                // getting the text file details
                coordinates = GetTextFileData(txtFile, coordinates);

                // adding each class names to a list to further processing - to create class wise csv file
                if (!classNames.Contains(className))
                    classNames.Add(className);

                string classFolderPath = Path.Combine(txtImageFolder.Text, opFolder, className);

                if (!Directory.Exists(classFolderPath))
                    Directory.CreateDirectory(classFolderPath);

                if (coordinates == null)
                {
                    return WriteToBlurtip(imageFile, fName);
                }

                // starting the process
                result = StartProcess(command);

                // return if there no response from api
                if (string.IsNullOrWhiteSpace(result))
                    return $"File {fName} has no response from API.";

                // iterating through the result expando object
                List<List<dynamic>> finalCoordinates = IterateExpondoObject(result);

                if(finalCoordinates == null)
                    return $"File {fName} getting an error response as {result} from API.";

                // zero response from api
                if (finalCoordinates.Count == 0)
                {
                    falseNegative++;

                    // processing and create zero response output file
                    retString = WriteToFNZeroResponse(imageFile, result, fName);
                }
                else
                {
                    groundTruthLabel = $"GroundTruth |  x1: {coordinates[0]} y1: {coordinates[1]} x2: {coordinates[2]} y2: {coordinates[3]}";

                    // finding centroid value - (x1 + x2)/2
                    inputCentroid_x = (coordinates[0] + coordinates[2]) / 2;
                    // finding centroid value - (y1 + y2)/2
                    inputCentroid_y = (coordinates[1] + coordinates[3]) / 2;

                    // multiple responses from api
                    if (finalCoordinates.Count > 1)
                    {
                        bool isMatching = false;
                        StringBuilder sbMultiResponse = new StringBuilder();
                        StringBuilder sbMatchingCoordinates = new StringBuilder();
                        sbMultiResponse.AppendLine($"{fName} : Found multiple responses,");

                        foreach (List<dynamic> coordinate in finalCoordinates)
                        {
                            // finding centroid value - (x1 + x2)/2
                            double responseCentroid_sub_x = (coordinate[0] + coordinate[2]) / 2;
                            // finding centroid value - (y1 + y2)/2
                            double responseCentroid_sub_y = (coordinate[1] + coordinate[3]) / 2;

                            // comparing with the input centroid value and centroid value from the response with the tolerance value
                            if ((inputCentroid_x >= responseCentroid_sub_x - lowerTol && inputCentroid_x <= responseCentroid_sub_x + upperTol)
                                && (inputCentroid_y >= responseCentroid_sub_y - lowerTol && inputCentroid_y <= responseCentroid_sub_y + upperTol))
                            {
                                truePositive++;

                                // matching coordinates in multiple response
                                isMatching = true;
                                sbMatchingCoordinates.AppendLine(groundTruthLabel);
                                sbMatchingCoordinates.AppendLine($"Matching Coordinates | x1: {coordinate[0]} y1: {coordinate[1]} x2: {coordinate[2]} y2: {coordinate[3]}");
                                sbMatchingCoordinates.AppendLine($"{fName} : Centroid | x: {responseCentroid_sub_x} | y: {responseCentroid_sub_y} | Confidence: {coordinate[4]}");
                            }
                            else
                            {
                                falsePositive++;
                            }

                            sbMultiResponse.AppendLine($"x: {responseCentroid_sub_x} | y: {responseCentroid_sub_y} | Confidence: {coordinate[4]}");
                        }

                        if (!isMatching)
                            sbMatchingCoordinates.AppendLine("There is no matching coordinate found.");

                        retString = sbMultiResponse.ToString();

                        // creating multiple response files in output folder
                        WriteToMultipleResponse(imageFile, result, retString, isMatching, sbMatchingCoordinates);
                    }
                    else
                    {
                        // single response from api
                        List<dynamic> finalCoordinate = finalCoordinates.FirstOrDefault();

                        // finding centroid value - (x1 + x2)/2
                        responseCentroid_x = (finalCoordinate[0] + finalCoordinate[2]) / 2;
                        // finding centroid value - (y1 + y2)/2
                        responseCentroid_y = (finalCoordinate[1] + finalCoordinate[3]) / 2;

                        // comparing with the input centroid value and centroid value from the response with the tolerance value
                        if (!((inputCentroid_x >= responseCentroid_x - lowerTol && inputCentroid_x <= responseCentroid_x + upperTol)
                            && (inputCentroid_y >= responseCentroid_y - lowerTol && inputCentroid_y <= responseCentroid_y + upperTol)))
                        {
                            retString = $"{fName} : Error | x: {responseCentroid_x} | y: {responseCentroid_y} | Confidence: {finalCoordinate[4]}";

                            falsePositive++;

                            // creating single response files in output folder
                            WriteToFPSingleResponse(imageFile, groundTruthLabel, result, retString, finalCoordinate);
                        }
                        else
                        {
                            return $"{fName} : Success";
                        }
                    }
                }

                // construct text data to write to csv file
                ConstructCSV(fName);

                return retString;
            }
            catch (Exception ex)
            {
                sbResult.AppendLine(ex.Message);
                sbResult.AppendLine(ex.StackTrace.ToString());
                txtOutput.Text = sbResult.ToString();
                throw ex;
            }
        }

        /// <summary>
        /// Constructing csv data
        /// </summary>
        /// <param name="fName"></param>
        private void ConstructCSV(string fName)
        {
            slNum++;
            sbCsv.AppendLine($"{slNum},{fName},{className},{truePositive},{trueNegative},{falsePositive},{falseNegative}");

            if (!dicCsv.ContainsKey($"{className}#{slNum}"))
                dicCsv.Add($"{className}#{slNum}", $"SlNo#,{fName},{className},{truePositive},{trueNegative},{falsePositive},{falseNegative}");
        }

        /// <summary>
        /// Writing result to csv file
        /// </summary>
        private void WriteToCSV()
        {
            short slNo = 0;
            string clsPath = string.Empty;
            csvFileName = "ErrorAnalysis_{0}.csv";

            string textVal = string.Empty;
            List<string> textToWrite = null;

            foreach (string cls in classNames)
            {
                slNo = 0;
                textToWrite = new List<string>();
                clsPath = Path.Combine(txtImageFolder.Text, opFolder, cls);
                string csvFName = string.Format(csvFileName, cls);

                if (!Directory.Exists(clsPath))
                    Directory.CreateDirectory(clsPath);

                foreach (KeyValuePair<string, string> item in dicCsv)
                {
                    if (item.Key.ToLower().Equals("header"))
                    {
                        textToWrite.Add(item.Value);
                    }
                    else if (cls.Equals(item.Key.Split('#')[0]))
                    {
                        slNo++;

                        textVal = item.Value.Replace("SlNo#", slNo.ToString());
                        textToWrite.Add(textVal);
                    }
                }

                File.WriteAllLines(Path.Combine(clsPath, csvFName), textToWrite);
            }

            File.WriteAllText(Path.Combine(txtImageFolder.Text, opFolder, string.Format(csvFileName, "All")), sbCsv.ToString());
        }

        /// <summary>
        /// Write output to single response folder (creating output file with the same name as image file)
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="groundTruthLabel"></param>
        /// <param name="result"></param>
        /// <param name="retString"></param>
        /// <param name="finalCoordinate"></param>
        private void WriteToFPSingleResponse(string imageFile, string groundTruthLabel, string result, string retString, List<dynamic> finalCoordinate)
        {
            string singleRespFolder = "FP-SingleResponse";
            StringBuilder sbTextToWrite = new StringBuilder();
            sbTextToWrite.AppendLine($"Response: {result}");
            sbTextToWrite.AppendLine(string.Empty);
            sbTextToWrite.AppendLine("Centroid and Confidence Values: ");
            sbTextToWrite.AppendLine(retString.Substring(retString.IndexOf('|') + 2));
            sbTextToWrite.AppendLine(groundTruthLabel);
            sbTextToWrite.AppendLine($"Response Coordinates |  x1: {finalCoordinate[0]} y1: {finalCoordinate[1]} x2: {finalCoordinate[2]} y2: {finalCoordinate[3]}");

            string singleRespFolPath = Path.Combine(txtImageFolder.Text, opFolder, className, singleRespFolder);

            if (!Directory.Exists(singleRespFolPath))
                Directory.CreateDirectory(singleRespFolPath);

            File.WriteAllText(Path.Combine(singleRespFolPath, $"{Path.GetFileNameWithoutExtension(imageFile)}.txt"), sbTextToWrite.ToString());
        }

        /// <summary>
        /// Write output to multiple response folder (creating output file with the same name as image file)
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="result"></param>
        /// <param name="retString"></param>
        /// <param name="isMatching"></param>
        /// <param name="sbMatchingCoordinates"></param>
        private void WriteToMultipleResponse(string imageFile, string result, string retString, bool isMatching, StringBuilder sbMatchingCoordinates)
        {
            string multiRespFolder = "MultipleResponse";
            StringBuilder sbTextToWrite = new StringBuilder();
            sbTextToWrite.AppendLine($"Response: {result}");
            sbTextToWrite.AppendLine(string.Empty);
            sbTextToWrite.AppendLine("Centroid and Confidence Values: ");
            sbTextToWrite.AppendLine(retString.Substring(retString.IndexOf(':') + 2));
            sbTextToWrite.AppendLine(sbMatchingCoordinates.ToString());

            string multiRespFolPath = Path.Combine(txtImageFolder.Text, opFolder, className, multiRespFolder);

            if (!Directory.Exists(multiRespFolPath))
                Directory.CreateDirectory(multiRespFolPath);

            string multiSubFolder = isMatching ? "TPWithFP" : "FPOnly";
            string multiSubFolPath = Path.Combine(multiRespFolPath, multiSubFolder);

            if (!Directory.Exists(multiSubFolPath))
                Directory.CreateDirectory(multiSubFolPath);

            File.WriteAllText(Path.Combine(multiSubFolPath, $"{Path.GetFileNameWithoutExtension(imageFile)}.txt"), sbTextToWrite.ToString());
        }

        /// <summary>
        /// Write output to zero response folder (creating output file with the same name as image file)
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="result"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        private string WriteToFNZeroResponse(string imageFile, string result, string fName)
        {
            string retVal = $"File {fName} has only zero co-ordinates response from API.";
            string zeroRespFolder = "FN-ZeroResponse";
            StringBuilder sbTextToWrite = new StringBuilder();
            sbTextToWrite.AppendLine(retVal);
            sbTextToWrite.AppendLine($"Response: {result}");

            string zeroRespFolPath = Path.Combine(txtImageFolder.Text, opFolder, className, zeroRespFolder);

            if (!Directory.Exists(zeroRespFolPath))
                Directory.CreateDirectory(zeroRespFolPath);

            File.WriteAllText(Path.Combine(zeroRespFolPath, $"{Path.GetFileNameWithoutExtension(imageFile)}.txt"), sbTextToWrite.ToString());

            return retVal;
        }

        /// <summary>
        /// Iterating through expando object to get the coordinates data from the api response
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static List<List<dynamic>> IterateExpondoObject(string result)
        {
            dynamic obj_1, obj_2, obj_3;
            bool hasValue = false;
            List<List<dynamic>> finalCoordinates = new List<List<dynamic>>();

            try
            {
                var converter = new ExpandoObjectConverter();
                dynamic objResult = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                foreach (KeyValuePair<string, dynamic> item_1 in objResult)
                {
                    obj_1 = item_1.Value;

                    foreach (KeyValuePair<string, dynamic> item_2 in obj_1)
                    {
                        obj_2 = item_2.Value;

                        foreach (List<dynamic> item_3 in obj_2)
                        {
                            obj_3 = item_3;

                            foreach (List<dynamic> item_4 in obj_3)
                            {
                                hasValue = false;

                                foreach (dynamic item_5 in item_4)
                                {
                                    if (hasValue = (item_5 != 0))
                                        break;
                                }

                                if (hasValue)
                                    finalCoordinates.Add(item_4);
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return finalCoordinates;
        }

        /// <summary>
        /// Starting the process
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string StartProcess(string command)
        {
            Process proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            proc.StartInfo = startInfo;
            proc.Start();

            StringBuilder sbResponse = new StringBuilder();
            while (!proc.HasExited)
            {
                sbResponse.Append(proc.StandardOutput.ReadToEnd());
            }

            proc.WaitForExit();

            return sbResponse.ToString();
        }

        /// <summary>
        /// Write output to blurtip folder (creating output file with the same name as image file)
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        private string WriteToBlurtip(string imageFile, string fName)
        {
            string retVal = $"File {fName} is not processed (not a 'car' category).";
            string blurTipFolder = "Blurtip";
            StringBuilder sbTextToWrite = new StringBuilder();
            sbTextToWrite.AppendLine(retVal);

            string blurTipFolPath = Path.Combine(txtImageFolder.Text, opFolder, className, blurTipFolder);

            if (!Directory.Exists(blurTipFolPath))
                Directory.CreateDirectory(blurTipFolPath);

            File.WriteAllText(Path.Combine(blurTipFolPath, $"{Path.GetFileNameWithoutExtension(imageFile)}.txt"), sbTextToWrite.ToString());
            return retVal;
        }

        /// <summary>
        /// Getting the text file data to process
        /// </summary>
        /// <param name="txtFile"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        private static List<dynamic> GetTextFileData(string txtFile, List<dynamic> coordinates)
        {
            string[] text = File.ReadAllLines(txtFile);
            string[] values = text[0].Split(' ');

            className = values[0].Trim().ToLower();

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
        /// Opening the output folder
        /// </summary>
        /// <param name="outputPath"></param>
        private static void OpenErrorFolder(string outputPath)
        {
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            Process folderOpenProc = new Process();
            folderOpenProc.StartInfo.FileName = windir + @"\explorer.exe";
            folderOpenProc.StartInfo.Arguments = outputPath;
            folderOpenProc.Start();
        }

        /// <summary>
        /// Validating input fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtJobId.Text)
                || string.IsNullOrWhiteSpace(txtImageFolder.Text)
                || string.IsNullOrWhiteSpace(txtIPAddress.Text))
            {
                if (string.IsNullOrWhiteSpace(txtJobId.Text))
                {
                    MessageBox.Show("Please enter job id..!", "Alert");
                }
                else if (string.IsNullOrWhiteSpace(txtImageFolder.Text))
                {
                    MessageBox.Show("Please select image folder..!", "Alert");
                }
                else if (string.IsNullOrWhiteSpace(txtIPAddress.Text))
                {
                    MessageBox.Show("Please enter IP Address..!", "Alert");
                }

                return false;
            }

            return true;
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
