using BepInEx;
using BepInEx.Logging;
using BSCP.Patches;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;

namespace BSCP
{
    [ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, false)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class BSCP : BaseUnityPlugin
    {
        public static BSCP Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static BSCPConfig BoundConfig { get; private set; } = null!;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            BoundConfig = new BSCPConfig(base.Config);

            HookAll();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void HookAll()
        {
            Logger.LogDebug("Hooking...");

            BigSlapPatch.Init();

            Logger.LogDebug("Finished Hooking!");
        }

        internal static void UnhookAll()
        {
            Logger.LogDebug("Unhooking...");

            HookEndpointManager.RemoveAllOwnedBy(Assembly.GetExecutingAssembly());

            Logger.LogDebug("Finished Unhooking!");
        }
    }
}
