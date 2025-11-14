#!/usr/bin/env bash


shopt -s globstar

if [[ $1 == "--debug" ]]; then
	shift
	csc -debug+ -optimize- -define:DEBUG -nowarn:8632 **/*.cs
else
	csc -nowarn:8632 **/*.cs
fi

if [[ $? -ne 0 ]]; then
	exit 1
fi

mono $@ main.exe

while [[ $# -gt 0 ]]; do
	case $1 in
		--profile=*)
			mprof-report output.mlpd | less
			shift
			;;
	esac
done
