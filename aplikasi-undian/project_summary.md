# Project Summary: Lucky Wheel Spinner (Aplikasi Undian)
**Target Reader:** AI Coding Agent (Antigravity / Cursor / Copilot)
**Project Type:** Desktop Application (Windows Forms)
**Language:** Visual Basic .NET (VB.NET)
**UI Language:** Bahasa Indonesia

## 1. Project Overview
Membangun aplikasi desktop *Lucky Wheel Spinner* (Roda Undian Berputar) menggunakan VB.NET Windows Forms. Aplikasi ini menggunakan pustaka bawaan `System.Drawing` (GDI+) untuk me-render roda undian secara dinamis tanpa *third-party UI library*, dan menggunakan SQLite untuk menyimpan riwayat pemenang secara permanen.

## 2. Tech Stack & Dependencies
*   **Framework:** .NET Framework 4.8 atau .NET 8.0 (Windows Forms).
*   **Graphics Engine:** Native GDI+ (`System.Drawing`).
*   **Database:** SQLite (Gunakan NuGet package `System.Data.SQLite.Core` atau `Microsoft.Data.Sqlite`).
*   **Audio:** `System.Media.SoundPlayer` (untuk efek suara *tick* standar bawaan sistem).

## 3. Arsitektur File & Modul
AI harus memisahkan logika (*Separation of Concerns*) ke dalam beberapa class berikut:

1.  `FormUtama.vb` & `FormUtama.Designer.vb`: Menangani event UI (Click, Timer Tick) dan layouting komponen.
2.  `Renderer.vb`: Class khusus yang menerima object `Graphics` dan state roda untuk menggambar irisan warna (Pie), teks melingkar, dan jarum penunjuk.
3.  `WheelEngine.vb`: Class untuk menangani state mesin undian (daftar peserta, perhitungan kecepatan, sudut putaran, deselerasi fisik, dan penentuan target pemenang).
4.  `DatabaseHelper.vb`: Class Singleton/Static untuk koneksi SQLite, DDL (inisialisasi tabel), dan operasi DML (Insert pemenang, Select riwayat).

## 4. UI/UX Specifications (Two-Column Layout)
*   **Main Form Size:** `950 x 580 px` (FixedDialog / Non-resizable), `StartPosition = CenterScreen`.
*   **Panel Kiri (Visual Utama):**
    *   `picRoda` (PictureBox): Ukuran `420 x 420 px`. Harus mengaktifkan fitur `DoubleBuffered` agar animasi mulus (tidak *flickering*).
    *   `btnPutar` (Button): Lebar `250 px`, tinggi `45 px`, Font Bold 12pt. Teks: "PUTAR RODA". Posisi di bawah `picRoda`.
*   **Panel Kanan (Dashboard & Data):**
    *   **GroupBox "Kelola Peserta":** Terdapat `txtInputNama` (TextBox), `btnTambah` (Button), `lstPeserta` (ListBox), `btnHapus` (Button), `btnImportTxt` (Button).
    *   **GroupBox "Pengaturan":** `chkEliminasi` (CheckBox: "Keluarkan pemenang dari putaran berikutnya"), `chkSuara` (CheckBox: "Bunyikan efek suara").
    *   **GroupBox "Riwayat Pemenang":** `dgvRiwayat` (DataGridView: 2 Kolom "Nama", "Waktu"), `btnBersihkanRiwayat` (Button).

## 5. Database Schema (SQLite)
Database file: `undian_history.db`
```sql
CREATE TABLE IF NOT EXISTS riwayat_undian (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nama_pemenang TEXT NOT NULL,
    waktu_menang DATETIME DEFAULT CURRENT_TIMESTAMP,
    jumlah_peserta INTEGER NOT NULL
);
```

## 6. Core Algorithms & Physics Engine

### A. Animasi & Fisika (WheelEngine.vb)
Sistem menggunakan **Animasi Deterministik**:
1.  Saat tombol "PUTAR RODA" ditekan, sistem langsung melakukan pengacakan pemenang: `Dim winnerIndex As Integer = Random.Next(0, lstPeserta.Items.Count)`.
2.  Hitung sudut target yang berada tepat di tengah segmen `winnerIndex` tersebut agar jarum (di sudut 270 derajat / atas tengah) berhenti pada nama tersebut.
3.  Set kecepatan awal (`speed = 40.0`).
4.  Timer Windows Forms berdetak setiap `15 ms`. Di dalam event tick:
    *   `currentAngle = (currentAngle + speed) Mod 360`
    *   `speed = speed * 0.985` (Friction / Deselerasi)
5.  Jika `speed < 0.2`, stop timer, pastikan `currentAngle` di-snap ke posisi akhir, dan *trigger event* selesai.

### B. Rendering (Renderer.vb)
*   **Irisan Roda:** Gunakan array warna (*Teal, Orange, Crimson, dll.*). Looping sebanyak jumlah peserta, hitung `sweepAngle = 360 / jumlahPeserta`. Gunakan `e.Graphics.FillPie`.
*   **Teks Peserta:** Gunakan `e.Graphics.TranslateTransform` ke titik pusat roda, lalu `RotateTransform` sesuai sudut tengah tiap irisan, dan `e.Graphics.DrawString` dengan format rata tengah (*Center-aligned*).
*   **Jarum Penunjuk:** Gambar polygon statis (segitiga terbalik warna `Color.Red` dengan border putih) tepat di atas koordinat pusat `picRoda`.

## 7. Instruksi Eksekusi untuk AI (Action Plan)
Kepada AI Agent, saat memulai implementasi, kerjakan dengan urutan fase berikut:
1.  **Fase 1:** Buat struktur kerangka project, class kosong, dan `DatabaseHelper.vb` beserta query SQLite-nya.
2.  **Fase 2:** Implementasi `FormUtama.Designer.vb` (Layouting kontrol Windows Forms).
3.  **Fase 3:** Implementasi `WheelEngine.vb` dan `Renderer.vb` untuk logika fisika dan penggambaran (GDI+).
4.  **Fase 4:** Wiring Event Handler di `FormUtama.vb` (Tombol tambah/hapus, Load DataGrid, Tombol Putar, dan integrasi Timer).