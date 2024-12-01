using Godot;
using System;

[SceneReference("GodotPlayGameService.tscn")]
public partial class GodotPlayGameService
{

    // Represents the Android plugin for the GodotPlayGameService.
    public Godot.Object Plugin { get; private set; }
    const string plugin_name1 = "GodotPlayGameServices";
    const string plugin_name2 = "GodotGooglePlayGameServices";

    public delegate void AuthenticationDelegate(bool isAuthenticated);
    // Event for when the user is authenticated
    public event AuthenticationDelegate UserAuthenticated;

    // Event for when server side access is requested
    public event AuthenticationDelegate ServerSideAccessRequested;
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
        Plugin?.Connect("userAuthenticated", this, nameof(OnUserAuthenticatedSignalConnected));
        Plugin?.Connect("serverSideAccessRequested", this, nameof(ServerSideAccessRequestedSignalConnected));
    }
    /// <summary>
    /// Invokes the UserAuthenticated event with the specified authentication status.
    /// </summary>
    /// <param name="isAuthenticated">The authentication status.</param>
    private void OnUserAuthenticatedSignalConnected(bool isAuthenticated)
    {
        UserAuthenticated?.Invoke(isAuthenticated);
    }

    /// <summary>
    /// Invokes the ServerSideAccessRequested event with the specified authentication status.
    /// </summary>
    /// <param name="isAuthenticated">The authentication status.</param>
    private void ServerSideAccessRequestedSignalConnected(bool isAuthenticated)
    {
        ServerSideAccessRequested?.Invoke(isAuthenticated);
    }
    /// <summary>
    /// Check if the user is authenticated.
    /// </summary>
    public void IsAuthenticated()
    {
        Plugin?.Call("isAuthenticated");
    }

    /// <summary>
    /// Sign in the user.
    /// </summary>
    public void SignIn()
    {
        Plugin?.Call("signIn");
    }

    /// <summary>
    /// Request server side access with the specified server client ID and force refresh token flag.
    /// </summary>
    /// <param name="serverClientId">The server client ID to request access for.</param>
    /// <param name="forceRefreshToken">Whether to force refresh the token.</param>
    public void RequestServerSideAccess(string serverClientId, bool forceRefreshToken)
    {
        Plugin?.Call("requestServerSideAccess", serverClientId, forceRefreshToken);
    }
}
