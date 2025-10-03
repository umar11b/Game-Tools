using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SolarSystemEditor
{
    /// <summary>
    /// Simple 2D solar system renderer for Azure VMs that don't support MonoGame
    /// </summary>
    public class SimpleSolarSystemRenderer : Control
    {
        private Timer animationTimer;
        private List<SimpleCelestialBody> celestialBodies;
        private float time = 0f;
        
        public SimpleSolarSystemRenderer()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.Opaque | 
                         ControlStyles.UserPaint | 
                         ControlStyles.ResizeRedraw |
                         ControlStyles.DoubleBuffer, true);
            
            celestialBodies = new List<SimpleCelestialBody>();
            
            // Start animation timer
            animationTimer = new Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += OnAnimationTick;
            animationTimer.Start();
        }
        
        public void AddSun()
        {
            if (celestialBodies.Count == 0) // Only one sun allowed
            {
                celestialBodies.Add(new SimpleCelestialBody
                {
                    Type = CelestialType.Sun,
                    X = this.Width / 2,
                    Y = this.Height / 2,
                    Radius = 30,
                    Color = Color.Yellow,
                    RotationSpeed = 2f
                });
            }
        }
        
        public void AddPlanet()
        {
            if (celestialBodies.Count >= 1) // Need sun first
            {
                Random rand = new Random();
                float distance = 80 + (celestialBodies.Count - 1) * 40;
                float angle = (float)(rand.NextDouble() * Math.PI * 2);
                
                celestialBodies.Add(new SimpleCelestialBody
                {
                    Type = CelestialType.Planet,
                    ParentIndex = 0, // Orbit around sun
                    OrbitDistance = distance,
                    OrbitAngle = angle,
                    Radius = 12,
                    Color = Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255)),
                    OrbitSpeed = 0.5f + (float)rand.NextDouble() * 1f,
                    RotationSpeed = 3f + (float)rand.NextDouble() * 2f
                });
            }
        }
        
        public void AddMoon()
        {
            if (celestialBodies.Count >= 2) // Need at least sun + planet
            {
                Random rand = new Random();
                // Find a planet to orbit around
                int planetIndex = 1 + rand.Next(Math.Min(celestialBodies.Count - 1, 4)); // Max 4 planets for moons
                if (planetIndex < celestialBodies.Count)
                {
                    float distance = 25;
                    float angle = (float)(rand.NextDouble() * Math.PI * 2);
                    
                    celestialBodies.Add(new SimpleCelestialBody
                    {
                        Type = CelestialType.Moon,
                        ParentIndex = planetIndex,
                        OrbitDistance = distance,
                        OrbitAngle = angle,
                        Radius = 6,
                        Color = Color.LightGray,
                        OrbitSpeed = 2f + (float)rand.NextDouble() * 2f,
                        RotationSpeed = 4f + (float)rand.NextDouble() * 2f
                    });
                }
            }
        }
        
        public void ClearSolarSystem()
        {
            celestialBodies.Clear();
        }
        
        private void OnAnimationTick(object sender, EventArgs e)
        {
            time += 0.016f; // 16ms = ~60 FPS
            this.Invalidate(); // Trigger repaint
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (celestialBodies.Count == 0)
            {
                // Draw placeholder text
                using (var font = new Font("Arial", 14))
                using (var brush = new SolidBrush(Color.White))
                {
                    string text = "Azure VM - Simple Solar System Renderer\nUse menu to add celestial bodies";
                    var rect = new RectangleF(10, 10, this.Width - 20, this.Height - 20);
                    e.Graphics.DrawString(text, font, brush, rect);
                }
                return;
            }
            
            // Update positions
            UpdatePositions();
            
            // Draw all celestial bodies
            foreach (var body in celestialBodies)
            {
                DrawCelestialBody(e.Graphics, body);
            }
            
            // Draw orbits
            DrawOrbits(e.Graphics);
        }
        
        private void UpdatePositions()
        {
            foreach (var body in celestialBodies)
            {
                if (body.ParentIndex >= 0 && body.ParentIndex < celestialBodies.Count)
                {
                    var parent = celestialBodies[body.ParentIndex];
                    body.OrbitAngle += body.OrbitSpeed * 0.016f;
                    
                    body.X = parent.X + (float)(Math.Cos(body.OrbitAngle) * body.OrbitDistance);
                    body.Y = parent.Y + (float)(Math.Sin(body.OrbitAngle) * body.OrbitDistance);
                }
                
                body.Rotation += body.RotationSpeed * 0.016f;
            }
        }
        
        private void DrawCelestialBody(Graphics g, SimpleCelestialBody body)
        {
            using (var brush = new SolidBrush(body.Color))
            {
                g.FillEllipse(brush, body.X - body.Radius, body.Y - body.Radius, 
                             body.Radius * 2, body.Radius * 2);
            }
            
            // Draw outline
            using (var pen = new Pen(Color.White, 1))
            {
                g.DrawEllipse(pen, body.X - body.Radius, body.Y - body.Radius, 
                             body.Radius * 2, body.Radius * 2);
            }
            
            // Draw type label
            using (var font = new Font("Arial", 8))
            using (var brush = new SolidBrush(Color.White))
            {
                string label = body.Type.ToString();
                var size = g.MeasureString(label, font);
                g.DrawString(label, font, brush, 
                           body.X - size.Width / 2, body.Y + body.Radius + 2);
            }
        }
        
        private void DrawOrbits(Graphics g)
        {
            using (var pen = new Pen(Color.FromArgb(50, 255, 255, 255), 1))
            {
                foreach (var body in celestialBodies)
                {
                    if (body.ParentIndex >= 0 && body.ParentIndex < celestialBodies.Count)
                    {
                        var parent = celestialBodies[body.ParentIndex];
                        g.DrawEllipse(pen, 
                                    parent.X - body.OrbitDistance, 
                                    parent.Y - body.OrbitDistance,
                                    body.OrbitDistance * 2, 
                                    body.OrbitDistance * 2);
                    }
                }
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    
    public class SimpleCelestialBody
    {
        public CelestialType Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public int ParentIndex { get; set; } = -1;
        public float OrbitDistance { get; set; }
        public float OrbitAngle { get; set; }
        public float OrbitSpeed { get; set; }
    }
    
    public enum CelestialType
    {
        Sun,
        Planet,
        Moon
    }
}
