!define NAME "MeGUI"
!define OUTFILE "megui-setup-x86.exe"
!define PRODUCT_VERSION "0.3.4.15"
!define PRODUCT_WEB_SITE "www.doom9.net"
!define INPUT_PATH "..\..\megui\trunk\bin\x86\Release"
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

Name "MeGUI ${PRODUCT_VERSION}"
OutFile "${OUTFILE}"
SetCompressor /FINAL /SOLID lzma

RequestExecutionLevel admin ; needed on Vista/Seven
SetDatablockOptimize on ; (can be off)
CRCCheck on ; (can be off)
AutoCloseWindow false ; (can be true for the window go away automatically at end)
ShowInstDetails show ; (can be show to have them shown, or nevershow to disable)
ShowUnInstDetails nevershow ; (can be show to have them shown, or nevershow to disable)
SetDateSave off ; (can be on to have files restored to their orginal date)

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "${INPUT_PATH}\gpl.txt"
#!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

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

InstallDir "$PROGRAMFILES\megui"

Section "MeGUI";

	SetOutPath "$INSTDIR"
	RMDir /r "$SMPROGRAMS\megui"

	SetOverwrite on
	File "${INPUT_PATH}\AvisynthWrapper.dll"
	File "${INPUT_PATH}\Changelog.txt"
	File "${INPUT_PATH}\gpl.txt"
	File "${INPUT_PATH}\ICSharpCode.SharpZipLib.dll"
	File "${INPUT_PATH}\megui.exe"
	File "${INPUT_PATH}\MessageBoxExLib.dll"
	File "${INPUT_PATH}\LinqBridge.dll"

	CreateDirectory "$INSTDIR\update_cache"
	CreateDirectory "$INSTDIR\tools"
	CreateDirectory "$INSTDIR\logs"

	SetOutPath "$INSTDIR\data\"
	File "${INPUT_PATH}\data\ContextHelp.xml"

	CreateDirectory "$SMPROGRAMS\${NAME}\"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Changelog.lnk" "$INSTDIR\Changelog.txt"
	CreateShortcut  "$SMPROGRAMS\${NAME}\GPL.lnk" "$INSTDIR\gpl.txt"
	CreateShortcut  "$SMPROGRAMS\${NAME}\MeGUI Modern Media Encoder.lnk" "$INSTDIR\megui.exe" "" "$INSTDIR\megui.exe"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Tools.lnk" "$INSTDIR\tools"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Log Files.lnk" "$INSTDIR\logs"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Auto-Update cache.lnk" "$INSTDIR\update_cache"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Uninstall MeGUI.lnk" "$INSTDIR\megui-uninstall.exe"


	; sets update_cache registry entry
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\MeGUI" "update_cache" "$INSTDIR\update_cache"

	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayName" "${NAME} (remove only)"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "UninstallString" '"$INSTDIR\megui-uninstall.exe"'
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayIcon" "$INSTDIR\megui.exe"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "Publisher" "MeGUI Team"

	; delete old registry entry when updating
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\MeGUI modern media encoder"

    ; write out uninstaller
	WriteUninstaller "$INSTDIR\megui-uninstall.exe"

SectionEnd ; end of default section


; ---------------------------------------------------------------------------
; begin uninstall settings/section
UninstallText "This will uninstall ${NAME} from your system"

Section Uninstall

	; add delete commands to delete whatever files/registry keys/etc you installed here.
	Delete /REBOOTOK "$INSTDIR\AvisynthWrapper.dll"
	Delete /REBOOTOK "$INSTDIR\Changelog.txt"
	Delete /REBOOTOK "$INSTDIR\gpl.txt"
	Delete /REBOOTOK "$INSTDIR\ICSharpCode.SharpZipLib.dll"
	Delete /REBOOTOK "$INSTDIR\megui.exe"
	Delete /REBOOTOK "$INSTDIR\MessageBoxExLib.dll"
	Delete /REBOOTOK "$INSTDIR\megui.ico"
	Delete /REBOOTOK "$INSTDIR\MediaInfo.dll"
	Delete /REBOOTOK "$INSTDIR\MediaInfoWrapper.dll"
	Delete /REBOOTOK "$INSTDIR\LinqBridge.dll"        
	Delete "$INSTDIR\megui-uninstall.exe"

	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\MeGUI"
	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}"

	RMDIR /r "$LOCALAPPDATA\${PRODUCT_WEB_SITE}"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Changelog.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\GPL.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\MeGUI Modern Media Encoder.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Tools.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Log Files.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Auto-Update cache.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Uninstall MeGUI.lnk"
	RMDIR "$SMPROGRAMS\${NAME}"
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
