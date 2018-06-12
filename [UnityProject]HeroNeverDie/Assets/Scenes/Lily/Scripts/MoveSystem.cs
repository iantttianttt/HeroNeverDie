using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour {

	public GameObject Player;
	public BoxCollider MoveSpace;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickToLeft()
	{
		Player.transform.position += new Vector3(-0.5f, 0, 0);
	}

	public void OnClickToRight()
	{
		Player.transform.position += new Vector3(0.5f, 0, 0);
	}

}
