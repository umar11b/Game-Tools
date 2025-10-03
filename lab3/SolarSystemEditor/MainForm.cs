using System;
using System.Windows.Forms;

namespace SolarSystemEditor
{
    public partial class MainForm : Form
    {
        private GameControl gameControl = null!;
        private GameEditor? game;
        private SplitContainer splitContainer = null!;
        
        public MainForm()
        {
            InitializeComponent();
            InitializeGameControl();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Main Form Properties
            this.Text = "Solar System Editor";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            
            // Create Menu Strip
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Top;
            
            // File Menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("&File");
            ToolStripMenuItem newItem = new ToolStripMenuItem("&New", null, NewProject_Click);
            ToolStripMenuItem openItem = new ToolStripMenuItem("&Open", null, OpenProject_Click);
            ToolStripMenuItem saveItem = new ToolStripMenuItem("&Save", null, SaveGame_Click);
            ToolStripMenuItem exitItem = new ToolStripMenuItem("E&xit", null, Exit_Click);
            
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { 
                newItem, 
                openItem, 
                saveItem, 
                new ToolStripSeparator(), 
                exitItem 
            });
            
            // Controls Menu
            ToolStripMenuItem controlsMenu = new ToolStripMenuItem("&Controls");
            ToolStripMenuItem addSunItem = new ToolStripMenuItem("Add &Sun", null, AddSun_Click);
            ToolStripMenuItem addPlanetItem = new ToolStripMenuItem("Add &Planet", null, AddPlanet_Click);
            ToolStripMenuItem addMoonItem = new ToolStripMenuItem("Add &Moon", null, AddMoon_Click);
            
            controlsMenu.DropDownItems.AddRange(new ToolStripItem[] { addSunItem, addPlanetItem, addMoonItem });
            
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, controlsMenu });
            
            // Create Status Strip
            StatusStrip statusStrip = new StatusStrip();
            statusStrip.Dock = DockStyle.Bottom;
            
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel("Ready");
            statusLabel.Name = "statusLabel";
            statusLabel.Spring = true;
            statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            statusStrip.Items.Add(statusLabel);
            
            // Create Split Container for main layout
            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.SplitterDistance = 800; // Game view gets more space
            splitContainer.SplitterWidth = 3;
            splitContainer.Panel1.BackColor = System.Drawing.Color.DarkBlue;
            splitContainer.Panel2.BackColor = System.Drawing.Color.LightGray;
            
            // GameControl will be added here in InitializeGameControl
            
            // Add controls to form
            this.Controls.Add(splitContainer);
            this.Controls.Add(statusStrip);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private void InitializeGameControl()
        {
            try
            {
                // Create GameControl
                gameControl = new GameControl();
                gameControl.GameInitialized += OnGameInitialized;
                
                // Add to the split container panel
                splitContainer.Panel1.Controls.Add(gameControl);
                
                Console.WriteLine("GameControl created and added to form");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating GameControl: {ex.Message}");
            }
        }
        
        // Menu Event Handlers
        
        /// <summary>
        /// File > New - Creates a new solar system project
        /// </summary>
        private void NewProject_Click(object? sender, EventArgs e)
        {
            // Clear existing solar system
            game?.ClearSolarSystem();
            UpdateStatus("New solar system project created");
        }
        
        /// <summary>
        /// File > Open - Opens an existing solar system project
        /// </summary>
        private void OpenProject_Click(object? sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open Solar System Project";
            openDialog.Filter = "Solar System Project (*.ssp)|*.ssp|All Files (*.*)|*.*";
            
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                game?.LoadGame();
                UpdateStatus($"Opened project: {System.IO.Path.GetFileName(openDialog.FileName)}");
            }
        }
        
        /// <summary>
        /// File > Save - Saves the current solar system project
        /// </summary>
        private void SaveGame_Click(object? sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save Solar System Project";
            saveDialog.Filter = "Solar System Project (*.ssp)|*.ssp|All Files (*.*)|*.*";
            saveDialog.DefaultExt = "ssp";
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                game?.SaveGame();
                UpdateStatus($"Project saved: {System.IO.Path.GetFileName(saveDialog.FileName)}");
            }
        }
        
        /// <summary>
        /// File > Exit - Exits the application
        /// </summary>
        private void Exit_Click(object? sender, EventArgs e)
        {
            // Clean up MonoGame
            game?.Exit();
            game = null;

            this.Close();
        }
        
        /// <summary>
        /// Controls > Add Sun - Adds a sun to the solar system
        /// </summary>
        private void AddSun_Click(object? sender, EventArgs e)
        {
            game?.AddSun();
            UpdateStatus("Sun added to solar system");
        }
        
        /// <summary>
        /// Controls > Add Planet - Adds a planet to the solar system
        /// </summary>
        private void AddPlanet_Click(object? sender, EventArgs e)
        {
            game?.AddPlanet();
            UpdateStatus("Planet added to solar system");
        }
        
        /// <summary>
        /// Controls > Add Moon - Adds moons for each planet
        /// </summary>
        private void AddMoon_Click(object? sender, EventArgs e)
        {
            game?.AddMoon();
            UpdateStatus("Moons added to solar system");
        }
        
        private void OnGameInitialized(object? sender, EventArgs e)
        {
            game = gameControl?.Game;
            if (game != null)
            {
                UpdateStatus("MonoGame Ready - Use menu to add celestial bodies");
                Console.WriteLine("Game initialized and ready for commands");
            }
            else
            {
                UpdateStatus("MonoGame Error - Check console for details");
                Console.WriteLine("Game initialization failed");
            }
        }
        
        private void UpdateStatus(string message)
        {
            var statusStrip = (StatusStrip)this.Controls[1];
            var statusLabel = (ToolStripStatusLabel)statusStrip.Items[0];
            statusLabel.Text = message;
        }
    }
}
