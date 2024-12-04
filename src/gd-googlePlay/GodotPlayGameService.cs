using System;
using Godot;

[SceneReference("GodotPlayGameService.tscn")]
public partial class GodotPlayGameService
{
    // Represents the Android plugin for the GodotPlayGameService.
    public Godot.Object Plugin { get; private set; }
    const string plugin_name1 = "GodotPlayGameServices";
    const string plugin_name2 = "GodotGooglePlayGameServices";

    public override void _Ready()
    {
        if (Plugin == null)
        {
            if (Engine.HasSingleton(plugin_name1))
            {
                Plugin = Engine.GetSingleton(plugin_name1);
                Plugin.Call("initialize");
            }
            else if (Engine.HasSingleton(plugin_name2))
            {
                Plugin = Engine.GetSingleton(plugin_name2);
                Plugin.Call("initialize");
            }
            else
            {
                GD.PrintErr("No plugin found.");
            }
        }

        // Connects signals from the AndroidPlugin instance to corresponding methods
        Plugin?.Connect("userAuthenticated", this, nameof(UserAuthenticated));
        Plugin?.Connect("serverSideAccessRequested", this, nameof(ServerSideAccessRequested));
        Plugin?.Connect("achievementUnlocked", this, nameof(AchievementUnlocked));
        Plugin?.Connect("achievementsLoaded", this, nameof(AchievementsLoaded));
        Plugin?.Connect("achievementRevealed", this, nameof(AchievementRevealed));
    }

    private void UserAuthenticated(bool isAuthenticated)
    {
    }

    private void ServerSideAccessRequested(bool isAuthenticated)
    {
    }

    private void AchievementUnlocked(bool isUnlocked, string achievementId)
    {
    }

    private void AchievementsLoaded(string json)
    {
    }

    private void AchievementRevealed(bool isRevealed, string achievementId)
    {
    }



    public void IsAuthenticated()
    {
        Plugin?.Call("isAuthenticated");
    }

    public void SignIn()
    {
        Plugin?.Call("signIn");
    }

    public void RequestServerSideAccess(string serverClientId, bool forceRefreshToken)
    {
        Plugin?.Call("requestServerSideAccess", serverClientId, forceRefreshToken);
    }

    public void IncrementAchievement(string achievementId, int amount)
    {
        Plugin?.Call("incrementAchievement", achievementId, amount);
    }

    public void LoadAchievements(bool forceReload)
    {
        Plugin?.Call("loadAchievements", forceReload);
    }

    public void RevealAchievement(string achievementId)
    {
        Plugin?.Call("revealAchievement", achievementId);
    }

    public void ShowAchievements()
    {
        Plugin?.Call("showAchievements");
    }

    public void UnlockAchievement(string achievementId)
    {
        Plugin?.Call("unlockAchievement", achievementId);
    }
}
