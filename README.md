This container can be run to directly build RocketMod plugins from git.

Example command:
docker run --name rocketmod-plugin-builder -it \
 -e GIT_REPOSITORY=https://github.com/fr34kyn01535/Kits.git \
 -e GIT_BRANCH=master \
 -e PROJECT_FILE=Kits.csproj \
 -v `pwd`/output:/build/output \
 rocketmod-plugin-builder:legacy