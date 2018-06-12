using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundNodeType
{
	Neutral,
	DemonCastle,
	HeroCastle,
	Town,
	DemonWatchTower,
}


public class GroundNode : MonoBehaviour {


	public List<GroundNode> NearByGroundNodeList;


	public GroundNodeType CurrentNodeType;
	[Range(0f, 100f)]
	public float BrightnessPoint;
	[Range(0f, 100f)]
	public float DarknessPoint;

	public GameObject Neutral_Obj;
	public GameObject DemonCastle_Obj;
	public GameObject HeroCastle_Obj;
	public GameObject Town_Obj;
	public GameObject DemonWatchTower_Obj;

	public void Initialize()
	{
		if (CurrentNodeType != GroundNodeType.Neutral)
		{
			ChangeNodeType(CurrentNodeType);
		}
	}


	public void PlusNodeEffect(float _PlusBrightnessPoint, float _DarknessPoint)
	{
		BrightnessPoint += _PlusBrightnessPoint;
		DarknessPoint   += _DarknessPoint;
	}

	//------------------------------------------------------
	//  Change Node Type
	//------------------------------------------------------
	public void ChangeNodeType(GroundNodeType _GroundNodeType)
	{
		switch (_GroundNodeType)
		{
			case GroundNodeType.Neutral:
				CurrentNodeType = GroundNodeType.Neutral;
				BrightnessPoint = 0f;
				DarknessPoint   = 0f;
				Neutral_Obj        .SetActive(true);
				DemonCastle_Obj    .SetActive(false);
				HeroCastle_Obj     .SetActive(false);
				Town_Obj           .SetActive(false);
				DemonWatchTower_Obj.SetActive(false);
				break;

			case GroundNodeType.Town:
				CurrentNodeType = GroundNodeType.Town;
				BrightnessPoint = 30f;
				DarknessPoint   = 0f;
				Neutral_Obj        .SetActive(false);
				DemonCastle_Obj    .SetActive(false);
				HeroCastle_Obj     .SetActive(false);
				Town_Obj           .SetActive(true);
				DemonWatchTower_Obj.SetActive(false);
				break;

			case GroundNodeType.HeroCastle:
				CurrentNodeType = GroundNodeType.HeroCastle;
				BrightnessPoint = 70f;
				DarknessPoint   = 0f;
				Neutral_Obj        .SetActive(false);
				DemonCastle_Obj    .SetActive(false);
				HeroCastle_Obj     .SetActive(true);
				Town_Obj           .SetActive(false);
				DemonWatchTower_Obj.SetActive(false);
				break;

			case GroundNodeType.DemonWatchTower:
				CurrentNodeType = GroundNodeType.DemonWatchTower;
				BrightnessPoint = 0f;
				DarknessPoint   = 30f;
				Neutral_Obj        .SetActive(false);
				DemonCastle_Obj    .SetActive(false);
				HeroCastle_Obj     .SetActive(false);
				Town_Obj           .SetActive(false);
				DemonWatchTower_Obj.SetActive(true);
				break;

			case GroundNodeType.DemonCastle:
				CurrentNodeType = GroundNodeType.DemonCastle;
				BrightnessPoint = 0f;
				DarknessPoint   = 70f;
				Neutral_Obj        .SetActive(false);
				DemonCastle_Obj    .SetActive(true);
				HeroCastle_Obj     .SetActive(false);
				Town_Obj           .SetActive(false);
				DemonWatchTower_Obj.SetActive(false);
				break;
		}
	}

	//------------------------------------------------------
	//  Update Node Effect
	//------------------------------------------------------
	public void UpdateNodeEffect()
	{
		switch (CurrentNodeType)
		{
			case GroundNodeType.Neutral:
				break;

			case GroundNodeType.Town:
				BrightnessPoint += 3f;
				break;

			case GroundNodeType.HeroCastle:
				BrightnessPoint += 5f;
				DarknessPoint   -= 2f;
				foreach (GroundNode node in NearByGroundNodeList)
				{
					node.PlusNodeEffect(3f, -1f);
				}
				break;

			case GroundNodeType.DemonWatchTower:
				DarknessPoint   += 3f;
				break;

			case GroundNodeType.DemonCastle:
				BrightnessPoint -= 2f;
				DarknessPoint   += 2f;
				foreach (GroundNode node in NearByGroundNodeList)
				{
					node.PlusNodeEffect(-1f, 3f);
				}
				break;
		}
	}


	//------------------------------------------------------
	//  Update Node State
	//------------------------------------------------------
	public void UpdateNodeState()
	{
		switch (CurrentNodeType)
		{
			case GroundNodeType.Neutral:
				if (BrightnessPoint > DarknessPoint)
				{
					if (BrightnessPoint >= 30f)
					{
						ChangeNodeType(GroundNodeType.Town);
					}
				}
				else if (BrightnessPoint < DarknessPoint)
				{
					if (DarknessPoint >= 30f)
					{
						ChangeNodeType(GroundNodeType.DemonWatchTower);
					}
				}
				break;

			case GroundNodeType.Town:
				if (BrightnessPoint > DarknessPoint)
				{
					if (BrightnessPoint >= 70f)
					{
						ChangeNodeType(GroundNodeType.HeroCastle);
					}
				}
				else if (BrightnessPoint < DarknessPoint)
				{
					if (DarknessPoint >= 30f)
					{
						ChangeNodeType(GroundNodeType.Neutral);
					}
				}
				break;

			case GroundNodeType.HeroCastle:
				if (BrightnessPoint > DarknessPoint)
				{

				}
				else if (BrightnessPoint < DarknessPoint)
				{
					if (DarknessPoint >= 70f)
					{
						ChangeNodeType(GroundNodeType.Town);
					}
				}
				break;

			case GroundNodeType.DemonWatchTower:
				if (BrightnessPoint > DarknessPoint)
				{
					if (BrightnessPoint >= 30f)
					{
						ChangeNodeType(GroundNodeType.Neutral);
					}
				}
				else if (BrightnessPoint < DarknessPoint)
				{
					if (DarknessPoint >= 70f)
					{
						ChangeNodeType(GroundNodeType.DemonCastle);
					}
				}
				break;

			case GroundNodeType.DemonCastle:
				if (BrightnessPoint > DarknessPoint)
				{
					if (BrightnessPoint >= 70f)
					{
						ChangeNodeType(GroundNodeType.DemonCastle);
					}
				}
				else if (BrightnessPoint < DarknessPoint)
				{

				}
				break;
		}
	}





	private void Start()
	{

	}


	private void Update()
	{

	}
}
