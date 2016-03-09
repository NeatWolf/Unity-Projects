using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int hazardCount;
    public GameObject[] hazards;
    public Vector3 hazardSpawnPosition;
    public Vector2 hazardSizeRange;
    public Transform planet;
    public Mission missionPrefab;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isPaused = false;
    [HideInInspector]
    public List<Mission> missions;

    public static GameManager instance;

    public delegate void NewMissionEventHandler(Mission mission);
    public static event NewMissionEventHandler NewMissionAcquired;

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
        DontDestroyOnLoad(gameObject);
    }

	void Start ()
    {
        SpawnHazards();
        Invoke("AddMission", 2);
        Invoke("AddMission2", 4);
	}
	
    void SpawnHazards()
    {
        //for (int i = 0; i < hazardCount; i++)
        //{
        //    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        //    Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnPosition.x, hazardSpawnPosition.x), Random.Range(-hazardSpawnPosition.y, hazardSpawnPosition.y), Random.Range(-hazardSpawnPosition.z, hazardSpawnPosition.z));
        //    GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
        //    float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
        //    hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        //}
        SpawnHazardsAroundSphere(planet, 3600, 2600, hazardCount);
    }

    private void SpawnHazardsAroundSphere(Transform sphere, float outerRadius, float innerRadius, float number)
    {
        for(int i = 0; i < number; i++)
        {
            GameObject hazard = hazards[Random.Range(0, hazards.Length)];
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + sphere.position;
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }
    }

    private void AddMission()
    {
        Mission mission = Instantiate(missionPrefab) as Mission;
        mission.MissionName = "Trouble over the Red Planet";
        mission.description = "Destroy all enemies over Mars.";
        mission.startingObjectives = new Dictionary<string, bool>()
        {
            { "Destroy all enemies (0/10)", false }
        };

        if (NewMissionAcquired != null)
        {
            NewMissionAcquired(mission);
        }
    }

    private void AddMission2()
    {
        Mission mission = Instantiate(missionPrefab) as Mission;
        mission.MissionName = "Shoot for the moon";
        mission.description = "Destroy all enemies near Deimos.";
        mission.startingObjectives = new Dictionary<string, bool>()
        {
            { "Destroy all enemies (0/10)", false }
        };

        if (NewMissionAcquired != null)
        {
            NewMissionAcquired(mission);
        }
    }
}
