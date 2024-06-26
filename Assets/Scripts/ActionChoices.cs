using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoices : MonoBehaviour
{
	private UIManager UIManagerRef;
	private GameObject UIManager;
	private CombatManager combatManagerRef;
	private AnimScript animScript;
	private Animator animator;
	private Animator animatorZ1;
	private Animator animatorZ2;
	private GameObject zombie1;
	private GameObject zombie2;


	public void Start()
	{
		UIManager = GameObject.Find("UIManagerPrefab(Clone)");
		UIManagerRef = UIManager.GetComponent<UIManager>();
		Debug.Log("UIManagerRef" + UIManagerRef.name);
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		//playerControllerRef = GameObject.Find("PlayerControllerPrefab(Clone)").GetComponent <PlayerController>();
		animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		animScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		zombie1 = GameObject.Find("Zombie 1");
		zombie2 = GameObject.Find("Zombie 2");
		animatorZ1 = zombie1.GetComponentInChildren<Animator>();
		animatorZ2 = zombie2.GetComponentInChildren<Animator>();
		
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
					//animatorRef.SetTrigger("IsPunching");

					UIManagerRef.targetCharacterIO.HP = punchScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
					Debug.Log("Punch Hit logic executed");
					//StartCoroutine(HandlePunchAnimation());

					animScript.HitAnimation(punchScriptRef, animator);
					//UIManagerRef.targetCharacterIO.HP = punchScriptRef.Hit(UIManagerRef.targetCharacterIO, UIManagerRef.attackerIO);
					
					if (UIManagerRef.targetCharacterGO != null && UIManagerRef.targetCharacterGO.name == "Zombie 1" )
					{
						animScript.GetHitAnimation(animator, animatorZ1);
					}
					else if (UIManagerRef.targetCharacterGO != null && UIManagerRef.targetCharacterGO.name == "Zombie 2")
					{
						animScript.GetHitAnimation(animator, animatorZ2 );
					}
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
					if (combatManagerRef.currentTurn.name == "Zombie 1")
					{
						animScript.HitAnimation(boneCrunchScriptRef, animatorZ1);
						animScript.GetHitAnimation(animatorZ1, animator);
					}
					else if (combatManagerRef.currentTurn.name == "Zombie 2")
					{
						animScript.HitAnimation(boneCrunchScriptRef, animatorZ2);
						animScript.GetHitAnimation(animatorZ2, animator);
					}
					
						
					
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
			//playerControllerRef.movesLeft--;
			//SetupIdle();
			UIManagerRef.isCharacterTurnOver = true;
			//Destroy(gameObject);
		}
		if (combatManagerRef.currentTurn.CompareTag("Monster"))
		{
			UIManagerRef.SetAttackChoiceHandled(true);
			//Destroy(gameObject);
		}


		//Destroy(gameObject);

	}

	
}
