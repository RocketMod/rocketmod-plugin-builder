#!/bin/bash
cd /build

if [[ -z "${GIT_BRANCH}" ]]; then
  GIT_BRANCH="master"
fi

if [[ -z "${GIT_REPOSITORY}" ]]; then
  echo -e "GIT_REPOSITORY must be specified!"
  exit
fi

echo -e "\nFetching repository..."
git clone --single-branch --branch $GIT_BRANCH $GIT_REPOSITORY ./workspace

cd ./workspace

# Hotfix: Replaces TargetFramework with .NET Standard 2.
find . -type f -name "*.csproj" -print0 | xargs -0 sed -i -E "s/<TargetFrameworks?>(.*)<\/TargetFrameworks?>/<TargetFramework>netstandard2.0<\/TargetFramework>/g"

echo -e "\nRestoring libraries..."
dotnet restore

mkdir ./lib
curl "https://rocketmod.net/unity3d/2017.4.17f1/UnityEngine.dll" -o "./lib/UnityEngine.dll"
curl "https://rocketmod.net/unity3d/2017.4.17f1/UnityEngine.CoreModule.dll" -o "./lib/UnityEngine.CoreModule.dll"

echo -e "\nStarting the build process..."
dotnet build . --no-restore -c Release -o /build/output/ -f netstandard2.0 --force
rm -rf /build/output/*.pdb

mkdir /build/dist/harbor -p
git log -1 --pretty=%B > /build/dist/harbor/git-commit-message.txt