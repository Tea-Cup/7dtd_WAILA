namespace WAILA {
	public class XUiC_WAILA : XUiController {
		private enum UpdateResult {
			Unchanged,
			Set,
			Reset
		}

		private Vector3i lastPos = Vector3i.zero;
		private string blockName = "";
		private string blockIcon = "";
		private bool hasBox = false;
		private bool hiddenBox = true;

		public override void Update(float _dt) {
			base.Update(_dt);
			UpdateResult result = UpdateTarget();
			if (result == UpdateResult.Reset) UnsetTarget();
			if (result != UpdateResult.Unchanged) RefreshBindings(true);

			if (Keybind.WasPressed) {
				hiddenBox = !hiddenBox;
				if (hiddenBox) TargetBox.SetVisible(false);
				else TargetBox.SetVisible(hasBox);
			}
		}

		private void SetTarget(string name, string icon) {
			blockName = name;
			blockIcon = icon;
			hasBox = true;
			if(!hiddenBox) TargetBox.SetVisible(true);
		}

		private void UnsetTarget() {
			blockName = "";
			blockIcon = "";
			hasBox = false;
			TargetBox.SetVisible(false);
		}

		private UpdateResult UpdateTarget() {
			EntityPlayerLocal player = GameManager.Instance.myEntityPlayerLocal;
			WorldRayHitInfo hitInfo = player.inventory.holdingItemData.hitInfo;
			Vector3i hitPos = hitInfo.hit.blockPos;

			if (!hitInfo.bHitValid || hitPos.y < 0 || hitPos.y >= 256) return UpdateResult.Reset;

			if (lastPos == hitPos) return UpdateResult.Unchanged;

			lastPos = hitPos;
			TargetBox.SetPosition(hitPos);

			ChunkCluster cluster = GameManager.Instance.World.ChunkClusters[hitInfo.hit.clrIdx];
			if (cluster == null) return UpdateResult.Reset;

			Chunk chunk = (Chunk)cluster.GetChunkFromWorldPos(hitPos);
			if (chunk == null) return UpdateResult.Reset;

			Vector3i blockPos = World.toBlock(hitPos);
			BlockValue value = chunk.GetBlock(blockPos);
			Block block = value.Block;

			string name = block.GetLocalizedBlockName();
			if (blockName == name) return UpdateResult.Unchanged;

			string icon = "";
			if (ItemClass.GetForId(block.blockID) is ItemClass item) {
				icon = value.ToItemValue().GetPropertyOverride("CustomIcon", item.GetIconName());
			}

			SetTarget(name, icon);
			return UpdateResult.Set;
		}

		public override bool GetBindingValue(ref string value, string bindingName) {
			switch (bindingName) {
				case "target_name":
					value = blockName;
					return true;
				case "target_icon":
					value = blockIcon;
					return true;
				default:
					return base.GetBindingValue(ref value, bindingName);
			}
		}
	}
}
