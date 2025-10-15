# TarkovShocker

**TarkovShocker** is a BepInEx plugin for *Escape from Tarkov* (SPT or modded clients) that integrates with OpenShock to provide haptic feedback (shock or vibration) when your player takes damage.

---

## Features

- Sends feedback via OpenShock devices on player damage.
- Configurable between shocking and vibration.
- Intensity is dynamic and sets to the amount of damage you take.
- Debug mode with console logging.
- Test key (F7) to trigger feedback manually.

---

## Updates / Changelog


### 1.0.2 - Small update

- Added a fixed or dynamic intensity option (dynamic is based on damage taken, fixed is a set value)
- Added intensity configuration to the config file and in-game menu

### 1.0.1 - Small QoL Update
- Added restrictions to duration in config (prevents breaking the plugin)
- Added drop-down menu for feedback type (Shock / Vibrate)
- Refactored codebase for future additions
- Cleaned up the look of the in-game config menu

---

## Installation for prebuilt release

1. Ensure you have **BepInEx installed** in your SPT or EFT folder.
2. Copy the `TarkovShocker` folder (containing `TarkovShocker.dll`) into your BepInEx/plugins/ directory:
3. Start the game — the plugin will auto-load.

---

## Configuration

Configuration is stored in `TarkovShocker.cfg` in the BepInEx `config` folder, or you can configure it in-game using the F12 menu.

| Option                  | Description                                           | Default       |
|-------------------------|-------------------------------------------------------|---------------|
| `Plugin Enabled`        | Enable or disable the plugin                          | `true`        |
| `ApiKey`                | Your OpenShock API key                                 | `""`          |
| `ApiHost`               | OpenShock API URL                                     | `"https://api.openshock.app"` |
| `DefaultShockerId`      | Default Shocker ID to trigger                         | `""`          |
| `Duration MS`           | Shock/vibration duration in milliseconds             | `500`         |
| `Shock or Vibration`    | `true` for Shock, `false` for Vibration              | `true`        |
| `Debug Mode`            | Enable console logging for events                     | `true`        |

---

## Usage

- Press **F7** in-game to test feedback manually (requires debug mode enabled).
- Damage events automatically trigger feedback according to your settings.

---

## Development

- Built with **BepInEx 5.x**, **HarmonyLib**, and **Unity 2020+ assemblies**.
- Harmony patch is applied to `Player.ApplyDamageInfo` to detect local player damage.
- Configurable through BepInEx config system.

---

### Build Instructions

1. Clone the repository.
2. Open the solution in **Visual Studio 2022** or newer.
3. Ensure references point to your **SPT / EFT Managed DLLs**.
4. Build the project — the DLL will auto-copy to `BepInEx/plugins/TarkovShocker`.

---

## Contributing

Contributions are welcome!  

- Fork the repository.
- Create a branch for your feature or fix.
- Submit a pull request with a clear description of changes.

---

## License

MIT License © 2025 Whitezom7
