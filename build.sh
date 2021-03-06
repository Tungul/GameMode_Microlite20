#!/bin/sh
echo "--- Pull changes"
git pull
echo "--- Build new zip"
zip -r GameMode_Microlite20.zip *
zip -d GameMode_Microlite20.zip build.sh README.md # exclude these files from finished add-on
mkdir Add-Ons; cp GameMode_Microlite20.zip Add-Ons
echo "--- Pushing to server"
echo "put Add-Ons/*" | ftp hammereditor.net && echo "--- FTP push success" # just making a minor note here: OH MY FUCKING GOD IT WORKED FINALLY
rm -rf Add-Ons/
echo "--- git add -A"
git add -A
echo "--- Commit"
git commit
echo "--- Push to github"
git push
echo "--- Complete"
