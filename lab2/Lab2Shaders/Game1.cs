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
    
    // Matrices used for rendering
    private Matrix m_world = Matrix.Identity;
    private Matrix m_view = Matrix.Identity;
    private Matrix m_projection = Matrix.Identity;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        m_device = new GraphicsDeviceManager(this); // Store the device pointer
        IsMouseVisible = true; // Make the mouse visible in the Mono window
        Content.RootDirectory = "Content"; // Set the content folder
    }

    protected override void Initialize()
    {
        // Basic effect by simply calling the constructor
        m_basicEffect = new BasicEffect(GraphicsDevice);

        // Rendering matrices (model at center of the world)
        m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        m_view = Matrix.CreateLookAt(new Vector3(0, 0, 3), // Camera position
                                     new Vector3(0, 0, 0), // Camera target
                                     Vector3.Up);          // Camera up
        m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                                                           m_device.GraphicsDevice.Viewport.AspectRatio,
                                                           0.1f,
                                                           100f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Call the CreateVertices() and CreateIndices() methods for icosahedron
        CreateVertices();
        CreateIndices();

        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        #region ConfigureBasicEffect
        // Set the BasicEffect's matrices to our game's current matrices
        m_basicEffect.World = m_world;
        m_basicEffect.View = m_view;
        m_basicEffect.Projection = m_projection;
        
        // Inform BasicEffect to use Vertex color information to render
        m_basicEffect.VertexColorEnabled = true;
        #endregion ConfigureBasicEffect

        #region ConfigureDevice
        // Clear the background with black
        GraphicsDevice.Clear(Color.Black);
        
        // Bind the vertex buffer data to the graphics device
        // (this is the data that will be sent to the GPU as soon as a Draw call is made)
        GraphicsDevice.SetVertexBuffer(m_vertexBuffer);
        
        // Set the indices for indexed rendering
        GraphicsDevice.Indices = m_indexBuffer;
        #endregion ConfigureDevice
        
        #region Render
        // Update the Render region in the Draw() method
        // Call DrawIndexedPrimitives to send the bound Vertices and Indices to the graphics device, and render it
        foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            // Draw the icosahedron using DrawIndexedPrimitives
            // PrimitiveType.TriangleList: Renders as individual triangles
            // 0: Starting vertex index in the buffer
            // 0: Starting index in the index buffer
            // 20: Number of triangles to draw (icosahedron has 20 triangular faces)
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 20);
        }
        #endregion Render

        base.Draw(gameTime);
    }

    private void CreateVertices()
    {
        // A temporary array, with 12 items in it, because the icosahedron has 12 distinct vertices
        VertexPositionColor[] vertices = new VertexPositionColor[12];

        // Vertex position and color information for icosahedron
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
        // Create an index array with 60 items (20 triangles × 3 indices per triangle)
        short[] indices = new short[60];

        // Define which vertices form each triangle of the icosahedron
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

        // Create IndexBuffer and copy the index data
        m_indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
        m_indexBuffer.SetData(indices);
    }
}
