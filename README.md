# OpenCodeLauncher

Лаунчер для управления [OpenCode](https://opencode.ai) сервером на Windows: запуск сервера для выбранного проекта на выбранном сетевом интерфейсе (Tailscale / LAN / loopback), проверка доступности, профили запуска, мониторинг сессий и статистики.

> **Дисклеймер:** этот проект не создан командой OpenCode, не аффилирован с ней и не поддерживается ею. «OpenCode» — товарный знак/название проекта [Anomaly (opencode.ai)](https://opencode.ai). Используется только название в описании назначения приложения.

## Плашки

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D6?style=flat-square&logo=windows)
![Windows](https://img.shields.io/badge/platform-Windows-blue?style=flat-square&logo=windows)
![OpenCode](https://img.shields.io/badge/powered%20by-OpenCode-000000?style=flat-square)
![Tailscale](https://img.shields.io/badge/Tailscale-ready-1c93c3?style=flat-square)
![License MIT](https://img.shields.io/badge/license-MIT-green?style=flat-square)

## Возможности

- **Запуск сервера** — выбор проекта (в т.ч. избранное), сетевого интерфейса и порта; сервер запускается на выбранном IP (например, Tailscale `100.x.x.x`), чтобы подключаться с телефона.
- **Базовая авторизация** — логин/пароль сервера задаются прямо в приложении и передаются серверу (`OPENCODE_SERVER_PASSWORD`), настройки хранятся рядом с exe.
- **Профили запуска** — сохранённые наборы «проект + IP + порт + модель + флаги», запуск в один клик.
- **Дашборд** — статус сервера, список сессий, статистика токенов/стоимости (`opencode session list`, `opencode stats`).
- **Системный трей** — сворачивание, старт/стоп из трея, автозапуск с Windows.
- **Логирование** — Serilog, логи в папке `logs/` рядом с приложением.
- **Хранение настроек** — `settings.json` рядом с exe (с авто-бэкапом `settings.json.bak`).

## Требования

- Windows 10/11, .NET 8 (или готовая self-contained сборка).
- Установленный [OpenCode](https://opencode.ai) CLI: `npm i -g opencode-ai` (запускается через `opencode.cmd`/`opencode.exe` из PATH).
- Опционально: [Tailscale](https://tailscale.com) для доступа с телефона по VPN-сети.

## Сборка и запуск

```powershell
# Debug
dotnet build OpenCodeLauncher.sln

# Тесты
dotnet test OpenCodeLauncher.sln

# Публикация (single-file, self-contained, без .NET на целевой машине)
dotnet publish src/OpenCodeLauncher/OpenCodeLauncher.csproj -c Release -r win-x64 `
  --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish
```

## Быстрый старт

1. Запустите `OpenCodeLauncher.exe`.
2. Вкладка **Запуск**: укажите проект, выберите IP (Tailscale/LAN), порт (например 4096), при необходимости логин/пароль → **Запустить сервер**.
3. На телефоне откройте `http://<выбранный-IP>:<порт>/` и введите логин/пароль.

## Структура проекта

```
OpenCodeLauncher.sln
├── src/OpenCodeLauncher/          # WinForms-приложение (UI, презентеры, MVP)
├── src/OpenCodeLauncher.Core/     # Сервисы и модели (без зависимости от WinForms)
├── tests/OpenCodeLauncher.Tests/  # xUnit-тесты
├── AGENTS.md                      # правила для агента
├── PLAN.md                        # план разработки
└── LICENSE
```

## Используемое ПО и лицензии

| Компонент | Лицензия |
|---|---|
| [OpenCode](https://github.com/anomalyco/opencode) | [MIT](https://github.com/anomalyco/opencode/blob/dev/LICENSE) |
| .NET 8 / Windows Forms | [MIT](https://github.com/dotnet/runtime/blob/main/LICENSE.TXT) |
| [Serilog](https://serilog.net) | Apache-2.0 |
| [xUnit](https://xunit.net) | Apache-2.0 |
| Tailscale (совместимость) | BSD-3-Clause |

## Лицензия проекта

Распространяется по лицензии **MIT** — см. [LICENSE](LICENSE).
