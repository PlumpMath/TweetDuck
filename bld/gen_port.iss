; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "TweetDuck"
#define MyAppPublisher "chylex"
#define MyAppURL "https://tweetduck.chylex.com"
#define MyAppExeName "TweetDuck.exe"

#define MyAppVersion GetFileVersion("..\bin\x86\Release\TweetDuck.exe")

[Setup]
AppId={{8C25A716-7E11-4AAD-9992-8B5D0C78AE06}
AppName={#MyAppName} Portable
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={sd}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename={#MyAppName}.Portable
VersionInfoVersion={#MyAppVersion}
LicenseFile=.\Resources\LICENSE
SetupIconFile=.\Resources\icon.ico
Uninstallable=no
UsePreviousAppDir=no
PrivilegesRequired=lowest
Compression=lzma
SolidCompression=yes
InternalCompressLevel=max
MinVersion=0,6.1

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\bin\x86\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\x86\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.xml,*.pdb,devtools_resources.pak,d3dcompiler_43.dll,widevinecdmadapter.dll"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall shellexec skipifsilent

[InstallDelete]
Type: filesandordirs; Name: "{app}\plugins\official\design-revert"
Type: filesandordirs; Name: "{app}\portable\storage\TD_Plugins\official\design-revert"

[Code]
var UpdatePath: String;

function TDGetNetFrameworkVersion: Cardinal; forward;

{ Check .NET Framework version on startup, ask user if they want to proceed if older than 4.5.2. }
function InitializeSetup: Boolean;
begin
  UpdatePath := ExpandConstant('{param:UPDATEPATH}')
  
  if TDGetNetFrameworkVersion() >= 379893 then
  begin
    Result := True;
    Exit;
  end;
  
  if (MsgBox('{#MyAppName} requires .NET Framework 4.5.2 or newer,'+#13+#10+'please download it from {#MyAppURL}'+#13+#10+#13+#10'Do you want to proceed with the setup anyway?', mbCriticalError, MB_YESNO or MB_DEFBUTTON2) = IDNO) then
  begin
    Result := False;
    Exit;
  end;
  
  Result := True;
end;

{ Set the installation path if updating. }
procedure InitializeWizard();
begin
  if (UpdatePath <> '') then
  begin
    WizardForm.DirEdit.Text := UpdatePath;
  end;
end;

{ Skip the install path selection page if running from an update installer. }
function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := (PageID = wpSelectDir) and (UpdatePath <> '')
end;

{ Return DWORD value containing the build version of .NET Framework. }
function TDGetNetFrameworkVersion: Cardinal;
var FrameworkVersion: Cardinal;

begin
  if RegQueryDWordValue(HKEY_LOCAL_MACHINE, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', FrameworkVersion) then
  begin
    Result := FrameworkVersion;
    Exit;
  end;
  
  Result := 0;
end;

{ Create a 'makeportable' file if running in portable mode. }
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    while not SaveStringToFile(ExpandConstant('{app}\makeportable'), '', False) do
    begin
      if MsgBox('Could not create a ''makeportable'' file in the installation folder. If the file is not present, the installation will not be fully portable.', mbCriticalError, MB_RETRYCANCEL) <> IDRETRY then
      begin
        break;
      end;
    end;
  end;
end;
