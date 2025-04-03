using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace AOP_3.Views;

public partial class MainWindow : Window
{
    private bool _isResizing = false;
    private Avalonia.Point _lastMousePosition;
    private ResizeDirection _resizeDirection;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnGraphPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border)
        {
            // Get the mouse position relative to the border
            var mousePosition = e.GetPosition(border);

            // Determine which corner or edge the cursor is near
            _resizeDirection = GetResizeDirection(border, mousePosition);

            if (_resizeDirection != ResizeDirection.None)
            {
                _isResizing = true;
                _lastMousePosition = e.GetPosition(this);
                border.Cursor = GetCursorForResizeDirection(_resizeDirection);
            }
        }
    }

    private void OnGraphPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isResizing && sender is Border border)
        {
            // Calculate the change in mouse position
            var currentMousePosition = e.GetPosition(this);
            var deltaX = currentMousePosition.X - _lastMousePosition.X;
            var deltaY = currentMousePosition.Y - _lastMousePosition.Y;

            // Resize the border based on the direction
            ResizeBorder(border, deltaX, deltaY);

            // Update the last mouse position
            _lastMousePosition = currentMousePosition;
        }
        else if (sender is Border hoverBorder)
        {
            // Update the cursor based on the hover position
            var mousePosition = e.GetPosition(hoverBorder);
            var direction = GetResizeDirection(hoverBorder, mousePosition);
            hoverBorder.Cursor = GetCursorForResizeDirection(direction);
        }
    }

    private void OnGraphPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isResizing)
        {
            _isResizing = false;
            _resizeDirection = ResizeDirection.None;

            if (sender is Border border)
            {
                border.Cursor = new Cursor(StandardCursorType.Arrow);
            }
        }
    }

    private ResizeDirection GetResizeDirection(Border border, Point mousePosition)
    {
        const double edgeThreshold = 10;

        bool nearLeft = mousePosition.X <= edgeThreshold;
        bool nearRight = mousePosition.X >= border.Bounds.Width - edgeThreshold;
        bool nearTop = mousePosition.Y <= edgeThreshold;
        bool nearBottom = mousePosition.Y >= border.Bounds.Height - edgeThreshold;

        if (nearLeft && nearTop) return ResizeDirection.TopLeft;
        if (nearRight && nearTop) return ResizeDirection.TopRight;
        if (nearLeft && nearBottom) return ResizeDirection.BottomLeft;
        if (nearRight && nearBottom) return ResizeDirection.BottomRight;
        if (nearLeft) return ResizeDirection.Left;
        if (nearRight) return ResizeDirection.Right;
        if (nearTop) return ResizeDirection.Top;
        if (nearBottom) return ResizeDirection.Bottom;

        return ResizeDirection.None;
    }

    private Cursor GetCursorForResizeDirection(ResizeDirection direction)
    {
        return direction switch
        {
            ResizeDirection.TopLeft => new Cursor(StandardCursorType.SizeAll),
            ResizeDirection.TopRight => new Cursor(StandardCursorType.SizeAll),
            ResizeDirection.BottomLeft => new Cursor(StandardCursorType.SizeAll),
            ResizeDirection.BottomRight => new Cursor(StandardCursorType.SizeAll),
            ResizeDirection.Left => new Cursor(StandardCursorType.SizeWestEast),
            ResizeDirection.Right => new Cursor(StandardCursorType.SizeWestEast),
            ResizeDirection.Top => new Cursor(StandardCursorType.SizeNorthSouth),
            ResizeDirection.Bottom => new Cursor(StandardCursorType.SizeNorthSouth),
            _ => new Cursor(StandardCursorType.Arrow),
        };
    }

    private void ResizeBorder(Border border, double deltaX, double deltaY)
    {
        const double minWidth = 100;
        const double minHeight = 100;

        switch (_resizeDirection)
        {
            case ResizeDirection.TopLeft:
                border.Width = Math.Max(minWidth, border.Width - deltaX);
                border.Height = Math.Max(minHeight, border.Height - deltaY);
                Canvas.SetLeft(border, Canvas.GetLeft(border) + deltaX);
                Canvas.SetTop(border, Canvas.GetTop(border) + deltaY);
                break;

            case ResizeDirection.TopRight:
                border.Width = Math.Max(minWidth, border.Width + deltaX);
                border.Height = Math.Max(minHeight, border.Height - deltaY);
                Canvas.SetTop(border, Canvas.GetTop(border) + deltaY);
                break;

            case ResizeDirection.BottomLeft:
                border.Width = Math.Max(minWidth, border.Width - deltaX);
                border.Height = Math.Max(minHeight, border.Height + deltaY);
                Canvas.SetLeft(border, Canvas.GetLeft(border) + deltaX);
                break;

            case ResizeDirection.BottomRight:
                border.Width = Math.Max(minWidth, border.Width + deltaX);
                border.Height = Math.Max(minHeight, border.Height + deltaY);
                break;

            case ResizeDirection.Left:
                border.Width = Math.Max(minWidth, border.Width - deltaX);
                Canvas.SetLeft(border, Canvas.GetLeft(border) + deltaX);
                break;

            case ResizeDirection.Right:
                border.Width = Math.Max(minWidth, border.Width + deltaX);
                break;

            case ResizeDirection.Top:
                border.Height = Math.Max(minHeight, border.Height - deltaY);
                Canvas.SetTop(border, Canvas.GetTop(border) + deltaY);
                break;

            case ResizeDirection.Bottom:
                border.Height = Math.Max(minHeight, border.Height + deltaY);
                break;
        }
    }

    private enum ResizeDirection
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Left,
        Right,
        Top,
        Bottom
    }
}