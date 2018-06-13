using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNodeManager : Singleton<GroundNodeManager>
{

	public List<GroundNodeBase> AllGroundNodeList;


	public void Initialize()
	{
		GroundNodeBase[] temp = FindObjectsOfType<GroundNodeBase>();
		for (int i = 0; i < temp.Length; i++)
		{
			if (temp[i].CurrentNodeDeta.NodeType != NodeType.None)
			{
				AllGroundNodeList.Add(temp[i]);
			}
		}

		foreach (GroundNodeBase node in AllGroundNodeList)
		{
			node.Initialize();
		}
	}

	public void UpdateAllGroundNode()
	{
		foreach (GroundNodeBase node in AllGroundNodeList)
		{
			node.NodeEffect();
		}
		foreach (GroundNodeBase node in AllGroundNodeList)
		{
			node.UpdateNode();
		}
	}

	private void Start ()
	{
		Initialize();
	}
	
	
	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			UpdateAllGroundNode();
		}
	}
}
