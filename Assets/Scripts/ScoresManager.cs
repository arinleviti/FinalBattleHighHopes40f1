using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoresManager : MonoBehaviour
{
	public TextMeshProUGUI playerHPText;
	public TextMeshProUGUI monster1HPText;
	public TextMeshProUGUI monster2HPText;
	public TextMeshProUGUI movesLeft;
	private PlayerStats playerStats;
	private MonsterStats firstMonsterStats;
	private MonsterStats secondMonsterStats;
	private CombatManager combatManager;
	private GameObject playerGO;
	private GameObject currentPlayer;
	public GameObject monsterGO;
	private int playerHP;
	private int firstMonsterHP;
	private int secondMonsterHP;
	private int movesLeftInt;
	private Animator animator;
	private AnimScript animScript;
	private bool isCharacterDead = false;
	private GameObject canvasYouWin;
	private GameObject canvasYouDie;
	private Button restartButton;
	private bool flag1 = false;
	private Button potionButton;
	private TextMeshProUGUI potionsLeftText;
	public int potionsLeftInt;
	private Material textMaterial;
	private Color originalOutlineColor;
	private float originalOutlineWidth;
	public Camera mainCamera;
	public GameObject midpoint;

	// Start is called before the first frame update
	private void Start()
	{
		// Cache the references to GameObjects and components
		GameObject player = GameObject.Find("Player");
		playerGO = player;
		playerStats = player.GetComponent<PlayerStats>();
		//potionsLeftInt = playerStats.PotionsAvailable;
		GameObject firstMonster = GameObject.Find("Zombie 1");
		firstMonsterStats = firstMonster.GetComponent<MonsterStats>();
		GameObject secondMonster = GameObject.Find("Zombie 2");
		secondMonsterStats = secondMonster.GetComponent<MonsterStats>();
		GameObject combatManagerObject = GameObject.Find("CombatManager");
		combatManager = combatManagerObject.GetComponent<CombatManager>();
		animator = GameObject.Find("OrkAssasin").GetComponent<Animator>();		
		animScript = GameObject.Find("AnimatorObj").GetComponent<AnimScript>();
		midpoint = GameObject.Find("Midpoint");
		textMaterial = playerHPText.fontMaterial;
		originalOutlineColor = textMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
		originalOutlineWidth = textMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
	}

	// Update is called once per frame
	void Update()
	{		
		playerHP = playerStats.HP;
		firstMonsterHP = firstMonsterStats.HP;
		secondMonsterHP = secondMonsterStats.HP;
		movesLeftInt = combatManager.movesLeft;
		currentPlayer = combatManager.currentTurn;

		playerHPText.text = $"Player HP: {playerHP}";
		monster1HPText.text = $"Zombie 1 HP: {firstMonsterHP}";
		monster2HPText.text = $"Zombie 2 HP: {secondMonsterHP}";
		//if (combatManager.currentTurn.CompareTag("Player") /*&& potionButton != null*/)
		//{
		//	potionButton = GameObject.Find("PotionCanvasPrefab").GetComponentInChildren<Button>();
		//	potionsLeftText = potionButton.GetComponentInChildren<TextMeshProUGUI>();
		//	potionsLeftInt = playerStats.PotionsAvailable;
		//	potionsLeftText.text = $"{potionsLeftInt}";
		//}
		movesLeft.text = $"Current Turn: {currentPlayer.name} Moves left: {movesLeftInt}";
		if (playerHP <= 0 && !isCharacterDead )
		{
			isCharacterDead = true;			
		}
		IsGameOver();		
	}
	
	private void IsGameOver()
	{
		if (firstMonsterHP <= 0 && secondMonsterHP <= 0 && !flag1)
		{
			canvasYouWin = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas YouWin"));
			AudioManager.instance.PlayEffect("VictoryClips", mainCamera.transform.position, 0);
			restartButton = canvasYouWin.GetComponentInChildren<Button>();			
			flag1 = true;
			restartButton.onClick.AddListener(RestartGame);
			StartCoroutine(WaitAndPauseGame(3));

		}
		if (playerHP <= 0 && !flag1)
		{
			canvasYouDie = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas YouDie"));
			AudioManager.instance.PlayEffect("GameOverClips", mainCamera.transform.position , 0);
			AudioManager.instance.PlayEffect("LastBreathClips", midpoint.transform.position, 0);
			restartButton = canvasYouDie.GetComponentInChildren<Button>();
			flag1 = true;
			animScript.PlayDeathAnim(playerGO, animator);
			restartButton.onClick.AddListener(RestartGame);
			StartCoroutine(WaitAndPauseGame(2));
		}
	}

    private void RestartGame()
    {
        // Ensure all object pools are cleared
        if (ObjPoolManager.Instance != null)
        {
            ObjPoolManager.Instance.ClearAllPools();

            // Optionally destroy the singleton instance to ensure it's recreated
            Destroy(ObjPoolManager.Instance.gameObject);
        }

        // Reset time scale to ensure gameplay resumes correctly
        Time.timeScale = 1;

        // Optional: Force garbage collection to clean up unused objects
        System.GC.Collect();

        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
 //   private void RestartGame()
	//{
	//	Time.timeScale = 1; // Reset time scale before restarting the game
	//	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	//}
	private IEnumerator WaitAndPauseGame(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Time.timeScale = 0;
	}
	public void TriggerGlowEffect()
	{
		StartCoroutine(GlowEffect());
	}
	private IEnumerator GlowEffect()
	{
		float duration = 2f;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			float t = Mathf.PingPong(elapsed * 2, 1);  // Creates a pulsing effect
			Color glowColor = Color.Lerp(originalOutlineColor, new Color(0.5f, 0, 0.5f), t);
			float glowWidth = Mathf.Lerp(originalOutlineWidth, originalOutlineWidth + 0.5f, t);
			// Apply the outline properties to the material
			textMaterial.SetColor(ShaderUtilities.ID_OutlineColor, glowColor);
			textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, glowWidth);
			elapsed += Time.deltaTime;
			yield return null;
		}
		textMaterial.SetColor(ShaderUtilities.ID_OutlineColor, originalOutlineColor);
		textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, originalOutlineWidth);
	}
}
