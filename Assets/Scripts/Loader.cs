using UnityEngine;

public class Loader : MonoBehaviour {

    [SerializeField]
    private GameObject gameManager;

    private void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);
    }
}
