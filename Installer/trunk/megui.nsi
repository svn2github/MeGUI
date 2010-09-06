!include "version.nsi"

!define NAME "MeGUI"
!define OUTFILE "MeGUI-setup-x86.exe"
!define PRODUCT_VERSION "${MeGUI_VERSION}"
!define INPUT_PATH "..\..\MeGUI\trunk\bin\x86\Release"
!define MUI_ICON "..\..\MeGUI\trunk\app.ico"
!define MUI_UNICON uninstall.ico
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP MeGUI.bmp
!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN "$INSTDIR\MeGUI.exe"
!include "MUI.nsh"
!include "Sections.nsh"
!include "LogicLib.nsh"


Name "MeGUI ${MeGUI_VERSION}"
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
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

VIProductVersion "${MeGUI_VERSION}"
VIAddVersionKey "CompanyName" "MeGUI Team"
VIAddVersionKey "ProductName" "MeGUI" 
VIAddVersionKey "ProductVersion" "${MeGUI_VERSION}"
VIAddVersionKey "FileDescription" "MeGUI installer"
VIAddVersionKey "FileVersion" "${MeGUI_VERSION}"
VIAddVersionKey "LegalCopyright" "MeGUI Team"
BrandingText " "

; ---------------------------------------------------------------------------

InstallDir "$PROGRAMFILES\MeGUI"

Section "MeGUI";

	SetOutPath "$INSTDIR"
	RMDir /r "$SMPROGRAMS\MeGUI"

	SetOverwrite on
	File "${INPUT_PATH}\Changelog.txt"
	File "${INPUT_PATH}\gpl.txt"
	File "${INPUT_PATH}\ICSharpCode.SharpZipLib.dll"
	File "${INPUT_PATH}\MeGUI.exe"
	File "${INPUT_PATH}\MessageBoxExLib.dll"
	File "${INPUT_PATH}\LinqBridge.dll"

	SetOutPath "$INSTDIR\data\"
	File "${INPUT_PATH}\data\ContextHelp.xml"

	CreateDirectory "$SMPROGRAMS\${NAME}\"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Changelog.lnk" "$INSTDIR\Changelog.txt"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Log Files.lnk" "$INSTDIR\logs"
	CreateShortcut  "$SMPROGRAMS\${NAME}\MeGUI Modern Media Encoder.lnk" "$INSTDIR\MeGUI.exe"
	CreateShortcut  "$SMPROGRAMS\${NAME}\Uninstall MeGUI.lnk" "$INSTDIR\MeGUI-uninstall.exe"


	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayName" "${NAME} (remove only)"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "UninstallString" '"$INSTDIR\MeGUI-uninstall.exe"'
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayIcon" "$INSTDIR\MeGUI.exe"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayVersion" "${MeGUI_VERSION}"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "URLInfoAbout" "www.doom9.net"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "Publisher" "MeGUI Team"

	; delete old registry entry when updating
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\MeGUI modern media encoder"

	; write out uninstaller
	WriteUninstaller "$INSTDIR\MeGUI-uninstall.exe"

SectionEnd ; end of default section


; ---------------------------------------------------------------------------
; begin uninstall settings/section
UninstallText "This will uninstall ${NAME} from your system"

Section Uninstall

	; add delete commands to delete whatever files/registry keys/etc you installed here.
	MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Do you want to remove all files?" IDYES deleteall
	
	Delete /REBOOTOK "$INSTDIR\AutoUpdate.xml"
	Delete /REBOOTOK "$INSTDIR\AvisynthWrapper.dll"
	Delete /REBOOTOK "$INSTDIR\Changelog.txt"
	Delete /REBOOTOK "$INSTDIR\gpl.txt"
	Delete /REBOOTOK "$INSTDIR\HdBrStreamExtractor.txt"
	Delete /REBOOTOK "$INSTDIR\ICSharpCode.SharpZipLib.dll"
	Delete /REBOOTOK "$INSTDIR\LinqBridge.dll"	
	Delete /REBOOTOK "$INSTDIR\MediaInfo.dll"
	Delete /REBOOTOK "$INSTDIR\MediaInfoWrapper.dll"
	Delete /REBOOTOK "$INSTDIR\MeGUI.exe"
	Delete /REBOOTOK "$INSTDIR\MeGUI.ico"
	Delete "$INSTDIR\MeGUI-uninstall.exe"
	Delete /REBOOTOK "$INSTDIR\MessageBoxExLib.dll"
	Delete /REBOOTOK "$INSTDIR\updatecopier.exe" 
	Delete /REBOOTOK "$INSTDIR\upgrade.xml" 
	RMDIR /r "$INSTDIR\data"
	RMDIR /r "$INSTDIR\extra"
	RMDIR /r "$INSTDIR\tools"
	RMDIR /r "$INSTDIR\update_cache"
	goto final
		
	deleteall:
		RMDIR /r "$INSTDIR"
		goto final

	final:
		DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\MeGUI"
		DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}"

	RMDIR /r "$LOCALAPPDATA\www.doom9.net"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Changelog.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\GPL.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\MeGUI Modern Media Encoder.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Tools.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Log Files.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Auto-Update cache.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\${NAME}\Uninstall MeGUI.lnk"
	RMDIR "$SMPROGRAMS\${NAME}"

SectionEnd ; end of uninstall section

; ---------------------------------------------------------------------------

Function .onInit
System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "MeGUI_mutex") i .R0'
IntCmp $R0 0 notRunning
	System::Call 'kernel32::CloseHandle(i $R0)'
	MessageBox MB_OK|MB_ICONEXCLAMATION "MeGUI is running. Please close it first." /SD IDOK
	Abort
notRunning:
FunctionEnd

Function un.onInit
System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "MeGUI_mutex") i .R0'
IntCmp $R0 0 notRunning
	System::Call 'kernel32::CloseHandle(i $R0)'
	MessageBox MB_OK|MB_ICONEXCLAMATION "MeGUI is running. Please close it first." /SD IDOK
	Abort
notRunning:
FunctionEnd

Function un.onUninstSuccess
	IfRebootFlag 0 NoReboot
		MessageBox MB_OK \ 
			"A file couldn't be deleted. It will be deleted at next reboot."
	NoReboot:
FunctionEnd
