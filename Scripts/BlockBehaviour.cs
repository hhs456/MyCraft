using Godot;
using System;

public partial class BlockBehaviour : GridMap
{
	public void DestroyBlock(Vector3 worldPos){
		var mapPos = LocalToMap(worldPos);
		SetCellItem(mapPos, -1);
	}
	public void PlaceBlock(Vector3 worldPos, int blockIndex){
		var mapPos = LocalToMap(worldPos);
		SetCellItem(mapPos, blockIndex);
	}
}
