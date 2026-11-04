Imports System.Windows.Forms

Public Class DoubleBufferedPictureBox
    Inherits PictureBox

    Public Sub New()
        MyBase.New()
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        Me.UpdateStyles()
    End Sub
End Class
