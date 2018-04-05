import numpy as np
import cv2
from skimage.external.tifffile import imread as tiffreader, imshow as tiffshow
import time
import os, glob,pdb
#import cv2.cv as cv


input_path = "./input3/"
output_dir = "./output_" + str(time.strftime("%Y_%m_%d_%H_%M_%S"))
output_dir = "./output7"
image_dir = ""
label_dir = ""




def create_output_dir(tiff_file_name):
	#pdb.set_trace()
	global image_dir, label_dir,output_dir
	image_dir = output_dir + "/images"
	label_dir = output_dir + "/labels"
	if not os.path.exists(output_dir):
		os.makedirs(output_dir)

	if not os.path.exists(image_dir):
		os.makedirs(image_dir)
	if not os.path.exists(image_dir + "/" + tiff_file_name):
		os.makedirs(image_dir + "/" + tiff_file_name)

	if not os.path.exists(label_dir):
		os.makedirs(label_dir)
	if not os.path.exists(label_dir + "/" + tiff_file_name):
		os.makedirs(label_dir + "/" + tiff_file_name)



def save_image(file_path, image):
	"""save a image into a specific location"""
	global image_dir
	#pdb.set_trace()
	cv2.imwrite(image_dir + file_path, image)


def save_labels(file_path, data):
	global label_dir
	filedata = open(label_dir + file_path, "w")
	filedata.write(data)
	filedata.close()

def load_tiff_files(input_path):
	return sorted(glob.glob(input_path.strip("/") + "/*.ti*"))

def scaleImg(oriimg,imgScale):
	imgScale = 0.5
	height, width = oriimg.shape
	newX,newY = oriimg.shape[1]*imgScale, oriimg.shape[0]*imgScale
	img = cv2.resize(oriimg,(int(newX),int(newY)))
	return img

def onClick(event, x, y, flags, params):
	
	
	global index, img, cent, rectColor, rectSize
	if event == cv2.EVENT_LBUTTONDOWN:
		cent = (x,y)
		p1 = (x-1,y-1)
		p2 = (x+1,y+1)
		img1 = img.copy()
		cv2.rectangle(img1,p1,p2,rectColor,rectSize)
		cv2.imshow('image',img1)

all_tiff_files = load_tiff_files(input_path)
total_tiff_files = len(all_tiff_files)
file_number = 0
#
while (total_tiff_files):

	rectColor = (255,0,0)
	rectSize = 2
	tiff_image_file  = tiffreader(all_tiff_files[file_number])
	tiff_file_name = os.path.splitext(os.path.basename(all_tiff_files[file_number]))[0]

	total_images_in_file = len(tiff_image_file)
	print total_images_in_file
	index = 0
	oriimg = tiff_image_file[0].copy()
	cent = (0,0)
	p1 = (0,0)
	p2 = (0,0)
	track = 0
	pset = 0
	imgScale = 0.5
	#cv2.namedWindow("image", cv2.WINDOW_AUTOSIZE);
	#img = scaleImg(oriimg,imgScale)
	#img = scaleImg(img,0.5)
	cv2.namedWindow('image')
	cv2.setMouseCallback('image',onClick)
	oriimg = tiff_image_file[index].copy()
	img = scaleImg(oriimg,imgScale)
	saveImg = np.zeros((100,100))
	cv2.imshow('image',img)
	while (1):
		print tiff_file_name, index
		create_output_dir(tiff_file_name)

		#cv2.resizeWindow('image',512,512)
		#cv2.waitKey(0)
		k = cv2.waitKey(0)

		if k == ord('s'):
			index = index + 1
			if total_images_in_file == index:
				break
			else:
				pass
			cv2.imshow('image',img)

			oriimg = tiff_image_file[index].copy()
			img = scaleImg(oriimg,imgScale)
			x = int(cent[0]/imgScale)
			y = int(cent[1]/imgScale)
			if index > 1:
				#cv2.imwrite('img'+str(index)+'.jpg',saveImg)
				save_image("/"+tiff_file_name+'/img'+str(index)+'.jpg',saveImg)
				savePoint = (x - p1[0], y - p1[1])
				print(savePoint)

				save_labels("/"+tiff_file_name+"/"+str(index)+'.txt',str(savePoint))
				# file = open(str(index)+'.txt','w')
				# file.write(str(savePoint))
				# file.close()
			p1 = (x - 25, y - 25)
			p2 = (x + 25, y + 25)
			saveImg = oriimg[p1[1]:p2[1],p1[0]:p2[0]]
			#print(saveImg.shape)
			#cv2.imshow('crop',saveImg)
			ps1 = (int(imgScale*p1[0]),int(imgScale*p1[1]))
			ps2 = (int(imgScale*p2[0]),int(imgScale*p2[1]))
			cv2.rectangle(img,ps1,ps2,rectColor,rectSize)
			cv2.imshow('image',img)
		elif k == ord('n'):
			#skipping to next sub image
			index = index + 1
			if total_images_in_file == index:
				break
			oriimg = tiff_image_file[index].copy()
			img = scaleImg(oriimg, imgScale)
			cv2.imshow('image', img)

			# print "continue"
			continue
		elif k == ord('x'):
			# skipping to next image
			break
		# elif k == ord('s'): # wait for 's' key to save and exit
		#     cv2.imwrite('hotgray.png',img)
		#     cv2.destroyAllWindows()"""
	file_number += 1
	total_tiff_files -= 1
