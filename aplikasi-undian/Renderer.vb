Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Collections.Generic

Public Class Renderer
    ' Palet warna modern, kontras, dan cerah untuk irisan roda
    Private Shared ReadOnly SliceColors As Color() = {
        Color.FromArgb(230, 57, 70),    ' Crimson Red
        Color.FromArgb(244, 162, 97),   ' Sandy Orange
        Color.FromArgb(42, 157, 143),   ' Emerald Teal
        Color.FromArgb(37, 99, 235),    ' Royal Blue
        Color.FromArgb(155, 93, 229),   ' Purple Amethyst
        Color.FromArgb(241, 91, 181),   ' Magenta Pink
        Color.FromArgb(0, 187, 249),    ' Vivid Cyan
        Color.FromArgb(234, 179, 8),    ' Bright Amber
        Color.FromArgb(16, 185, 129),   ' Vibrant Green
        Color.FromArgb(238, 108, 77),   ' Coral
        Color.FromArgb(131, 56, 236),   ' Violet
        Color.FromArgb(71, 85, 105),    ' Slate Grey
        Color.FromArgb(255, 190, 11),   ' Warm Yellow
        Color.FromArgb(17, 138, 178),   ' Ocean Blue
        Color.FromArgb(231, 111, 81),   ' Burnt Sienna
        Color.FromArgb(6, 214, 160)     ' Mint Green
    }

    ''' <summary>
    ''' Menggambar seluruh komponen roda undian, irisan nama peserta, center hub, dan jarum penunjuk.
    ''' </summary>
    Public Shared Sub Render(g As Graphics, bounds As Rectangle, participants As IList(Of String), currentAngle As Single)
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit
        g.InterpolationMode = InterpolationMode.HighQualityBicubic
        g.PixelOffsetMode = PixelOffsetMode.HighQuality

        Dim cx As Single = bounds.Width / 2.0F
        Dim cy As Single = bounds.Height / 2.0F
        Dim radius As Single = Math.Min(cx, cy) - 22.0F ' Sisakan ruang untuk jarum dan rim
        Dim wheelRect As New RectangleF(cx - radius, cy - radius, radius * 2.0F, radius * 2.0F)

        ' 1. Gambar bayangan luar roda (Shadow effect)
        Using shadowBrush As New SolidBrush(Color.FromArgb(35, 0, 0, 0))
            g.FillEllipse(shadowBrush, cx - radius + 3, cy - radius + 5, radius * 2.0F, radius * 2.0F)
        End Using

        ' 2. Gambar Roda (Irisan / Segmen)
        If participants Is Nothing OrElse participants.Count = 0 Then
            DrawEmptyWheel(g, wheelRect, cx, cy)
        ElseIf participants.Count = 1 Then
            DrawSingleParticipantWheel(g, wheelRect, cx, cy, radius, participants(0))
        Else
            DrawMultiParticipantWheel(g, wheelRect, cx, cy, radius, participants, currentAngle)
        End If

        ' 3. Gambar Lingkaran Rim Luar (Outer Rim dengan aksen stud/pin)
        DrawOuterRim(g, wheelRect, cx, cy, radius)

        ' 4. Gambar Pusat Roda (Center Hub Cap)
        DrawCenterHub(g, cx, cy)

        ' 5. Gambar Jarum Penunjuk Statis (Pointer di atas tengah / 270 derajat)
        DrawPointer(g, cx, cy - radius)
    End Sub

    ''' <summary>
    ''' Tampilan roda saat belum ada peserta.
    ''' </summary>
    Private Shared Sub DrawEmptyWheel(g As Graphics, rect As RectangleF, cx As Single, cy As Single)
        Using bgBrush As New SolidBrush(Color.FromArgb(240, 243, 246))
            g.FillEllipse(bgBrush, rect)
        End Using
        Using borderPen As New Pen(Color.FromArgb(203, 213, 225), 3.0F)
            g.DrawEllipse(borderPen, rect)
        End Using

        Using font As New Font("Segoe UI", 12.0F, FontStyle.Italic)
            Using textBrush As New SolidBrush(Color.FromArgb(100, 116, 139))
                Using sf As New StringFormat()
                    sf.Alignment = StringAlignment.Center
                    sf.LineAlignment = StringAlignment.Center
                    g.DrawString("Daftar peserta kosong" & vbCrLf & "Tambahkan nama untuk memulai", font, textBrush, rect, sf)
                End Using
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Tampilan roda saat hanya ada 1 peserta.
    ''' </summary>
    Private Shared Sub DrawSingleParticipantWheel(g As Graphics, rect As RectangleF, cx As Single, cy As Single, radius As Single, name As String)
        Using brush As New SolidBrush(SliceColors(0))
            g.FillEllipse(brush, rect)
        End Using
        Using penWhite As New Pen(Color.White, 2.0F)
            g.DrawEllipse(penWhite, rect)
        End Using

        Using font As New Font("Segoe UI", 14.0F, FontStyle.Bold)
            Using textBrush As New SolidBrush(Color.White)
                Using sf As New StringFormat()
                    sf.Alignment = StringAlignment.Center
                    sf.LineAlignment = StringAlignment.Center
                    g.DrawString(name, font, textBrush, rect, sf)
                End Using
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Menggambar irisan roda untuk multi-peserta.
    ''' </summary>
    Private Shared Sub DrawMultiParticipantWheel(g As Graphics, rect As RectangleF, cx As Single, cy As Single, radius As Single, participants As IList(Of String), currentAngle As Single)
        Dim count As Integer = participants.Count
        Dim sweepAngle As Single = 360.0F / count

        ' Tentukan ukuran font sesuai kepadatan peserta
        Dim fontSize As Single = 11.5F
        If count > 32 Then
            fontSize = 7.0F
        ElseIf count > 20 Then
            fontSize = 8.5F
        ElseIf count > 12 Then
            fontSize = 10.0F
        End If

        Using fontText As New Font("Segoe UI", fontSize, FontStyle.Bold)
            Using penBorder As New Pen(Color.FromArgb(235, 255, 255, 255), 2.0F)
                Using textBrush As New SolidBrush(Color.White)
                    Using shadowBrush As New SolidBrush(Color.FromArgb(80, 0, 0, 0))
                        Using sf As New StringFormat()
                            sf.Alignment = StringAlignment.Center
                            sf.LineAlignment = StringAlignment.Center
                            sf.Trimming = StringTrimming.EllipsisCharacter
                            sf.FormatFlags = StringFormatFlags.NoWrap

                            For i As Integer = 0 To count - 1
                                Dim startAngle As Single = (currentAngle + i * sweepAngle) Mod 360.0F
                                Dim colorIdx As Integer = i Mod SliceColors.Length

                                ' Gambar irisan kue (Pie)
                                Using sliceBrush As New SolidBrush(SliceColors(colorIdx))
                                    g.FillPie(sliceBrush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle)
                                End Using

                                ' Garis batas antar irisan
                                g.DrawPie(penBorder, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle)

                                ' Gambar Teks Melingkar Radially
                                Dim midAngle As Single = startAngle + (sweepAngle / 2.0F)
                                Dim state As GraphicsState = g.Save()
                                g.TranslateTransform(cx, cy)
                                g.RotateTransform(midAngle)

                                ' Area penempatan teks di dalam irisan (dari dekat pusat ke dekat tepi)
                                Dim textInnerR As Single = 42.0F
                                Dim textOuterR As Single = radius - 16.0F
                                Dim textWidth As Single = textOuterR - textInnerR
                                Dim textHeight As Single = Math.Max(18.0F, CSng(2.0 * Math.PI * (textInnerR + textWidth * 0.5) / count * 0.85))
                                textHeight = Math.Min(textHeight, 38.0F)

                                Dim textRect As New RectangleF(textInnerR, -textHeight / 2.0F, textWidth, textHeight)

                                ' Sedikit bayangan teks untuk kontras sempurna
                                Dim shadowRect As New RectangleF(textRect.X + 1.0F, textRect.Y + 1.0F, textRect.Width, textRect.Height)
                                g.DrawString(participants(i), fontText, shadowBrush, shadowRect, sf)

                                ' Teks utama
                                g.DrawString(participants(i), fontText, textBrush, textRect, sf)

                                g.Restore(state)
                            Next
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Menggambar bingkai lingkaran luar roda dengan stud rivet logam.
    ''' </summary>
    Private Shared Sub DrawOuterRim(g As Graphics, rect As RectangleF, cx As Single, cy As Single, radius As Single)
        ' Garis lingkar luar ganda
        Using rimPen As New Pen(Color.FromArgb(220, 255, 255, 255), 4.0F)
            g.DrawEllipse(rimPen, rect)
        End Using

        Using rimOuterPen As New Pen(Color.FromArgb(148, 163, 184), 1.5F)
            g.DrawEllipse(rimOuterPen, rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2)
        End Using

        ' Titik-titik dekoratif (Studs) di sekeliling rim
        Const studCount As Integer = 24
        For s As Integer = 0 To studCount - 1
            Dim angleRad As Double = (s * (360.0 / studCount)) * Math.PI / 180.0
            Dim sx As Single = cx + CSng(Math.Cos(angleRad) * (radius - 5))
            Dim sy As Single = cy + CSng(Math.Sin(angleRad) * (radius - 5))

            Using studBrush As New SolidBrush(Color.FromArgb(254, 240, 138)) ' Emas lembut
                g.FillEllipse(studBrush, sx - 2.5F, sy - 2.5F, 5.0F, 5.0F)
            End Using
        Next
    End Sub

    ''' <summary>
    ''' Menggambar tutup pusat roda (Center Hub) yang elegan.
    ''' </summary>
    Private Shared Sub DrawCenterHub(g As Graphics, cx As Single, cy As Single)
        Const hubRadius As Single = 30.0F
        Dim hubRect As New RectangleF(cx - hubRadius, cy - hubRadius, hubRadius * 2.0F, hubRadius * 2.0F)

        ' Bayangan hub
        Using shadowBrush As New SolidBrush(Color.FromArgb(60, 0, 0, 0))
            g.FillEllipse(shadowBrush, hubRect.X + 2, hubRect.Y + 3, hubRect.Width, hubRect.Height)
        End Using

        ' Lingkaran dasar hub (Dark Slate Metallic)
        Using hubBrush As New LinearGradientBrush(hubRect, Color.FromArgb(51, 65, 85), Color.FromArgb(15, 23, 42), LinearGradientMode.ForwardDiagonal)
            g.FillEllipse(hubBrush, hubRect)
        End Using

        ' Ring luar emas / putih
        Using borderPen As New Pen(Color.FromArgb(250, 204, 21), 3.0F)
            g.DrawEllipse(borderPen, hubRect)
        End Using

        ' Lingkaran inti kecil di tengah
        Const innerR As Single = 12.0F
        Dim innerRect As New RectangleF(cx - innerR, cy - innerR, innerR * 2.0F, innerR * 2.0F)
        Using innerBrush As New LinearGradientBrush(innerRect, Color.FromArgb(254, 240, 138), Color.FromArgb(202, 138, 4), LinearGradientMode.Vertical)
            g.FillEllipse(innerBrush, innerRect)
        End Using
    End Sub

    ''' <summary>
    ''' Menggambar jarum penunjuk (Pointer) statis di atas tengah roda (270 derajat).
    ''' </summary>
    Private Shared Sub DrawPointer(g As Graphics, cx As Single, rimTopY As Single)
        ' Jarum penunjuk segitiga terbalik
        ' Ujung jarum menembus masuk sedikit ke dalam roda (~16 px)
        Dim tipY As Single = rimTopY + 18.0F
        Dim topBaseY As Single = rimTopY - 14.0F
        Dim halfWidth As Single = 15.0F

        Dim points() As PointF = {
            New PointF(cx, tipY),                      ' Ujung bawah yang menunjuk ke roda
            New PointF(cx - halfWidth, topBaseY),      ' Sudut kiri atas
            New PointF(cx + halfWidth, topBaseY)       ' Sudut kanan atas
        }

        ' Bayangan jarum
        Dim shadowPoints() As PointF = {
            New PointF(cx + 2.0F, tipY + 3.0F),
            New PointF(cx - halfWidth + 2.0F, topBaseY + 3.0F),
            New PointF(cx + halfWidth + 2.0F, topBaseY + 3.0F)
        }
        Using shadowBrush As New SolidBrush(Color.FromArgb(70, 0, 0, 0))
            g.FillPolygon(shadowBrush, shadowPoints)
        End Using

        ' Tubuh jarum (Merah Crimson elegan dengan gradient halus)
        Dim pointerRect As New RectangleF(cx - halfWidth, topBaseY, halfWidth * 2.0F, tipY - topBaseY)
        Using pBrush As New LinearGradientBrush(pointerRect, Color.FromArgb(239, 68, 68), Color.FromArgb(185, 28, 28), LinearGradientMode.Vertical)
            g.FillPolygon(pBrush, points)
        End Using

        ' Border putih tebal sesuai spesifikasi
        Using pPen As New Pen(Color.White, 3.0F)
            pPen.LineJoin = LineJoin.Round
            g.DrawPolygon(pPen, points)
        End Using

        ' Pin kepala jarum (Engsel atas)
        Dim pinR As Single = 9.0F
        Dim pinRect As New RectangleF(cx - pinR, topBaseY - pinR * 0.7F, pinR * 2.0F, pinR * 2.0F)
        Using pinBrush As New LinearGradientBrush(pinRect, Color.White, Color.FromArgb(203, 213, 225), LinearGradientMode.Vertical)
            g.FillEllipse(pinBrush, pinRect)
        End Using
        Using pinBorderPen As New Pen(Color.FromArgb(100, 116, 139), 1.5F)
            g.DrawEllipse(pinBorderPen, pinRect)
        End Using
    End Sub
End Class
