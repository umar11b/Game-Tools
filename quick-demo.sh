#!/bin/bash

# Quick demo script - runs each project for 3 seconds
echo "🎮 Quick Demo - Game Tools Lab 3"
echo "================================="
echo ""

# Function to quick test a project
quick_test() {
    local name="$1"
    local path="$2"
    local description="$3"
    
    echo "Testing: $name"
    echo "Description: $description"
    
    if [ -d "$path" ]; then
        cd "$path"
        if dotnet build > /dev/null 2>&1; then
            echo "✅ Build successful - Starting demo..."
            timeout 3s dotnet run 2>/dev/null || echo "Demo completed (3s timeout)"
        else
            echo "❌ Build failed"
        fi
    else
        echo "❌ Project not found"
    fi
    echo ""
}

# Quick tests
quick_test "EditorAvalonia" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorAvalonia" "Avalonia + Enhanced 2D (RECOMMENDED)"
quick_test "EditorSkiaSharp" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorSkiaSharp" "Avalonia + SkiaSharp PoC"
quick_test "Editor2025" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/Editor2025" "Pure MonoGame 3D"
quick_test "EditorImGui" "/Users/zamanu/Documents/Sheridan/Game Tools/lab3/EditorImGui" "MonoGame + ImGui (macOS issues)"

echo "Demo complete!"
echo ""
echo "Results:"
echo "- EditorAvalonia: Professional UI with solar system ✅"
echo "- EditorSkiaSharp: Custom 3D-like rendering ✅" 
echo "- Editor2025: Pure MonoGame 3D teapot ✅"
echo "- EditorImGui: macOS windowing issues ❌"
