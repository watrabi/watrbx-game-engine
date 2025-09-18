rem File to generate all flavors of the bootstrapper

call:generateFlavorFunc Gametest setup-gametest.watrbx.wtf gametest.watrbx.wtf %1
call:generateFlavorFunc Gametest1 setup.gametest1.pizzaboxer.fun www.gametest1.pizzaboxer.fun %1
call:generateFlavorFunc Gametest2 setup.gametest2.pizzaboxer.fun www.gametest2.pizzaboxer.fun %1
call:generateFlavorFunc Gametest3 setup.gametest3.pizzaboxer.fun www.gametest3.pizzaboxer.fun %1
call:generateFlavorFunc Gametest4 setup.gametest4.pizzaboxer.fun www.gametest4.pizzaboxer.fun %1
call:generateFlavorFunc Gametest5 setup.gametest5.pizzaboxer.fun www.gametest5.pizzaboxer.fun %1
call:generateFlavorFunc Sitetest setup-sitetest.watrbx.wtf sitetest.watrbx.wtf %1
call:generateFlavorFunc Sitetest1 setup.sitetest1.pizzaboxer.fun www.sitetest1.pizzaboxer.fun %1
call:generateFlavorFunc Sitetest2 setup.sitetest2.pizzaboxer.fun www.sitetest2.pizzaboxer.fun %1
call:generateFlavorFunc Sitetest3 setup.sitetest3.pizzaboxer.fun www.sitetest3.pizzaboxer.fun %1
call:generateFlavorFunc Sitetest4 setup.sitetest4.pizzaboxer.fun www.sitetest4.pizzaboxer.fun %1

goto:eof

:generateFlavorFunc
echo Generating bootstrappers for %~4

rmdir /S /Q ..\..\UploadBits\Win32-%~4%~1-BootstrapperClient\ 
rmdir /S /Q ..\..\UploadBits\Win32-%~4%~1-BootstrapperQTStudio\ 
rmdir /S /Q ..\..\UploadBits\Win32-%~4%~1-BootstrapperRccService\

xcopy /Y /S ..\..\UploadBits\Win32-%~4-BootstrapperClient\* ..\..\UploadBits\Win32-%~4%~1-BootstrapperClient\ 
xcopy /Y /S ..\..\UploadBits\Win32-%~4-BootstrapperQTStudio\* ..\..\UploadBits\Win32-%~4%~1-BootstrapperQTStudio\ 
xcopy /Y /S ..\..\UploadBits\Win32-%~4-BootstrapperRccService\* ..\..\UploadBits\Win32-%~4%~1-BootstrapperRccService\

Resources\rtc.exe /plhd01="%~2" /plhd02="%~3" /plhd03="..\..\UploadBits\Win32-%~4%~1-BootstrapperClient\RobloxPlayerLauncher.exe" /plhd04="..\..\UploadBits\Win32-%~4%~1-BootstrapperClient\RobloxPlayerLauncher.exe" /F:"Resources\updateBootstrapperRC.rts"
Resources\rtc.exe /plhd01="%~2" /plhd02="%~3" /plhd03="..\..\UploadBits\Win32-%~4%~1-BootstrapperQTStudio\RobloxStudioLauncherBeta.exe" /plhd04="..\..\UploadBits\Win32-%~4%~1-BootstrapperQTStudio\RobloxStudioLauncherBeta.exe" /F:"Resources\updateBootstrapperRC.rts"
Resources\rtc.exe /plhd01="%~2" /plhd02="%~3" /plhd03="..\..\UploadBits\Win32-%~4%~1-BootstrapperRccService\Roblox.exe" /plhd04="..\..\UploadBits\Win32-%~4%~1-BootstrapperRccService\Roblox.exe" /F:"Resources\updateBootstrapperRC.rts"

goto:eof

