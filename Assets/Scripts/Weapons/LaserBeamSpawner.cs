using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserBeamSpawner : MonoBehaviour
{
    public GameObject laserBeam;
    public EnemyShipSpawner shipSpawner;

    public float scene1Cooldown = 3f; 
    public float scene2Cooldown = 1.8f; 

    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        float currentCooldown = GetCooldownForScene();

        if (cooldownTimer >= currentCooldown)
        {
            SpawnLaserFromAliveShip();
            cooldownTimer = 0f;
        }
    }

    float GetCooldownForScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (index == 2)    
            return scene2Cooldown;

        return scene1Cooldown; 
    }

    void SpawnLaserFromAliveShip()
    {
        List<GameObject> ships = shipSpawner.SpawnedShips;
        ships.RemoveAll(ship => ship == null);

        if (ships.Count == 0)
            return;

        GameObject ship = ships[Random.Range(0, ships.Count)];
        Instantiate(laserBeam, ship.transform.position, Quaternion.identity, transform);
    }

}
