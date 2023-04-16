using UnityEngine;

public class AtkHandler : MonoBehaviour
{
    public int speed = 500;
    public string layer;
    public Transform[] shootPoints;

    public void Atk()
    {
        foreach (var shootPoint in shootPoints)
        {
            var bullet = BulletPool.instance.Rent();

            bullet.layer = LayerMask.NameToLayer(layer);
            bullet.transform.position = shootPoint.position;

            if (bullet.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddForce(shootPoint.forward * speed);
            }
        }
    }
}
