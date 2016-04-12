using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public GameOverScreen winScreen;
    public CameraController cameraController;
    public Transform playerStartingTransform;
    public GameObject[] asteroidPrefabs;
    public Vector2 asteroidSizeRange;
    public Quest firstQuest;
    public Transform deimos;
    public int deimosAsteroidCount;
    public GameObject deimosTravelObjective;
    public Transform deimosSpawnPoint;
    public GameObject enemyShipPrefab;
    public AudioClip dialogue1;
    public AudioClip ambienceClip;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isShootingEnabled = true;
    [HideInInspector]
    public bool isCursorVisible = true;
    [HideInInspector]
    public bool isMenuOpen = false;
    [HideInInspector]
    public PauseType pauseType = PauseType.none;

    public static GameManager instance;
    public static Transform playerTransform;
    public static QuestManager questManager;

    private Player player;
    private AudioSource audioAmbience;

    public enum PauseType
    {
        none,
        pauseMenu,
        questMenu,
        gameOver
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        questManager = gameObject.GetComponent<QuestManager>();
        audioAmbience = gameObject.AddComponent<AudioSource>();
        audioAmbience.clip = ambienceClip;
        audioAmbience.loop = true;
        audioAmbience.volume = 0.25f;
    }

	void Start ()
    {
        // Spawn asteroids around Deimos
        SpawnHazardsAroundSphere(deimos, 3600, 2600, deimosAsteroidCount);

        // Position player at start transform
        playerTransform.position = playerStartingTransform.position;
        playerTransform.rotation = playerStartingTransform.rotation;

        // Lock player controls until after intro dialogue
        player = playerTransform.GetComponent<Player>();
        //player.LockControls(true);
        //StartCoroutine(player.LockControlsDelayed(false, 26.5f));
        //player.LockControls(false);

        //player.Dock(playerStartingTransform);

        audioAmbience.Play();

        // Add quest after intro dialogue
        Invoke("InitializeTargetPracticeQuest", 5.5f);
        DialogueManager.instance.BeginDialogue(dialogue1);

        //Invoke("KillPlayer", 10);
        Invoke("DisplayWinScreen", 10);
	}

    private void KillPlayer()
    {
        playerTransform.SendMessage("Damage", new DamageInfo(gameObject, 50));
    }

    public void GameOver()
    {
        gameOverScreen.Display();
    }

    public void DisplayWinScreen()
    {
        player.LockControls(true);
        player.LockMovement(true);
        winScreen.Display();
    }

    public void CloseWinScreen()
    {
        player.LockControls(false);
        player.LockMovement(false);
    }

    public void DockPlayer()
    {
        if (player != null)
        {
            player.Dock(playerStartingTransform);
            cameraController.PerformDock();
        }
    }

    public void UndockPlayer()
    {
        if (player != null)
        {
            player.Undock();
            cameraController.PerformUndock();
        }
    }
	
    private void SpawnHazards()
    {
        //for (int i = 0; i < hazardCount; i++)
        //{
        //    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        //    Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnPosition.x, hazardSpawnPosition.x), Random.Range(-hazardSpawnPosition.y, hazardSpawnPosition.y), Random.Range(-hazardSpawnPosition.z, hazardSpawnPosition.z));
        //    GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
        //    float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
        //    hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        //}
    }

    private void SpawnHazardsAroundSphere(Transform sphere, float outerRadius, float innerRadius, float number)
    {
        for(int i = 0; i < number; i++)
        {
            GameObject hazard = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + sphere.position;
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(asteroidSizeRange.x, asteroidSizeRange.y);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }
    }

    /// <summary>
    /// Initialize the 'Target Practice' quest and add it to the Quest Manager
    /// </summary>
    private void InitializeTargetPracticeQuest()
    {
        // OBJECTIVE 1 - TRAVEL TO DEIMOS
        ObjectiveTarget deimos = deimosTravelObjective.AddComponent<ObjectiveTarget>();
        Objective firstObjective = firstQuest.GetObjectiveAtIndex(0);
        firstObjective.AssignTarget(deimos);

        // OBJECTIVE 2 - DEFEAT RAIDERS
        SpawnEnemyTargetsAtObjective(firstQuest, 1, enemyShipPrefab, 5, deimosSpawnPoint.position);

        questManager.Add(firstQuest);
        questManager.SetActiveQuest(firstQuest);
    }

    private void SpawnEnemyTargetsAtObjective(Quest quest, int ObjectiveIndex, GameObject shipPrefab, int count, Vector3 spawnPosition)
    {
        List<ObjectiveTarget> enemiesObj1 = new List<ObjectiveTarget>();
        for (int i = 0; i < count; i++)
        {
            GameObject enemyShip = Instantiate(shipPrefab, new Vector3((i * 10f) + spawnPosition.x, spawnPosition.y, spawnPosition.z), Quaternion.identity) as GameObject;
            ObjectiveTarget enemyTarget = enemyShip.AddComponent<ObjectiveTarget>();
            enemiesObj1.Add(enemyTarget);
        }

        Objective objective = quest.GetObjectiveAtIndex(ObjectiveIndex);
        objective.AssignTargets(enemiesObj1.ToArray());
    }
}
