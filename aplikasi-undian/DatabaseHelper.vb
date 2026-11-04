Imports System.Data
Imports System.IO
Imports Microsoft.Data.Sqlite

Public Class DatabaseHelper
    Private Shared ReadOnly DbFileName As String = "undian_history.db"
    Private Shared ReadOnly DbPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName)
    Private Shared ReadOnly ConnectionString As String = $"Data Source={DbPath}"

    ''' <summary>
    ''' Inisialisasi skema database SQLite (membuat tabel jika belum ada).
    ''' </summary>
    Public Shared Sub InitializeDatabase()
        Try
            Using conn As New SqliteConnection(ConnectionString)
                conn.Open()
                Dim createTableSql As String = "
                    CREATE TABLE IF NOT EXISTS riwayat_undian (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nama_pemenang TEXT NOT NULL,
                        waktu_menang DATETIME DEFAULT CURRENT_TIMESTAMP,
                        jumlah_peserta INTEGER NOT NULL
                    );"
                Using cmd As New SqliteCommand(createTableSql, conn)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New ApplicationException($"Gagal menginisialisasi database: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Menyimpan data pemenang undian ke tabel riwayat_undian.
    ''' </summary>
    Public Shared Function InsertWinner(namaPemenang As String, jumlahPeserta As Integer) As Boolean
        Try
            Using conn As New SqliteConnection(ConnectionString)
                conn.Open()
                Dim insertSql As String = "
                    INSERT INTO riwayat_undian (nama_pemenang, waktu_menang, jumlah_peserta)
                    VALUES (@nama, datetime('now', 'localtime'), @jumlah);"
                Using cmd As New SqliteCommand(insertSql, conn)
                    cmd.Parameters.AddWithValue("@nama", namaPemenang)
                    cmd.Parameters.AddWithValue("@jumlah", jumlahPeserta)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error InsertWinner: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Mengambil seluruh data riwayat pemenang untuk ditampilkan di DataGridView.
    ''' Kolom: Nama, Waktu
    ''' </summary>
    Public Shared Function GetHistory() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Nama", GetType(String))
        dt.Columns.Add("Waktu", GetType(String))

        Try
            Using conn As New SqliteConnection(ConnectionString)
                conn.Open()
                Dim querySql As String = "
                    SELECT nama_pemenang, waktu_menang 
                    FROM riwayat_undian 
                    ORDER BY id DESC;"
                Using cmd As New SqliteCommand(querySql, conn)
                    Using reader As SqliteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim nama As String = reader.GetString(0)
                            Dim waktu As String = If(reader.IsDBNull(1), "-", reader.GetString(1))
                            dt.Rows.Add(nama, waktu)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error GetHistory: {ex.Message}")
        End Try

        Return dt
    End Function

    ''' <summary>
    ''' Menghapus seluruh riwayat undian dari database.
    ''' </summary>
    Public Shared Function ClearHistory() As Boolean
        Try
            Using conn As New SqliteConnection(ConnectionString)
                conn.Open()
                Dim deleteSql As String = "DELETE FROM riwayat_undian;"
                Using cmd As New SqliteCommand(deleteSql, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' Reset auto-increment sequence
                Dim resetSeqSql As String = "DELETE FROM sqlite_sequence WHERE name='riwayat_undian';"
                Using cmdSeq As New SqliteCommand(resetSeqSql, conn)
                    Try
                        cmdSeq.ExecuteNonQuery()
                    Catch
                        ' Table sqlite_sequence might not exist if no inserts occurred
                    End Try
                End Using
                Return True
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error ClearHistory: {ex.Message}")
            Return False
        End Try
    End Function
End Class
