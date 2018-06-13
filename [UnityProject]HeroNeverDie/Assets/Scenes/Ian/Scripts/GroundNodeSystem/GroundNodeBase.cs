using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GroundNodeBase : MonoBehaviour {

	public List<GroundNodeBase> NearByGroundNodeList;
	public NodeDeta CurrentNodeDeta;




	//------------------------------------------------------
	//  Initialize
	//------------------------------------------------------
	public virtual void Initialize()
	{
		if (CurrentNodeDeta.NodeType == NodeType.None) { return; }

	}



	//------------------------------------------------------
	//  Add Point
	//------------------------------------------------------
	public virtual void AddPoint(float _BrightnessPoint, float _DarknessPoint)
	{
		CurrentNodeDeta.BasicLightPoint += _BrightnessPoint;
		CurrentNodeDeta.BasicDarkPoint += _DarknessPoint;
	}



	//------------------------------------------------------
	//  Node Effect
	//------------------------------------------------------
	public virtual void NodeEffect()
	{
		if (CurrentNodeDeta.NodeType == NodeType.None) { return; }

		CurrentNodeDeta.BasicLightPoint += CurrentNodeDeta.LightPointGenerate;
		CurrentNodeDeta.BasicDarkPoint  += CurrentNodeDeta.DarkPointGenerate;
		foreach (GroundNodeBase node in NearByGroundNodeList)
		{
			node.AddPoint(CurrentNodeDeta.LightPointEffect, CurrentNodeDeta.DarkPointEffect);
		}
	}




	//------------------------------------------------------
	//  Update Node
	//------------------------------------------------------
	public virtual void UpdateNode()
	{
		if (CurrentNodeDeta.NodeType == NodeType.None) { return; }
	}





	private void Update()
	{

	}
}
