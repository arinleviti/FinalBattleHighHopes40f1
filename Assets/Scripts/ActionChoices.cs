using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoices : MonoBehaviour
{
	private UIManager UIManagerRef;
	private CombatManager combatManagerRef;

	public void Start()
	{
		UIManagerRef = GameObject.Find("UIManagerPrefab(Clone)").GetComponent<UIManager>();
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
	}
	public void HandleAttackChoice()
	{

		switch (UIManagerRef.attackTypeChosen)
		{
			case AttackType.Punch:
				UIManagerRef.actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/PunchPrefab"));
				Punch punchScriptRef = UIManagerRef.actionPrefab.GetComponent<Punch>();
				if (punchScriptRef != null)
				{
					UIManagerRef.targetCharacterIO.HP = punchScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
					Debug.Log("Punch Hit method executed");
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			case AttackType.BoneCrunch:
				UIManagerRef.actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/BoneCrunchPrefab"));
				BoneCrunch boneCrunchScriptRef = UIManagerRef.actionPrefab.GetComponent<BoneCrunch>();
				if (boneCrunchScriptRef != null)
				{
					UIManagerRef.targetCharacterIO.HP = boneCrunchScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			case AttackType.MistyFist:
				UIManagerRef.actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/MistyFistPrefab"));
				MistyFist mistyFistScriptRef = UIManagerRef.actionPrefab.GetComponent<MistyFist>();
				if (mistyFistScriptRef != null)
				{
					UIManagerRef.targetCharacterIO.HP = mistyFistScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			case AttackType.Claw:
				UIManagerRef.actionPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/ClawPrefab"));
				Claw clawScriptRef = UIManagerRef.actionPrefab.GetComponent<Claw>();
				if (clawScriptRef != null)
				{
					UIManagerRef.targetCharacterIO.HP = clawScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
				}
				else
				{
					Debug.LogError("Punch component not found on instantiated prefab!");
				}
				break;
			default:
				Debug.Log("Invalid attack type");
				break;
		}
		if (combatManagerRef.currentTurn.CompareTag("Player"))
		{
			UIManagerRef.canvas.enabled = false;
			
		}
		UIManagerRef.isCharacterTurnOver = true;
		Destroy(gameObject);
	}

}
