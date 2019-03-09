
This container can be run to directly build RocketMod 5 plugins from git.

Example command:
docker run --rm -it \
 -e GIT_REPOSITORY=https://github.com/fr34kyn01535/Kits.git \
 -e GIT_BRANCH=master \
 -v ./output:/build/output \
fr34kyn01535/rocketmod-plugin-builder