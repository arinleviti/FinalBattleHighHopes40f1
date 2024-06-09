using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoices : MonoBehaviour
{
	private UIManager UIManagerRef;
	private GameObject UIManager;
	private CombatManager combatManagerRef;
	private AnimScript animScript;
	
	

	public void Start()
	{
		UIManager = GameObject.Find("UIManagerPrefab(Clone)");
		UIManagerRef=	UIManager.GetComponent<UIManager>();
		Debug.Log("UIManagerRef" + UIManagerRef.name);
		combatManagerRef = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		//playerControllerRef = GameObject.Find("PlayerControllerPrefab(Clone)").GetComponent <PlayerController>();
		//animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();
		animScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
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
					animScript.PunchAnimation();
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
			//SetupIdle();
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

	//private void HandlePunchAnimation()
	//{
	//	isWalking = false;
	//	animator.SetBool("IsWalking", isWalking);
	//	animator.SetTrigger("IsPunching");
	//	animator.Play("idle1");
	//	//WaitForPunchAnimation();
	//}

	//private void WaitForPunchAnimation()
	//{
	//	//AnimatorStateInfo: A structure that holds information about the current state of an Animator.Gets the current state of the Animator for the first layer (index 0).
		
	//	AnimatorStateInfo punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
	//	while (!punchStateInfo.IsName("Punch"))
	//	{
			
	//		//normalizedTime is a value between 0 and 1 that represents the progress of the animation (0 is the start, and 1 is the end).
	//		punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
	//	}
	//	while (punchStateInfo.normalizedTime < 1.0f)
	//	{
			
	//		punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
	//	}
	//	animator.Play("idle1");
	//}
	//void SetupIdle()
	//{
	//	isWalking = false;
	//	animator.SetBool("IsWalking", isWalking);

	//	isIdle = true;
	//	animator.SetBool("IsIdle", isIdle);
	//}
}
