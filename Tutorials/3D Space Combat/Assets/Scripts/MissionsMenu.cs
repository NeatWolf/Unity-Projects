using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionsMenu : MonoBehaviour
{
    public GameObject missionsMenuCanvas;
    public VerticalLayoutGroup missionsGroup;
    public MissionDetails detailsPanel;

    private List<Mission> missions;

    private bool isOpen;

    void Start()
    {
        missions = new List<Mission>();
        GameManager.NewMissionAcquired += new GameManager.NewMissionEventHandler(OnNewMissionAcquired);
    }

    void Update()
    {
        if (isOpen)
        {
            missionsMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameManager.instance.isPaused = true;
        }
        else
        {
            missionsMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            GameManager.instance.isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
        }
    }

    public void DisplayMissionDetails(string missionName, string description, Dictionary<string, bool> objectives)
    {
        detailsPanel.SetDetails(missionName, description, objectives);
    }

    private void OnNewMissionAcquired(Mission mission)
    {
        mission.SetOnClickMissionDetails(new Mission.MissionDetailsHandler(DisplayMissionDetails));
        missions.Add(mission);
        mission.SetParent(missionsGroup);
        print("mission added");
    }
}
