using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace SolarSystemEditor
{
    /// <summary>
    /// Custom UserControl that properly hosts MonoGame with WinForms integration
    /// </summary>
    public partial class GameControl : UserControl
    {
        private GameEditor? game;
        private bool isInitialized = false;
        
        public GameEditor? Game => game;
        public SimpleSolarSystemRenderer? SimpleRenderer => simpleRenderer;
        public event EventHandler? GameInitialized;
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
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
        
        private SimpleSolarSystemRenderer? simpleRenderer;
        
        private void InitializeMonoGame()
        {
            try
            {
                // Hide console window
                var consoleHandle = GetConsoleWindow();
                ShowWindow(consoleHandle, 0); // 0 = SW_HIDE
                
                // Create MonoGame instance with proper WinForms integration
                game = new GameEditor(this.Handle, this.Width, this.Height);
                
                // Fire the initialized event
                this.Invoke(new Action(() => GameInitialized?.Invoke(this, EventArgs.Empty)));
                
                Console.WriteLine("MonoGame control initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MonoGame: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine("Falling back to simple renderer for Azure VM compatibility");
                
                // Create simple renderer fallback for Azure VMs
                simpleRenderer = new SimpleSolarSystemRenderer();
                this.Controls.Add(simpleRenderer);
                
                // Fire the initialized event
                GameInitialized?.Invoke(this, EventArgs.Empty);
            }
        }
        
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            
            // Clean up MonoGame
            game?.Exit();
            game = null;
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            // Notify the game about size changes
            game?.ResizeGraphicsDevice(this.Width, this.Height);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw background if MonoGame isn't rendering
            if (game == null)
            {
                e.Graphics.Clear(System.Drawing.Color.DarkBlue);
                using (var font = new System.Drawing.Font("Arial", 12))
                using (var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                {
                    e.Graphics.DrawString("Initializing MonoGame...", font, brush, 10, 10);
                }
            }
        }
    }
}
