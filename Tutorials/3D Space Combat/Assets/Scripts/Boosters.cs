using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosters : MonoBehaviour {

    [SerializeField]
    private float combatBoostSpeed = 400.0f;
    [SerializeField]
    private float boostSpeed = 1000.0f;
    [SerializeField]
    private CameraController cameraController;

    private bool _isBoosting = false;

    public float BoostSpeed { get; set; }

    void Start()
    {
        GameManager.instance.IsInCombatChanged += IsInCombatChanged;
    }

    public void RecalculateBoost()
    {
        if (Input.GetAxis("Vertical") > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            if (!_isBoosting)
            {
                _isBoosting = true;
                SetBoostSpeed(1f, GetBoostSpeed());
                GameManager.instance.IsShootingEnabled = false;
                CameraControllerEnterBoost();
            }
        }
        else if (_isBoosting)
        {
            _isBoosting = false;
            SetBoostSpeed(1f, 0f);
            GameManager.instance.IsShootingEnabled = true;
            cameraController.ExitBoost();
        }
    }

    private float GetBoostSpeed()
    {
        return GameManager.instance.IsInCombat ? combatBoostSpeed : boostSpeed;
    }

    private void SetBoostSpeed(float time, float value)
    {
        Go.to(this, time, new GoTweenConfig().floatProp("BoostSpeed", value));
    }

    private void CameraControllerEnterBoost()
    {
        if (GameManager.instance.IsInCombat)
        {
            cameraController.EnterCombatBoost();
        }
        else
        {
            cameraController.EnterBoost();
        }
    }

    private void IsInCombatChanged(object sender, System.EventArgs e)
    {
        if (Input.GetAxis("Vertical") <= 0f || !Input.GetKey(KeyCode.LeftShift)) return;

        SetBoostSpeed(1f, GetBoostSpeed());
        CameraControllerEnterBoost();
    }
}
