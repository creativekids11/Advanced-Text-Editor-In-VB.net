Imports System.Drawing.Font
Imports System.Drawing.FontFamily
Imports System.Drawing.FontConverter
Public Class Form1
    Dim CurrentFont As FontFamily
    Dim CurrentSize As Single
    Dim CurrentForeColor As Color
    Dim Docname As String

    Private Sub BindCombo()
        'Function to load fonts into ComboBox
        ComboBox1.DrawMode = DrawMode.OwnerDrawFixed
        ComboBox1.Font = New Font("Verdana, 10pt", 10)
        ComboBox1.ItemHeight = 20
        Dim objFontFamily As FontFamily
        Dim objFontCollection As System.Drawing.Text.FontCollection
        objFontCollection = New System.Drawing.Text.InstalledFontCollection()
        For Each objFontFamily In objFontCollection.Families
            ComboBox1.Items.Add(objFontFamily.Name)
        Next
    End Sub

    Private Sub ComboBox1_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ComboBox1.DrawItem
        'Draws the fonts to the ComboBox
        e.DrawBackground()
        If (e.State And DrawItemState.Focus) <> 0 Then
            e.DrawFocusRectangle()
        End If
        Dim objBrush As Brush = Nothing
        Try
            objBrush = New SolidBrush(e.ForeColor)
            Dim _FontName As String = ComboBox1.Items(e.Index)
            Dim _font As Font
            Dim _fontfamily = New FontFamily(_FontName)
            If _fontfamily.IsStyleAvailable(FontStyle.Regular) Then
                _font = New Font(_fontfamily, 14, FontStyle.Regular)
            ElseIf _fontfamily.IsStyleAvailable(FontStyle.Bold) Then
                _font = New Font(_fontfamily, 14, FontStyle.Bold)
            ElseIf _fontfamily.IsStyleAvailable(FontStyle.Italic) Then
                _font = New Font(_fontfamily, 14, FontStyle.Italic)
            End If
            e.Graphics.DrawString(_FontName, _font, objBrush, e.Bounds)
        Catch ex As Exception
        Finally
            If objBrush IsNot Nothing Then
                objBrush.Dispose()
            End If
            objBrush = Nothing
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim result As Integer = MsgBox("Would you want to save the Document?", MsgBoxStyle.YesNo, Title:="Save Document")
        If result = MsgBoxResult.Yes Then
            'Calculates if the document should be updated or saved as a new document
            If Docname = "" Then
                SaveAsDocument()
            Else
                SaveDocument()
            End If
            'Exits the program
            End
            'Don't use "Me.Close()" for this part
        ElseIf result = MsgBoxResult.No Then
            RichTextBoxPrintCtrl1.Clear()
            PanelFileMenu.Hide()
            'Exits the program
            End
            'Don't use "Me.Close()" for this part
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Loads the startup settings (Starting Font, Load Fonts into ComboBox etc.)
        ComboBox1.Text = "Arial"
        CurrentFont = New FontFamily(ComboBox1.Text)
        CurrentSize = "10"
        RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize)
        BindCombo()
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        'Paste function
        RichTextBoxPrintCtrl1.Paste()
        UpdateProperties()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        'Changes the color of the PictureBox to the current ForeColor
        PictureBoxColorSelected.BackColor = CurrentForeColor
        'Sets the Size Label Text
        LabelTextSize.Text = CurrentSize.ToString
        'Counts the words for the Word Count and sets the text for WordCount
        Spelling1.Text = RichTextBoxPrintCtrl1.Text
        LabelWordCount.Text = "WordCount : " & Spelling1.WordCount.ToString
        'Enables/Disables the undo button
        If RichTextBoxPrintCtrl1.CanUndo = True Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
        'Enables/Disables the redo button
        If RichTextBoxPrintCtrl1.CanRedo = True Then
            Button3.Enabled = True
        Else
            Button3.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        'Undo function
        RichTextBoxPrintCtrl1.Undo()
        UpdateProperties()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        'Redo function
        RichTextBoxPrintCtrl1.Redo()
        UpdateProperties()
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        'Cut function
        RichTextBoxPrintCtrl1.Cut()
        UpdateProperties()
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        'Copy function
        RichTextBoxPrintCtrl1.Copy()
        UpdateProperties()
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        'Set the color of the selected text
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            CurrentForeColor = ColorDialog1.Color
            RichTextBoxPrintCtrl1.SelectionColor = CurrentForeColor
            UpdateProperties()
        End If
        UpdateProperties()
    End Sub

    Private Sub Button18_Click(sender As System.Object, e As System.EventArgs) Handles Button18.Click
        'Set the font of the selected text
        FontDialog1.Font = RichTextBoxPrintCtrl1.SelectionFont
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            CurrentFont = FontDialog1.Font.FontFamily
            CurrentSize = FontDialog1.Font.Size
            ComboBox1.Text = CurrentFont.Name.ToString
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize)
        End If
        UpdateProperties()
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles ButtonBold.Click
        'Sets the selected text to bold
        If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Bold Then
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Regular)
        Else
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Bold)
        End If
        UpdateProperties()
    End Sub

    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles ButtonItalic.Click
        'Sets the selected text to italic
        If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Italic Then
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Regular)
        Else
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Italic)
        End If
        UpdateProperties()
    End Sub

    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles ButtonUnderlined.Click
        'Sets the selected text to underline
        If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Underline Then
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Regular)
        Else
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Underline)
        End If
        UpdateProperties()
    End Sub

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles ButtonStrikeout.Click
        'Sets the selected text to strikeout
        If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Strikeout Then
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Regular)
        Else
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize, FontStyle.Strikeout)
        End If
        UpdateProperties()
    End Sub

    Private Sub Button15_Click(sender As System.Object, e As System.EventArgs) Handles ButtonLeft.Click
        'Sets the alignment of the selected line of text to the left
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Left
        UpdateProperties()
    End Sub

    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCenter.Click
        'Sets the alignment of the selected line of text to the center
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Center
        UpdateProperties()
    End Sub

    Private Sub Button17_Click(sender As System.Object, e As System.EventArgs) Handles ButtonRight.Click
        'Sets the alignment of the selected line of text to the right
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Right
        UpdateProperties()
    End Sub

    Private Sub Button19_Click(sender As System.Object, e As System.EventArgs) Handles Button19.Click
        'Superscript
        RichTextBoxPrintCtrl1.SelectionCharOffset = 10
        RichTextBoxPrintCtrl1.SelectedText = RichTextBoxPrintCtrl1.SelectedText
        RichTextBoxPrintCtrl1.SelectionCharOffset = 0
        RichTextBoxPrintCtrl1.SelectionCharOffset = -10
        RichTextBoxPrintCtrl1.SelectionCharOffset = 0
        UpdateProperties()
    End Sub

    Public Sub InsertPicture()
        'Use the code 'InsertPicture()' to show the dialog and insert a picture
        Try
            Dim GetPicture As New OpenFileDialog
            GetPicture.Filter = "PNGs (*.png), Bitmaps (*.bmp), GIFs (*.gif), JPEGs (*.jpg)|*.bmp;*.gif;*.jpg;*.png|PNGs (*.png)|*.png|Bitmaps (*.bmp)|*.bmp|GIFs (*.gif)|*.gif|JPEGs (*.jpg)|*.jpg"
            GetPicture.FilterIndex = 1
            GetPicture.InitialDirectory = "C:\"
            If GetPicture.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim SelectedPicture As String = GetPicture.FileName
                Dim Picture As Bitmap = New Bitmap(SelectedPicture)
                Dim cboard As Object = Clipboard.GetData(System.Windows.Forms.DataFormats.Text)
                Clipboard.SetImage(Picture)
                Dim PictureFormat As DataFormats.Format = DataFormats.GetFormat(DataFormats.Bitmap)
                If RichTextBoxPrintCtrl1.CanPaste(PictureFormat) Then
                    RichTextBoxPrintCtrl1.Paste(PictureFormat)
                End If
                Clipboard.Clear()
                Clipboard.SetText(cboard)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button20_Click(sender As System.Object, e As System.EventArgs) Handles Button20.Click
        'Subscript
        RichTextBoxPrintCtrl1.SelectionCharOffset = -10
        RichTextBoxPrintCtrl1.SelectedText = RichTextBoxPrintCtrl1.SelectedText
        RichTextBoxPrintCtrl1.SelectionCharOffset = 0
        RichTextBoxPrintCtrl1.SelectionCharOffset = 10
        RichTextBoxPrintCtrl1.SelectionCharOffset = 0
        UpdateProperties()
    End Sub

    Private Sub Button21_Click(sender As System.Object, e As System.EventArgs) Handles Button21.Click
        InsertPicture()
        UpdateProperties()
    End Sub

    Private Sub Button22_Click(sender As System.Object, e As System.EventArgs) Handles Button22.Click
        'Inserts the TimeOfDay and the Date in the following format : 00:00:00 PM/AM D-M-Y
        If RichTextBoxPrintCtrl1.Text = "" Then
            RichTextBoxPrintCtrl1.Text = TimeOfDay & " " & DateString
        ElseIf Not RichTextBoxPrintCtrl1.Text = "" Then
            RichTextBoxPrintCtrl1.SelectedText = " " & TimeOfDay & " " & DateString & " "
        End If
        UpdateProperties()
    End Sub

#Region "PrintFunction"
    Private checkPrint As Integer

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        checkPrint = 0
    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        checkPrint = RichTextBoxPrintCtrl1.Print(checkPrint, RichTextBoxPrintCtrl1.TextLength, e)
        If checkPrint < RichTextBoxPrintCtrl1.TextLength Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If
    End Sub
#End Region

    Private Sub Button23_Click(sender As System.Object, e As System.EventArgs) Handles Button23.Click
        'Prints the document
        PrintDocument1.DocumentName = Docname
        PrintDocument1.Print()
    End Sub

    Private Sub SpellChecker_DeletedWord(ByVal sender As Object, ByVal e As NetSpell.SpellChecker.SpellingEventArgs) Handles Spelling1.DeletedWord
        'When deleting a word in the SpellCheck Dialog
        Dim start As Integer = Me.RichTextBoxPrintCtrl1.SelectionStart
        Dim length As Integer = Me.RichTextBoxPrintCtrl1.SelectionLength
        Me.RichTextBoxPrintCtrl1.Select(e.TextIndex, e.Word.Length)
        Me.RichTextBoxPrintCtrl1.SelectedText = ""
        If ((start + length) > Me.RichTextBoxPrintCtrl1.Text.Length) Then
            length = 0
        End If
        Me.RichTextBoxPrintCtrl1.Select(start, length)
    End Sub

    Private Sub SpellChecker_ReplacedWord(ByVal sender As Object, ByVal e As NetSpell.SpellChecker.ReplaceWordEventArgs) Handles Spelling1.ReplacedWord
        'When replacing a word in the SpellCheck Dialog
        Dim start As Integer = Me.RichTextBoxPrintCtrl1.SelectionStart
        Dim length As Integer = Me.RichTextBoxPrintCtrl1.SelectionLength
        Me.RichTextBoxPrintCtrl1.Select(e.TextIndex, e.Word.Length)
        Me.RichTextBoxPrintCtrl1.SelectedText = e.ReplacementWord
        If ((start + length) > Me.RichTextBoxPrintCtrl1.Text.Length) Then
            length = 0
        End If
        Me.RichTextBoxPrintCtrl1.Select(start, length)
    End Sub

    Private Sub Button24_Click(sender As System.Object, e As System.EventArgs) Handles Button24.Click
        'Starts the SpellCheck
        Spelling1.SpellCheck()
    End Sub

    Private Sub Button25_Click(sender As System.Object, e As System.EventArgs) Handles Button25.Click
        'Shows the Find Form
        FindTextForm.Show()
    End Sub

    Private Sub Button26_Click(sender As System.Object, e As System.EventArgs) Handles Button26.Click
        If RichTextBoxPrintCtrl1.SelectedText = "" Then
            'Speaks the entire document (if no text is selected)
            Dim SAPI2
            SAPI2 = CreateObject("SAPI.spvoice")
            SAPI2.Speak(RichTextBoxPrintCtrl1.Text)
            LabelStatus.Text = "Status : " & "Finished speaking document"
        Else
            'Speaks the selected text
            Dim SAPI
            SAPI = CreateObject("SAPI.spvoice")
            SAPI.Speak(RichTextBoxPrintCtrl1.SelectedText)
            LabelStatus.Text = "Status : " & "Finished speaking document"
        End If
        UpdateProperties()
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles ComboBox1.SelectedValueChanged
        'Changes the Font
        If ComboBox1.Text = "" Then
            ComboBox1.Text = "Arial"
            CurrentFont = New FontFamily(ComboBox1.Text)
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize)
        Else
            CurrentFont = New FontFamily(ComboBox1.Text)
            RichTextBoxPrintCtrl1.SelectionFont = New Font(CurrentFont, CurrentSize)
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Shows/Hides the File Menu
        If PanelFileMenu.Visible = True Then
            PanelFileMenu.Hide()
        Else
            PanelFileMenu.Show()
        End If
    End Sub

    Private Sub Button29_Click(sender As System.Object, e As System.EventArgs) Handles Button29.Click
        'Prints the document instantly
        PrintDocument1.DocumentName = Docname
        PrintDocument1.Print()
    End Sub

    Private Sub Button30_Click(sender As System.Object, e As System.EventArgs) Handles Button30.Click
        'Closes the form (duh :P)
        Me.Close()
    End Sub

    Private Sub Button27_Click(sender As System.Object, e As System.EventArgs) Handles Button27.Click
        'Asks the user if the document should be saved
        Dim result As Integer = MsgBox("Would you want to save the Document?", MsgBoxStyle.YesNo, Title:="New Document")
        If result = MsgBoxResult.Yes Then
            'Calculates if the document should be updated or saved as a new document
            If Docname = "" Then
                SaveAsDocument()
            Else
                SaveDocument()
            End If
        ElseIf result = MsgBoxResult.No Then
            RichTextBoxPrintCtrl1.Clear()
            PanelFileMenu.Hide()
        End If
    End Sub

    Public Sub SaveDocument()
        Try
            RichTextBoxPrintCtrl1.SaveFile(Docname)
            LabelStatus.Text = "Status : " & "Document Saved"
        Catch ex As Exception
            LabelStatus.Text = "Status : " & "Document did not save successfully"
        End Try
    End Sub

    Public Sub SaveAsDocument()
        Dim SaveFileDialog1 As New SaveFileDialog
        SaveFileDialog1.Filter = "Rich Text Files|*.rtf|Text Files|*.txt"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            RichTextBoxPrintCtrl1.SaveFile(SaveFileDialog1.FileName)
            Me.Text = "TVHD ATE - " & SaveFileDialog1.FileName.ToString
            Docname = SaveFileDialog1.FileName.ToString
            LabelStatus.Text = "Status : " & "Document Saved"
        End If
    End Sub

    Public Sub OpenDocument()
        Dim OpenFileDialog1 As New OpenFileDialog
        OpenFileDialog1.Filter = "Rich Text Files|*.rtf"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Text = "TVHD ATE - " & OpenFileDialog1.FileName.ToString
            RichTextBoxPrintCtrl1.LoadFile(OpenFileDialog1.FileName)
            Spelling1.Text = RichTextBoxPrintCtrl1.Text
            Docname = OpenFileDialog1.FileName.ToString
            LabelStatus.Text = "Status : " & "Document Loaded"
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        'Calculates if the document should be updated or saved as a new document
        If Docname = "" Then
            SaveAsDocument()
        Else
            SaveDocument()
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        'Saves the document as a new file
        SaveAsDocument()
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Dim result As Integer = MsgBox("Would you want to save the Document?", MsgBoxStyle.YesNo, Title:="Open Document")
        If result = MsgBoxResult.Yes Then
            'Calculates if the document should be updated or saved as a new document
            If Docname = "" Then
                SaveAsDocument()
            Else
                SaveDocument()
            End If
            'Opens a document (duh :P)
            OpenDocument()
        ElseIf result = MsgBoxResult.No Then
            RichTextBoxPrintCtrl1.Clear()
            PanelFileMenu.Hide()
            'Opens a document (duh :P)
            OpenDocument()
        End If
    End Sub

    Private Sub RichTextBoxPrintCtrl1_SelectionChanged(sender As Object, e As System.EventArgs) Handles RichTextBoxPrintCtrl1.SelectionChanged
        LabelStatus.Text = ""
        UpdateProperties()
    End Sub


    'TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD
    Private Sub ButtonAbout_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        'Shows the About Dialog
        AboutForm.ShowDialog()
    End Sub
    'TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD


    Public Sub UpdateProperties()
        'Updates the properties for the selected text
        Try
            CurrentFont = RichTextBoxPrintCtrl1.SelectionFont.FontFamily
            CurrentForeColor = RichTextBoxPrintCtrl1.SelectionColor
            CurrentSize = RichTextBoxPrintCtrl1.SelectionFont.Size
            ComboBox1.Text = CurrentFont.Name.ToString
            LabelTextSize.Text = CurrentSize.ToString
            ColorDialog1.Color = CurrentForeColor
            If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Bold Then
                ButtonBold.BackColor = Color.Gainsboro
            Else
                ButtonBold.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Italic Then
                ButtonItalic.BackColor = Color.Gainsboro
            Else
                ButtonItalic.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Underline Then
                ButtonUnderlined.BackColor = Color.Gainsboro
            Else
                ButtonUnderlined.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionFont.Style = FontStyle.Strikeout Then
                ButtonStrikeout.BackColor = Color.Gainsboro
            Else
                ButtonStrikeout.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Left Then
                ButtonLeft.BackColor = Color.Gainsboro
            Else
                ButtonLeft.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Center Then
                ButtonCenter.BackColor = Color.Gainsboro
            Else
                ButtonCenter.BackColor = Color.WhiteSmoke
            End If
            If RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Right Then
                ButtonRight.BackColor = Color.Gainsboro
            Else
                ButtonRight.BackColor = Color.WhiteSmoke
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxColorSelected_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxColorSelected.Click
        'Set the color of the selected text
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            CurrentForeColor = ColorDialog1.Color
            RichTextBoxPrintCtrl1.SelectionColor = CurrentForeColor
            UpdateProperties()
        End If
        UpdateProperties()
    End Sub
End Class


'TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD

'I hope you enjoyed this project, please use this wisely and make sure you leave a back-link to my YouTube channel or website.
'Please SUBSCRIBE to my YouTube Channel : http://www.youtube.com/user/HDTechView

'TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD ----------- TECHVIEWHD