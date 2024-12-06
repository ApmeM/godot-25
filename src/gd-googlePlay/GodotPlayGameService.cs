using Godot;

[SceneReference("GodotPlayGameService.tscn")]
public partial class GodotPlayGameService
{
    // Represents the Android plugin for the GodotPlayGameService.
    public Godot.Object Plugin { get; private set; }
    const string plugin_name = "GodotGooglePlayGameServices";

    public override void _Ready()
    {
        if (Plugin == null)
        {
            if (Engine.HasSingleton(plugin_name))
            {
                // For debug connection do:
                // adb shell
                // logcat | grep "APP NOT CORRECTLY CONFIGURED TO USE GOOGLE PLAY GAME SERVICES"
                Plugin = Engine.GetSingleton(plugin_name);
                Plugin.Call("initialize");
                SignIn();
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
        this.Text += $"UserAuthenticated: {isAuthenticated}\n";
    }

    private void ServerSideAccessRequested(bool isAuthenticated)
    {
        this.Text += $"ServerSideAccessRequested: {isAuthenticated}\n";
    }

    private void AchievementUnlocked(bool isUnlocked, string achievementId)
    {
        this.Text += $"AchievementUnlocked: {isUnlocked} achievement: {achievementId}\n";
    }

    private void AchievementsLoaded(string json)
    {
        this.Text += $"AchievementsLoaded: {json}\n";
    }

    private void AchievementRevealed(bool isRevealed, string achievementId)
    {
        this.Text += $"AchievementRevealed {isRevealed} {achievementId}\n";
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
