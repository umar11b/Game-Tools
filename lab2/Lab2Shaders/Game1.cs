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
    
    // Task 2: Current primitive type
    private PrimitiveType m_currentPrimitiveType = PrimitiveType.LineList;
    
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
        // Create simple vertices for Task 2 primitives
        CreateVertices();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Cycle through primitive types with spacebar
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            switch (m_currentPrimitiveType)
            {
                case PrimitiveType.LineList:
                    m_currentPrimitiveType = PrimitiveType.LineStrip;
                    break;
                case PrimitiveType.LineStrip:
                    m_currentPrimitiveType = PrimitiveType.TriangleList;
                    break;
                case PrimitiveType.TriangleList:
                    m_currentPrimitiveType = PrimitiveType.TriangleStrip;
                    break;
                case PrimitiveType.TriangleStrip:
                    m_currentPrimitiveType = PrimitiveType.LineList;
                    break;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Configure BasicEffect
        m_basicEffect.World = m_world;
        m_basicEffect.View = m_view;
        m_basicEffect.Projection = m_projection;
        m_basicEffect.VertexColorEnabled = true;

        // Clear screen
        GraphicsDevice.Clear(Color.Black);
        
        // Bind vertex buffer
        GraphicsDevice.SetVertexBuffer(m_vertexBuffer);

        // Render current primitive type
        foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            // Draw based on current primitive type
            switch (m_currentPrimitiveType)
            {
                case PrimitiveType.LineList:
                    // Draw individual lines
                    GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 2);
                    break;
                case PrimitiveType.LineStrip:
                    // Draw connected lines
                    GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, 3);
                    break;
                case PrimitiveType.TriangleList:
                    // Draw individual triangles
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    break;
                case PrimitiveType.TriangleStrip:
                    // Draw connected triangles
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                    break;
            }
        }

        base.Draw(gameTime);
    }

    private void CreateVertices()
    {
        // Simple vertices for demonstrating different primitive types
        VertexPositionColor[] vertices = new VertexPositionColor[6];

        // Line vertices
        vertices[0] = new VertexPositionColor(new Vector3(-1, 0, 0), Color.Red);
        vertices[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Red);
        
        // Line strip vertices
        vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0), Color.Green);
        vertices[3] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Green);
        vertices[4] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0), Color.Green);
        
        // Triangle vertex
        vertices[5] = new VertexPositionColor(new Vector3(0, -0.5f, 0), Color.Blue);

        m_vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
        m_vertexBuffer.SetData<VertexPositionColor>(vertices);
    }
}