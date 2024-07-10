using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    public Animator animator;
	public Animator monsterAnim;
	public CombatManager combatManager;
	

	private bool isIdle = false;
	private bool isWalking = false;
	//private GameObject currentTurn;
	

	// Start is called before the first frame update
	void Start()
    {
		//combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
		
	}

    // Update is called once per frame
    void Update()
    {
		//currentTurn = combatManager.currentTurn;
    }

	public void HitAnimation(IAction action, Animator animator)
	{
		StartCoroutine(WaitForPunchAnimation(action, animator));	
	}

	private IEnumerator WaitForPunchAnimation(IAction action, Animator animator )
	{
		isWalking = false;
		animator.SetBool("IsWalking", isWalking);

		switch (action)
		{
			case Punch:
				animator.SetTrigger("IsPunching");
				break;
			case BoneCrunch:
				animator.SetTrigger("IsBoneCrunching");
				break;
			default:
				Debug.LogWarning("Unknown action type.");
				break;
		}

				//AnimatorStateInfo: A structure that holds information about the current state of an Animator.Gets the current state of the Animator for the first layer (index 0).

				AnimatorStateInfo hitStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		while (!hitStateInfo.IsName("Attack"))
		{
			yield return null;
			//normalizedTime is a value between 0 and 1 that represents the progress of the animation (0 is the start, and 1 is the end).
			hitStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		}
		while (hitStateInfo.normalizedTime < 1.0f)
		{
			yield return null;
			hitStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		}
		animator.Play("idle1");
	}

	public void SetupIdle()
	{
		isWalking = false;
		animator.SetBool("IsWalking", isWalking);

		isIdle = true;
		animator.SetBool("IsIdle", isIdle);
	}

	public void PlayDeathAnim(GameObject characterToRemove, Animator animator)
	{
		StartCoroutine(TimeToDie( characterToRemove, animator));
	}

	private IEnumerator TimeToDie(GameObject characterToRemove, Animator animator)
	{
		yield return new WaitForSeconds(0.25f);
		animator.SetInteger("HP", -2);
		Debug.Log("Initiated Dying");
		yield return new WaitForSeconds(5);
		Debug.Log("Finished dying");
		Destroy(characterToRemove);
	}
	public void SetupWalkingMonster(GameObject attacker, Animator animator, Vector3 clickPosition)
	{
		isIdle = false;
		animator.SetBool("IsIdle", isIdle);
		isWalking = true;
		animator.SetBool("IsWalking", isWalking);

	}
	public Vector3 SetupWalking(GameObject attacker, Animator animator, Vector3 clickPosition)
	{

		isIdle = false;
		animator.SetBool("IsIdle", isIdle);
		Vector3 direction = new Vector3(clickPosition.x, attacker.transform.position.y, clickPosition.z) - attacker.transform.position;
		direction.Normalize();
		attacker.transform.rotation = Quaternion.LookRotation(direction);

		isWalking = true;
			animator.SetBool("IsWalking", isWalking);

		return direction;
	}

	public void SetUpIdle(GameObject attacker, Animator animator, GameObject midpoint)
	{

		isWalking = false;
		animator.SetBool("IsWalking", isWalking);

		isIdle = true;
		animator.SetBool("IsIdle", isIdle);
		//Vector3 direction = new Vector3(midpoint.transform.position.x, attacker.transform.position.y, midpoint.transform.position.z) - attacker.transform.position;
		//direction.Normalize();
		//attacker.transform.rotation = Quaternion.LookRotation(direction);
		
	}

	public void GetHitAnimation(Animator animAttacker, Animator animAttacked)
	{
		StartCoroutine(SetupGetHit(animAttacker, animAttacked));
	}

	public IEnumerator SetupGetHit(Animator animAttacker, Animator animAttacked)
	{
		//AnimatorStateInfo hitStateInfo = animAttacker.GetCurrentAnimatorStateInfo(0);
		//while (!hitStateInfo.IsName("Attack"))
		//{
		//	yield return null;
		//	hitStateInfo = animAttacker.GetCurrentAnimatorStateInfo(0);
		//}
		//while (hitStateInfo.normalizedTime < 1f)
		//{
		//	yield return null;
		//	hitStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		//}
		yield return new WaitForSeconds(0.3f);
		animAttacked.SetTrigger("GetHit");
		
	}
	
}
