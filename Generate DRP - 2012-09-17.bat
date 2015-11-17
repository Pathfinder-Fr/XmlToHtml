@echo off
@set In=..\Db\Raw\WikiXml-2012-09-17\Pathfinder-RPG
@set Profile=.\Profiles\DRP
@set Zip=..\Db\Raw\Static-2012-09-07.7z
@set Out=..\Static\2012-09-17

"bin\xmltohtml.exe" generate "%In%" "%Profile%\Specifics" -t:"%Profile%\template.html" -v:"%Profile%\Overrides" -o:"%out%" -r:"%Profile%\Replaces" > "Generate.log"

robocopy "%Profile%\Includes" "%out%" /NP /NJH /TEE /LOG+:"Generate.log"

"tools\7z.exe" a "%Zip%" "%Out%" >> "Generate.log"