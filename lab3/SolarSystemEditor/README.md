# Solar System Editor

A WinForms application with MonoGame integration for creating and editing 3D solar systems.

## Project Structure

### Core Files
- **MainForm.cs** - Main WinForms UI with menu system and layout
- **GameControl.cs** - Custom control for embedding MonoGame rendering
- **GameEditor.cs** - Main MonoGame Game class with solar system logic
- **SolarSystemObject.cs** - Object class for sun, planets, and moons
- **Program.cs** - Application entry point

### Content Assets
- **Sun.fbx** - 3D model for the sun
- **World.fbx** - 3D model for planets
- **Moon.fbx** - 3D model for moons
- **SunDiffuse.jpg** - Texture for the sun
- **WorldDiffuse.jpg** - Texture for planets
- **MoonDiffuse.jpg** - Texture for moons
- **MyShader.fx** - Custom shader for rendering
- **Content.mgcb** - MonoGame content pipeline file

## Features

### UI Components
- Menu strip with File and Controls menus
- Status strip for displaying current operations
- Split container layout with game view and properties panel
- Custom MonoGame control embedded in WinForms

### Solar System Features
- Add Sun (center object with rotation)
- Add Planets (up to 5, orbit around sun)
- Add Moons (orbit around each planet)
- Real-time orbital mechanics and rotation
- 3D rendering with custom shaders
- Camera positioned at (0, 0, 300) as required

### Project Management
- Create new solar system projects (.ssp files)
- Save/Load functionality
- Status updates for all operations

## Requirements

- .NET 9.0
- Windows Forms
- MonoGame Framework 3.8.4

## Building and Running

1. Open the project in Visual Studio or use .NET CLI
2. Build the solution
3. Run the application
4. Use the menu to add solar system objects and manage projects

## Usage

1. **Create Project**: File > Project > Create to start a new solar system
2. **Add Objects**: Controls menu to add Sun, Planets, and Moons
3. **Save/Load**: File menu to save or load your solar system
4. **View**: Watch your solar system render in real-time with orbital mechanics

The editor provides a complete solar system creation experience with proper 3D rendering, orbital mechanics, and project management capabilities.
