#!/bin/sh
git pull
rm GameMode_Microlite20.zip
zip GameMode_Microlite20.zip *
zip -d GameMode_Microlite20.zip build.sh README.md # exclude these files from finished add-on
git add -A
git commit
git push
