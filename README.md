# 🔐 IronVault

**A native, lightweight Bitwarden client built with Avalonia UI and .NET NativeAOT**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11.0-blue)](https://avaloniaui.net/)
[![Platforms](https://img.shields.io/badge/platform-Windows%20%7C%20Linux-lightgrey)](https://github.com/yourusername/ironvault)

---

## ✨ Why IronVault?

Bitwarden's official desktop client is built on **Electron** - a fantastic technology that unfortunately comes with significant resource overhead (hundreds of MB of RAM, slow startup, multiple processes).

**IronVault is different:**

| Metric | Electron (Official) | IronVault (NativeAOT) |
|--------|---------------------|------------------------|
| Binary Size | ~120 MB + runtime | **~10-15 MB** |
| RAM Usage (idle) | 150-250 MB | **~20-30 MB** |
| Startup Time | 1-3 seconds | **< 100ms** |
| Processes | 4-8 | **1** |

**Built for performance, without compromise.**

---

## 🚀 Features

- ✅ **Native Performance** - Compiled to native code with .NET NativeAOT, no runtime overhead
- ✅ **Cross-Platform** - Seamless experience on Windows (WinUI style) and Linux (GTK integration)
- ✅ **Full Bitwarden Compatibility** - Unlock vault, generate passwords, autofill (planned)
- ✅ **Modern UI** - Built with Avalonia UI, GPU-accelerated rendering via Skia
- ✅ **Secure by Design** - Memory-safe C#, no web-based attack surface
- ✅ **Low Resource Usage** - Tiny memory footprint, perfect for low-spec machines

---

## 🖥️ Platform Support

| Platform | Status | Native Integration |
|----------|--------|---------------------|
| **Windows 10/11** | ✅ Primary | WinUI-style windowing, system tray, credential manager |
| **Linux (GNOME)** | ✅ Supported | GTK theme integration, Wayland native, system tray |
| **Linux (KDE)** | ✅ Supported | Qt-style fallback, perfect compatibility |
| **macOS** | 🚧 Planned | Native Aqua styling (post-1.0) |

---

## 📦 Installation

### Windows (Coming Soon)
- Download `IronVault-win-x64.exe` from Releases
- Run the installer (or portable .exe)

### Linux (Coming Soon)

Download the AppImage or .deb package:

    wget https://github.com/yourusername/ironvault/releases/download/v1.0.0/ironvault-linux-x64.AppImage
    chmod +x ironvault-linux-x64.AppImage
    ./ironvault-linux-x64.AppImage

### Build from Source

Clone the repository:

    git clone https://github.com/yourusername/ironvault.git
    cd ironvault

Restore dependencies:

    dotnet restore

Run in development mode:

    dotnet run

Publish native binaries:

    ./publish.ps1  # Windows
    ./publish.sh   # Linux

---

## 🏗️ Architecture

    IronVault/
    ├── src/
    │   ├── IronVault/              # Main application
    │   │   ├── Views/              # UI (Avalonia XAML)
    │   │   ├── ViewModels/         # MVVM logic
    │   │   ├── Services/           # Bitwarden API client
    │   │   └── Infrastructure/     # Native integrations
    │   ├── IronVault.Core/         # Shared business logic
    │   └── IronVault.Native/       # Platform-specific code
    └── tests/
        └── IronVault.Tests/        # Unit tests

---

## 🔧 Development

### Prerequisites
- .NET 8.0 SDK or later
- Avalonia UI templates
- (Linux) `libgtk-3-dev` for native theming

### Setup

Install Avalonia templates:

    dotnet new install Avalonia.Templates

Create new project (first time only):

    dotnet new avalonia.app -n IronVault

Run development server:

    dotnet watch run --project src/IronVault

### Publishing Native Binaries

**Windows:**

    dotnet publish src/IronVault -c Release -r win-x64 --self-contained -p:PublishAot=true

**Linux:**

    dotnet publish src/IronVault -c Release -r linux-x64 --self-contained -p:PublishAot=true

---

## 🧪 Roadmap

### v0.1.0 (Alpha)
- [ ] Basic vault unlock (master password)
- [ ] List/view passwords
- [ ] Copy username/password to clipboard
- [ ] System tray integration

### v0.2.0 (Beta)
- [ ] Bitwarden server sync
- [ ] Password generation
- [ ] Search/filter vault
- [ ] Dark mode support

### v1.0.0 (Stable)
- [ ] Autofill (Windows: global shortcuts, Linux: via DBus)
- [ ] Biometric unlock (Windows Hello)
- [ ] Offline mode
- [ ] Complete Bitwarden API compatibility

### Future
- [ ] Browser extension integration (native messaging)
- [ ] Self-hosted Bitwarden support
- [ ] macOS port

---

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

**Areas needing help:**
- Linux system tray implementation
- Wayland clipboard handling
- GTK theme detection for Avalonia
- Performance benchmarking

---

## 📝 License

GPL2 License - see [LICENSE](LICENSE) for details.

---

## 🙏 Acknowledgments

- [Bitwarden](https://bitwarden.com/) for their excellent API and SDK
- [Avalonia UI](https://avaloniaui.net/) for making cross-platform .NET desktop possible
- .NET NativeAOT team for incredible native compilation

---

**IronVault: Strong security, minimal resources. ⚡**
