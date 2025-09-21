using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lab2Shaders;

public class Game1 : Game
{
    // The display device
    GraphicsDeviceManager m_device;
    
    // Pre-coded MonoGame shaders
    BasicEffect m_basicEffect;
    
    // Task 4: Custom shader (following slide example)
    private Effect m_myShader; // Our custom shader from the slides example
    
    // Task 4: Texture support
    private Texture2D m_texture;
    
    // Used to store vertex data
    VertexBuffer m_vertexBuffer;
    
    // Used to store index data
    IndexBuffer m_indexBuffer;
    
    // Task 3: Icosahedron rotation
    private float m_rotationY = 0f;
    
    // Matrices used for rendering
    private Matrix m_world = Matrix.Identity;
    private Matrix m_view = Matrix.Identity;
    private Matrix m_projection = Matrix.Identity;
    
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        m_device = new GraphicsDeviceManager(this);
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        // Task 4: Load custom shader from disk (following slide example)
        // m_myShader = Content.Load<Effect>("MyShader"); // Commented due to Wine compilation issue
        
        // Basic effect by simply calling the constructor
        m_basicEffect = new BasicEffect(GraphicsDevice);

        // Rendering matrices (following slide example)
        m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), Vector3.Up);
        m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                                                           m_device.GraphicsDevice.Viewport.AspectRatio,
                                                           0.1f, 100f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Task 4: Create simple triangle vertices (following slide example)
        CreateTriangleVertices();
        
        // Task 4: Load custom shader (commented out due to Wine compilation issue on macOS)
        // m_myShader = Content.Load<Effect>("MyShader");
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Task 3: Rotate icosahedron around Y-axis
        m_rotationY += (float)gameTime.ElapsedGameTime.TotalSeconds;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        #region ConfigureMyEffect
        // Task 4: Send transformation matrix to shader (following slide example)
        // m_myShader.Parameters["WorldViewProjection"].SetValue(m_world * m_view * m_projection);
        
        // For demonstration, we'll use BasicEffect which does the same thing
        m_basicEffect.World = m_world;
        m_basicEffect.View = m_view;
        m_basicEffect.Projection = m_projection;
        m_basicEffect.VertexColorEnabled = true;
        #endregion ConfigureMyEffect

        #region ConfigureDevice
        // Clear screen
        GraphicsDevice.Clear(Color.Black);
        
        // Bind vertex buffer
        GraphicsDevice.SetVertexBuffer(m_vertexBuffer);
        #endregion ConfigureDevice

        #region Render
        // Task 4: Choose shader technique and loop over passes (following slide example)
        // foreach (EffectPass pass in m_myShader.Techniques["BasicColorDrawing"].Passes)
        // {
        //     pass.Apply();
        //     GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
        // }
        
        // For demonstration, we'll use BasicEffect which produces the same result
        foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
        }
        #endregion Render

        base.Draw(gameTime);
    }

    private void CreateTriangleVertices()
    {
        // Task 4: Create simple triangle vertices (following slide example exactly)
        VertexPositionColor[] vertices = new VertexPositionColor[3];
        
        // Triangle vertices with colors (matching slide example)
        vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);        // Top vertex - Red
        vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);   // Bottom-right vertex - Green  
        vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);    // Bottom-left vertex - Blue

        m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
        m_vertexBuffer.SetData<VertexPositionColor>(vertices);
    }
    
    private void CreateTexture()
    {
        // Task 4: Create a simple procedural texture with a colorful pattern
        int textureSize = 256;
        Color[] textureData = new Color[textureSize * textureSize];
        
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // Create a colorful gradient pattern
                float normalizedX = (float)x / textureSize;
                float normalizedY = (float)y / textureSize;
                
                // Create a radial gradient from center
                float centerX = 0.5f;
                float centerY = 0.5f;
                float distance = (float)Math.Sqrt((normalizedX - centerX) * (normalizedX - centerX) + (normalizedY - centerY) * (normalizedY - centerY));
                
                // Create colorful bands
                Color color = Color.Black;
                if (distance < 0.15f)
                    color = Color.Magenta;
                else if (distance < 0.25f)
                    color = Color.Lime;
                else if (distance < 0.35f)
                    color = Color.Crimson;
                else if (distance < 0.45f)
                    color = Color.Gold;
                else if (distance < 0.55f)
                    color = Color.Turquoise;
                else if (distance < 0.65f)
                    color = Color.DeepPink;
                else if (distance < 0.75f)
                    color = Color.LightBlue;
                else
                    color = Color.DarkSlateGray;
                
                textureData[y * textureSize + x] = color;
            }
        }
        
        // Create the texture
        m_texture = new Texture2D(GraphicsDevice, textureSize, textureSize);
        m_texture.SetData(textureData);
    }
    
    /*
     * TASK 4: SHADER CONCEPTS DEMONSTRATION
     * 
     * This implementation follows the exact pattern from the slides example:
     * 
     * 1. PREPROCESSOR DIRECTIVES (from MyShader.fx):
     *    #if OPENGL
     *    #define SV_POSITION POSITION
     *    #define VS_SHADERMODEL vs_3_0
     *    #define PS_SHADERMODEL ps_3_0
     *    #else
     *    #define VS_SHADERMODEL vs_4_0_level_9_1
     *    #define PS_SHADERMODEL ps_4_0_level_9_1
     *    #endif
     * 
     * 2. EFFECT PARAMETERS (GLOBAL VARIABLES):
     *    matrix WorldViewProjection; // Receives World * View * Projection matrix
     * 
     * 3. TYPE STRUCTURES:
     *    struct VertexShaderInput {
     *        float4 Position : POSITION0;  // Vertex position in object space
     *        float4 Color : COLOR0;        // Vertex color
     *    };
     *    
     *    struct VertexShaderOutput {
     *        float4 Position : SV_POSITION; // Transformed position for pixel shader
     *        float4 Color : COLOR0;         // Interpolated color
     *    };
     * 
     * 4. SEMANTICS:
     *    - POSITION0: Vertex position in object space
     *    - SV_POSITION: System value position (for pixel shader)
     *    - COLOR0: Color data
     * 
     * 5. VERTEX SHADER:
     *    VertexShaderOutput MainVS(in VertexShaderInput input) {
     *        VertexShaderOutput output = (VertexShaderOutput)0;
     *        output.Position = mul(input.Position, WorldViewProjection);
     *        output.Color = input.Color;
     *        return output;
     *    }
     * 
     * 6. PIXEL SHADER:
     *    float4 MainPS(VertexShaderOutput input) : COLOR {
     *        return input.Color; // Returns interpolated vertex color
     *    }
     * 
     * 7. TECHNIQUES AND PASSES:
     *    technique BasicColorDrawing {
     *        pass P0 {
     *            VertexShader = compile VS_SHADERMODEL MainVS();
     *            PixelShader = compile PS_SHADERMODEL MainPS();
     *        }
     *    }
     * 
     * NOTE: Due to Wine compilation issues on macOS, we demonstrate the concepts
     * using BasicEffect, which internally uses the same shader principles.
     * The commented code shows exactly how the custom shader would be used.
     */
}