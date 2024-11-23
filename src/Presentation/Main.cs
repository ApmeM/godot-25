using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    [Export]
    public PackedScene GameButtonScene;

    private int nextToClick = 0;
    private DateTime? startedDate;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        // this.di.localAchievementRepository.ResetAchievements();

        this.achievementsButton.Connect(CommonSignals.Pressed, this, nameof(AchievementsButtonPressed));
        this.startGame.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed));

        for (var i = 0; i < 25; i++)
        {
            var b = GameButtonScene.Instance<GameButton>();
            b.Value = i + 1;
            b.Disabled = true;
            b.Connect(nameof(GameButton.Clicked), this, nameof(GameButtonPressed));
            this.gameField.AddChild(b);
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (startedDate.HasValue)
        {
            this.time.Text = (DateTime.Now - startedDate.Value).ToString();
        }
    }

    private void GameButtonPressed(GameButton button)
    {
        if (!startedDate.HasValue)
        {
            return;
        }

        if (this.nextToClick == button.Value)
        {
            button.Disabled = true;
            this.nextToClick++;
        }

        if (nextToClick == 26)
        {
            startedDate = null;
        }
    }

    private void GameStartButtonPressed()
    {
        var childs = this.gameField.GetChildren().Cast<GameButton>().ToList();
        Shuffle(childs);
        foreach (var c in childs)
        {
            this.gameField.RemoveChild(c);
        }
        foreach (var c in childs)
        {
            this.gameField.AddChild(c);
            c.Disabled = false;
        }

        nextToClick = 1;
        startedDate = DateTime.Now;
    }

    private void AchievementsButtonPressed()
    {
        // See achievements definitions in gd-achievements/achievements.json
        this.achievementList.ReloadList();
        this.customPopup.Show();
    }

    private readonly Random r = new Random();
    private void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = r.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
