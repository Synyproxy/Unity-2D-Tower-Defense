using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager :Singleton<TowerManager> {

    private TowerBtn towerBtnPressed;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); //Raycast from lower left corner to where we clicked

            if(hit.collider.tag == "BuildSite")
            {
                hit.collider.tag = "BuildSiteFull";
                PlaceTower(hit);
            }

          

        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        EnableDragSprite(towerBtnPressed.DragSprite);
    }
    
     public void PlaceTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);

            newTower.transform.position = hit.transform.position;

            DisableDragSprite();
        }
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Bug you have to reset position to see the drag 
        transform.position = new Vector2(transform.position.x, transform.position.y );
    }

    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
    }

}
