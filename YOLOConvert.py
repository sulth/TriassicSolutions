import os
path = os.getcwd()
path = path+'/Detectnet'
f = []
for (dirpath, dirnames, filenames) in os.walk(path):
	f.extend(filenames)
	break
print(f)
for filename in f:
	file = open('Detectnet/'+filename)
	opf = open('Yolo/'+filename,'w')
	lines = file.readlines()
	imh = 1024
	imw = 1024
	labelstr = ''
	for s in lines:
		words = s.split()
		if words:
			label = words[0]
			x1 = float(words[4])
			y1 = float(words[5])
			x2 = float(words[6])
			y2 = float(words[7])
			mx = x1 + (x2 - x1)/2
			my = y1 + (y2 - y1)/2
			w = x2 - x1
			h = y2 - y1
			if label == 'Car':
				labelstr = labelstr+'0 '+str((mx/imw))+' '+str((my/imh))+' '+str((w/imw))+' '+str((h/imh))+'\n'
			else:
				labelstr = labelstr+'1 '+str((mx/imw))+' '+str((my/imh))+' '+str((w/imw))+' '+str((h/imh))+'\n'
	opf.write(''.join(labelstr))