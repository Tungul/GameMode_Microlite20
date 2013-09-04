#!/bin/sh
echo "> git pull"
git pull
echo "remove old zip"
rm GameMode_Microlite20.zip
echo "create new zip"
zip GameMode_Microlite20.zip *
zip -d GameMode_Microlite20.zip build.sh README.md # exclude these files from finished add-on
echo "git add -A"
git add -A
echo "commit"
git commit
echo "push"
git push
echo "done"