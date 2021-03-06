﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager> {

    public TowerBtn towerBtnPressed { get; set; }
    private SpriteRenderer spriteRenderer;

    private List<Tower> towersList = new List<Tower>();
    private List<Collider2D> buildList = new List<Collider2D>();
    private Collider2D buildTile;

	// Use this for initialization
	void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        buildTile = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); //Raycast from lower left corner to where we clicked

            if(hit.collider.tag == "BuildSite")
            {
                buildTile = hit.collider;
                buildTile.tag = "BuildSiteFull"; 
                RegisterBuildSite(buildTile);
                PlaceTower(hit);
            }
        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        buildList.Add(buildTag);
    }

    public void RegisterTower(Tower tower)
    {
        towersList.Add(tower);
    }

    public void RenameTagsBuildSites()
    {
        foreach(Collider2D buildTag in buildList)
        {
            buildTag.tag = "BuildSite";
        }

        buildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach(Tower tower in towersList)
        {
            Destroy(tower.gameObject);
        }

        towersList.Clear();
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        if(towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            EnableDragSprite(towerBtnPressed.DragSprite);
        }
        
    }
    
     public void PlaceTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            Tower newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            DisableDragSprite();
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);

            BuyTower(towerBtnPressed.TowerPrice);
            RegisterTower(newTower);
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

    public void BuyTower(int price)
    {
        GameManager.Instance.SubtractMoney(price);
    }



}
