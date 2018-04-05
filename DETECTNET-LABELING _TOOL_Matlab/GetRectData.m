dirName = '/Documents/outputsJetson/marchaprilout1'
fileList = list_files(dirName,0); %list of files
numFiles = length(fileList);%Number of files
k = 0;
for i=1:numFiles
    fileName = [dirName '/' char(fileList(i))];
 %  nFrames = input(['Number of frames in ' char(fileList(i)) ' = ']);
    motionData = [];
   % for j=1:nFrames
        k = k + 1;
        file = fopen(['Train/Label/' num2str(k) '.txt'],'w');
        %I = imread(fileName,j);
        I = imread(fileName);
         [Ir,rect] = imcrop(I);
        % Is=imresize(Ir,[1000,1000])
         %[Ic,rect] = imcrop(I);
        fprintf(file,'Car 0.0 0 0.0 %1.1f %1.1f %1.1f %1.1f 0.0 0.0 0.0 0.0 0.0 0.0 0.0 ',rect(1),rect(2),rect(1)+rect(3),rect(2)+rect(4));
         
        %     pts = getpoint(I);
%         motionData = [motionData;j pts];
        
         imwrite(I,['Train/Image/' num2str(k) '.png'],'png');
        fclose(file);
        close all;
   % end
%     csvwrite(['MotionData_' char(fileList{i}(1:end-4)) '.csv'],motionData);
end