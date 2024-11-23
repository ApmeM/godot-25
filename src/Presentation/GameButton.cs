using System;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("GameButton.tscn")]
public partial class GameButton
{
    private int value;
    private bool disabled;

    [Export]
    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            if (IsInsideTree())
            {
                this.label.Text = value.ToString();
            }
        }
    }

    [Export]
    public bool Disabled
    {
        get => disabled;
        set
        {
            this.disabled = value;
            if (IsInsideTree())
            {
                this.textureButton.Disabled = value;
            }
        }
    }

    [Signal]
    public delegate void Clicked(GameButton button);

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.Value = this.value;
        this.Disabled = this.disabled;
        this.textureButton.Connect(CommonSignals.Pressed, this, nameof(ButtonPressed));
    }

    private void ButtonPressed()
    {
        this.EmitSignal(nameof(Clicked), this);
    }
}
