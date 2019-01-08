rm ./output/ -R
docker build . -t rocketmod-plugin-builder:latest
echo ">>>>>>>>>>>>>>>>>>>>>>>>" 
docker run --name rocketmod-plugin-builder -it \
 -e GIT_REPOSITORY=https://github.com/fr34kyn01535/Kits.git \
 -e GIT_BRANCH=master \
 -e PROJECT_FILE=Kits.csproj \
 -v `pwd`/output:/build/output \
 rocketmod-plugin-builder:latest
echo "<<<<<<<<<<<<<<<<<<<<<<<<" 
docker rm -f rocketmod-plugin-builder 
docker rmi -f rocketmod-plugin-builder 
