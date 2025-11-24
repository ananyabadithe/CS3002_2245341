using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject enemyShip;
    public LaserBeamController LaserBeam;

    private List<GameObject> spawnedShips = new List<GameObject>();

    private Vector2 position_1 = new Vector2(-13.5f, 11.5f);
    private Vector2 position_2 = new Vector2(-8.5f, 12.5f);
    private Vector2 position_3 = new Vector2(-3.5f, 11.5f);

    void Start()
    {
        spawnedShips.Add(Instantiate(enemyShip, position_1, transform.rotation));
        spawnedShips.Add(Instantiate(enemyShip, position_2, transform.rotation));
        spawnedShips.Add(Instantiate(enemyShip, position_3, transform.rotation));
    }

    private void Update()
    {
       
    }

    public List<GameObject> SpawnedShips
    {
        get { return spawnedShips; }
    }
}
