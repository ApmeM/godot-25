using System;
using System.Collections.Generic;
using System.Linq;
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

    public Dictionary<string, string> buttonToLeaderboardMap = new Dictionary<string, string>{
        {"StartGame25", "CgkIsoWNyY0BEAIQBg"},
        {"StartGame99", "CgkIsoWNyY0BEAIQBw"},
        {"StartGameRnd", "CgkIsoWNyY0BEAIQCA"},
        {"StartGamePlus", "CgkIsoWNyY0BEAIQCQ"},
        {"StartGameMult", "CgkIsoWNyY0BEAIQCg"},
    };

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        // this.di.localAchievementRepository.ResetAchievements();
        var progress = this.di.repository.LoadProgress();
        this.achievementsButton.Connect(CommonSignals.Pressed, this, nameof(AchievementsButtonPressed));
        this.ratingButton.Connect(CommonSignals.Pressed, this, nameof(RatingButtonPressed));
        foreach (LevelButton b in this.GetTree().GetNodesInGroup(Groups.LevelButton))
        {
            b.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed), new Godot.Collections.Array { b });
            b.Visible = b.Visible || progress.Contains(b.Name);
            var fire = b.GetChild<AnimatedSprite>(0);
            fire.Visible = progress.Contains(b.Name + "Fire");
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

    private void LevelPassed(float bestScore, LevelButton button)
    {
        if (buttonToLeaderboardMap.ContainsKey(button.Name))
        {
            var leaderboard = buttonToLeaderboardMap[button.Name];
            if (this.googlePlay.IsEnabled())
            {
                this.googlePlay.leaderboardsSubmitScore(leaderboard, bestScore);
            }
            else
            {
            }
        }

        button.GetChild<AnimatedSprite>(0).Visible = true;
        di.repository.SaveProgress(button.Name + "Fire");

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


    private void RatingButtonPressed()
    {
        if (this.googlePlay.IsEnabled())
        {
            this.googlePlay.leaderboardsShowAll();
        }
        else
        {
            GD.Print("Not implemented");
        }
    }
}
