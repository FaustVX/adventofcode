#!/bin/sh

git apply lib/aoc/patches/upgrade/cocona/*.patch
git commit -A -m "Updated AoC lib"
