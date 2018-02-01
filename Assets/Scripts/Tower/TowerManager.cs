using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager :Singleton<TowerManager> {

    private TowerBtn towerBtnPressed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); //Raycast from lower left corner to where we clicked

            if(hit.collider.tag == "BuildSite")
                PlaceTower(hit);

        }
	}

    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
    }
    
     public void PlaceTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);

            newTower.transform.position = hit.transform.position;
        }
    }

}
