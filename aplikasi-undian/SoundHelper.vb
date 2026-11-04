Imports System.IO
Imports System.Media

Public Class SoundHelper
    Private Shared _tickPlayer As SoundPlayer
    Private Shared _winPlayer As SoundPlayer
    Private Shared _initialized As Boolean = False
    Private Shared _lastTickTime As Long = 0

    ''' <summary>
    ''' Inisialisasi audio synth WAV di memory.
    ''' </summary>
    Public Shared Sub Initialize()
        If _initialized Then Return
        Try
            Dim tickStream As MemoryStream = CreateTickWaveStream()
            _tickPlayer = New SoundPlayer(tickStream)
            _tickPlayer.Load()

            Dim winStream As MemoryStream = CreateWinWaveStream()
            _winPlayer = New SoundPlayer(winStream)
            _winPlayer.Load()

            _initialized = True
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Sound init error: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Memainkan efek suara tick saat jarum melewati pembatas segmen roda.
    ''' Memiliki batasan frekuensi minimum agar tidak overlap/pecah pada kecepatan tinggi.
    ''' </summary>
    Public Shared Sub PlayTick()
        Dim nowTicks As Long = Environment.TickCount64
        ' Batasi maksimal 1 suara per 35 ms
        If nowTicks - _lastTickTime < 35 Then Return
        _lastTickTime = nowTicks

        Try
            If Not _initialized Then Initialize()
            _tickPlayer?.Play()
        Catch
            ' Fallback ke system sound jika ada masalah driver
            Try
                SystemSounds.Beep.Play()
            Catch
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Memainkan efek suara kemenangan saat putaran selesai dan pemenang ditentukan.
    ''' </summary>
    Public Shared Sub PlayWinnerSound()
        Try
            If Not _initialized Then Initialize()
            _winPlayer?.Play()
        Catch
            Try
                SystemSounds.Asterisk.Play()
            Catch
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Membuat stream WAV (PCM 16-bit 22050Hz Mono) untuk efek suara 'tick' roda.
    ''' </summary>
    Private Shared Function CreateTickWaveStream() As MemoryStream
        Const sampleRate As Integer = 22050
        Const durationMs As Integer = 30
        Dim totalSamples As Integer = CInt(sampleRate * (durationMs / 1000.0))
        Dim pcmData(totalSamples - 1) As Short

        ' Buat suara klik frekuensi 1200Hz dengan peluruhan eksponensial cepat
        For i As Integer = 0 To totalSamples - 1
            Dim t As Double = i / CDbl(sampleRate)
            Dim decay As Double = Math.Exp(-i / (totalSamples * 0.15))
            Dim wave As Double = Math.Sin(2.0 * Math.PI * 1200.0 * t)
            pcmData(i) = CShort(wave * decay * 28000)
        Next

        Return BuildWavStream(pcmData, sampleRate)
    End Function

    ''' <summary>
    ''' Membuat stream WAV (PCM 16-bit 22050Hz Mono) untuk fanfare / nada kemenangan.
    ''' </summary>
    Private Shared Function CreateWinWaveStream() As MemoryStream
        Const sampleRate As Integer = 22050
        ' Nada arpeggio kemenangan: G5 (784Hz), C6 (1046Hz), E6 (1318Hz), G6 (1568Hz)
        Dim notes() As Double = {784.0, 1046.5, 1318.5, 1568.0}
        Dim noteDurations() As Integer = {100, 100, 120, 350} ' Durasi ms
        Dim totalDurationMs As Integer = 0
        For Each d In noteDurations
            totalDurationMs += d
        Next

        Dim totalSamples As Integer = CInt(sampleRate * (totalDurationMs / 1000.0))
        Dim pcmData(totalSamples - 1) As Short

        Dim currentSample As Integer = 0
        For n As Integer = 0 To notes.Length - 1
            Dim freq As Double = notes(n)
            Dim durMs As Integer = noteDurations(n)
            Dim noteSamples As Integer = CInt(sampleRate * (durMs / 1000.0))

            For i As Integer = 0 To noteSamples - 1
                If currentSample >= totalSamples Then Exit For
                Dim t As Double = i / CDbl(sampleRate)
                ' Amplitude envelope (attack cepat, sustain, lalu decay)
                Dim env As Double = Math.Min(1.0, i / (noteSamples * 0.1)) * Math.Exp(-i / (noteSamples * 0.8))
                ' Harmonik kombinasi nada murni + oktaf
                Dim wave As Double = 0.7 * Math.Sin(2.0 * Math.PI * freq * t) + 0.3 * Math.Sin(4.0 * Math.PI * freq * t)
                pcmData(currentSample) = CShort(wave * env * 24000)
                currentSample += 1
            Next
        Next

        Return BuildWavStream(pcmData, sampleRate)
    End Function

    ''' <summary>
    ''' Membungkus raw PCM 16-bit ke dalam format file WAV standar.
    ''' </summary>
    Private Shared Function BuildWavStream(pcmData() As Short, sampleRate As Integer) As MemoryStream
        Dim stream As New MemoryStream()
        Using writer As New BinaryWriter(stream, System.Text.Encoding.ASCII, leaveOpen:=True)
            Dim byteCount As Integer = pcmData.Length * 2
            ' RIFF header
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"))
            writer.Write(36 + byteCount)
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"))
            ' fmt chunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "))
            writer.Write(16) ' SubChunk1Size (16 for PCM)
            writer.Write(CShort(1)) ' AudioFormat (1 = PCM)
            writer.Write(CShort(1)) ' NumChannels (1 = Mono)
            writer.Write(sampleRate) ' SampleRate
            writer.Write(sampleRate * 2) ' ByteRate (SampleRate * NumChannels * BitsPerSample/8)
            writer.Write(CShort(2)) ' BlockAlign (NumChannels * BitsPerSample/8)
            writer.Write(CShort(16)) ' BitsPerSample
            ' data chunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"))
            writer.Write(byteCount)
            For Each sample In pcmData
                writer.Write(sample)
            Next
        End Using
        stream.Position = 0
        Return stream
    End Function
End Class
