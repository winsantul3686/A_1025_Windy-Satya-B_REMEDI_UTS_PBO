using System;
using System.Collections.Generic;

namespace WarehouseApp
{
    // ============================
    // INTERFACE: Abstraksi (Interface)
    // Mendefinisikan kontrak transaksi yang harus dimiliki oleh semua barang
    // ============================
    interface ITransaksi
    {
        void BarangMasuk(int jumlah);
        void BarangKeluar(int jumlah);
    }

    // ============================
    // KELAS ABSTRAK: Barang (Super Class)
    // Menerapkan ENKAPSULASI (private/protected field)
    // dan menjadi dasar untuk Inheritance (pewarisan)
    // ============================
    abstract class Barang : ITransaksi
    {
        // Atribut dienkapsulasi (tidak bisa diubah langsung dari luar)
        private string kodeBarang;
        private string namaBarang;
        private int stok;

        // Getter dan Setter (mengatur akses variabel private)
        public string KodeBarang { get { return kodeBarang; } }
        public string NamaBarang { get { return namaBarang; } set { namaBarang = value; } }
        public int Stok { get { return stok; } }

        // Konstruktor
        protected Barang(string kode, string nama, int stok)
        {
            this.kodeBarang = kode;
            this.namaBarang = nama;
            this.stok = stok;
        }

        // Method protected agar hanya bisa dipanggil di dalam class turunan
        protected void TambahStok(int jumlah)
        {
            stok += jumlah;
        }

        protected void KurangiStok(int jumlah)
        {
            stok -= jumlah;
        }

        // Implementasi metode interface
        public virtual void BarangMasuk(int jumlah)
        {
            if (jumlah > 0)
                TambahStok(jumlah);
            else
                Console.WriteLine("Jumlah barang masuk harus lebih dari 0!");
        }

        public virtual void BarangKeluar(int jumlah)
        {
            if (jumlah <= 0)
                Console.WriteLine("Jumlah keluar harus lebih dari 0!");
            else if (jumlah > stok)
                Console.WriteLine("Stok tidak mencukupi!");
            else
                KurangiStok(jumlah);
        }

        // Abstraksi: setiap jenis barang wajib punya cara menampilkan info
        public abstract void InfoBarang();
    }

    // ============================
    // SUBCLASS 1: Barang Umum
    // Mewarisi kelas Barang
    // ============================
    class BarangUmum : Barang
    {
        public BarangUmum(string kode, string nama, int stok) : base(kode, nama, stok) { }

        // Polimorfisme: override cara menampilkan info barang
        public override void InfoBarang()
        {
            Console.WriteLine($"[Umum] {KodeBarang} - {NamaBarang} (Stok: {Stok})");
        }
    }

    // ============================
    // SUBCLASS 2: Barang Mudah Pecah
    // Mewarisi kelas Barang dan override aturan transaksi keluar
    // ============================
    class BarangMudahPecah : Barang
    {
        public BarangMudahPecah(string kode, string nama, int stok) : base(kode, nama, stok) { }

        // Override method BarangKeluar (Polimorfisme)
        public override void BarangKeluar(int jumlah)
        {
            if (jumlah > 10)
                Console.WriteLine("Barang mudah pecah hanya boleh keluar maksimal 10 unit!");
            else
                base.BarangKeluar(jumlah);
        }

        public override void InfoBarang()
        {
            Console.WriteLine($"[Mudah Pecah] {KodeBarang} - {NamaBarang} (Stok: {Stok})");
        }
    }

    // ============================
    // CLASS: Transaksi
    // Menyimpan data transaksi barang masuk dan keluar
    // ============================
    class Transaksi
    {
        public string KodeBarang { get; set; }
        public string JenisTransaksi { get; set; }
        public int Jumlah { get; set; }
        public DateTime Tanggal { get; set; }

        public Transaksi(string kode, string jenis, int jumlah)
        {
            KodeBarang = kode;
            JenisTransaksi = jenis;
            Jumlah = jumlah;
            Tanggal = DateTime.Now;
        }

        public void InfoTransaksi()
        {
            Console.WriteLine($"{Tanggal:dd/MM/yyyy HH:mm} | {JenisTransaksi} | Kode: {KodeBarang} | Jumlah: {Jumlah}");
        }
    }

    // ============================
    // CLASS UTAMA: Program (Menu Aplikasi)
    // Menampilkan menu interaktif ke pengguna
    // ============================
    class Program
    {
        // List untuk menyimpan data barang dan transaksi
        static List<Barang> daftarBarang = new List<Barang>();
        static List<Transaksi> riwayat = new List<Transaksi>();

        static void Main()
        {
            // ==== DATA AWAL (contoh isi warehouse) ====
            daftarBarang.Add(new BarangUmum("U001", "Kardus", 50));
            daftarBarang.Add(new BarangUmum("U002", "Lakban", 30));
            daftarBarang.Add(new BarangMudahPecah("P001", "Gelas Kaca", 20));
            daftarBarang.Add(new BarangMudahPecah("P002", "Piring Keramik", 15));

            // ==== LOOP MENU UTAMA ====
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SISTEM MANAJEMEN WAREHOUSE ===");
                Console.WriteLine("1. Tambah Barang");
                Console.WriteLine("2. Ubah Nama Barang");
                Console.WriteLine("3. Lihat Daftar Barang");
                Console.WriteLine("4. Barang Masuk");
                Console.WriteLine("5. Barang Keluar");
                Console.WriteLine("6. Lihat Riwayat Transaksi");
                Console.WriteLine("7. Lihat Ringkasan Stok");
                Console.WriteLine("0. Keluar");
                Console.Write("Pilih menu: ");
                string pilihan = Console.ReadLine();

                // Pilihan menu
                switch (pilihan)
                {
                    case "1": TambahBarang(); break;
                    case "2": UbahNamaBarang(); break;
                    case "3": LihatDaftarBarang(); break;
                    case "4": BarangMasuk(); break;
                    case "5": BarangKeluar(); break;
                    case "6": LihatRiwayat(); break;
                    case "7": LihatRingkasan(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }

                Console.WriteLine("\nTekan ENTER untuk kembali ke menu...");
                Console.ReadLine();
            }
        }

        // ============================
        // FITUR 1: Tambah Barang Baru
        // ============================
        static void TambahBarang()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH BARANG ===");
            Console.Write("Kode Barang: ");
            string kode = Console.ReadLine();
            Console.Write("Nama Barang: ");
            string nama = Console.ReadLine();
            Console.Write("Stok Awal: ");
            int stok = int.Parse(Console.ReadLine());
            Console.Write("Jenis (1. Umum, 2. Mudah Pecah): ");
            string jenis = Console.ReadLine();

            if (jenis == "1")
                daftarBarang.Add(new BarangUmum(kode, nama, stok));
            else
                daftarBarang.Add(new BarangMudahPecah(kode, nama, stok));

            Console.WriteLine("Barang berhasil ditambahkan!");
        }

        // ============================
        // FITUR 2: Ubah Nama Barang
        // ============================
        static void UbahNamaBarang()
        {
            Console.Clear();
            Console.WriteLine("=== UBAH NAMA BARANG ===");
            Console.WriteLine("Daftar Barang Saat Ini:");
            foreach (var b in daftarBarang)
                b.InfoBarang();

            Console.Write("\nMasukkan kode barang yang ingin diubah: ");
            string kode = Console.ReadLine();
            Barang barang = daftarBarang.Find(b => b.KodeBarang == kode);

            if (barang != null)
            {
                Console.Write($"Nama baru untuk {barang.NamaBarang}: ");
                barang.NamaBarang = Console.ReadLine();
                Console.WriteLine("Nama barang berhasil diubah!");
            }
            else
            {
                Console.WriteLine("Kode barang tidak ditemukan!");
            }
        }

        // ============================
        // FITUR 3: Lihat Daftar Barang
        // ============================
        static void LihatDaftarBarang()
        {
            Console.Clear();
            Console.WriteLine("=== DAFTAR BARANG ===");
            foreach (var b in daftarBarang)
                b.InfoBarang();
        }

        // ============================
        // FITUR 4: Transaksi Barang Masuk
        // ============================
        static void BarangMasuk()
        {
            Console.Clear();
            Console.WriteLine("=== TRANSAKSI BARANG MASUK ===");
            LihatDaftarBarang();
            Console.Write("\nMasukkan kode barang: ");
            string kode = Console.ReadLine();
            Barang barang = daftarBarang.Find(b => b.KodeBarang == kode);

            if (barang != null)
            {
                Console.Write("Jumlah masuk: ");
                int jumlah = int.Parse(Console.ReadLine());
                barang.BarangMasuk(jumlah);
                riwayat.Add(new Transaksi(kode, "IN", jumlah));
                Console.WriteLine("Transaksi berhasil!");
            }
            else
            {
                Console.WriteLine("Kode barang tidak ditemukan!");
            }
        }

        // ============================
        // FITUR 5: Transaksi Barang Keluar
        // ============================
        static void BarangKeluar()
        {
            Console.Clear();
            Console.WriteLine("=== TRANSAKSI BARANG KELUAR ===");
            LihatDaftarBarang();
            Console.Write("\nMasukkan kode barang: ");
            string kode = Console.ReadLine();
            Barang barang = daftarBarang.Find(b => b.KodeBarang == kode);

            if (barang != null)
            {
                Console.Write("Jumlah keluar: ");
                int jumlah = int.Parse(Console.ReadLine());
                barang.BarangKeluar(jumlah);
                riwayat.Add(new Transaksi(kode, "OUT", jumlah));
                Console.WriteLine("Transaksi berhasil!");
            }
            else
            {
                Console.WriteLine("Kode barang tidak ditemukan!");
            }
        }

        // ============================
        // FITUR 6: Lihat Riwayat Transaksi
        // ============================
        static void LihatRiwayat()
        {
            Console.Clear();
            Console.WriteLine("=== RIWAYAT TRANSAKSI ===");
            if (riwayat.Count == 0)
                Console.WriteLine("Belum ada transaksi.");
            else
                foreach (var t in riwayat)
                    t.InfoTransaksi();
        }

        // ============================
        // FITUR 7: Lihat Ringkasan Stok
        // ============================
        static void LihatRingkasan()
        {
            Console.Clear();
            Console.WriteLine("=== RINGKASAN STOK ===");
            int total = 0;
            foreach (var b in daftarBarang)
            {
                b.InfoBarang();
                total += b.Stok;
            }
            Console.WriteLine($"\nTotal seluruh stok barang: {total}");
        }
    }
}
