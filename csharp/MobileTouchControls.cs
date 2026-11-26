using Godot;

namespace OperationSteelTide;

public partial class MobileTouchControls : Control
{
    private TacticalPlayer? _player;
    private MobileVirtualJoystick? _joystick;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        FocusMode = FocusModeEnum.None;
    }

    public void BindPlayer(TacticalPlayer player)
    {
        _player = player;
        BuildControls();
    }

    private void BuildControls()
    {
        if (_joystick is not null)
        {
            return;
        }

        _joystick = new MobileVirtualJoystick
        {
            Name = "MovementJoystick",
            AnchorLeft = 0.0f,
            AnchorTop = 1.0f,
            AnchorRight = 0.0f,
            AnchorBottom = 1.0f,
            OffsetLeft = 22.0f,
            OffsetTop = -194.0f,
            OffsetRight = 190.0f,
            OffsetBottom = -26.0f
        };
        _joystick.ValueChanged += value => _player?.SetMobileMoveInput(value);
        AddChild(_joystick);

        AddActionButton("FIRE", GameInputActions.Fire, -98, -190, 78, 78);
        AddActionButton("AIM", GameInputActions.Aim, -184, -178, 64, 64);
        AddActionButton("RELOAD", GameInputActions.Reload, -101, -105, 58, 58);
        AddActionButton("JUMP", GameInputActions.Jump, -177, -101, 58, 58);
        AddActionButton("CROUCH", GameInputActions.Crouch, -247, -101, 58, 58);
        AddActionButton("PRONE", GameInputActions.Prone, -247, -167, 58, 58);
        AddActionButton("USE", GameInputActions.Interact, -177, -31, 58, 54);
        AddActionButton("GRENADE", GameInputActions.ThrowGrenade, -101, -31, 68, 54);

        AddActionButton("SPRINT", GameInputActions.Sprint, 198, -111, 70, 62, leftAnchored: true);
        AddActionButton("1", GameInputActions.WeaponPrimary, -142, 64, 48, 46, topAnchored: true);
        AddActionButton("2", GameInputActions.WeaponSecondary, -88, 64, 48, 46, topAnchored: true);
        AddActionButton("3", GameInputActions.WeaponSidearm, -34, 64, 48, 46, topAnchored: true);
    }

    private void AddActionButton(
        string caption,
        StringName action,
        float offsetX,
        float offsetY,
        float width,
        float height,
        bool leftAnchored = false,
        bool topAnchored = false)
    {
        var button = new Button
        {
            Text = caption,
            FocusMode = FocusModeEnum.None,
            MouseFilter = MouseFilterEnum.Stop,
            CustomMinimumSize = new Vector2(width, height),
            Size = new Vector2(width, height),
            Modulate = new Color(1.0f, 1.0f, 1.0f, 0.88f)
        };
        button.AddThemeFontSizeOverride("font_size", caption.Length > 5 ? 10 : 13);

        if (leftAnchored)
        {
            button.AnchorLeft = 0.0f;
            button.AnchorRight = 0.0f;
        }
        else
        {
            button.AnchorLeft = 1.0f;
            button.AnchorRight = 1.0f;
        }

        if (topAnchored)
        {
            button.AnchorTop = 0.0f;
            button.AnchorBottom = 0.0f;
        }
        else
        {
            button.AnchorTop = 1.0f;
            button.AnchorBottom = 1.0f;
        }

        button.OffsetLeft = offsetX;
        button.OffsetRight = offsetX + width;
        button.OffsetTop = offsetY;
        button.OffsetBottom = offsetY + height;
        button.ButtonDown += () => _player?.SetMobileAction(action, true);
        button.ButtonUp += () => _player?.SetMobileAction(action, false);
        AddChild(button);
    }

    public override void _ExitTree()
    {
        _player?.SetMobileMoveInput(Vector2.Zero);
        _player?.SetMobileAction(GameInputActions.Fire, false);
        _player?.SetMobileAction(GameInputActions.Aim, false);
        _player?.SetMobileAction(GameInputActions.Sprint, false);
        _player?.SetMobileAction(GameInputActions.Jump, false);
        _player?.SetMobileAction(GameInputActions.Crouch, false);
        _player?.SetMobileAction(GameInputActions.Prone, false);
        _player?.SetMobileAction(GameInputActions.Reload, false);
        _player?.SetMobileAction(GameInputActions.Interact, false);
        _player?.SetMobileAction(GameInputActions.ThrowGrenade, false);
        _player?.SetMobileAction(GameInputActions.WeaponPrimary, false);
        _player?.SetMobileAction(GameInputActions.WeaponSecondary, false);
        _player?.SetMobileAction(GameInputActions.WeaponSidearm, false);
    }
}
