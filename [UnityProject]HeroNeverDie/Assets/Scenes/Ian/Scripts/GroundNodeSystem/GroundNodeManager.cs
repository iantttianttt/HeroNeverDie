using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNodeManager : Singleton<GroundNodeManager>
{

	public List<GroundNode> AllGroundNodeList;


	public void Initialize()
	{
		GroundNode[] temp = FindObjectsOfType<GroundNode>();
		for (int i = 0; i < temp.Length; i++)
		{
			AllGroundNodeList.Add(temp[i]);
		}

		foreach (GroundNode node in AllGroundNodeList)
		{
			node.Initialize();
		}
	}

	public void UpdateAllGroundNode()
	{
		foreach (GroundNode node in AllGroundNodeList)
		{
			node.UpdateNodeEffect();
		}
		foreach (GroundNode node in AllGroundNodeList)
		{
			node.UpdateNodeState();
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
