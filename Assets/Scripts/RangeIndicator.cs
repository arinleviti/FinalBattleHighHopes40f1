using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
	public bool isTargetInRange = false;
	public List<GameObject> targetsInRange;

	public Material originalMaterial;
	public Material newMaterial;

	public CombatManager combatManagerScript;
	//public PlayerController playerControllerScript;
	//public MonstersController monstersControllerScript;
	private int index = 1;
	private bool isInitialized = false;
	public float detectionRadius = 5f;
	public LayerMask detectionLayer;

	// Start is called before the first frame update
	void Start()
	{
		originalMaterial = GetComponent<Material>();
		combatManagerScript = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		targetsInRange = new List<GameObject>();
		targetsInRange = GetInRangeTargets();
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

	//public void CheckRange()
	//{
	//	targetsInRange.Clear();

	//	Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
	//	foreach (Collider collider in colliders)
	//	{
	//		GameObject target = collider.gameObject;
	//		targetsInRange.Add(target);
	//	}
	//}

	public void CheckRange()
	{
		targetsInRange.Clear();
		detectionRadius = gameObject.transform.localScale.x / 2;
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
		foreach (Collider collider in colliders)
		{
			GameObject target = collider.gameObject;
			if (!targetsInRange.Contains(target))
			{
				targetsInRange.Add(target);
				Debug.Log($"Added {target.name} to targetsInRange. InstanceID: {target.GetInstanceID()}");
			}
		}
		foreach (GameObject target in targetsInRange)
		{
			Debug.Log($"Range Indicator contains: {target.name} with InstanceID: {target.GetInstanceID()}");
		}
	}

	public List<GameObject> GetInRangeTargets()
	{
		CheckRange();
		return targetsInRange;
	}

	//private void OnTriggerEnter(Collider other)
	//{
		
	//	//isTargetInRange = true;
	//	if (!targetsInRange.Contains(other.gameObject))
	//	{
	//		targetsInRange.Add(other.gameObject);
	//		Debug.Log($"Added {other.gameObject.name} to targetsInRange.");
	//	}

	//	index++;
	//	foreach (GameObject target in targetsInRange)
	//	{

	//		Debug.Log("Range Indicator contains: " + target.name + index);
	//	}
	//}
	
	//private void OnTriggerExit(Collider other)
	//{
	//	targetsInRange.Remove(other.gameObject);
	//}

	//public List<GameObject> GetTargetsInRange()
	//{
	//	return targetsInRange;
	//}
}
