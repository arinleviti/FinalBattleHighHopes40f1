using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    public Animator animator;

	private bool isIdle = false;
	private bool isWalking = false;
	

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PunchAnimation()
	{
		StartCoroutine(WaitForPunchAnimation());	
	}

	private IEnumerator WaitForPunchAnimation()
	{
		isWalking = false;
		animator.SetBool("IsWalking", isWalking);
		animator.SetTrigger("IsPunching");

		//AnimatorStateInfo: A structure that holds information about the current state of an Animator.Gets the current state of the Animator for the first layer (index 0).

		AnimatorStateInfo punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		while (!punchStateInfo.IsName("Punch"))
		{
			yield return null;
			//normalizedTime is a value between 0 and 1 that represents the progress of the animation (0 is the start, and 1 is the end).
			punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		}
		while (punchStateInfo.normalizedTime < 1.0f)
		{
			yield return null;
			punchStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		}
		animator.Play("idle1");
	}

	void SetupIdle()
	{
		isWalking = false;
		animator.SetBool("IsWalking", isWalking);

		isIdle = true;
		animator.SetBool("IsIdle", isIdle);
	}

	public void PlayDeathAnim(GameObject characterToRemove)
	{
		StartCoroutine(TimeToDie( characterToRemove));
	}

	private IEnumerator TimeToDie(GameObject characterToRemove)
	{
		animator.SetInteger("HP", -2);
		Debug.Log("Initiated Dying");
		yield return new WaitForSeconds(5);
		Debug.Log("Finished dying");
		Destroy(characterToRemove);
	}

	public Vector3 SetupWalking(GameObject attacker, Vector3 clickPosition)
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

	public void SetUpIdle(GameObject attacker, GameObject midpoint)
	{

		isWalking = false;
		animator.SetBool("IsWalking", isWalking);

		isIdle = true;
		animator.SetBool("IsIdle", isIdle);
		Vector3 direction = new Vector3(midpoint.transform.position.x, attacker.transform.position.y, midpoint.transform.position.z) - attacker.transform.position;
		direction.Normalize();
		attacker.transform.rotation = Quaternion.LookRotation(direction);
		
	}
}
