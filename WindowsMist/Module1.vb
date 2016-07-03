'Option Explicit On

Imports System
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Net.Mime.MediaTypeNames

Module Module1

    'Dim steam As String = "https://ninite.com/steam/"
    Dim FilePath As String = CurDir()

    'Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
    '(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
                     (ByVal hWnd As IntPtr, ByVal hWndChildAfterA As IntPtr, ByVal lpszClass As String, ByVal lpszWindow As String) As IntPtr
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
                     (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As String) As IntPtr
    Const WM_SETTEXT As Integer = &HC

    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
(ByVal lpClassName As String, ByVal lpWindowName As String) As Long



    'Dim steam As String = "https://ninite.com/steam/ninite.exe"
    'Dim fileName As String = "steam.exe"
    Dim myStringWebResource As String = Nothing
    ' Create a new WebClient instance.
    Dim myWebClient As New WebClient()

    Dim cDrive As System.IO.DriveInfo
    Dim dDrive As System.IO.DriveInfo

    Dim cSpace As Double
    Dim dSpace As Double

    Dim mst As String = "C:\Users\Default\AppData\Roaming\bg\WindowsMist.png"

    Dim pic As New Bitmap(1920, 1080)

    Sub Main()



        Try


            If Directory.Exists("C:\Users\Default\AppData\Roaming\bg\") = False Then
                Directory.CreateDirectory("C:\Users\Default\AppData\Roaming\bg\")
                My.Resources.WindowsMist.Save(mst)
            End If




            cDrive = My.Computer.FileSystem.GetDriveInfo("C:\")
            dDrive = My.Computer.FileSystem.GetDriveInfo("D:\")

            'Dim p As New ProcessStartInfo

            Dim Everest_Registry As Microsoft.Win32.RegistryKey =
    My.Computer.Registry.CurrentUser.OpenSubKey("Software\Valve\Steam")
            If Everest_Registry Is Nothing Then
                'key does not exist
                Console.WriteLine("Thank you for choosing Windows Mist.          " & vbCrLf &
                "" & vbCrLf &
                "Windows Mist is the equivalent to SteamOS only for Windows.     " & vbCrLf &
                "This allows you to turn your machine to a functioning SteamBox  " & vbCrLf &
                "without the hassle of installing the linux version of Steam OS  " & vbCrLf &
                "on your computer.  Windows Mist solely designed to convert any  " & vbCrLf &
                "fresh install of windows to run steam in big picture mode.      " & vbCrLf &
                "This is converting windows to a partial console. UAC has been   " & vbCrLf &
                "disabled. For further assistance please head to the forum.      " & vbCrLf &
                "http://windowsmist.lefora.com/" & vbCrLf &
                "" & vbCrLf &
                "================================================================" & vbCrLf &
                "DO NOT USE THIS MACHINE FOR PERESONAL USE. YOU HAVE BEEN WARNED." & vbCrLf &
                "================================================================" & vbCrLf &
                "" & vbCrLf &
                "Press Enter to continue.")

                Console.ReadLine()

                'Set UAC from Batch file.
                If Directory.Exists("C:\Users\Default\AppData\Roaming\uac\") = False Then
                    Directory.CreateDirectory("C:\Users\Default\AppData\Roaming\uac\")
                End If
                Dim uac As String = "C:\Users\Default\AppData\Roaming\uac\setUAC.bat"
                System.IO.File.WriteAllText(uac, My.Resources.setUAC)
                Process.Start(uac)

                ' We need to set our auto login through the registry.
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoAdminLogon", 1, Microsoft.Win32.RegistryValueKind.DWord)

                Console.WriteLine("Checking directory.")

                If Directory.Exists("C:\Users\Default\AppData\Roaming\power\") = False Then
                    Directory.CreateDirectory("C:\Users\Default\AppData\Roaming\power\")
                End If

                Console.WriteLine("Setting the power options.")


                'Set our power options.
                Dim power As String = "C:\Users\Default\AppData\Roaming\power\Powercfg.bat"
                System.IO.File.WriteAllText(power, My.Resources.Powercfg)
                Process.Start(power)

                Call downloadSteam()
            Else
                'Steam is already installed. lets launch steam.

                Console.WriteLine("launching steam.")

                Call startSteam()

                'My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432NODE\Valve\Steam", "InstallPath", "D:\Steam")

            End If

        Catch ex As Exception
            Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace)
            Console.ReadLine()

            Exit Sub

        End Try

    End Sub

    Private Sub startSteam()
        'Locate and start steam.
        Process.Start("C:\program files\steam\steam.exe")
        Exit Sub
    End Sub


    Private Sub downloadSteam()
        Try


            'This should copy the resource to the specified location.
            'Process.Start(steam)
            Console.WriteLine("Checking directory.")

            If Directory.Exists("C:\Users\Default\AppData\Roaming\steam\") = False Then
                Directory.CreateDirectory("C:\Users\Default\AppData\Roaming\steam\")
            End If

            Console.WriteLine("Running Ninite Steam.")


            Dim filename As String = "C:\Users\Default\AppData\Roaming\steam\NiniteSteamInstaller.exe"
            System.IO.File.WriteAllBytes(filename, My.Resources.NiniteSteamInstaller)
            'If you want to then run it:
            Process.Start(filename)

            'Check if our ninite is done.

            Dim proc As Boolean = False

            Do Until proc = True
                Dim p() As Process
                p = Process.GetProcessesByName("NiniteSteamInstaller")
                If p.Count > 0 Then
                    ' Process is running
                    'Console.WriteLine("NiNite is installing.")

                Else
                    'Console.WriteLine("Ninite completed." & p.Count)
                    proc = True 'exit do to continue.
                End If
            Loop

            Call installDX()

        Catch ex As Exception
            Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace)
            Console.ReadLine()
            Exit Sub

        End Try
    End Sub

    Private Sub installDX()
        Try


            Console.WriteLine("Checking directory.")

            If Directory.Exists("C:\Users\Default\AppData\Roaming\dxweb\") = False Then
                Directory.CreateDirectory("C:\Users\Default\AppData\Roaming\dxweb\")
            End If

            Console.WriteLine("Running standard DirectX 9 installer.")


            Dim filename As String = "C:\Users\Default\AppData\Roaming\dxweb\dxwebsetup.exe"
            System.IO.File.WriteAllBytes(filename, My.Resources.dxwebsetup)
            'If you want to then run it:
            Process.Start(filename)

            Dim proc As Boolean = False
            Process.Start("C:\program files\steam\steam.exe")

            Do Until proc = True
                Dim p() As Process
                p = Process.GetProcessesByName("dxwebsetup")
                If p.Count > 0 Then
                    ' Process is running
                    ' Console.WriteLine("dxwsetup is running.")

                Else

                    proc = True 'exit do to continue.
                End If
            Loop


            'Console.WriteLine("Press Enter once DX has been installed to continue." & vbCrLf & "Press Enter to continue.")
            ' Console.ReadLine()

            Call SetInstallPath()

        Catch ex As Exception
            Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace)
            Console.ReadLine()
            Exit Sub
        End Try

    End Sub

    Private Sub SetInstallPath()
        Try
            Console.WriteLine("Running Steam update and first time launch to add registry settings." & vbCrLf & "DO NOT CLOSE THE APPLICATION UNTIL THE LOGIN SCREEN APPEARS.")

            Dim proc As Boolean = False

            Do Until proc = True
                Dim p() As Process
                p = Process.GetProcessesByName("Steam")
                If p.Count > 0 Then
                    ' Process is running
                    'Console.WriteLine("Steam is updating.")
                Else
                    ' Console.WriteLine("Steam Finished.")
                    proc = True 'exit do to continue.
                End If
            Loop

            'Console.WriteLine("Steam up to date.  Press Entere to continue.")
            'Console.ReadLine()

            'Shell("tskill team.exe")

            cSpace = cDrive.TotalSize * 0.000000001
            dSpace = dDrive.TotalSize * 0.000000001
            If cSpace < 500 Then
                If dSpace < 500 Then
                    MsgBox("Insufficiant Storage space.  Please install a 500GB HDD or greater.  System will shut down." & cSpace)
                    Console.WriteLine("Shutting down system.")
                    'Console.ReadLine()

                Else
                    'We will use D Drive as the drive itself has enough space to continue. 64Bit ONLY
                    Dim architecture As Integer = Runtime.InteropServices.Marshal.SizeOf(GetType(IntPtr)) * 8

                    If architecture = 64 Then
                        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "InstallPath", "D:\Steam", Microsoft.Win32.RegistryValueKind.String)
                        Console.WriteLine("Set Steam installpath to drive (D:\Steam).")
                    Else
                        Console.WriteLine("Using default settings.")
                    End If

                    Call BigPictureMode()


                End If
                MsgBox("Insufficiant Storage space.  Must be greater than 500GB or install a second drive. " & cSpace)
                Process.Start("C:\Windows\System32\cmd.exe", "shutdown /s")
            Else
                'We will use C Drive as the drive itself has enough space to continue. 
                MsgBox("You meet the requirments.")
                Call BigPictureMode()

            End If

        Catch ex As Exception

            If ex.Message.Contains("The device is not ready.") Then
                Console.WriteLine("Using default settings. Your HDD could not be determined. Using C:\ as default." & vbCrLf & "Press Enter to continue.")
                Console.ReadLine()
                Call BigPictureMode()

            Else
                Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace & vbCrLf & "Press Enter to continue.")
                Console.ReadLine()
                Exit Sub
            End If


        End Try
    End Sub
    Private Sub BigPictureMode()
        Try


            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "StartupMode", 1, Microsoft.Win32.RegistryValueKind.DWord)

            'Let's Copy Windows Mist from the "USB drive" to another directory to auto launch and run Steam and reeboot windows. 

            Call restartWindows()



        Catch ex As Exception
            Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace)
            Console.ReadLine()
            Exit Sub

        End Try
    End Sub

    Public Sub restartWindows()

        Try


            Dim windowsMist As String = CurDir()
            If File.Exists("C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\WindowsMist.exe") = False Then
                My.Computer.FileSystem.CopyFile(windowsMist & "\WindowsMist.exe", "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\WindowsMist.exe")
            End If
            Console.WriteLine("Setup is complete. You may now press Enter to restart the machine.")
            Console.ReadLine()


            'This restarts windows.
            Process.Start("C:\Windows\System32\cmd.exe", "shutdown /r")
        Catch ex As Exception
            Console.WriteLine(ex.Message & " Stack Trace:" & vbCrLf & ex.StackTrace)
            Console.ReadLine()
            Exit Sub

        End Try
    End Sub

End Module
