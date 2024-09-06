using HarmonyLib;

namespace WAILA {
	public class MainClass : IModApi {
		public void InitMod(Mod _modInstance) {
			new Harmony("FoxyTeacup.WAILA").PatchAll();
			TargetBox.Initialize();
			Keybind.Load();
		}
	}
}
