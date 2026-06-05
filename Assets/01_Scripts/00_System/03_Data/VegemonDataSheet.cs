using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class VegemonDataSheet : ScriptableObject
{
	public List<VegemonData> VegemonSheet; // Replace 'EntityType' to an actual type that is serializable.
	public List<VegemonData> EnemySheet; // Replace 'EntityType' to an actual type that is serializable.
}
