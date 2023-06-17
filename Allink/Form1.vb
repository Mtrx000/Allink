Imports System.IO
Imports System.Net
Imports System.Threading.Tasks

'                                                               Allink Project
'                                                                          By Mtrx000
'                                                               CONTACT ME ON DISCORD TO USE THIS CODE
'                                                         Discord: r3vil_                                                                         By Mtrx000
Public Class Form1
    Dim ftpServer As String = "ftp://ftp.com/"
    Dim ftpUsername As String = My.Settings.ftp
    Dim ftpPassword As String = My.Settings.ftpass

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                         By Mtrx000

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Désactiver le bouton pendant l'opération
        Button1.Enabled = False

        ' Exécuter l'opération dans un thread séparé
        Task.Run(Sub()
                     ' Générer un nom aléatoire de 5 caractères pour le dossier
                     Dim folderName As String = GenerateRandomName(5)

                     ' Créer le dossier dans %temp%
                     Dim tempFolderPath As String = Path.Combine(Path.GetTempPath(), folderName)
                     Directory.CreateDirectory(tempFolderPath)

                     ' Créer le fichier index.html dans le dossier temporaire
                     Dim siteUrl As String = TextBox1.Text
                     If IsSiteAllowed(siteUrl) Then
                         Dim indexFilePath As String = Path.Combine(tempFolderPath, "index.html")
                         CreateIndexFile(indexFilePath, siteUrl)
                     Else
                         ' Réactiver le bouton après l'opération avec un MessageBox indiquant que le site n'est pas autorisé
                         Invoke(Sub()
                                    Button1.Enabled = True
                                    MessageBox.Show("Le site spécifié n'est pas autorisé.")
                                End Sub)
                         Return
                     End If

                     ' Envoyer le dossier depuis %temp% vers le serveur FTP
                     SendFolderToFTP(tempFolderPath, folderName)

                     ' Supprimer le dossier temporaire après l'envoi sur le serveur FTP
                     Directory.Delete(tempFolderPath, True)

                     ' Afficher le nom du dossier créé dans un MessageBox sur le thread de l'interface utilisateur
                     Dim folderCreatedMessage As String = "Your Link: yourwebsite.com/" & folderName
                     Invoke(Sub()
                                Button1.Enabled = True
                                MessageBox.Show(folderCreatedMessage)
                            End Sub)
                 End Sub)
    End Sub

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                         By Mtrx000


    Private Function GenerateRandomName(length As Integer) As String
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim random As New Random()
        Dim result As New String(Enumerable.Repeat(chars, length) _
            .Select(Function(s) s(random.Next(s.Length))).ToArray())
        Return result
    End Function

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                         By Mtrx000
    Private Sub CreateIndexFile(filePath As String, siteUrl As String)
        Dim fileContents As String = "<html>" & vbCrLf &
                                "<head>" & vbCrLf &
                                "<title>Allink</title>" & vbCrLf &
                                "<meta http-equiv='refresh' content='5; URL=" & siteUrl & "'>" & vbCrLf &
                                "</head>" & vbCrLf &
                                "<body>" & vbCrLf &
                                "</body>" & vbCrLf &
                                "</html>"
        File.WriteAllText(filePath, fileContents)
    End Sub

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                         By Mtrx000


    Private Function IsSiteAllowed(siteUrl As String) As Boolean
        Dim blockedSites As String() = {"google.com", "yahoo.com"}
        Return Not blockedSites.Contains(siteUrl)
    End Function

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                          By Mtrx000


    Private Sub SendFolderToFTP(folderPath As String, folderName As String)
        Dim request As FtpWebRequest = WebRequest.Create(ftpServer & "/" & folderName)
        request.Method = WebRequestMethods.Ftp.MakeDirectory
        request.Credentials = New NetworkCredential(ftpUsername, ftpPassword)

        Dim response As FtpWebResponse = CType(request.GetResponse(), FtpWebResponse)
        response.Close()

        Dim ftpFolderPath As String = ftpServer & "/" & folderName

        Dim files As String() = Directory.GetFiles(folderPath)
        For Each file In files
            Dim fileName As String = Path.GetFileName(file)
            Dim ftpFilePath As String = ftpFolderPath & "/" & fileName
            UploadFileToFTP(file, ftpFilePath)
        Next
    End Sub

    '                                                               Allink Project
    '                                                                          By Mtrx000
    '                                                               CONTACT ME ON DISCORD TO USE THIS CODE
    '                                                         Discord: r3vil_                                                                        By Mtrx000

    Private Sub UploadFileToFTP(filePath As String, ftpFilePath As String)
        Dim request As FtpWebRequest = WebRequest.Create(ftpFilePath)
        request.Method = WebRequestMethods.Ftp.UploadFile
        request.Credentials = New NetworkCredential(ftpUsername, ftpPassword)

        Using fileStream As FileStream = File.OpenRead(filePath)
            Dim buffer(fileStream.Length - 1) As Byte
            fileStream.Read(buffer, 0, buffer.Length)
            fileStream.Close()

            Using requestStream As Stream = request.GetRequestStream()
                requestStream.Write(buffer, 0, buffer.Length)
                requestStream.Close()
            End Using
        End Using

        Dim response As FtpWebResponse = CType(request.GetResponse(), FtpWebResponse)
        response.Close()
    End Sub
End Class
'                                                               Allink Project
'                                                                          By Mtrx000
'                                                               CONTACT ME ON DISCORD TO USE THIS CODE
'                                                         Discord: r3vil_