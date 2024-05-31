@echo off

cd /d "%~dp0"

:注册文件关联
set filename=HanvonPainter.reg

echo Windows Registry Editor Version 5.00>>%filename%
echo.>>%filename%

echo [-HKEY_CLASSES_ROOT\.sddoc]>>%filename%
echo [-HKEY_CLASSES_ROOT\CoolPaint]>>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\.sddoc]>>%filename%
echo @="CoolPaint">>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\.sddoc\DefaultIcon]>>%filename%
echo @="%cd:\=\\%\\CoolPaint.ico">>%filename%
echo.>>%filename%

echo [-HKEY_CLASSES_ROOT\.hw]>>%filename%
echo [-HKEY_CLASSES_ROOT\CoolPaint]>>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\.hw]>>%filename%
echo @="CoolPaint">>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\.hw\DefaultIcon]>>%filename%
echo @="%cd:\=\\%\\CoolPaint.ico">>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\CoolPaint]>>%filename%
echo "filetype"="CoolPaint">>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\CoolPaint\DefaultIcon]>>%filename%
echo @="%cd:\=\\%\\CoolPaint.ico">>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\CoolPaint\shell]>>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\CoolPaint\shell\Open]>>%filename%
echo.>>%filename%

echo [HKEY_CLASSES_ROOT\CoolPaint\shell\Open\command]>>%filename%
echo @="%cd:\=\\%\\CoolPaint.exe %%1">>%filename%
echo.>>%filename%

regedit -s "%cd%\%filename%"

del %filename%
