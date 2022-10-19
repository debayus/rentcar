<h3 align="center">RENT CAR</h3>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Daftar isi</summary>
  <ol>
    <li><a href="#latar_belakang">Latar Belakang</a></li>
    <li><a href="#masalah_yang_dihadapi">Masalah yang dihadapi</a></li>
    <li><a href="#solusi">Solusi</a></li>
    <li><a href="#batasan_masalah">Batasan Masalah</a></li>
    <li><a href="#usecase_diagram">Usecase Diagram</a></li>
    <li>
      <a href="#activity_diagram">Activity Diagram</a>
      <ul>
        <li><a href="#activity_diagram_1">Menitipkan Kendaraan</a></li>
        <li><a href="#activity_diagram_2">Melihat Laporan Kendaraan</a></li>
        <li><a href="#activity_diagram_3">Mengelola Kondisi Kendaraan</a></li>
        <li><a href="#activity_diagram_4">Melihat Ketersediaan Kendaraan</a></li>
        <li><a href="#activity_diagram_5">Menyewa Kendaraan</a></li>
        <li><a href="#activity_diagram_6">Melihat Laporan Omset</a></li>
      </ul>
    </li>
    <li><a href="#entity_relationship_diagram">Entity Relationship Diagram</a></li>
  </ol>
</details>

# Rent Car

Rent Car adalah suatu unit usaha di bidang jasa penyewaan kendaraan motor maupun mobil. 
vendor atau pemilik mobil dapat menitipkan kendaraannya untuk disewakan.

adapun alur dari proses penyewaan kendaraan sebagai berikut.

1	vendor dapat menitipkan kendaraannya
2	custusmer telpon / datang ke kantor untuk memesan kendaraan
3	melakukan diskusi harga dan tanggal yang diinginkan
4	mencetak surat perjanjian dan di tandatangani
5	melakukan pembayaran dp / langsung lunas
6	admin melihat kelengkapan dan kesiapan kendaraan
7	kendaraan di foto kemudian di serahkan ke customer dan menitipkan tanda pengenal
8	kemungkinan perpanjang sewa bisa terjadi
9	pengembalian kendaraan oleh customer
10	pengecekan dan memfoto kondisi kendaraan
11	pelunasan biaya / biaya tambahan bisa terjadi
12  pengembalian tanda pengenal

========================================================================

Masalah yang di hadapi

1	customer tidak bisa mengubungi saat jam tutup
2	ketersediaan kendaraan yang sulit di ketahui
3	sering lupa harga sewa setiap kendaraan
4	catatan yang menumpuk sering hilang
5	1 kendaraan disewa lebih dari 2 orang di waktu yang sama
6	foto kebanyakan, susah di cari
7	rekam omset sulit dilakukan
8	vendor tidak tau keadalan mobil

========================================================================

Solusi

1   adanya system berbasis website untuk customer agar bisa melihat ketersediaan kendaraan dan melakukan pemesan
2   adanya system untuk mengelola data kendaraan dan pemesanan
3   adanya system berbasis website untuk vendor adar bisa melihat kondisi kendaraannya

========================================================================

Batasan Masalah

1	aplikasi ini hanya berbasis website
2	aplikasi ini mengunakan freamework asp.net core
3	aplikasi ini berbasis database dengan sql server 2008 r2
4	mengeloa data kendaraan dan penyewaan hanya oleh admin
5   vendor hanya dapat melihat kondisi kendaraan
6	customer hanya dapat melihat kendaraan yang tersedia dan melakukan penyewaan
7	customer hanya dapat pemesanan online dengan mengisi identitas diri secara lengkap
8   data online valid jika sudah pembayaran dp
9	aplikasi ini hanya mencetak surat perjanjian sesuai inputan
10	aplikasi ini hanya dapat menyimpan foto keadaan mobil
11	aplikasi ini hanya mencatat pembayaran
12	aplikasi ini hanya dapat menghasilkan laporan omset, laporan sewa dan laporan kondisi mobil
