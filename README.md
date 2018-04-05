
# AI ObjectDetection Labelling Tools.

# DETECTNET-LABELING TOOL

The Tool allows easy labelling of images and creates text file compatible with DETECTNET / KITTI data set format.

It offers the most easiest and convinent way of labelling an image.

 It aslo allows the user to draw rectangles around objects ,name these rectangles . Label.”txt” files are created in the same folder with the image and contains labels and their bounding box coordinates. 

The tool also offers the option of categorizing occluded images. 



The label files contain the following information,                                                                         

         Values Name      Description
----------------------------------------------------------------------------
          1    type         Describes the type of object: 'Car', 'Van', 'Truck',
                            'Pedestrian','DontCare'
                      
          1    truncated    Float from 0 (non-truncated) to 1 (truncated), where
                            truncated refers to the object leaving image boundaries
          1    occluded     Integer (0,1,2,3) indicating occlusion state:
                            0 = fully visible, 1 = partly occluded
                            2 = largely occluded, 3 = unknown
     
          4    bbox         2D bounding box of object in the image (0-based index):
                            contains left, top, right, bottom pixel coordinates
  

Here, 'DontCare' labels denote regions in which objects have not been labeled,
for example because they have been too far away from the laser scanner. To
prevent such objects from being counted as false positives our evaluation
script will ignore objects detected in don't care regions of the test set.

The tool can be easily run by copying the folder in the require location. No additional installation is necessary. 
The Tool was tested under the following environment
Windows 10
Windows 7

The tool can also run using Matlab code.

# DETECTNET-ERROR ANALYSIS AUTOMATION

Why comparing Machine Learning algorithms to human level performance? 
                                   
We have to address data mismatch and analyse the false detection. We, human are reluctant to do error analysis manually in each of the thousands of data. Our automation tool makes it easy for you !!!

You can get a single real number evaluation metric of Accuracy from the confusion metrics that quickly tells you the ML Algorithm is better or worse: try analyse your model's Bias or Variance ...

The tool is developed to analyze the Model accuracy . The images along with their labels are selected and sent for checking.
      
The tool offers the advantage over detect net by analyzing multiple images and labels without interruption. The tool was checked  with 20000 images and produced the results within a limited span of time. The Output folder will contain  a spreadsheet of files with error, The response of various classes in .txt in format.
          
PREREQUISITES
Language Used-C#
.Net Framework
Visual Studio 2013


# TOOLS FOR FASTER RCNN

# DETECTNET TO PY-FASTER RCNN LABEL CONVERSION TOOL

The caffe py faster RCNN requires that the labels of Pascal VOC2007 format.

The tool converts the Kittydataset into PASCAL VOC2007 Format. The output folder contains the images and their corresponding labels in XML format .

The input images and their labels should be divided into 3 folders( Train ,Val and Test) before conversion.
 
The output folder will be named VOC2007 having the following folders
-Annotations
-JPEG Images
-ImageSets

PREREQUISITES
Language Used-C#
.Net Framework
Visual Studio 2013

# DETECTNET-YOLO LABEL CONVERSION TOOL

YOLO, short for You Only Look Once, is a real-time object recognition algorithm. The YOLO Conversion tool converts the Kitty Data set into yolo format


Before Conversion

DontCare 0.0 0 0.0 32.0 17.0 186.0 103.0 0.0 0.0 0.0 0.0 0.0 0.0 0.0
Car 0.0 1 0.0 272.0 417.0 354.0 527.0 0.0 0.0 0.0 0.0 0.0 0.0 0.0
Pedestrian 0.0 1 0.0 559.0 598.0 881.0 876.0 0.0 0.0 0.0 0.0 0.0 0.0 0.0

After Conversion

Class Number box1_x1_ratio box1_y1_ratio box1_width_ratio box1_height_ratio
Class Number box2_x1_ratio box2_y1_ratio box2_width_ratio box2_height_ratio

The tool can run using Matlab code.




