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
                this.Text = "Plugin 1 found\n";
            }
            else if (Engine.HasSingleton(plugin_name2))
            {
                Plugin = Engine.GetSingleton(plugin_name2);
                Plugin.Call("initialize");
                this.Text = "Plugin 2 found\n";
            }
            else
            {
                this.Text = "No plugin found.\n";
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
        this.Text += "UserAuthenticated\n";
    }

    private void ServerSideAccessRequested(bool isAuthenticated)
    {
        this.Text += "ServerSideAccessRequested\n";
    }

    private void AchievementUnlocked(bool isUnlocked, string achievementId)
    {
        this.Text += "AchievementUnlocked\n";
    }

    private void AchievementsLoaded(string json)
    {
        this.Text += "AchievementsLoaded\n";
    }

    private void AchievementRevealed(bool isRevealed, string achievementId)
    {
        this.Text += "AchievementRevealed\n";
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
