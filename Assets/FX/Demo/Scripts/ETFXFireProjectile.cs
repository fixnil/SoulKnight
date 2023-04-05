using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicToonFX
{
    public class ETFXFireProjectile : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] projectiles;
        [Header("Missile spawns at attached game object")]
        public Transform spawnPosition;
        [HideInInspector]
        public int currentProjectile = 0;
        public float speed = 500;

        //    MyGUI _GUI;
        private RaycastHit _hit;
        private ETFXButtonScript _selectedProjectileButton;

        private void Start()
        {
            _selectedProjectileButton = GameObject.Find("Button").GetComponent<ETFXButtonScript>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.NextEffect();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.NextEffect();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                this.PreviousEffect();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.PreviousEffect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) //On left mouse down-click
            {
                if (!EventSystem.current.IsPointerOverGameObject()) //Checks if the mouse is not over a UI part
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, 100f)) //Finds the point where you click with the mouse
                    {
                        var projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity); //Spawns the selected projectile
                        projectile.transform.LookAt(_hit.point); //Sets the projectiles rotation to look at the point clicked
                        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody
                    }
                }
            }

            Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        }

        public void NextEffect() //Changes the selected projectile to the next. Used by UI
        {
            if (currentProjectile < projectiles.Length - 1)
            {
                currentProjectile++;
            }
            else
            {
                currentProjectile = 0;
            }

            _selectedProjectileButton.GetProjectileNames();
        }

        public void PreviousEffect() //Changes selected projectile to the previous. Used by UI
        {
            if (currentProjectile > 0)
            {
                currentProjectile--;
            }
            else
            {
                currentProjectile = projectiles.Length - 1;
            }

            _selectedProjectileButton.GetProjectileNames();
        }

        public void AdjustSpeed(float newSpeed) //Used by UI to set projectile speed
        {
            speed = newSpeed;
        }
    }
}