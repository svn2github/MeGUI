!define NAME "MeGUI modern media encoder"
!define OUTFILE "megui-setup.exe"
!define PRODUCT_VERSION "0.3.1.1029"
!define PRODUCT_WEB_SITE "www.doom9.net"
!define INPUT_PATH "..\..\megui\trunk\dist\bigdist\"
!define FILE1 "AvisynthWrapper.dll"
!define FILE2 "Changelog.txt"
!define FILE3 "gpl.txt"
!define FILE4 "ICSharpCode.SharpZipLib.dll"
!define FILE5 "megui.exe"
!define FILE6 "MessageBoxExLib.dll"
!define FILE7 "megui.ico"
!define FILE8 "MediaInfo.dll"
!define FILE9 "MediaInfoWrapper.dll"
!define HELP "Data\*.xml"
!define UNINST_NAME "megui-uninstall.exe"
!define MUI_ICON megui.ico
!define MUI_UNICON uninstall.ico
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP megui.bmp
!define MUI_COMPONENTSPAGE_SMALLDESC
!include "MUI.nsh"
!include "Sections.nsh"
!include "LogicLib.nsh"

; ---------------------------------------------------------------------------
; NOTE: this .NSI script is designed for NSIS v2.07+
; ---------------------------------------------------------------------------

Name "${NAME}"
OutFile "${OUTFILE}"
SetCompressor /FINAL /SOLID lzma

SetOverwrite ifnewer
SetDatablockOptimize on ; (can be off)
CRCCheck on ; (can be off)
AutoCloseWindow false ; (can be true for the window go away automatically at end)
ShowInstDetails show ; (can be show to have them shown, or nevershow to disable)
ShowUnInstDetails show ; (can be show to have them shown, or nevershow to disable)
SetDateSave off ; (can be on to have files restored to their orginal date)

!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "French"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "Spanish"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "TradChinese"
!insertmacro MUI_LANGUAGE "Japanese"
!insertmacro MUI_LANGUAGE "Korean"
!insertmacro MUI_LANGUAGE "Italian"
!insertmacro MUI_LANGUAGE "Dutch"
!insertmacro MUI_LANGUAGE "Danish"
!insertmacro MUI_LANGUAGE "Swedish"
!insertmacro MUI_LANGUAGE "Norwegian"
!insertmacro MUI_LANGUAGE "Finnish"
!insertmacro MUI_LANGUAGE "Greek"
!insertmacro MUI_LANGUAGE "Russian"
!insertmacro MUI_LANGUAGE "Portuguese"
!insertmacro MUI_LANGUAGE "Polish"
!insertmacro MUI_LANGUAGE "Ukrainian"
!insertmacro MUI_LANGUAGE "Czech"
!insertmacro MUI_LANGUAGE "Slovak"
!insertmacro MUI_LANGUAGE "Croatian"
!insertmacro MUI_LANGUAGE "Bulgarian"
!insertmacro MUI_LANGUAGE "Hungarian"
!insertmacro MUI_LANGUAGE "Thai"
!insertmacro MUI_LANGUAGE "Romanian"
!insertmacro MUI_LANGUAGE "Latvian"
!insertmacro MUI_LANGUAGE "Macedonian"
!insertmacro MUI_LANGUAGE "Estonian"
!insertmacro MUI_LANGUAGE "Turkish"
!insertmacro MUI_LANGUAGE "Lithuanian"
!insertmacro MUI_LANGUAGE "Slovenian"
!insertmacro MUI_LANGUAGE "Serbian"
!insertmacro MUI_LANGUAGE "SerbianLatin"
!insertmacro MUI_LANGUAGE "Arabic"
!insertmacro MUI_LANGUAGE "Farsi"
!insertmacro MUI_LANGUAGE "Hebrew"
!insertmacro MUI_LANGUAGE "Indonesian"
!insertmacro MUI_LANGUAGE "Mongolian"
!insertmacro MUI_LANGUAGE "Luxembourgish"
!insertmacro MUI_LANGUAGE "Albanian"
!insertmacro MUI_LANGUAGE "Breton"
!insertmacro MUI_LANGUAGE "Belarusian"
!insertmacro MUI_LANGUAGE "Icelandic"
!insertmacro MUI_LANGUAGE "Malay"
!insertmacro MUI_LANGUAGE "Bosnian"
!insertmacro MUI_LANGUAGE "Kurdish"
!insertmacro MUI_LANGUAGE "Irish"
!insertmacro MUI_LANGUAGE "Uzbek"
!insertmacro MUI_LANGUAGE "Galician"
!insertmacro MUI_LANGUAGE "Afrikaans"

; ---------------------------------------------------------------------------

InstallDir $PROGRAMFILES\megui

Section "";

	SetOutPath $INSTDIR
        RMDir /r $SMPROGRAMS\megui

        SetOverwrite on
	File "${INPUT_PATH}${FILE1}"
	File "${INPUT_PATH}${FILE2}"
	File "${INPUT_PATH}${FILE3}"
	File "${INPUT_PATH}${FILE4}"
	File "${INPUT_PATH}${FILE5}"
	File "${INPUT_PATH}${FILE6}"
	File "${FILE7}"
	File "${INPUT_PATH}${FILE8}"
	File "${INPUT_PATH}${FILE9}"
	CreateDirectory $INSTDIR\update_cache
	CreateDirectory $INSTDIR\tools
	CreateDirectory $INSTDIR\logs
	
        SetOutPath "$INSTDIR\Data\"
        File "${INPUT_PATH}..\${HELP}"

	CreateDirectory $SMPROGRAMS\MeGUI
	CreateShortcut "$SMPROGRAMS\MeGUI\Changelog.lnk" $INSTDIR\${FILE2}
	CreateShortcut "$SMPROGRAMS\MeGUI\GPL.lnk" $INSTDIR\${FILE3}
	CreateShortcut "$SMPROGRAMS\MeGUI\MeGUI Modern Media Encoder.lnk" $INSTDIR\${FILE5} "" $INSTDIR\megui.ico
	CreateShortcut "$SMPROGRAMS\MeGUI\Tools.lnk" $INSTDIR\tools
	CreateShortcut "$SMPROGRAMS\MeGUI\Log Files.lnk" $INSTDIR\logs
	CreateShortcut "$SMPROGRAMS\MeGUI\Auto-Update cache.lnk" $INSTDIR\update_cache
	CreateShortcut "$SMPROGRAMS\MeGUI\Uninstall MeGUI.lnk" $INSTDIR\megui-uninstall.exe

	; sets update_cache registry entry
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\MeGUI" "update_cache" "$INSTDIR\update_cache"

	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayName" "${NAME} (remove only)"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "UninstallString" '"$INSTDIR\${UNINST_NAME}"'
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayIcon" "$INSTDIR\megui.exe"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
       
        ; write out uninstaller
	WriteUninstaller "$INSTDIR\${UNINST_NAME}"

SectionEnd ; end of default section


  

; ---------------------------------------------------------------------------

; begin uninstall settings/section
UninstallText "This will uninstall ${NAME} from your system"

Section Uninstall
	
	; add delete commands to delete whatever files/registry keys/etc you installed here.
	Delete /REBOOTOK "$INSTDIR\${FILE1}"
	Delete /REBOOTOK "$INSTDIR\${FILE2}"
	Delete /REBOOTOK "$INSTDIR\${FILE3}"
	Delete /REBOOTOK "$INSTDIR\${FILE4}"
	Delete /REBOOTOK "$INSTDIR\${FILE5}"
	Delete /REBOOTOK "$INSTDIR\${FILE6}"
	Delete /REBOOTOK "$INSTDIR\${FILE7}"
	Delete /REBOOTOK "$INSTDIR\${FILE8}"
	Delete /REBOOTOK "$INSTDIR\${FILE9}"
	Delete "$INSTDIR\${UNINST_NAME}"

	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\MeGUI"
        DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}"

        RMDIR /r "$LOCALAPPDATA\${PRODUCT_WEB_SITE}"
        RMDIR /r "$SMPROGRAMS\MeGUI"
        RMDIR /r "$INSTDIR"

SectionEnd ; end of uninstall section

; ---------------------------------------------------------------------------

Function un.onUninstSuccess
	IfRebootFlag 0 NoReboot
		MessageBox MB_OK \ 
			"A file couldn't be deleted. It will be deleted at next reboot."
	NoReboot:
FunctionEnd

; ---------------------------------------------------------------------------
; eof
; ---------------------------------------------------------------------------
