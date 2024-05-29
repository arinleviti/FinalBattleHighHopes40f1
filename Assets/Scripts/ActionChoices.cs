using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoices : MonoBehaviour
{
	private UIManager UIManagerRef;
	private GameObject UIManager;
	private CombatManager combatManagerRef;
	//private PlayerController playerControllerRef;
	private Animator animatorRef;
	private bool isIdle = false;
	private bool isWalking = false;

	public void Start()
	{
		UIManager = GameObject.Find("UIManagerPrefab(Clone)");
		UIManagerRef=	UIManager.GetComponent<UIManager>();
		Debug.Log("UIManagerRef" + UIManagerRef.name);
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		//playerControllerRef = GameObject.Find("PlayerControllerPrefab(Clone)").GetComponent <PlayerController>();
		animatorRef = GameObject.Find("OrkAssasin").GetComponent<Animator>();
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
					HandlePunchAnimation();
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
			//playerControllerRef.movesLeft--;
			SetupIdle();
			UIManagerRef.isCharacterTurnOver = true;
			Destroy(gameObject);
		}
		if (combatManagerRef.currentTurn.CompareTag("Monster"))
		{
			UIManagerRef.SetAttackChoiceHandled(true);
			Destroy(gameObject);
		}
		
		
		//Destroy(gameObject);
		
	}

	private void HandlePunchAnimation()
	{
		animatorRef.SetTrigger("IsPunching");
		//animatorRef.Play("idle1");
	}

	//private IEnumerator HandlePunchAnimation()
	//{
	//	animatorRef.SetTrigger("IsPunching");
	//	// Wait for the punch animation to finish
	//	yield return new WaitForSeconds(1.0f); // The punch animation lasts 1 second

	//	// Transition to the idle animation
	//	animatorRef.Play("idle1");
	//	Debug.Log("Punch animation ended and transitioned to idle1.");
	//}
	void SetupIdle()
	{
		isWalking = false;
		animatorRef.SetBool("IsWalking", isWalking);

		isIdle = true;
		animatorRef.SetBool("IsIdle", isIdle);
	}
}
