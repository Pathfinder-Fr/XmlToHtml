@echo off
@set In=..\Db\Raw\Out\Pathfinder-RPG
@set Profile=.\Profiles\DRP
@set Zip=..\Db\Raw\Static.7z
@set Out=..\Static\Nightly

"bin\xmltohtml.exe" generate "%In%" "%Profile%\Specifics" -t:"%Profile%\template.html" -v:"%Profile%\Overrides" -o:"%out%" -r:"%Profile%\Replaces"

robocopy "%Profile%\Includes" "%out%" /NP /NJH

"tools\7z.exe" a "%Zip%" "%Out%"