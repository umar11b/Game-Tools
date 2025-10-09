/*
 * MonoGameControl - WinForms UserControl for hosting MonoGame
 */

using System;
using System.Windows.Forms;

namespace EditorRestart
{
    public class MonoGameControl : UserControl
    {
        public GameEditor? Game { get; private set; }
        public event EventHandler? GameInitialized;

        public MonoGameControl()
        {
            // Set control properties for proper rendering
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            UpdateStyles();

            this.Dock = DockStyle.Fill;
            this.BackColor = System.Drawing.Color.Black;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode)
            {
                InitializeMonoGame();
            }
        }

        private void InitializeMonoGame()
        {
            try
            {
                // Create MonoGame instance with proper WinForms integration
                Game = new GameEditor(this.Handle, this.Width, this.Height);
                Game.Run();

                // Fire the initialized event
                GameInitialized?.Invoke(this, EventArgs.Empty);

                Console.WriteLine("MonoGame control initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing MonoGame: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error initializing MonoGame: {ex.Message}\n\nPlease check your graphics drivers.", 
                    "MonoGame Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (Game != null)
            {
                Game.Dispose();
                Game = null;
            }

            base.OnHandleDestroyed(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (Game != null)
            {
                Game.ResizeViewport(this.Width, this.Height);
            }
        }
    }
}

