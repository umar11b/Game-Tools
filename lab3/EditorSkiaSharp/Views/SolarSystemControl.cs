using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Threading;

namespace EditorSkiaSharp.Views;

public class SolarSystemControl : Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        AvaloniaProperty.Register<SolarSystemControl, IBrush?>(nameof(Background));

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    private Timer? _animationTimer;
    private double _sunRotation = 0;
    private double _planetRotation = 0;
    private double _moonRotation = 0;
    private double _teapotRotation = 0;
    
    public bool ShowTeapot { get; set; } = false;
    public bool SunExists { get; set; } = true;
    public bool PlanetExists { get; set; } = true;
    public bool MoonExists { get; set; } = true;

    public SolarSystemControl()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        _animationTimer = new Timer(AnimateObjects, null, 0, 16); // ~60 FPS
    }

    private void AnimateObjects(object? state)
    {
        _sunRotation += 0.02;
        _planetRotation += 0.03;
        _moonRotation += 0.05;
        _teapotRotation += 0.01;

        // Force UI update
        Avalonia.Threading.Dispatcher.UIThread.Post(() => InvalidateVisual());
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        DrawSolarSystem(context);
    }

    private void DrawSolarSystem(DrawingContext context)
    {
        var centerX = Bounds.Width / 2;
        var centerY = Bounds.Height / 2;

        // Draw title
        var titleBrush = new SolidColorBrush(Colors.White);
        var titleText = new FormattedText("Solar System Editor - Avalonia PoC", 
            System.Globalization.CultureInfo.CurrentCulture, 
            FlowDirection.LeftToRight, 
            new Typeface("Arial"), 
            24, 
            titleBrush);
        context.DrawText(titleText, new Avalonia.Point(10, 30));

        // Draw Sun
        if (SunExists)
        {
            DrawRotatingSphere(context, centerX - 200, centerY, 60, Colors.Yellow, _sunRotation, "SUN");
        }

        // Draw Planet
        if (PlanetExists)
        {
            double planetX = centerX + Math.Cos(_planetRotation * 0.01) * 100;
            double planetY = centerY + Math.Sin(_planetRotation * 0.01) * 50;
            DrawRotatingSphere(context, planetX, planetY, 40, Colors.Blue, _planetRotation, "PLANET");
        }

        // Draw Moon
        if (MoonExists)
        {
            double moonX = centerX + 150 + Math.Cos(_moonRotation * 0.02) * 80;
            double moonY = centerY + Math.Sin(_moonRotation * 0.02) * 40;
            DrawRotatingSphere(context, moonX, moonY, 20, Colors.LightGray, _moonRotation, "MOON");
        }

        // Draw Teapot (box)
        if (ShowTeapot)
        {
            DrawRotatingBox(context, centerX, centerY - 100, 50, Colors.Brown, _teapotRotation, "TEAPOT");
        }
    }

    private void DrawRotatingSphere(DrawingContext context, double x, double y, double radius, Color color, double rotation, string label)
    {
        // Create gradient for 3D effect
        var gradient = new RadialGradientBrush
        {
            Center = new RelativePoint(0.3, 0.3, RelativeUnit.Relative),
            RadiusX = new RelativeScalar(0.55, RelativeUnit.Relative),
            RadiusY = new RelativeScalar(0.55, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new GradientStop(color, 0),
                new GradientStop(Colors.DarkGray, 1)
            }
        };

        // Draw main sphere
        context.DrawGeometry(gradient, null, new EllipseGeometry(new Avalonia.Rect(x - radius, y - radius, radius * 2, radius * 2)));

        // Draw highlight for 3D effect
        var highlightBrush = new SolidColorBrush(Colors.White);
        var highlightRadius = radius * 0.2;
        context.DrawGeometry(highlightBrush, null, new EllipseGeometry(new Avalonia.Rect(x - radius * 0.3 - highlightRadius, y - radius * 0.3 - highlightRadius, highlightRadius * 2, highlightRadius * 2)));

        // Draw rotation indicator
        var lineBrush = new SolidColorBrush(Colors.White);
        var pen = new Pen(lineBrush, 2);
        double rotX = x + Math.Cos(rotation) * radius;
        double rotY = y + Math.Sin(rotation) * radius;
        context.DrawLine(pen, new Avalonia.Point(x, y), new Avalonia.Point(rotX, rotY));

        // Draw label
        var labelBrush = new SolidColorBrush(Colors.White);
        var labelText = new FormattedText(label, 
            System.Globalization.CultureInfo.CurrentCulture, 
            FlowDirection.LeftToRight, 
            new Typeface("Arial"), 
            12, 
            labelBrush);
        context.DrawText(labelText, new Avalonia.Point(x - 20, y + radius + 15));
    }

    private void DrawRotatingBox(DrawingContext context, double x, double y, double size, Color color, double rotation, string label)
    {
        var rect = new Avalonia.Rect(x - size/2, y - size/2, size, size);
        
        // Draw shadow
        var shadowBrush = new SolidColorBrush(Colors.DarkGray);
        var shadowRect = new Avalonia.Rect(rect.X + size * 0.1, rect.Y + size * 0.1, size, size);
        context.DrawGeometry(shadowBrush, null, new RectangleGeometry(shadowRect));
        
        // Draw main box
        var boxBrush = new SolidColorBrush(color);
        context.DrawGeometry(boxBrush, null, new RectangleGeometry(rect));
        
        // Draw rotation indicator
        var lineBrush = new SolidColorBrush(Colors.White);
        var pen = new Pen(lineBrush, 2);
        double rotX = x + Math.Cos(rotation) * size * 0.6;
        double rotY = y + Math.Sin(rotation) * size * 0.6;
        context.DrawLine(pen, new Avalonia.Point(x, y), new Avalonia.Point(rotX, rotY));
        
        // Draw label
        var labelBrush = new SolidColorBrush(Colors.White);
        var labelText = new FormattedText(label, 
            System.Globalization.CultureInfo.CurrentCulture, 
            FlowDirection.LeftToRight, 
            new Typeface("Arial"), 
            12, 
            labelBrush);
        context.DrawText(labelText, new Avalonia.Point(x - 20, y + size/2 + 15));
    }

    protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _animationTimer?.Dispose();
    }
}
