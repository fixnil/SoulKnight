using UnityEngine;

public class ETFXProjectileScript : MonoBehaviour
{
    public GameObject impactParticle; // Effect spawned when projectile hits a collider
    public GameObject projectileParticle; // Effect attached to the gameobject as child
    public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
    [Header("Adjust if not using Sphere Collider")]
    public float colliderRadius = 1f;
    [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
    public float collideOffset = 0.15f;

    private void Start()
    {
        projectileParticle = Instantiate(projectileParticle, this.transform.position, this.transform.rotation);
        projectileParticle.transform.parent = this.transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, this.transform.position, this.transform.rotation);
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }
    }

    private void FixedUpdate()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            this.transform.rotation = Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity); // Sets rotation to look at direction of movement
        }

        float radius; // Sets the radius of the collision detection
        if (this.transform.GetComponent<SphereCollider>())
        {
            radius = this.transform.GetComponent<SphereCollider>().radius;
        }
        else
        {
            radius = colliderRadius;
        }

        var direction = this.transform.GetComponent<Rigidbody>().velocity; // Gets the direction of the projectile, used for collision detection
        if (this.transform.GetComponent<Rigidbody>().useGravity)
        {
            direction += Physics.gravity * Time.deltaTime; // Accounts for gravity if enabled
        }

        direction = direction.normalized;

        var detectionDistance = this.transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; // Distance of collision detection for this frame

        if (Physics.SphereCast(this.transform.position, radius, direction, out var hit, detectionDistance)) // Checks if collision will happen
        {
            this.transform.position = hit.point + (hit.normal * collideOffset); // Move projectile to point of collision

            var impactP = Instantiate(impactParticle, this.transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)); // Spawns impact effect

            var trails = this.GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                         //Component at [0] is that of the parent i.e. this object (if there is any)
            for (var i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
            {
                var trail = trails[i];

                if (trail.gameObject.name.Contains("Trail"))
                {
                    trail.transform.SetParent(null); // Detaches the trail from the projectile
                    Destroy(trail.gameObject, 2f); // Removes the trail after seconds
                }
            }

            Destroy(projectileParticle, 3f); // Removes particle effect after delay
            Destroy(impactP, 3.5f); // Removes impact effect after delay
            Destroy(this.gameObject); // Removes the projectile
        }
    }
}