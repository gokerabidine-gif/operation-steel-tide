using System;
using Godot;

namespace OperationSteelTide;

public partial class MobileVirtualJoystick : Control
{
    public event Action<Vector2>? ValueChanged;

    private const float Radius = 72.0f;
    private const float KnobTravel = 43.0f;
    private int _touchIndex = -1;
    private Vector2 _value;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Stop;
        FocusMode = FocusModeEnum.None;
        QueueRedraw();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed && _touchIndex < 0)
            {
                _touchIndex = touch.Index;
                UpdateFromPosition(touch.Position);
                AcceptEvent();
            }
            else if (!touch.Pressed && touch.Index == _touchIndex)
            {
                _touchIndex = -1;
                SetValue(Vector2.Zero);
                AcceptEvent();
            }
            return;
        }

        if (@event is InputEventScreenDrag drag && drag.Index == _touchIndex)
        {
            UpdateFromPosition(drag.Position);
            AcceptEvent();
        }
    }

    private void UpdateFromPosition(Vector2 position)
    {
        var delta = position - Size * 0.5f;
        var length = delta.Length();
        var next = length > Radius * 0.78f
            ? delta.Normalized()
            : delta / (Radius * 0.78f);
        SetValue(next.LimitLength(1.0f));
    }

    private void SetValue(Vector2 value)
    {
        _value = value.Length() < 0.12f ? Vector2.Zero : value;
        QueueRedraw();
        ValueChanged?.Invoke(_value);
    }

    public override void _Draw()
    {
        var center = Size * 0.5f;
        DrawCircle(center, Radius, new Color(0.025f, 0.055f, 0.06f, 0.58f));
        DrawCircle(center, Radius - 5.0f, new Color(0.22f, 0.34f, 0.34f, 0.25f));
        DrawCircle(center + _value * KnobTravel, 27.0f, new Color(0.25f, 0.83f, 0.72f, 0.82f));
        DrawCircle(center + _value * KnobTravel, 19.0f, new Color(0.07f, 0.15f, 0.16f, 0.88f));
    }
}
