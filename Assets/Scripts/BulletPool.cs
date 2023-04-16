using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    public int capacity = 200;
    public GameObject bulletPrefab;

    private readonly Queue<GameObject> _bullets = new();

    private void Awake()
    {
        instance = this;

        for (var i = 0; i < capacity; i++)
        {
            var bullet = Instantiate(bulletPrefab);

            bullet.SetActive(false);

            _bullets.Enqueue(bullet);
        }
    }

    public GameObject Rent()
    {
        if (!_bullets.TryDequeue(out var bullet))
        {
            bullet = Instantiate(bulletPrefab);
        }

        bullet.SetActive(true);

        return bullet;
    }

    public void Return(GameObject bullet)
    {
        bullet.SetActive(false);

        _bullets.Enqueue(bullet);
    }
}
