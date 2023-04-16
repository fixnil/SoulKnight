using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            BulletPool.instance.Return(this.gameObject);
        }
    }

    private void OnEnable()
    {
        lifeTime = 5f;
    }

    private void OnDisable()
    {
        if (this.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }
    }
}
