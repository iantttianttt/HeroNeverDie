using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
	None,
	Neutral,
	DemonCastle,
	HeroCastle,
	Town,
	DemonWatchTower,
}

[System.Serializable]
public class NodeDeta
{
	[BoxGroup()]
	public NodeType NodeType;
	[BoxGroup()]
	public GameObject TypeObject;

	[Title("Point Data")]
	[BoxGroup()]
	public float BasicLightPoint;
	[BoxGroup()]
	public float BasicDarkPoint;
	[BoxGroup()]
	public float LightPointEffect;
	[BoxGroup()]
	public float DarkPointEffect;
	[BoxGroup()]
	public float LightPointGenerate;
	[BoxGroup()]
	public float DarkPointGenerate;
}



public class NodeTypeManager : Singleton<GroundNodeManager>
{

	public List<NodeDeta> NodeTypeDetaList;







	//------------------------------------------------------
	//  Change Node Type
	//------------------------------------------------------
	public void SetNodeType(NodeType _TargetNodeType, GroundNodeBase _TargetNode)
	{
		if (_TargetNode == null || _TargetNodeType == NodeType.None) { return; }
		foreach (NodeDeta TypeData in NodeTypeDetaList)
		{
			if (TypeData.NodeType == _TargetNodeType)
			{
				_TargetNode.CurrentNodeDeta = TypeData;
			}
		}
	}


	void Start () {
		
	}
	

	void Update () {
		
	}
}
