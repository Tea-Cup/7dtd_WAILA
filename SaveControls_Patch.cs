using HarmonyLib;

namespace WAILA {
	[HarmonyPatch(typeof(GameOptionsManager), nameof(GameOptionsManager.SaveControls))]
	internal class SaveControls_Patch {
		public static void Postfix() {
			Keybind.Save();
		}
	}
}
