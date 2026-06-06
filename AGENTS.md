# 🤖 IronVault Developer Agent Guide (AGENTS.md)

Welcome, Developer Agent! This document is designed to help you quickly understand the **IronVault** codebase, its architecture, guidelines, and constraints, so you can write high-quality, secure, and compatible code.

---

## 📌 Repository Overview

**IronVault** is a native, lightweight Bitwarden client designed to be a fast, low-overhead alternative to the official Electron-based client.
- **Languages/Frameworks**: C# 13, .NET 9.0, Avalonia UI (v12.x)
- **Target Platform**: Windows (WinUI style) and Linux (GTK/Wayland/KDE)
- **Compilation Goal**: **.NET NativeAOT** (Ahead-Of-Time compilation). Performance & low memory consumption are top priorities.

---

## 🏗️ Project Architecture

The solution ([IronVault.sln](file:///D:/Projects/ironvault/IronVault.sln)) is structured into the following projects:

1. **[IronVault.App](file:///D:/Projects/ironvault/src/IronVault.App)**:
   - **Role**: The UI layer and entry point.
   - **Technologies**: Avalonia UI (XAML), MVVM structure.
   - **Key files**: [Program.cs](file:///D:/Projects/ironvault/src/IronVault.App/Program.cs), [App.axaml](file:///D:/Projects/ironvault/src/IronVault.App/App.axaml), [MainWindow.axaml](file:///D:/Projects/ironvault/src/IronVault.App/MainWindow.axaml).
2. **[IronVault.Core](file:///D:/Projects/ironvault/src/IronVault.Core)**:
   - **Role**: Core models, interfaces, configurations, state management, and basic business logic.
   - **Constraints**: Pure .NET, no UI dependencies.
3. **[IronVault.Bitwarden](file:///D:/Projects/ironvault/src/IronVault.Bitwarden)**:
   - **Role**: Bitwarden API integrations, networking, cryptography (unlocking vault, key derivation, decrypting items).
4. **[IronVault.Platform](file:///D:/Projects/ironvault/src/IronVault.Platform)**:
   - **Role**: Platform-specific native APIs, e.g. system tray, credentials manager (Windows Credential Manager, Linux Secret Service), DBus/Wayland integrations.
5. **[Tests](file:///D:/Projects/ironvault/tests)**:
   - Unit tests are located under the `tests/` directory:
     - [IronVault.Core.Tests](file:///D:/Projects/ironvault/tests/IronVault.Core.Tests)
     - [IronVault.Bitwarden.Tests](file:///D:/Projects/ironvault/tests/IronVault.Bitwarden.Tests)

---

## ⚠️ Critical Constraint: NativeAOT Compatibility

Since IronVault compiles using .NET NativeAOT for speed and size optimizations:
1. **No Dynamic Reflection**: Avoid runtime reflection-based behaviors (e.g. `Type.GetType`, `Assembly.Load`, or automatic dependency registration by scanning assemblies). Use compile-time code generation or explicit configuration.
2. **AOT-Safe JSON Serialization**: Never use runtime-reflection-based JSON serialization.
   - Use `System.Text.Json` source generator (`[JsonSerializable]`) for all JSON payloads.
   - Pass `JsonSerializerContext` to serializer methods.
3. **Trim-Friendly Code**: Be careful with generic virtual methods, reflection, and generic structures that might be trimmed.

---

## 💻 Coding Style & Guidelines

1. **Language Version**: C# 13 and .NET 9.0 features.
   - Prefer **primary constructors** for viewmodels and services.
   - Use **collection expressions** (`[1, 2, 3]`) instead of traditional arrays/lists initialization where appropriate.
   - Use **file-scoped namespaces** to reduce nesting.
2. **Nullable Reference Types**: Keep `#nullable enable` active on all files.
3. **Comments & Documentation**:
   - Preserve existing unrelated comments and docstrings.
   - Document any non-obvious architecture or workaround decisions.
4. **Asynchronous Programming**:
   - Always use `Task` / `ValueTask` for async operations.
   - Use `ConfigureAwait(false)` when context is not required (e.g., in Core and Bitwarden libraries).
   - Use `Dispatcher.UIThread.InvokeAsync` / `Post` only when you must manipulate Avalonia UI controls from a background thread.
5. **UI & Styling**:
   - Use Compiled Bindings by default (`x:CompileBindings="True"`).
   - Follow MVVM pattern: Views shouldn't contain business logic.
   - Keep designs premium and polished. Avoid default controls styling; leverage Avalonia Fluent theme.

---

## 🔒 Security Guidelines

- **Sensitive Data Handling**: Never store master passwords or decrypted master keys in plain text `string` variables long-term.
- Clear/overwrite security-sensitive arrays/buffers after use.
- Cryptographic keys should be handled with utmost care following standard Bitwarden guidelines.

---

## 🛠️ CLI Reference

Here are the typical commands you can run when working on this repository:

| Action | Command |
| :--- | :--- |
| **Restore packages** | `dotnet restore` |
| **Build solution** | `dotnet build` |
| **Run unit tests** | `dotnet test` |
| **Run app in dev mode** | `dotnet run --project src/IronVault.App` |
| **AOT Publish (Windows)** | `dotnet publish src/IronVault.App -c Release -r win-x64 --self-contained -p:PublishAot=true` |
| **AOT Publish (Linux)** | `dotnet publish src/IronVault.App -c Release -r linux-x64 --self-contained -p:PublishAot=true` |

---

Happy coding, agent! Let's build a secure and lightning-fast Bitwarden client. 🔐⚡
