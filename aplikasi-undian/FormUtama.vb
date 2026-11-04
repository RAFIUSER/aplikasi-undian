Imports System.IO
Imports System.Text

Public Class FormUtama
    Private WithEvents _engine As New WheelEngine()

    Private Sub FormUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Inisialisasi Database SQLite
        Try
            DatabaseHelper.InitializeDatabase()
        Catch ex As Exception
            MessageBox.Show($"Terjadi kesalahan saat inisialisasi database:" & vbCrLf & ex.Message,
                            "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' 2. Inisialisasi Audio Synthesizer
        SoundHelper.Initialize()

        ' 3. Masukkan peserta default untuk demo pertama kali
        Dim defaultParticipants As String() = {
            "Rafi", "Bintang", "Rain",
            "Bulan", "Catur"
        }

        For Each pName As String In defaultParticipants
            lstPeserta.Items.Add(pName)
        Next
        SyncParticipants()

        ' 4. Muat riwayat pemenang dari SQLite
        LoadHistoryGrid()

        ' 5. Invalidate tampilan roda
        picRoda.Invalidate()
    End Sub

    ''' <summary>
    ''' Menyinkronkan daftar nama di ListBox ke WheelEngine.
    ''' </summary>
    Private Sub SyncParticipants()
        _engine.Participants.Clear()
        For Each item In lstPeserta.Items
            _engine.Participants.Add(item.ToString())
        Next
        lblTotalPeserta.Text = $"Total Peserta: {lstPeserta.Items.Count}"
        picRoda.Invalidate()
    End Sub

    ''' <summary>
    ''' Mengambil data riwayat pemenang dan menampilkan di DataGridView.
    ''' </summary>
    Private Sub LoadHistoryGrid()
        Dim dt = DatabaseHelper.GetHistory()
        dgvRiwayat.DataSource = dt

        ' Atur header kolom jika ada
        If dgvRiwayat.Columns.Contains("Nama") Then
            dgvRiwayat.Columns("Nama").HeaderText = "Nama Pemenang"
            dgvRiwayat.Columns("Nama").FillWeight = 55
        End If
        If dgvRiwayat.Columns.Contains("Waktu") Then
            dgvRiwayat.Columns("Waktu").HeaderText = "Waktu Menang"
            dgvRiwayat.Columns("Waktu").FillWeight = 45
        End If

        lblTotalRiwayat.Text = $"Total Pemenang: {dt.Rows.Count}"
    End Sub

    ''' <summary>
    ''' Mengatur status enabled tombol saat roda sedang berputar.
    ''' </summary>
    Private Sub SetControlsEnabled(enabled As Boolean)
        btnPutar.Enabled = enabled
        btnTambah.Enabled = enabled
        btnHapus.Enabled = enabled
        btnImportTxt.Enabled = enabled
        btnBersihkanRiwayat.Enabled = enabled
        txtInputNama.Enabled = enabled
        lstPeserta.Enabled = enabled
        chkEliminasi.Enabled = enabled

        If enabled Then
            btnPutar.BackColor = Color.FromArgb(16, 185, 129) ' Hijau
            btnPutar.Text = "PUTAR RODA"
        Else
            btnPutar.BackColor = Color.FromArgb(148, 163, 184) ' Abu-abu non-aktif
            btnPutar.Text = "SEDANG MEMUTAR..."
        End If
    End Sub

    ' =========================================================================
    ' EVENT HANDLER PUTARAN RODA & FISIKA
    ' =========================================================================

    Private Sub btnPutar_Click(sender As Object, e As EventArgs) Handles btnPutar.Click
        If _engine.Participants.Count < 2 Then
            MessageBox.Show("Silakan tambahkan minimal 2 peserta untuk memulai undian!",
                            "Peserta Kurang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtInputNama.Focus()
            Return
        End If

        SetControlsEnabled(False)
        If _engine.StartSpin() Then
            tmrSpin.Start()
        Else
            SetControlsEnabled(True)
        End If
    End Sub

    Private Sub tmrSpin_Tick(sender As Object, e As EventArgs) Handles tmrSpin.Tick
        _engine.UpdatePhysics()
        picRoda.Invalidate()
    End Sub

    Private Sub _engine_WheelTicked(segmentIndex As Integer) Handles _engine.WheelTicked
        If chkSuara.Checked Then
            SoundHelper.PlayTick()
        End If
    End Sub

    Private Sub _engine_SpinCompleted(winnerName As String, winnerIndex As Integer) Handles _engine.SpinCompleted
        tmrSpin.Stop()
        picRoda.Invalidate()

        ' Mainkan efek suara kemenangan jika diaktifkan
        If chkSuara.Checked Then
            SoundHelper.PlayWinnerSound()
        End If

        ' Simpan pemenang ke database SQLite
        Dim totalPesertaSaatUndi As Integer = _engine.Participants.Count
        DatabaseHelper.InsertWinner(winnerName, totalPesertaSaatUndi)

        ' Refresh tabel riwayat pemenang
        LoadHistoryGrid()

        ' Tampilkan dialog ucapan selamat
        MessageBox.Show($"Selamat kepada:" & vbCrLf & vbCrLf &
                        $"★  {winnerName}  ★" & vbCrLf & vbCrLf &
                        $"telah memenangkan undian!",
                        "Pemenang Terpilih!", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Jika opsi eliminasi dicentang, hapus pemenang dari daftar
        If chkEliminasi.Checked AndAlso winnerIndex >= 0 AndAlso winnerIndex < lstPeserta.Items.Count Then
            Dim eliminatedName As String = lstPeserta.Items(winnerIndex).ToString()
            lstPeserta.Items.RemoveAt(winnerIndex)
            SyncParticipants()
            MessageBox.Show($"Pemenang '{eliminatedName}' telah dikeluarkan dari putaran berikutnya.",
                            "Eliminasi Pemenang", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        SetControlsEnabled(True)
    End Sub

    ' =========================================================================
    ' EVENT HANDLER PENGGAMBARAN GDI+ (RENDERER)
    ' =========================================================================

    Private Sub picRoda_Paint(sender As Object, e As PaintEventArgs) Handles picRoda.Paint
        Renderer.Render(e.Graphics, picRoda.ClientRectangle, _engine.Participants, _engine.CurrentAngle)
    End Sub

    ' =========================================================================
    ' EVENT HANDLER KELOLA PESERTA
    ' =========================================================================

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        TambahPeserta()
    End Sub

    Private Sub txtInputNama_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInputNama.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            TambahPeserta()
        End If
    End Sub

    Private Sub TambahPeserta()
        Dim nama As String = txtInputNama.Text.Trim()
        If String.IsNullOrWhiteSpace(nama) Then
            MessageBox.Show("Masukkan nama peserta terlebih dahulu!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtInputNama.Focus()
            Return
        End If

        lstPeserta.Items.Add(nama)
        txtInputNama.Clear()
        txtInputNama.Focus()
        SyncParticipants()
    End Sub

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If lstPeserta.SelectedIndex < 0 Then
            MessageBox.Show("Pilih nama peserta yang ingin dihapus dari daftar!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedIdx As Integer = lstPeserta.SelectedIndex
        lstPeserta.Items.RemoveAt(selectedIdx)

        ' Pertahankan seleksi pada item berikutnya jika ada
        If lstPeserta.Items.Count > 0 Then
            lstPeserta.SelectedIndex = Math.Min(selectedIdx, lstPeserta.Items.Count - 1)
        End If

        SyncParticipants()
    End Sub

    Private Sub btnImportTxt_Click(sender As Object, e As EventArgs) Handles btnImportTxt.Click
        If openFileDialogTxt.ShowDialog(Me) = DialogResult.OK Then
            Try
                Dim lines As String() = File.ReadAllLines(openFileDialogTxt.FileName, Encoding.UTF8)
                Dim importedCount As Integer = 0

                For Each line In lines
                    Dim trimmed As String = line.Trim()
                    If Not String.IsNullOrWhiteSpace(trimmed) Then
                        lstPeserta.Items.Add(trimmed)
                        importedCount += 1
                    End If
                Next

                SyncParticipants()
                MessageBox.Show($"Berhasil mengimpor {importedCount} nama peserta dari file.",
                                "Import Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show($"Gagal membaca file: {ex.Message}",
                                "Kesalahan File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' =========================================================================
    ' EVENT HANDLER RIWAYAT PEMENANG
    ' =========================================================================

    Private Sub btnBersihkanRiwayat_Click(sender As Object, e As EventArgs) Handles btnBersihkanRiwayat.Click
        If dgvRiwayat.Rows.Count = 0 Then
            MessageBox.Show("Riwayat pemenang masih kosong.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus seluruh riwayat pemenang?" & vbCrLf &
                                      "Tindakan ini tidak dapat dibatalkan.",
                                      "Konfirmasi Hapus Riwayat",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            If DatabaseHelper.ClearHistory() Then
                LoadHistoryGrid()
                MessageBox.Show("Seluruh riwayat pemenang berhasil dibersihkan.",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Gagal membersihkan riwayat pemenang dari database.",
                                "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
End Class
