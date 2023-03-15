#!/bin/sh

git format-patch -o patches/upgrade/cocona main..upgrade/cocona ./.vscode/ ./adventofcode.csproj 
