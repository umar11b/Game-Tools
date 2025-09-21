using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab2Shaders;

public class Game1 : Game
{
    // The display device
    GraphicsDeviceManager m_device;
    
    // Pre-coded MonoGame shaders
    BasicEffect m_basicEffect;
    
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
        // Basic effect by simply calling the constructor
        m_basicEffect = new BasicEffect(GraphicsDevice);

        // Rendering matrices
        m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), Vector3.Up);
        m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                                                           m_device.GraphicsDevice.Viewport.AspectRatio,
                                                           0.1f, 100f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create icosahedron vertices and indices for Task 3
        CreateVertices();
        CreateIndices();
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
        // Task 3: Update world matrix with position and rotation
        m_world = Matrix.CreateTranslation(new Vector3(0.5f, 0.5f, 0.5f)) * 
                  Matrix.CreateRotationY(m_rotationY);
        
        // Configure BasicEffect
        m_basicEffect.World = m_world;
        m_basicEffect.View = m_view;
        m_basicEffect.Projection = m_projection;
        m_basicEffect.VertexColorEnabled = true;

        // Clear screen
        GraphicsDevice.Clear(Color.Black);
        
        // Bind vertex and index buffers
        GraphicsDevice.SetVertexBuffer(m_vertexBuffer);
        GraphicsDevice.Indices = m_indexBuffer;

        // Render icosahedron with index buffer
        foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            // Draw icosahedron using indexed primitives
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 20);
        }

        base.Draw(gameTime);
    }

    private void CreateVertices()
    {
        // Task 3: Icosahedron vertices (12 vertices with colors)
        VertexPositionColor[] vertices = new VertexPositionColor[12];

        // Icosahedron vertex positions and colors
        vertices[0] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, 0.42532500f), Color.Red);
        vertices[1] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, 0.42532500f), Color.Orange);
        vertices[2] = new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, -0.42532500f), Color.Yellow);
        vertices[3] = new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, -0.42532500f), Color.Green);
        vertices[4] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, 0.26286500f), Color.Blue);
        vertices[5] = new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, -0.26286500f), Color.Indigo);
        vertices[6] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, 0.26286500f), Color.Purple);
        vertices[7] = new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, -0.26286500f), Color.White);
        vertices[8] = new VertexPositionColor(new Vector3(0.42532500f, 0.26286500f, 0.0000000f), Color.Cyan);
        vertices[9] = new VertexPositionColor(new Vector3(-0.42532500f, 0.26286500f, 0.0000000f), Color.Black);
        vertices[10] = new VertexPositionColor(new Vector3(0.42532500f, -0.26286500f, 0.0000000f), Color.DodgerBlue);
        vertices[11] = new VertexPositionColor(new Vector3(-0.42532500f, -0.26286500f, 0.0000000f), Color.Crimson);

        m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
        m_vertexBuffer.SetData<VertexPositionColor>(vertices);
    }

    private void CreateIndices()
    {
        // Task 3: Icosahedron indices (60 indices for 20 triangles)
        short[] indices = new short[60];

        // Define icosahedron triangular faces
        indices[0] = 0; indices[1] = 6; indices[2] = 1;
        indices[3] = 0; indices[4] = 11; indices[5] = 6;
        indices[6] = 1; indices[7] = 4; indices[8] = 0;
        indices[9] = 1; indices[10] = 8; indices[11] = 4;
        indices[12] = 1; indices[13] = 10; indices[14] = 8;
        indices[15] = 2; indices[16] = 5; indices[17] = 3;
        indices[18] = 2; indices[19] = 9; indices[20] = 5;
        indices[21] = 2; indices[22] = 11; indices[23] = 9;
        indices[24] = 3; indices[25] = 7; indices[26] = 2;
        indices[27] = 3; indices[28] = 10; indices[29] = 7;
        indices[30] = 4; indices[31] = 8; indices[32] = 5;
        indices[33] = 4; indices[34] = 9; indices[35] = 0;
        indices[36] = 5; indices[37] = 8; indices[38] = 3;
        indices[39] = 5; indices[40] = 9; indices[41] = 4;
        indices[42] = 6; indices[43] = 10; indices[44] = 1;
        indices[45] = 6; indices[46] = 11; indices[47] = 7;
        indices[48] = 7; indices[49] = 10; indices[50] = 6;
        indices[51] = 7; indices[52] = 11; indices[53] = 2;
        indices[54] = 8; indices[55] = 10; indices[56] = 3;
        indices[57] = 9; indices[58] = 11; indices[59] = 0;

        m_indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
        m_indexBuffer.SetData(indices);
    }
}