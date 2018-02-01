using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager :Singleton<TowerManager> {

    private TowerBtn towerBtnPressed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;

        Debug.Log("Pressed: " + towerBtnPressed.gameObject);
    }
}
