#!/bin/bash

# Demo script for Game Tools Lab 3 projects
# Tests all approaches: MonoGame, Avalonia, ImGui, SkiaSharp

echo "🎮 Game Tools Lab 3 - Demo Script"
echo "================================="
echo ""

# Function to test a project
test_project() {
    local name="$1"
    local path="$2"
    local description="$3"
    
    echo "Testing: $name"
    echo "Path: $path"
    echo "Description: $description"
    echo "---"
    
    if [ -d "$path" ]; then
        echo "✅ Project exists"
        cd "$path"
        
        # Try to build
        echo "Building..."
        if dotnet build > /dev/null 2>&1; then
            echo "✅ Build successful"
            echo "Running (press Ctrl+C to stop)..."
            echo ""
            dotnet run
        else
            echo "❌ Build failed"
        fi
    else
        echo "❌ Project not found"
    fi
    
    echo ""
    echo "Press Enter to continue to next project..."
    read -r
    echo "================================="
    echo ""
}

# Test all projects
echo "1. EditorAvalonia - Avalonia + Enhanced 2D (RECOMMENDED)"
test_project "EditorAvalonia" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorAvalonia" "Professional UI with enhanced 2D solar system"

echo "2. EditorSkiaSharp - Avalonia + SkiaSharp PoC"
test_project "EditorSkiaSharp" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorSkiaSharp" "Custom Avalonia control with 3D-like effects"

echo "3. Editor2025 - Pure MonoGame 3D"
test_project "Editor2025" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/Editor2025" "Standalone MonoGame 3D teapot"

echo "4. EditorImGui - MonoGame + ImGui (PROBLEMATIC)"
test_project "EditorImGui" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorImGui" "MonoGame with ImGui (has macOS issues)"

echo "5. Lab2Shaders - Shader Demo"
test_project "Lab2Shaders" "/Users/zamanu/Documents/Sheridan/Game Tools/lab2/Lab2Shaders" "MonoGame shader demonstration"

echo "Demo complete!"
echo ""
echo "Summary:"
echo "- EditorAvalonia: ✅ Works (recommended for assignment)"
echo "- EditorSkiaSharp: ✅ Works (PoC alternative)"
echo "- Editor2025: ✅ Works (pure MonoGame)"
echo "- EditorImGui: ❌ macOS issues (window doesn't appear)"
echo "- Lab2Shaders: ✅ Works (shader demo)"
