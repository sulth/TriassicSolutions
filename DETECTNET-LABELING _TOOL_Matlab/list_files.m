function valSubDirList = list_files(dirName,appendExtn)
    if appendExtn ~= 0
        dirName = [dirName appendExtn];
    end
    dirData = dir(dirName);
    subDirList = {dirData.name}';
    validIndex = ~ismember(subDirList,{'.','..'}); %list of sub directories
    valSubDirList = subDirList(validIndex); %remove . and .. from the list
end