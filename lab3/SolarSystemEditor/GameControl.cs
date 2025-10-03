using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemEditor
{
    /// <summary>
    /// Custom UserControl that hosts MonoGame with proper graphics device management
    /// </summary>
    public partial class GameControl : UserControl
    {
        private GameEditor? game;
        private bool isInitialized = false;
        private System.Threading.Thread? gameThread;
        
        public GameEditor? Game => game;
        public event EventHandler? GameInitialized;
        
        public GameControl()
        {
            InitializeComponent();
            
            // Set control properties for proper rendering
            this.Size = new System.Drawing.Size(800, 600);
            this.BackColor = System.Drawing.Color.Black;
            this.Dock = DockStyle.Fill;
            
            // Enable double buffering for smoother rendering
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.Opaque | 
                     ControlStyles.UserPaint | 
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer, true);
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "GameControl";
            this.ResumeLayout(false);
        }
        
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            if (!isInitialized)
            {
                isInitialized = true;
                InitializeMonoGame();
            }
        }
        
        private void InitializeMonoGame()
        {
            try
            {
                // Create the MonoGame instance with proper graphics device setup
                game = new GameEditor(this.Handle, this.Width, this.Height);
                
                // Start the game in a separate thread to maintain 60 FPS
                gameThread = new System.Threading.Thread(() =>
                {
                    try
                    {
                        game.Run();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Game thread error: {ex.Message}");
                    }
                });
                
                gameThread.IsBackground = true;
                gameThread.Start();
                
                // Fire the initialized event
                this.Invoke(new Action(() => GameInitialized?.Invoke(this, EventArgs.Empty)));
                
                Console.WriteLine("MonoGame control initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MonoGame: {ex.Message}");
                
                // Create a fallback game instance for UI purposes
                game = new GameEditor();
                GameInitialized?.Invoke(this, EventArgs.Empty);
            }
        }
        
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            
            // Clean up MonoGame
            game?.Exit();
            game = null;
            
            // Wait for game thread to finish
            gameThread?.Join(1000);
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            // Notify the game about size changes for graphics device adjustment
            game?.ResizeGraphicsDevice(this.Width, this.Height);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw a simple background if MonoGame isn't rendering
            if (game == null)
            {
                e.Graphics.Clear(System.Drawing.Color.DarkBlue);
                using (var font = new System.Drawing.Font("Arial", 12))
                using (var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                {
                    e.Graphics.DrawString("Solar System Editor", font, brush, 10, 10);
                    e.Graphics.DrawString("Initializing MonoGame...", font, brush, 10, 40);
                }
            }
        }
        
        /// <summary>
        /// Gets the control handle for MonoGame graphics device creation
        /// </summary>
        public IntPtr ControlHandle => this.Handle;
    }
}
