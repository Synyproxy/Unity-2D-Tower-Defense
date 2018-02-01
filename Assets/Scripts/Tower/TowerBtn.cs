using UnityEngine;

public class TowerBtn : MonoBehaviour {

	[SerializeField]
    private GameObject towerObject;

    public GameObject TowerObject
    {
        get{
            return towerObject;
        }
    }
}
