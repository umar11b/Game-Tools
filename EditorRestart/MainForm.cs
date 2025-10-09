/*
 * MainForm - Main WinForms window for Solar System Editor
 */

using System;
using System.Windows.Forms;

namespace EditorRestart
{
    public partial class MainForm : Form
    {
        private MonoGameControl gameControl = null!;
        private ToolStripStatusLabel statusLabel = null!;

        public MainForm()
        {
            InitializeComponent();
            InitializeGameControl();
        }

        private void InitializeComponent()
        {
            // Set up the form
            this.Text = "Solar System Editor";
            this.Width = 1024;
            this.Height = 768;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create menu strip
            MenuStrip menuStrip = new MenuStrip();

            // File menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("New Project", null, NewProject_Click);
            fileMenu.DropDownItems.Add("Open Project", null, OpenProject_Click);
            fileMenu.DropDownItems.Add("Save Game", null, SaveGame_Click);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Exit", null, Exit_Click);
            menuStrip.Items.Add(fileMenu);

            // Controls menu
            ToolStripMenuItem controlsMenu = new ToolStripMenuItem("Controls");
            controlsMenu.DropDownItems.Add("Add Sun", null, AddSun_Click);
            controlsMenu.DropDownItems.Add("Add Planet", null, AddPlanet_Click);
            controlsMenu.DropDownItems.Add("Add Moon", null, AddMoon_Click);
            menuStrip.Items.Add(controlsMenu);

            // Add menu strip to form
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Create status strip
            StatusStrip statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready");
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void InitializeGameControl()
        {
            try
            {
                // Create MonoGame control
                gameControl = new MonoGameControl();
                gameControl.GameInitialized += OnGameInitialized;
                gameControl.Dock = DockStyle.Fill;

                // Add to the form
                this.Controls.Add(gameControl);
                gameControl.BringToFront();

                Console.WriteLine("GameControl created and added to form");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating GameControl: {ex.Message}");
                MessageBox.Show($"Error creating GameControl: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnGameInitialized(object? sender, EventArgs e)
        {
            UpdateStatus("MonoGame initialized successfully");
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(message)));
                return;
            }

            statusLabel.Text = message;
        }

        // Menu event handlers
        private void NewProject_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.NewProject();
                UpdateStatus("New project created");
            }
        }

        private void OpenProject_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.LoadGame();
                UpdateStatus("Game loaded");
            }
        }

        private void SaveGame_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.SaveGame();
                UpdateStatus("Game saved");
            }
        }

        private void Exit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void AddSun_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.AddSun();
                UpdateStatus("Sun added to solar system");
            }
        }

        private void AddPlanet_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.AddPlanet();
                UpdateStatus("Planet added to solar system");
            }
        }

        private void AddMoon_Click(object? sender, EventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.AddMoon();
                UpdateStatus("Moon added to solar system");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (gameControl?.Game != null)
            {
                gameControl.Game.Exit();
            }

            base.OnFormClosing(e);
        }
    }
}

