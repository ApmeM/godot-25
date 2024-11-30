using System;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("GameButton.tscn")]
[Tool]
public partial class GameButton
{
    private int value;
    private bool disabled;

    [Export]
    public int Value
    {
        get => this.value;
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
        this.AddToGroup(Groups.GameButton);
    }

    public async void Shake()
    {
        const int distance = 3;
        const float time = 0.05f;
        const int repeats = 2;

        this.tween.InterpolateProperty(this.textureButton, "rect_position", new Vector2(0, 0), new Vector2(distance, 0), time / 2);
        this.tween.Start();
        await this.ToSignal(this.tween, "tween_completed");

        for (var i = 0; i < repeats; i++)
        {
            this.tween.InterpolateProperty(this.textureButton, "rect_position", new Vector2(distance, 0), new Vector2(-distance, 0), time);
            this.tween.Start();
            await this.ToSignal(this.tween, "tween_completed");
            this.tween.InterpolateProperty(this.textureButton, "rect_position", new Vector2(-distance, 0), new Vector2(distance, 0), time);
            this.tween.Start();
            await this.ToSignal(this.tween, "tween_completed");
        }

        this.tween.InterpolateProperty(this.textureButton, "rect_position", new Vector2(distance, 0), new Vector2(0, 0), time / 2);
        this.tween.Start();
        await this.ToSignal(this.tween, "tween_completed");

    }

    private void ButtonPressed()
    {
        this.EmitSignal(nameof(Clicked), this);
    }
}

public partial class Groups
{
    public static string GameButton = nameof(GameButton);
}
