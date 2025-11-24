using UnityEngine;

public class EnemyAlienController : MonoBehaviour
{
    public float blockSpeed = 25f;  
    public bool isLeftAlien = true;

    private float minX;
    private float maxX;

    void Start()
    {
        if (isLeftAlien)
        {
            minX = -17f;
            maxX = -8.5f;
        }
        else
        {
            minX = -8.5f;
            maxX = 0f;
        }
    }

    void Update()
    {
        if (BulletController.activeBullets.Count == 0)
            return;

        BulletController targetBullet = GetMostDangerousBullet();
        if (targetBullet == null) return;

        float targetX = targetBullet.transform.position.x;

        targetX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 newPos = transform.position;
        newPos.x = Mathf.MoveTowards(transform.position.x, targetX, blockSpeed * Time.deltaTime);

        transform.position = newPos;
    }

    BulletController GetMostDangerousBullet()
    {
        BulletController closest = null;
        float highestY = float.MinValue;

        foreach (var bullet in BulletController.activeBullets)
        {
            if (bullet == null) continue;

            if (bullet.transform.position.y > highestY)
            {
                highestY = bullet.transform.position.y;
                closest = bullet;
            }
        }

        return closest;
    }
}
