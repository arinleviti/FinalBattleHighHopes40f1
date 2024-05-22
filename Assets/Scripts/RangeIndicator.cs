using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
	public bool isTargetInRange = false;
	public List<GameObject> targetsInRange = new List<GameObject>();

	public Material originalMaterial;
	public Material newMaterial;

	public CombatManager combatManagerScript;
	//public PlayerController playerControllerScript;
	//public MonstersController monstersControllerScript;

	// Start is called before the first frame update
	void Start()
	{
		originalMaterial = GetComponent<Material>();
		combatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();

	}

	// Update is called once per frame
	void Update()
	{
		//if (GameObject.Find("PlayerControllerPrefab(Clone)"))
		//{
			//playerControllerScript = GameObject.Find("PlayerControllerPrefab(Clone)").GetComponent<PlayerController>();
			if (combatManagerScript.currentTurn.CompareTag("Player") && combatManagerScript.movesLeft < 2)
			{
				
				GetComponent<Renderer>().material = newMaterial;
			}
		//}
		//if (GameObject.Find("MonstersControllerPrefab(Clone)"))
		//{
			//monstersControllerScript = GameObject.Find("MonstersControllerPrefab(Clone)").GetComponent<MonstersController>();
			if (combatManagerScript.currentTurn.CompareTag("Monster") && combatManagerScript.movesLeft < 2)
			{
				
				GetComponent<Renderer>().material = newMaterial;
			}
		//}

	}

	private void OnTriggerEnter(Collider other)
	{
		isTargetInRange = true;
		if (!targetsInRange.Contains(other.gameObject))
		{
			targetsInRange.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		targetsInRange.Remove(other.gameObject);
	}

	public List<GameObject> GetTargetsInRange()
	{
		return targetsInRange;
	}
}
