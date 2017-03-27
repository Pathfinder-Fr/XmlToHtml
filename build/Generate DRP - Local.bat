@echo off
@set In=..\..\scripts\wikixml\\Pathfinder-RPG
@set Profile=..\Profiles\DRP
@set Out=..\..\scripts\html\Nightly

pushd ..\artifacts

xmltohtml.exe generate "%In%" "%Profile%\Specifics" -t:"%Profile%\template.html" -v:"%Profile%\Overrides" -o:"%out%" -r:"%Profile%\Replaces"

rem robocopy "%Profile%\Includes" "%out%" /NP /NJH
rem "tools\7z.exe" a "%Zip%" "%Out%"

popd