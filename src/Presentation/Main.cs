using System.Collections.Generic;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    public Dictionary<string, string> buttonToAchievementMap = new Dictionary<string, string>{
        {"StartGame25", "CgkIsoWNyY0BEAIQAQ"},
        {"StartGame99", "CgkIsoWNyY0BEAIQAg"},
        {"StartGameRnd", "CgkIsoWNyY0BEAIQAw"},
        {"StartGamePlus", "CgkIsoWNyY0BEAIQBA"},
        {"StartGameMult", "CgkIsoWNyY0BEAIQBQ"},
    };

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        // this.di.localAchievementRepository.ResetAchievements();
        var progress = this.di.repository.LoadProgress();
        this.achievementsButton.Connect(CommonSignals.Pressed, this, nameof(AchievementsButtonPressed));
        foreach (LevelButton b in this.GetTree().GetNodesInGroup(Groups.LevelButton))
        {
            b.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed), new Godot.Collections.Array { b });
            b.Visible = b.Visible || progress.Contains(b.Name);
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
        if (buttonToAchievementMap.ContainsKey(button.Name))
        {
            var achievement = buttonToAchievementMap[button.Name];
            if (this.googlePlay.IsEnabled())
            {
                this.googlePlay.achievementsUnlock(achievement, true);
            }
            else
            {
                this.achievementNotifications.UnlockAchievement(achievement);
            }
        }

        if (nextLevel == null)
        {
            return;
        }
        nextLevel.Visible = true;

        di.repository.SaveProgress(nextLevel.Name);
    }

    private void ExitGameClicked()
    {
        this.gameContainer.ClearChildren();
        this.gameContainer.Visible = false;
        this.menuLayer.Visible = true;
    }

    private void AchievementsButtonPressed()
    {
        if (this.googlePlay.IsEnabled())
        {
            this.googlePlay.achievementsShow();
        }
        else
        {
            // See achievements definitions in gd-achievements/achievements.json
            this.achievementList.ReloadList();
            this.customPopup.Show();
        }
    }
}
