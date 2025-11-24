using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAlienSpawner : MonoBehaviour
{
    public GameObject enemyAlien;

    private List<GameObject> spawnedAliens = new List<GameObject>();

    private Vector2 leftPosition;
    private Vector2 rightPosition;

    public List<GameObject> SpawnedAliens
    {
        get { return spawnedAliens; }
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            leftPosition = new Vector2(-13.5f, 9.5f);
            rightPosition = new Vector2(-3.5f, 9.5f);
        }
        else
        {
            leftPosition = new Vector2(-13.5f, 10f);
            rightPosition = new Vector2(-3.5f, 10f);
        }
        SpawnAlien(true);   // left alien
        SpawnAlien(false);  // right alien
    }

    void Update()
    {
        //spawnedAliens.RemoveAll(a => a == null);

        //MaintainAliens();
    }

    private void MaintainAliens()
    {
        bool hasLeft = false;
        bool hasRight = false;

        // Check existing aliens
        foreach (var alien in spawnedAliens)
        {
            if (alien == null) continue;

            EnemyAlienController ctrl = alien.GetComponent<EnemyAlienController>();
            if (ctrl == null) continue;

            if (ctrl.isLeftAlien) hasLeft = true;
            else hasRight = true;
        }

        if (!hasLeft)
            SpawnAlien(true);

        if (!hasRight)
            SpawnAlien(false);
    }

    private void SpawnAlien(bool spawnLeft)
    {
        Vector2 spawnPos = spawnLeft ? leftPosition : rightPosition;

        GameObject alien = Instantiate(enemyAlien, spawnPos, Quaternion.identity);

        EnemyAlienController controller = alien.GetComponent<EnemyAlienController>();
        if (controller != null)
        {
            controller.isLeftAlien = spawnLeft;
        }

        spawnedAliens.Add(alien);
    }
}
