This container can be run to directly build RocketMod plugins from git.

Example command:
docker run --name rocketmod-plugin-builder -it \
 -e GIT_REPOSITORY=https://github.com/fr34kyn01535/Kits.git \
 -e GIT_BRANCH=master \
 -e PROJECT_FILE=Kits.csproj \
<<<<<<< HEAD
 -v `pwd`/output:/build/dist \
 rocketmod-plugin-builder:legacy

 
=======
 -v `pwd`/output:/build/output \
fr34kyn01535/rocketmod-plugin-builder:legacy
>>>>>>> c6c8609a68ad60dda8a7775e3cbbc449d7e036cf
