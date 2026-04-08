git add .
set note=%date%%time%;
set note=%note: =%
git commit -m %note%
set var="b"
git pull origin %1 2>&1 && git push -u origin %1 || set var="" && set /p var=use local or server(useserver):
if /i "%var%"=="useserver" goto y
if /i %var%=="b" goto b
git reset --hard
pause
exit
:y
git reset --hard FETCH_HEAD
git pull origin %1
exit
:b