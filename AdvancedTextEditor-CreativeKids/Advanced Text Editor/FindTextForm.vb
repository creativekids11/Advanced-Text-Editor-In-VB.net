Public Class FindTextForm

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Cancels the find function
        Me.Close()
    End Sub

    Private Sub Button19_Click(sender As System.Object, e As System.EventArgs) Handles Button19.Click
        'Finds the text in the document
        Form1.RichTextBoxPrintCtrl1.Find(ComboBox1.Text)

        'OPTIONAL : Form1.RichTextBoxPrintCtrl1.Find(ComboBox1.Text, RichTextBoxFinds.MatchCase)  OR  Form1.RichTextBoxPrintCtrl1.Find(ComboBox1.Text, RichTextBoxFinds.NoHighlight)  OR  Form1.RichTextBoxPrintCtrl1.Find(ComboBox1.Text, RichTextBoxFinds.Reverse)  OR  Form1.RichTextBoxPrintCtrl1.Find(ComboBox1.Text, RichTextBoxFinds.WholeWord)

    End Sub
End Class