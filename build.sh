#!/bin/sh
echo "--- Pull changes"
git pull
echo "--- Remove old zip"
rm GameMode_Microlite20.zip
echo "--- Build new zip"
zip GameMode_Microlite20.zip *
zip -d GameMode_Microlite20.zip build.sh README.md # exclude these files from finished add-on
echo "--- Pushing to server"
echo "cd Add-Ons ; put GameMode_Microlite20.zip" | ftp hammereditor.net && echo "--- FTP push success
echo "--- git add -A"
git add -A
echo "--- Commit"
git commit
echo "--- Push to github"
git push
echo "--- Complete"