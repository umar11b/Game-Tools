# Solar System Editor - WinForms Edition

A 3D Solar System Editor built with **MonoGame** and **Windows Forms**.

## Project Structure

```
EditorRestart/
├── EditorRestart.sln          # Visual Studio Solution file
├── EditorRestart.csproj       # Project file
├── Program.cs                 # Entry point
├── MainForm.cs                # Main WinForms window with menus
├── MonoGameControl.cs         # UserControl that hosts MonoGame
├── GameEditor.cs              # MonoGame Game class (3D rendering)
├── SolarSystemObject.cs       # Solar system object model
└── Content/                   # Game assets
    ├── Content.mgcb           # MonoGame Content Pipeline
    ├── Sun.fbx                # Sun 3D model
    ├── World.fbx              # Planet 3D model
    ├── Moon.fbx               # Moon 3D model
    ├── SunDiffuse.jpg         # Sun texture
    ├── WorldDiffuse.jpg       # Planet texture
    ├── MoonDiffuse.jpg        # Moon texture
    └── MyShader.fx            # Custom shader
```

## How to Open in Visual Studio

1. Open **Visual Studio 2022**
2. Click **File → Open → Project/Solution**
3. Navigate to: `C:\Users\umar\Documents\Projects\Game-Tools\EditorRestart\`
4. Open **`EditorRestart.sln`**
5. Press **F5** to build and run

## How to Run from Command Line

```bash
cd C:/Users/umar/Documents/Projects/Game-Tools/EditorRestart
dotnet run
```

## Features

### Menu: File
- **New Project**: Clear all objects and start fresh
- **Open Project**: Load saved solar system from file
- **Save Game**: Save current solar system to file
- **Exit**: Close the application

### Menu: Controls
- **Add Sun**: Add a sun at the center (only one allowed)
- **Add Planet**: Add a planet orbiting the sun
- **Add Moon**: Add a moon orbiting the last added planet

## Technical Details

- **Framework**: .NET 8.0 with Windows Forms
- **Game Engine**: MonoGame 3.8.4 (WindowsDX)
- **3D Rendering**: Uses MyShader.fx for custom rendering
- **Camera**: Positioned at (0, 0, 300) looking at origin
- **Background**: Black
- **Assets**: FBX models with JPG textures

## Requirements

- Windows 11
- .NET 8.0 SDK
- Visual Studio 2022 (recommended)
- DirectX-compatible graphics card

## Notes

- The MonoGame window is embedded directly in the WinForms blue area
- Real-time 3D rendering at 60 FPS
- Orbital mechanics: planets orbit the sun, moons orbit planets
- All objects rotate on their Y-axis
- Save files are stored as `solar_system_save.txt`

## Troubleshooting

If you get graphics errors:
1. Make sure your graphics drivers are up to date
2. The project uses DirectX backend (WindowsDX)
3. If running on Azure VM, graphics support may be limited

