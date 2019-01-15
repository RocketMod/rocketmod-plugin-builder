rm ./output/-R
mkdir ./output/
docker build . -t rocketmod-plugin-builder:legacy
echo ">>>>>>>>>>>>>>>>>>>>>>>>" 
docker run --name rocketmod-plugin-builder -it \
 -e GIT_REPOSITORY=https://github.com/fr34kyn01535/Kits.git \
 -e GIT_BRANCH=master \
 -e PROJECT_FILE=Kits.csproj \
 rocketmod-plugin-builder:legacy
 #-v `pwd`/output:/build/output \
echo "<<<<<<<<<<<<<<<<<<<<<<<<" 
docker rm -f rocketmod-plugin-builder 
docker rmi -f rocketmod-plugin-builder:legacy
