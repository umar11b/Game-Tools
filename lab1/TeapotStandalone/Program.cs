using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TeapotStandalone
{
    public class TeapotGame : Game
    {
        GraphicsDeviceManager m_device; // The display device
        Model m_model; // The variable our imported FBX model will use
        private Matrix m_world = Matrix.Identity;
        private Matrix m_view = Matrix.Identity;
        private Matrix m_projection = Matrix.Identity;

        public TeapotGame()
        {
            m_device = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            m_view = Matrix.CreateLookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), Vector3.Up);
            m_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), m_device.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_model = Content.Load<Model>("teapot");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_world *= Matrix.CreateRotationY(0.1f);
            foreach (var mesh in m_model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = m_world;
                    effect.View = m_view;
                    effect.Projection = m_projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }

    public class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("=== Teapot Test ===");
            Console.WriteLine("Starting 3D teapot rendering...");
            Console.WriteLine("Press ESC to exit");
            Console.WriteLine();
            
            using (var game = new TeapotGame())
            {
                game.Run();
            }
        }
    }
}
