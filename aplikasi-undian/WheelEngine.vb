Imports System
Imports System.Collections.Generic

Public Class WheelEngine
    ' Konfigurasi Fisika Roda
    Public Const DefaultFriction As Single = 0.985F
    Public Const MinSpeedThreshold As Single = 0.2F
    Public Const PointerAngle As Single = 270.0F ' Jarum penunjuk berada di arah jam 12 (atas tengah)

    ' Events
    Public Event SpinStarted()
    Public Event WheelTicked(segmentIndex As Integer)
    Public Event SpinCompleted(winnerName As String, winnerIndex As Integer)

    ' Properties
    Public Property Participants As New List(Of String)()
    Public Property CurrentAngle As Single = 0.0F
    Public Property Speed As Single = 0.0F
    Public Property IsSpinning As Boolean = False
    Public Property WinnerIndex As Integer = -1
    Public Property TargetAngle As Single = 0.0F

    Private _lastPointerSegment As Integer = -1

    ''' <summary>
    ''' Mengembalikan sudut sapuan (sweep angle) tiap segmen peserta dalam derajat.
    ''' </summary>
    Public ReadOnly Property SweepAngle As Single
        Get
            If Participants.Count = 0 Then Return 360.0F
            Return 360.0F / Participants.Count
        End Get
    End Property

    ''' <summary>
    ''' Mendapatkan indeks peserta yang saat ini berada tepat di bawah jarum penunjuk (270 derajat).
    ''' </summary>
    Public Function GetCurrentSegmentAtPointer() As Integer
        If Participants.Count = 0 Then Return -1
        Dim relAngle As Single = (PointerAngle - CurrentAngle) Mod 360.0F
        If relAngle < 0.0F Then relAngle += 360.0F
        Dim seg As Integer = CInt(Math.Floor(relAngle / SweepAngle))
        If seg < 0 Then seg = 0
        If seg >= Participants.Count Then seg = Participants.Count - 1
        Return seg
    End Function

    ''' <summary>
    ''' Memulai putaran roda secara deterministik.
    ''' </summary>
    Public Function StartSpin() As Boolean
        If IsSpinning Then Return False
        If Participants.Count < 1 Then Return False

        ' 1. Acak pemenang
        WinnerIndex = Random.Shared.Next(0, Participants.Count)

        ' 2. Hitung targetAngle agar jarum di 270 derajat mendarat di tengah irisan winnerIndex
        Dim sweep As Single = SweepAngle
        Dim targetRelativeCenter As Single = (WinnerIndex + 0.5F) * sweep
        Dim target As Single = (PointerAngle - targetRelativeCenter) Mod 360.0F
        If target < 0.0F Then target += 360.0F
        TargetAngle = target

        ' 3. Hitung jarak putaran total (D = k putaran penuh + delta ke target)
        Dim currentNorm As Single = CurrentAngle Mod 360.0F
        If currentNorm < 0.0F Then currentNorm += 360.0F
        CurrentAngle = currentNorm

        Dim delta As Single = (TargetAngle - CurrentAngle) Mod 360.0F
        If delta < 0.0F Then delta += 360.0F

        ' Tambahkan antara 6 sampai 8 putaran penuh agar animasi berputar dramatis
        Dim fullSpins As Integer = Random.Shared.Next(6, 9)
        Dim totalDistance As Single = (fullSpins * 360.0F) + delta

        ' 4. Hitung kecepatan awal yang tepat agar deselerasi fisik (speed * 0.985)
        ' mendarat pas di target saat speed < 0.2
        Speed = CalculateInitialSpeed(totalDistance, DefaultFriction, MinSpeedThreshold)
        IsSpinning = True
        _lastPointerSegment = GetCurrentSegmentAtPointer()

        RaiseEvent SpinStarted()
        Return True
    End Function

    ''' <summary>
    ''' Di-trigger setiap tick timer (15 ms) untuk memperbarui sudut dan kecepatan roda.
    ''' </summary>
    Public Sub UpdatePhysics()
        If Not IsSpinning Then Return

        ' Periksa perpindahan segmen untuk efek suara tick
        Dim prevSegment As Integer = GetCurrentSegmentAtPointer()

        ' Update sudut
        CurrentAngle = (CurrentAngle + Speed) Mod 360.0F
        If CurrentAngle < 0.0F Then CurrentAngle += 360.0F

        Dim newSegment As Integer = GetCurrentSegmentAtPointer()
        If newSegment <> prevSegment Then
            RaiseEvent WheelTicked(newSegment)
        End If

        ' Deselerasi fisik
        Speed *= DefaultFriction

        ' Periksa apakah sudah mencapai ambang batas berhenti
        If Speed < MinSpeedThreshold Then
            ' Snap ke target angle untuk presisi mutlak
            CurrentAngle = TargetAngle
            IsSpinning = False
            Speed = 0.0F

            Dim winnerName As String = If(WinnerIndex >= 0 AndAlso WinnerIndex < Participants.Count, Participants(WinnerIndex), String.Empty)
            RaiseEvent SpinCompleted(winnerName, WinnerIndex)
        End If
    End Sub

    ''' <summary>
    ''' Menghitung initial speed (v0) secara presisi menggunakan binary search
    ''' sehingga total jarak dari deret diskrit v_{t+1} = v_t * friction hingga v < minSpeed
    ''' sama dengan total distance yang diinginkan.
    ''' </summary>
    Private Function CalculateInitialSpeed(totalDistance As Single, friction As Single, minSpeed As Single) As Single
        ' Perkiraan analitik deret geometri: v0 ~ totalDistance * (1 - friction) + minSpeed
        Dim low As Single = totalDistance * (1.0F - friction) * 0.8F
        Dim high As Single = (totalDistance * (1.0F - friction) + minSpeed) * 1.5F

        For iter As Integer = 0 To 30
            Dim mid As Single = (low + high) / 2.0F
            Dim simulatedDist As Single = SimulateDistance(mid, friction, minSpeed)

            If Math.Abs(simulatedDist - totalDistance) < 0.01F Then
                Return mid
            ElseIf simulatedDist < totalDistance Then
                low = mid
            Else
                high = mid
            End If
        Next

        Return (low + high) / 2.0F
    End Function

    ''' <summary>
    ''' Menyimulasikan total rotasi diskrit untuk nilai awal v0 tertentu.
    ''' </summary>
    Private Function SimulateDistance(v0 As Single, friction As Single, minSpeed As Single) As Single
        Dim v As Single = v0
        Dim dist As Single = 0.0F
        While v >= minSpeed
            dist += v
            v *= friction
        End While
        Return dist
    End Function
End Class
