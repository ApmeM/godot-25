using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        // this.di.localAchievementRepository.ResetAchievements();

        this.achievementsButton.Connect(CommonSignals.Pressed, this, nameof(AchievementsButtonPressed));
        foreach (LevelButton b in this.GetTree().GetNodesInGroup(Groups.LevelButton))
        {
            b.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed), new Godot.Collections.Array { b });
        }
    }

    private void GameStartButtonPressed(LevelButton button)
    {
        this.gameContainer.ClearChildren();
        var game = button.GameToStart.Instance();
        this.gameContainer.AddChild(game);
        game.Connect("ExitClick", this, nameof(ExitGameClicked));
        this.gameContainer.Visible = true;
        this.menuLayer.Visible = false;
    }

    private void ExitGameClicked()
    {
        this.gameContainer.ClearChildren();
        this.gameContainer.Visible = false;
        this.menuLayer.Visible = true;
    }

    private void AchievementsButtonPressed()
    {
        // See achievements definitions in gd-achievements/achievements.json
        this.achievementList.ReloadList();
        this.customPopup.Show();
    }
}
