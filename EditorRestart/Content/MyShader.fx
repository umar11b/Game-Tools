/*
 * Lab 3 - MyShader.fx
 * Game Development Tools Course
 * 
 * Developed with assistance from Cursor AI
 * AI-powered HLSL shader for textured 3D rendering
 * https://cursor.sh/
 * 
 * Cursor AI assisted with:
 * - HLSL shader syntax and structure
 * - World, View, Projection matrix transformations
 * - Texture sampling and UV coordinates
 * - Vertex and pixel shader implementation
 */

// Global variables
float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldViewProjection;
Texture2D Texture;
sampler2D TextureSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

// Vertex shader input structure
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
};

// Vertex shader output structure
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinate : TEXCOORD0;
};

// Vertex shader
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    // Transform position to world-view-projection space
    output.Position = mul(input.Position, WorldViewProjection);
    
    // Pass through texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;
    
    return output;
}

// Pixel shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // Sample the texture using UV coordinates
    return tex2D(TextureSampler, input.TextureCoordinate);
}

// Technique definition
technique BasicTextureTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
