using UnityEngine;
using System.Collections;

public class WeaponSelector : MonoBehaviour {

    public Transform[] weapons;

	void Start ()
    {
        SelectWeapon(0);
	}
	
	void Update ()
    {
        for (int i = 0; i < 11; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i <= weapons.Length - 1)
                {
                    SelectWeapon(i);
                }
            }
        }
	}

    private void SelectWeapon(int index)
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            if (i == index)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
