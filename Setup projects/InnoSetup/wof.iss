; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!
; DEPENDENCIES: 
; DONE WITH Inno Setup 5 UNICODE!, InnoIDE (editor), 
; isxdl.dll in scripts is a file from IsTool program - required!
#define MyAppNameShort "WOF2_RotL_"
#define MyAppName "Wings of Fury 2 - Return of the legend"
#define MyAppVersion "3.3"
#define MyAppPublisher "Ravelore"
#define MyAppURL "http://www.wingsoffury2.com"
#define MyAppExeName "Wof.exe"
#define MyAppSetupPath "W:\wings\WoF\Setup projects\InnoSetup"
#define MyAppSourcePath "W:\wings\WoF"
#define MyDXRedistVer "directx_mar2009_redist_small"
#define MyDOTNETURL "http://download.microsoft.com/download/5/6/7/567758a3-759e-473e-bf8f-52154438565a/dotnetfx.exe"
#define MyDOTNETRequired "2.0"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppID={{33473D00-7679-478F-9C91-0D7DCB44F7B9}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=false
InfoAfterFile=W:\wings\WoF\Setup projects\InnoSetup\after.txt
OutputDir={#MyAppSetupPath}\output
OutputBaseFilename={#MyAppNameShort}{#MyAppVersion}
;SetupIconFile=WofIcon.ico
Compression=lzma2/Max
SolidCompression=true
AlwaysShowDirOnReadyPage=true
MinVersion=,5.1.2600sp1
VersionInfoCompany=Ravenlore
VersionInfoDescription=WW2 plane simulator and shooter
VersionInfoProductName=Wings of Fury 2 - Return of the legend
VersionInfoProductVersion=3.3
PrivilegesRequired=poweruser

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "basque"; MessagesFile: "compiler:Languages\Basque.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "slovak"; MessagesFile: "compiler:Languages\Slovak.isl"
Name: "slovenian"; MessagesFile: "compiler:Languages\Slovenian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "{#MyAppSourcePath}\WofIcon.ico"; DestDir: "{app}"; Flags: IgnoreVersion
Source: "{#MyAppSourcePath}\WofIconE.ico"; DestDir: "{app}"; Flags: IgnoreVersion
Source: "{#MyAppSourcePath}\Wof.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: {#MyAppSourcePath}\bin\Release\*; DestDir: {app}\bin\release; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "wof_secure,\Wof.vshost.exe,\Wof.blur,\Wof.exe,\none.xml,\ogre.cfg,\custom_levels\*.xml,\levels\*.xml,\survival.dat,*.old,\wof.renamed.xml,*.lnk,\enhanced.dat,\wofconf.dat,\game.dat,\highscores.dat,*.manifest,*.config,*.pdb,*.log,*.manifest,_ReSharper.Wof,_ReSharper.Wof.vshost"
Source: {#MyAppSourcePath}\bin\Release\wof_secure\Wof.exe; DestDir: {app}\bin\release; Flags: ignoreversion recursesubdirs createallsubdirs; 
Source: {#MyAppSourcePath}\bin\Release\wof_secure\EnhancedVersionHelper.exe; DestDir: {app}\bin\release; Flags: ignoreversion recursesubdirs createallsubdirs; 

Source: {#MyAppSourcePath}\media\*; DestDir: {app}\media; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: \materials\textures\ads\*,thumbs.db
Source: {#MyAppSourcePath}\licenses\*; DestDir: {app}\licenses; Flags: ignoreversion recursesubdirs createallsubdirs

Source: {#MyAppSourcePath}\COPYING; DestDir: {app}; Flags: ignoreversion
Source: {#MyAppSourcePath}\readme.txt; DestDir: {app}; Flags: IgnoreVersion
Source: {#MyAppSourcePath}\app.config; DestDir: {app}; Flags: IgnoreVersion

; Dependencies

; VC Redist 2008SP1
Source: {#MyAppSetupPath}\Dependencies\vcredist_x86.exe; DestDir: {app}; Flags: IgnoreVersion deleteafterinstall

; DirectX (march 2009) (only needed files in here)
Source: {#MyAppSetupPath}\Dependencies\{#MyDXRedistVer}\*; DestDir: {tmp}\DIRECTX ;Flags: deleteafterinstall

; managed directx (part of it)
Source: {#MyAppSetupPath}\Dependencies\mdirectx\*; DestDir: {app}\bin\release

; .NET Framework - script needs it
Source: {#MyAppSetupPath}\Dependencies\isxdl.dll; DestDir: {tmp}; Flags: deleteafterinstall


[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\WofIcon.ico"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\WofIcon.ico"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon;  IconFilename: "{app}\WofIcon.ico"
Name: "{commondesktop}\Get Enhanced Version"; Filename: "{app}\bin\release\EnhancedVersionHelper.exe"; Tasks: desktopicon; IconFilename: "{app}\WofIconE.ico"

[Run]
Filename: {app}\vcredist_x86.exe; Parameters: "/q:a /c:""VCREDI~3.EXE /q:a /c:""""msiexec /i vcredist.msi /qb!"""" """; StatusMsg: Installing VC Redistributable 2008
Filename: {tmp}\DIRECTX\DXSETUP.exe; Parameters: "/SILENT"; Flags: skipifsilent; StatusMsg: Updating DirectX 

Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, "&", "&&")}}"; Flags: nowait postinstall skipifsilent
;Filename: "{app}\EnhancedVersionHelper.exe"; Description: "{cm:LaunchProgram,{#StringChange("Get Enhanced Version", "&", "&&")}}"; Flags: nowait postinstall skipifsilent

[Code]
var
	dotnetRedistPath: string;
	downloadNeeded: boolean;
	dotNetNeeded: boolean;
	memoDependenciesNeeded: string;
	Version: TWindowsVersion;

procedure isxdl_AddFile(URL, Filename: PAnsiChar); // changed from PChar which works with NON-UNICODE innosetup
external 'isxdl_AddFile@files:isxdl.dll stdcall';
function isxdl_DownloadFiles(hWnd: Integer): Integer;
external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';
function isxdl_SetOption(Option, Value: PAnsiChar): Integer;
external 'isxdl_SetOption@files:isxdl.dll stdcall';


const
	dotnetRedistURL = '{#MyDOTNETURL}';

//*********************************************************************************
// This is where all starts.
//*********************************************************************************
function InitializeSetup(): Boolean;

begin

	Result := true;
	dotNetNeeded := false;
	GetWindowsVersionEx(Version);
	memoDependenciesNeeded := memoDependenciesNeeded + '      VC redist 2008 SP1 [included]' #13;
	memoDependenciesNeeded := memoDependenciesNeeded + '      DirectX 9.0c March 2009 or newer [included]' #13;

	//*********************************************************************************
	// Check for the existance of .NET 2.0
	//*********************************************************************************
    if (not RegKeyExists(HKLM, 'Software\Microsoft\.NETFramework\policy\v{#MyDOTNETRequired}')) then
		begin
			dotNetNeeded := true;

			if (not IsAdminLoggedOn()) then
				begin
					MsgBox('The game needs the Microsoft .NET Framework {#MyDOTNETRequired} to be installed by an Administrator', mbInformation, MB_OK);
					Result := false;
				end
			else
				begin
					memoDependenciesNeeded := memoDependenciesNeeded + '      .NET Framework {#MyDOTNETRequired} [download], RESTART MAY BE REQUIRED' #13;
					dotnetRedistPath := ExpandConstant('{src}\dotnetfx.exe');
					if not FileExists(dotnetRedistPath) then
						begin
							dotnetRedistPath := ExpandConstant('{tmp}\dotnetfx.exe');
							if not FileExists(dotnetRedistPath) then
								begin
									isxdl_AddFile(dotnetRedistURL, dotnetRedistPath);
									downloadNeeded := true;
								end
						end
						;
					SetIniString('install', 'dotnetRedist', dotnetRedistPath, ExpandConstant('{tmp}\dep.ini'));
				end
		end;

end;

function NextButtonClick(CurPage: Integer): Boolean;

var
  hWnd: Integer;
  ResultCode: Integer;
  ResultXP: boolean;
  Result2003: boolean;

begin

  Result := true;
  ResultXP := true;
  Result2003 := true;

  //*********************************************************************************
  // Only run this at the "Ready To Install" wizard page.
  //*********************************************************************************
  if CurPage = wpReady then
	begin

		hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));

		// don't try to init isxdl if it's not needed because it will error on < ie 3

		//*********************************************************************************
		// Download the .NET 2.0 redistribution file.
		//*********************************************************************************
		if downloadNeeded and (dotNetNeeded = true) then
			begin
				isxdl_SetOption('label', 'Downloading Microsoft .NET Framework {#MyDOTNETRequired}');
				isxdl_SetOption('description', 'This program needs to install the Microsoft .NET Framework {#MyDOTNETRequired}. Please wait while Setup is downloading extra files to your computer.');
				if isxdl_DownloadFiles(hWnd) = 0 then Result := false;
			end;

		//*********************************************************************************
		// Run the install file for .NET Framework 2.0. This is usually dotnetfx.exe
		//*********************************************************************************
        	if (dotNetNeeded = true) then
			begin

				if Exec(ExpandConstant(dotnetRedistPath), '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
					begin

						// handle success if necessary; ResultCode contains the exit code
						if not (ResultCode = 0) then
							begin

								Result := false;

							end
					end
					else
						begin

							// handle failure if necessary; ResultCode contains the error code
							Result := false;

						end
			end;

	end;

end;

//*********************************************************************************
// Updates the memo box shown right before the install actually starts.
//*********************************************************************************
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  s: string;

begin

  if memoDependenciesNeeded <> '' then s := s + 'Dependencies that will be automatically downloaded And installed:' + NewLine + memoDependenciesNeeded + NewLine;
  s := s + MemoDirInfo + NewLine + NewLine;

  Result := s

end;
