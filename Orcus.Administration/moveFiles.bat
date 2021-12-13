@echo off
if not exist "libraries" mkdir libraries
if exist "cs-CZ" rmdir /Q /S "cs-CZ"
if exist "de" rmdir /Q /S "de"
if exist "es" rmdir /Q /S "es"
if exist "fr" rmdir /Q /S "fr"
if exist "hu" rmdir /Q /S "hu"
if exist "it" rmdir /Q /S "it"
if exist "ja-JP" rmdir /Q /S "ja-JP"
if exist "pt-BR" rmdir /Q /S "pt-BR"
if exist "ro" rmdir /Q /S "ro"
if exist "ru" rmdir /Q /S "ru"
if exist "sv" rmdir /Q /S "sv"
if exist "zh-Hans" rmdir /Q /S "zh-Hans"
if exist "Orcus.Administration.pdb" del "Orcus.Administration.pdb"
move *.dll libraries
move *.pdb libraries
move *.xml libraries
move *.config libraries
move libraries\Orcus.Administration.exe.config Orcus.Administration.exe.config
move libraries\Orcus.Administration.exe.config Orcus.Administration.exe.config
attrib +h "Orcus.Administration.exe.config"