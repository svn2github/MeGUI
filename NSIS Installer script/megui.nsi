!define NAME "MeGUI modern media encoder"
!define OUTFILE "megui-install.exe"
!define INPUT_PATH "..\trunk\dist\bigdist\"
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
!define MUI_UNICON megui.ico
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

        SetOutPath "$INSTDIR\Data\"
        File "${INPUT_PATH}..\${HELP}"

	CreateDirectory $SMPROGRAMS\megui
	CreateShortcut "$SMPROGRAMS\megui\changelog.lnk" $INSTDIR\${FILE2}
	CreateShortcut "$SMPROGRAMS\megui\gpl.lnk" $INSTDIR\${FILE3}
	CreateShortcut "$SMPROGRAMS\megui\meGUI modern media encoder.lnk" $INSTDIR\${FILE5} "" $INSTDIR\x264.ico
	CreateShortcut "$SMPROGRAMS\megui\uninstall megui.lnk" $INSTDIR\megui-uninstall.exe

	; write out uninstaller
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayName" "${NAME} (remove only)"
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "UninstallString" '"$INSTDIR\${UNINST_NAME}"'
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
        RMDir /r "$INSTDIR"
   
	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}"
	RMDir /r "$SMPROGRAMS\megui"


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
