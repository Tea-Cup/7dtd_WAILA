using UnityEngine;

namespace WAILA {
	public class TargetBox {
		private const string CATEGORY = "WAILA";
		private const string BOX_ID = "SingleInstance";
		private static SelectionCategory Category => SelectionBoxManager.Instance.GetCategory(CATEGORY);
		public static SelectionBox Instance {
			get {
				SelectionBox box = Category.GetBox(BOX_ID);
				if (box != null) return box;
				box = Category.AddBox(BOX_ID, Vector3i.zero, Vector3i.one);
				box.SetVisible(false);
				box.SetSizeVisibility(false);
				return box;
			}
		}

		public static void Initialize() {
			SelectionBoxManager.Instance.CreateCategory(
				CATEGORY,
				new Color(1f, 1f, 1f, 0.1f),
				new Color(1f, 1f, 1f, 0.1f),
				new Color(1f, 1f, 1f, 0.1f),
				false,
				null
			);
			SelectionBoxManager.Instance.SetActive(CATEGORY, BOX_ID, true);
			SetVisible(false);
			Instance.frame.SetActive(false);
		}

		public static void SetPosition(Vector3i pos) {
			Instance.SetPositionAndSize(pos, Vector3i.one);
		}

		public static void SetVisible(bool visible) {
			Instance.SetVisible(visible);
		}
	}
}
