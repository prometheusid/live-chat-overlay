# Live Chat Overlay

A lightweight, always-on-top YouTube live chat overlay for Windows. It is built
with C# and WPF, and uses Microsoft Edge WebView2 to display the chat.

## Features

- Borderless, transparent, always-on-top window.
- YouTube live chat styling with a transparent page background.
- Click-through mode, toggled from the toolbar or with **Ctrl+Shift+O**.
- Draggable handle in the top-left corner; disable click-through before moving.
- Starts with an empty URL field and on-screen setup instructions.
- Single-file, self-contained Windows x64 publish output.

## Requirements

- Windows 10 or later (x64).
- Microsoft Edge WebView2 Runtime.
- .NET 8 SDK to build from source.

The published executable includes the .NET runtime, but WebView2 Runtime must
still be installed separately. WebView2 is used to render YouTube chat.

## Quick start

Download the latest Windows x64 build from the
[v1.0.0 GitHub Release](https://github.com/prometheusid/live-chat-overlay/releases/tag/v1.0.0).
Extract the ZIP, then:

1. Start `LiveChatOverlay.exe`.
2. Open the YouTube stream and its live chat pop-out.
3. Copy the live chat page URL and paste it into the overlay's URL field.
4. Select **Load**.
5. Position the overlay using the dotted handle in the top-left corner.
6. Enable **Click-through** or press **Ctrl+Shift+O** when you want mouse input
   to pass through to the game.

To move the window or use its toolbar, turn click-through off first. When
click-through is enabled, use **Ctrl+Shift+O** to turn it off because mouse
clicks are passed to the window underneath.

For games, **Borderless Windowed** is generally the most compatible display
mode. Exclusive fullscreen may cover desktop overlays.

## Build from source

Install the .NET 8 SDK, open PowerShell in the repository directory, then run:

```powershell
dotnet run
```

## Publish a standalone Windows executable

```powershell
dotnet publish LiveChatOverlay.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

The executable is written to:

```text
bin\Release\net8.0-windows\win-x64\publish\LiveChatOverlay.exe
```

The executable can be shared on its own with Windows x64 users who have the
WebView2 Runtime installed. For a GitHub repository, keep generated `bin/` and
`obj/` files out of source control; attach the published executable to a
versioned GitHub Release if you want to distribute a ready-to-run build.

## Troubleshooting

- **The chat area is blank:** check the URL, internet connection, and that the
  URL opens a YouTube live chat page.
- **The app reports a WebView2 error:** install or repair the Microsoft Edge
  WebView2 Runtime.
- **The overlay is behind a game:** switch the game to Borderless Windowed or
  Windowed mode. Overlay behavior in exclusive fullscreen depends on the game
  and Windows.
- **The overlay will not move:** turn off click-through, then drag the dotted
  handle in the top-left corner.
- **The toolbar does not respond:** click-through may be enabled. Press
  **Ctrl+Shift+O** to toggle it off.

YouTube can change its chat page structure at any time. If its markup changes,
the custom CSS in `YoutubeChat.css` may need updating. Google Fonts are loaded
from the internet.

## License and third-party assets

The original application source code and `YoutubeChat.css` are licensed under
the MIT License; see `LICENSE`. The logo originated from PNGWing and is not
covered by MIT. It is included under separate non-commercial-use terms as
understood by the project maintainer. Do not use the logo in a commercial
product or paid distribution; replace or remove `Assets/live-chat.png` and
`Assets/live-chat.ico` for those uses. Check the original asset page and its
current terms before reuse.

Microsoft.Web.WebView2, the WebView2 Runtime, and the Google Fonts used by the
chat page are third-party components/services and remain under their own terms.

## Privacy and account behavior

The overlay loads the URL you provide in WebView2. It does not bypass YouTube
sign-in requirements. The sign-in prompt and chat input are hidden by styling;
this app is intended for viewing chat and does not send messages.

## Project contents

- `App.xaml`, `App.xaml.cs`, `MainWindow.xaml`, `MainWindow.xaml.cs` — WPF app.
- `YoutubeChat.css` — styling injected into YouTube chat pages.
- `Assets/live-chat.png`, `Assets/live-chat.ico` — app logo and icon.
- `LiveChatOverlay.csproj` — build and package configuration.
- `.gitignore` — excludes generated and machine-specific files.
- `LICENSE` — MIT License for the original source and CSS, with the logo
  exclusion noted above.

---

# Live Chat Overlay (Bahasa Indonesia)

Overlay YouTube Live Chat yang ringan untuk Windows, dengan jendela transparan
dan selalu berada di atas. Aplikasi dibuat dengan C# dan WPF, serta menggunakan
Microsoft Edge WebView2 untuk menampilkan chat.

## Fitur

- Jendela tanpa border, transparan, dan selalu di atas.
- Styling YouTube Live Chat dengan latar halaman transparan.
- Mode click-through dari toolbar atau shortcut **Ctrl+Shift+O**.
- Pegangan titik di pojok kiri atas untuk menggeser jendela; matikan
  click-through sebelum menggeser.
- Kolom URL kosong dan petunjuk penggunaan saat aplikasi pertama dibuka.
- Hasil publish satu file mandiri untuk Windows x64.

## Persyaratan

- Windows 10 atau lebih baru (x64).
- Microsoft Edge WebView2 Runtime.
- .NET 8 SDK untuk build dari source.

File EXE hasil publish sudah menyertakan .NET Runtime, tetapi WebView2 Runtime
tetap harus dipasang secara terpisah. WebView2 digunakan untuk menampilkan chat
YouTube.

## Cara cepat menggunakan

Unduh build Windows x64 dari
[GitHub Release v1.0.0](https://github.com/prometheusid/live-chat-overlay/releases/tag/v1.0.0).
Ekstrak file ZIP, lalu:

1. Jalankan `LiveChatOverlay.exe`.
2. Buka stream YouTube dan pop-out live chat-nya.
3. Salin URL halaman live chat, lalu tempel ke kolom URL overlay.
4. Klik **Load**.
5. Atur posisi overlay dengan pegangan titik di pojok kiri atas.
6. Aktifkan **Click-through** atau tekan **Ctrl+Shift+O** agar klik mouse
   diteruskan ke game.

Untuk menggeser jendela atau menggunakan toolbar, matikan click-through
terlebih dahulu. Saat click-through aktif, gunakan **Ctrl+Shift+O** untuk
menonaktifkannya karena klik mouse diteruskan ke jendela di bawah overlay.

Untuk bermain game, mode **Borderless Windowed** umumnya paling kompatibel.
Exclusive fullscreen dapat menutupi overlay desktop.

## Build dari source

Pasang .NET 8 SDK, buka PowerShell di folder repository, lalu jalankan:

```powershell
dotnet run
```

## Membuat file executable Windows mandiri

```powershell
dotnet publish LiveChatOverlay.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

File executable akan dibuat di:

```text
bin\Release\net8.0-windows\win-x64\publish\LiveChatOverlay.exe
```

EXE dapat dibagikan sendiri kepada pengguna Windows x64 yang sudah memasang
WebView2 Runtime. Untuk repository GitHub, jangan masukkan file hasil build
`bin/` dan `obj/` ke source control; lampirkan EXE pada GitHub Release
bernomor versi jika ingin membagikan build siap pakai.

## Pemecahan masalah

- **Area chat kosong:** periksa URL, koneksi internet, dan pastikan URL membuka
  halaman YouTube Live Chat.
- **Muncul error WebView2:** pasang atau perbaiki Microsoft Edge WebView2
  Runtime.
- **Overlay tertutup game:** ubah mode tampilan game ke Borderless Windowed
  atau Windowed. Perilaku overlay pada exclusive fullscreen bergantung pada
  game dan Windows.
- **Overlay tidak bisa digeser:** matikan click-through, lalu seret pegangan
  titik di pojok kiri atas.
- **Toolbar tidak merespons:** click-through mungkin sedang aktif. Tekan
  **Ctrl+Shift+O** untuk menonaktifkannya.

YouTube dapat mengubah struktur halaman chat kapan saja. Jika markup berubah,
CSS di `YoutubeChat.css` mungkin perlu diperbarui. Google Fonts dimuat melalui
koneksi internet.

## Lisensi dan aset pihak ketiga

Source code asli aplikasi dan `YoutubeChat.css` menggunakan lisensi MIT;
lihat file `LICENSE`. Logo berasal dari PNGWing dan tidak tercakup MIT. Logo
disertakan dengan ketentuan penggunaan nonkomersial sebagaimana dipahami oleh
maintainer proyek. Jangan gunakan logo untuk produk komersial atau distribusi
berbayar; ganti atau hapus `Assets/live-chat.png` dan `Assets/live-chat.ico`
untuk penggunaan tersebut. Periksa halaman aset asli dan ketentuan terkininya
sebelum menggunakan ulang logo.

Microsoft.Web.WebView2, WebView2 Runtime, dan Google Fonts yang dipakai halaman
chat adalah komponen/layanan pihak ketiga dengan ketentuan lisensi masing-masing.

## Privasi dan akun

Overlay memuat URL yang Anda berikan di WebView2. Aplikasi tidak melewati
persyaratan login YouTube. Prompt login dan kolom chat hanya disembunyikan oleh
styling; aplikasi ini ditujukan untuk membaca chat dan tidak mengirim pesan.

## Isi proyek

- `App.xaml`, `App.xaml.cs`, `MainWindow.xaml`, `MainWindow.xaml.cs` — aplikasi
  WPF.
- `YoutubeChat.css` — styling yang diterapkan pada halaman YouTube Live Chat.
- `Assets/live-chat.png`, `Assets/live-chat.ico` — logo dan ikon aplikasi.
- `LiveChatOverlay.csproj` — konfigurasi build dan packaging.
- `.gitignore` — mengecualikan file hasil build dan file khusus komputer lokal.
- `LICENSE` — lisensi MIT untuk source code dan CSS asli, dengan pengecualian
  logo seperti dijelaskan di atas.
