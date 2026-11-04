<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormUtama
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents picRoda As DoubleBufferedPictureBox
    Friend WithEvents btnPutar As Button
    Friend WithEvents grpPeserta As GroupBox
    Friend WithEvents lblInputNama As Label
    Friend WithEvents txtInputNama As TextBox
    Friend WithEvents btnTambah As Button
    Friend WithEvents lstPeserta As ListBox
    Friend WithEvents btnHapus As Button
    Friend WithEvents btnImportTxt As Button
    Friend WithEvents lblTotalPeserta As Label
    Friend WithEvents grpPengaturan As GroupBox
    Friend WithEvents chkEliminasi As CheckBox
    Friend WithEvents chkSuara As CheckBox
    Friend WithEvents grpRiwayat As GroupBox
    Friend WithEvents dgvRiwayat As DataGridView
    Friend WithEvents btnBersihkanRiwayat As Button
    Friend WithEvents lblTotalRiwayat As Label
    Friend WithEvents tmrSpin As Timer
    Friend WithEvents openFileDialogTxt As OpenFileDialog

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Dim dataGridViewCellStyle1 As New System.Windows.Forms.DataGridViewCellStyle()
        Dim dataGridViewCellStyle2 As New System.Windows.Forms.DataGridViewCellStyle()

        picRoda = New DoubleBufferedPictureBox()
        btnPutar = New Button()
        grpPeserta = New GroupBox()
        lblTotalPeserta = New Label()
        btnImportTxt = New Button()
        btnHapus = New Button()
        lstPeserta = New ListBox()
        btnTambah = New Button()
        txtInputNama = New TextBox()
        lblInputNama = New Label()
        grpPengaturan = New GroupBox()
        chkSuara = New CheckBox()
        chkEliminasi = New CheckBox()
        grpRiwayat = New GroupBox()
        lblTotalRiwayat = New Label()
        btnBersihkanRiwayat = New Button()
        dgvRiwayat = New DataGridView()
        tmrSpin = New Timer(components)
        openFileDialogTxt = New OpenFileDialog()

        CType(picRoda, System.ComponentModel.ISupportInitialize).BeginInit()
        grpPeserta.SuspendLayout()
        grpPengaturan.SuspendLayout()
        grpRiwayat.SuspendLayout()
        CType(dgvRiwayat, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()

        '
        ' picRoda
        '
        picRoda.BackColor = System.Drawing.Color.White
        picRoda.BorderStyle = BorderStyle.FixedSingle
        picRoda.Location = New System.Drawing.Point(30, 30)
        picRoda.Name = "picRoda"
        picRoda.Size = New System.Drawing.Size(420, 420)
        picRoda.TabIndex = 0
        picRoda.TabStop = False

        '
        ' btnPutar
        '
        btnPutar.BackColor = System.Drawing.Color.FromArgb(16, 185, 129)
        btnPutar.Cursor = Cursors.Hand
        btnPutar.FlatAppearance.BorderSize = 0
        btnPutar.FlatStyle = FlatStyle.Flat
        btnPutar.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnPutar.ForeColor = System.Drawing.Color.White
        btnPutar.Location = New System.Drawing.Point(115, 480)
        btnPutar.Name = "btnPutar"
        btnPutar.Size = New System.Drawing.Size(250, 45)
        btnPutar.TabIndex = 1
        btnPutar.Text = "PUTAR RODA"
        btnPutar.UseVisualStyleBackColor = False

        '
        ' grpPeserta
        '
        grpPeserta.Controls.Add(lblTotalPeserta)
        grpPeserta.Controls.Add(btnImportTxt)
        grpPeserta.Controls.Add(btnHapus)
        grpPeserta.Controls.Add(lstPeserta)
        grpPeserta.Controls.Add(btnTambah)
        grpPeserta.Controls.Add(txtInputNama)
        grpPeserta.Controls.Add(lblInputNama)
        grpPeserta.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        grpPeserta.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59)
        grpPeserta.Location = New System.Drawing.Point(480, 20)
        grpPeserta.Name = "grpPeserta"
        grpPeserta.Size = New System.Drawing.Size(445, 230)
        grpPeserta.TabIndex = 2
        grpPeserta.TabStop = False
        grpPeserta.Text = "Kelola Peserta"

        '
        ' lblInputNama
        '
        lblInputNama.AutoSize = True
        lblInputNama.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblInputNama.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105)
        lblInputNama.Location = New System.Drawing.Point(15, 22)
        lblInputNama.Name = "lblInputNama"
        lblInputNama.Size = New System.Drawing.Size(81, 15)
        lblInputNama.TabIndex = 0
        lblInputNama.Text = "Nama Peserta:"

        '
        ' txtInputNama
        '
        txtInputNama.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        txtInputNama.Location = New System.Drawing.Point(15, 41)
        txtInputNama.Name = "txtInputNama"
        txtInputNama.Size = New System.Drawing.Size(295, 24)
        txtInputNama.TabIndex = 1

        '
        ' btnTambah
        '
        btnTambah.BackColor = System.Drawing.Color.FromArgb(37, 99, 235)
        btnTambah.Cursor = Cursors.Hand
        btnTambah.FlatAppearance.BorderSize = 0
        btnTambah.FlatStyle = FlatStyle.Flat
        btnTambah.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnTambah.ForeColor = System.Drawing.Color.White
        btnTambah.Location = New System.Drawing.Point(320, 39)
        btnTambah.Name = "btnTambah"
        btnTambah.Size = New System.Drawing.Size(110, 28)
        btnTambah.TabIndex = 2
        btnTambah.Text = "Tambah"
        btnTambah.UseVisualStyleBackColor = False

        '
        ' lstPeserta
        '
        lstPeserta.Font = New System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lstPeserta.FormattingEnabled = True
        lstPeserta.ItemHeight = 17
        lstPeserta.Location = New System.Drawing.Point(15, 75)
        lstPeserta.Name = "lstPeserta"
        lstPeserta.Size = New System.Drawing.Size(295, 106)
        lstPeserta.TabIndex = 3

        '
        ' btnHapus
        '
        btnHapus.BackColor = System.Drawing.Color.FromArgb(239, 68, 68)
        btnHapus.Cursor = Cursors.Hand
        btnHapus.FlatAppearance.BorderSize = 0
        btnHapus.FlatStyle = FlatStyle.Flat
        btnHapus.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnHapus.ForeColor = System.Drawing.Color.White
        btnHapus.Location = New System.Drawing.Point(320, 75)
        btnHapus.Name = "btnHapus"
        btnHapus.Size = New System.Drawing.Size(110, 30)
        btnHapus.TabIndex = 4
        btnHapus.Text = "Hapus"
        btnHapus.UseVisualStyleBackColor = False

        '
        ' btnImportTxt
        '
        btnImportTxt.BackColor = System.Drawing.Color.FromArgb(71, 85, 105)
        btnImportTxt.Cursor = Cursors.Hand
        btnImportTxt.FlatAppearance.BorderSize = 0
        btnImportTxt.FlatStyle = FlatStyle.Flat
        btnImportTxt.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnImportTxt.ForeColor = System.Drawing.Color.White
        btnImportTxt.Location = New System.Drawing.Point(320, 115)
        btnImportTxt.Name = "btnImportTxt"
        btnImportTxt.Size = New System.Drawing.Size(110, 32)
        btnImportTxt.TabIndex = 5
        btnImportTxt.Text = "Import .txt"
        btnImportTxt.UseVisualStyleBackColor = False

        '
        ' lblTotalPeserta
        '
        lblTotalPeserta.AutoSize = True
        lblTotalPeserta.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblTotalPeserta.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        lblTotalPeserta.Location = New System.Drawing.Point(15, 198)
        lblTotalPeserta.Name = "lblTotalPeserta"
        lblTotalPeserta.Size = New System.Drawing.Size(84, 15)
        lblTotalPeserta.TabIndex = 6
        lblTotalPeserta.Text = "Total Peserta: 0"

        '
        ' grpPengaturan
        '
        grpPengaturan.Controls.Add(chkSuara)
        grpPengaturan.Controls.Add(chkEliminasi)
        grpPengaturan.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        grpPengaturan.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59)
        grpPengaturan.Location = New System.Drawing.Point(480, 258)
        grpPengaturan.Name = "grpPengaturan"
        grpPengaturan.Size = New System.Drawing.Size(445, 60)
        grpPengaturan.TabIndex = 3
        grpPengaturan.TabStop = False
        grpPengaturan.Text = "Pengaturan"

        '
        ' chkEliminasi
        '
        chkEliminasi.AutoSize = True
        chkEliminasi.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        chkEliminasi.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85)
        chkEliminasi.Location = New System.Drawing.Point(15, 25)
        chkEliminasi.Name = "chkEliminasi"
        chkEliminasi.Size = New System.Drawing.Size(262, 19)
        chkEliminasi.TabIndex = 0
        chkEliminasi.Text = "Keluarkan pemenang dari putaran berikutnya"
        chkEliminasi.UseVisualStyleBackColor = True

        '
        ' chkSuara
        '
        chkSuara.AutoSize = True
        chkSuara.Checked = True
        chkSuara.CheckState = CheckState.Checked
        chkSuara.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        chkSuara.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85)
        chkSuara.Location = New System.Drawing.Point(290, 25)
        chkSuara.Name = "chkSuara"
        chkSuara.Size = New System.Drawing.Size(133, 19)
        chkSuara.TabIndex = 1
        chkSuara.Text = "Bunyikan efek suara"
        chkSuara.UseVisualStyleBackColor = True

        '
        ' grpRiwayat
        '
        grpRiwayat.Controls.Add(lblTotalRiwayat)
        grpRiwayat.Controls.Add(btnBersihkanRiwayat)
        grpRiwayat.Controls.Add(dgvRiwayat)
        grpRiwayat.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        grpRiwayat.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59)
        grpRiwayat.Location = New System.Drawing.Point(480, 326)
        grpRiwayat.Name = "grpRiwayat"
        grpRiwayat.Size = New System.Drawing.Size(445, 225)
        grpRiwayat.TabIndex = 4
        grpRiwayat.TabStop = False
        grpRiwayat.Text = "Riwayat Pemenang"

        '
        ' dgvRiwayat
        '
        dgvRiwayat.AllowUserToAddRows = False
        dgvRiwayat.AllowUserToDeleteRows = False
        dgvRiwayat.AllowUserToResizeRows = False
        dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRiwayat.BackgroundColor = System.Drawing.Color.White
        dgvRiwayat.BorderStyle = BorderStyle.Fixed3D
        dgvRiwayat.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(241, 245, 249)
        dataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85)
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(241, 245, 249)
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(51, 65, 85)
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        dgvRiwayat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1
        dgvRiwayat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        dataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59)
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(224, 242, 254)
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(3, 105, 161)
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        dgvRiwayat.DefaultCellStyle = dataGridViewCellStyle2
        dgvRiwayat.EnableHeadersVisualStyles = False
        dgvRiwayat.Location = New System.Drawing.Point(15, 24)
        dgvRiwayat.MultiSelect = False
        dgvRiwayat.Name = "dgvRiwayat"
        dgvRiwayat.ReadOnly = True
        dgvRiwayat.RowHeadersVisible = False
        dgvRiwayat.RowTemplate.Height = 25
        dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRiwayat.Size = New System.Drawing.Size(415, 155)
        dgvRiwayat.TabIndex = 0

        '
        ' lblTotalRiwayat
        '
        lblTotalRiwayat.AutoSize = True
        lblTotalRiwayat.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblTotalRiwayat.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139)
        lblTotalRiwayat.Location = New System.Drawing.Point(15, 192)
        lblTotalRiwayat.Name = "lblTotalRiwayat"
        lblTotalRiwayat.Size = New System.Drawing.Size(100, 15)
        lblTotalRiwayat.TabIndex = 1
        lblTotalRiwayat.Text = "Total Pemenang: 0"

        '
        ' btnBersihkanRiwayat
        '
        btnBersihkanRiwayat.BackColor = System.Drawing.Color.FromArgb(239, 68, 68)
        btnBersihkanRiwayat.Cursor = Cursors.Hand
        btnBersihkanRiwayat.FlatAppearance.BorderSize = 0
        btnBersihkanRiwayat.FlatStyle = FlatStyle.Flat
        btnBersihkanRiwayat.Font = New System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        btnBersihkanRiwayat.ForeColor = System.Drawing.Color.White
        btnBersihkanRiwayat.Location = New System.Drawing.Point(285, 185)
        btnBersihkanRiwayat.Name = "btnBersihkanRiwayat"
        btnBersihkanRiwayat.Size = New System.Drawing.Size(145, 30)
        btnBersihkanRiwayat.TabIndex = 2
        btnBersihkanRiwayat.Text = "Bersihkan Riwayat"
        btnBersihkanRiwayat.UseVisualStyleBackColor = False

        '
        ' tmrSpin
        '
        tmrSpin.Interval = 15

        '
        ' openFileDialogTxt
        '
        openFileDialogTxt.Filter = "File Teks (*.txt)|*.txt|Semua File (*.*)|*.*"
        openFileDialogTxt.Title = "Import Daftar Peserta dari File .txt"

        '
        ' FormUtama
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = System.Drawing.Color.FromArgb(248, 250, 252)
        ClientSize = New System.Drawing.Size(950, 580)
        Controls.Add(grpRiwayat)
        Controls.Add(grpPengaturan)
        Controls.Add(grpPeserta)
        Controls.Add(btnPutar)
        Controls.Add(picRoda)
        Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        Name = "FormUtama"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Lucky Wheel Spinner - Aplikasi Undian"

        CType(picRoda, System.ComponentModel.ISupportInitialize).EndInit()
        grpPeserta.ResumeLayout(False)
        grpPeserta.PerformLayout()
        grpPengaturan.ResumeLayout(False)
        grpPengaturan.PerformLayout()
        grpRiwayat.ResumeLayout(False)
        grpRiwayat.PerformLayout()
        CType(dgvRiwayat, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

End Class
