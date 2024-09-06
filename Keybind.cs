using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using InControl;
using Platform;

namespace WAILA {
	public static class Keybind {
		private static PlayerAction action;
		private const string PREF_KEY = "WAILA";

		public static bool WasPressed => action.WasPressed;

		public static void Load() {
			action = new PlayerAction("WAILA", PlatformManager.NativePlatform.Input.PrimaryPlayer);
			action.AddDefaultBinding(Key.PadMinus);
			action.UserData = new PlayerActionData.ActionUserData(
				"inpActWAILA", null,
				PlayerActionData.GroupUI,
				PlayerActionData.EAppliesToInputType.KbdMouseOnly,
				true, false, false
			);

			LoadKeybind(action);
		}

		public static void Save() {
			SaveKeybind(action);
		}

		private static bool LoadKeybind(PlayerAction action) {
			if (!SdPlayerPrefs.HasKey(PREF_KEY)) return false;
			string base64 = SdPlayerPrefs.GetString(PREF_KEY, "");
			byte[] data = Convert.FromBase64String(base64);
			using (MemoryStream stream = new MemoryStream(data))
			using (BinaryReader br = new BinaryReader(stream)) {
				br.ReadString();
				PlayerAction_Load(action, br, 2);
			}
			return true;
		}

		private static void SaveKeybind(PlayerAction action) {
			using (MemoryStream stream = new MemoryStream())
			using (BinaryWriter bw = new BinaryWriter(stream)) {
				PlayerAction_Save(action, bw);
				string base64 = Convert.ToBase64String(stream.ToArray());
				SdPlayerPrefs.SetString(PREF_KEY, base64);
			}
		}

		private static readonly MethodInfo method_PA_Load = AccessTools.Method(typeof(PlayerAction), "Load");
		private static readonly MethodInfo method_PA_Save = AccessTools.Method(typeof(PlayerAction), "Save");

		private static void PlayerAction_Load(PlayerAction action, BinaryReader reader, ushort dataFormatVersion) {
			method_PA_Load.Invoke(action, new object[] { reader, dataFormatVersion });
		}

		private static void PlayerAction_Save(PlayerAction action, BinaryWriter writer) {
			method_PA_Save.Invoke(action, new object[] { writer });
		}
	}
}
