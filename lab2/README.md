# Lab 2 - Shaders, HLSL, Fonts and Texturing

## Course Information

- **Course:** Game Development Tools
- **Lab:** Lab 2 - Week 03 Content
- **Student:** [Your Name]
- **Date:** September 2024

## AI-Assisted Development

This lab was developed with significant assistance from **Cursor AI**, an AI-powered code editor and development assistant.

### Cursor AI Contributions

- **Project Setup:** MonoGame project configuration and cross-platform compatibility
- **Shader Implementation:** HLSL code structure and shader concepts
- **Code Generation:** Vertex buffer, primitive type, and rendering pipeline code
- **Debugging Support:** macOS Wine compatibility issues and build optimization
- **Educational Guidance:** Comprehensive explanations of shader concepts and MonoGame architecture

### Cursor AI Citation

```
Cursor AI. (2024). AI-powered code editor and development assistant.
Retrieved from https://cursor.sh/

AI-assisted development for Game Development Tools Lab 2.
Code generation, debugging, and educational support provided by Cursor AI.
```

## Lab 2 Tasks Completed

### Task 1: Icosahedron with IndexBuffers

- 12 colorful vertices forming 20 triangular faces
- Efficient vertex reuse with IndexBuffer
- Comprehensive 3D geometry demonstration

### Task 2: Primitive Types

- LineList: Individual disconnected lines
- LineStrip: Connected lines forming a path
- TriangleList: Individual triangles
- TriangleStrip: Connected triangles
- Interactive cycling with SPACEBAR

### Task 3: Rotating Icosahedron

- Positioned at (0.5f, 0.5f, 0.5f) as required
- Continuous Y-axis rotation around its own pivot
- IndexBuffer rendering with 60 indices

### Task 4: Custom Shader Triangle

- Red, Green, Blue vertex colors with smooth interpolation
- Complete HLSL shader workflow demonstration
- Comprehensive shader concepts documentation
- MyShader.fx with all required HLSL elements

## Technical Implementation

### Shader Concepts Covered

1. **Preprocessor Directives:** OpenGL/DirectX compatibility
2. **Effect Parameters:** WorldViewProjection matrix handling
3. **Type Structures:** VertexShaderInput/Output definitions
4. **Semantics:** POSITION0, SV_POSITION, COLOR0
5. **Vertex Shader:** MainVS with transformation
6. **Pixel Shader:** MainPS with color interpolation
7. **Techniques and Passes:** BasicColorDrawing technique

### Cross-Platform Considerations

- macOS Wine compilation limitations addressed
- BasicEffect fallback implementation
- Educational value maintained despite technical constraints

## Repository Structure

```
lab2/
├── Lab2Shaders/
│   ├── Game1.cs (Main game class with all tasks)
│   ├── Content/
│   │   ├── MyShader.fx (Custom HLSL shader)
│   │   └── Content.mgcb (Content pipeline configuration)
│   └── Lab2Shaders.csproj (Project file)
└── README.md (This file)
```

## Git Tags for Demonstration

- `lab2-task1` - Icosahedron with IndexBuffers
- `lab2-task2` - Primitive Types demonstration
- `lab2-task3` - Rotating Icosahedron
- `lab2-task4` - Custom Shader Triangle

## Acknowledgments

Special thanks to **Cursor AI** for providing AI-powered development assistance, code generation, debugging support, and educational guidance throughout this lab implementation.

**Cursor AI Website:** https://cursor.sh/
