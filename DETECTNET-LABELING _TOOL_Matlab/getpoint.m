function [pnts] = getpoint (I)
figure('name','Doubleclick to set location');
imshow(I);
[c r] = getpts(1);
pnts = uint32([c r]);
if size(pnts)>1
    pnts = [pnts(1,1) pnts(1,2)];
end
close all;
end