using System;
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
        game.Connect(nameof(GameBase.ExitClick), this, nameof(ExitGameClicked));
        game.Connect(nameof(GameBase.LevelPassed), this, nameof(LevelPassed), new Godot.Collections.Array(button));
        this.gameContainer.Visible = true;
        this.menuLayer.Visible = false;
    }

    private void LevelPassed(LevelButton button)
    {
        var nextLevel = button.GetNextLevel();
        if (nextLevel == null)
        {
            return;
        }
        nextLevel.Visible = true;
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
