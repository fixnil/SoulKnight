using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
    public class ETFXButtonScript : MonoBehaviour
    {
        public GameObject Button;
        private Text _myButtonText;
        private string _projectileParticleName;       // The variable to update the text component of the button

        private ETFXFireProjectile _effectScript;     // A variable used to access the list of projectiles
        private ETFXProjectileScript _projectileScript;

        public float buttonsX;
        public float buttonsY;
        public float buttonsSizeX;
        public float buttonsSizeY;
        public float buttonsDistance;

        private void Start()
        {
            _effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
            this.GetProjectileNames();
            _myButtonText = Button.transform.Find("Text").GetComponent<Text>();
            _myButtonText.text = _projectileParticleName;
        }

        private void Update()
        {
            _myButtonText.text = _projectileParticleName;
            //		print(projectileParticleName);
        }

        public void GetProjectileNames()            // Find and diplay the name of the currently selected projectile
        {
            // Access the currently selected projectile's 'ProjectileScript'
            _projectileScript = _effectScript.projectiles[_effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
            _projectileParticleName = _projectileScript.projectileParticle.name;  // Assign the name of the currently selected projectile to projectileParticleName
        }

        public bool OverButton()        // This function will return either true or false
        {
            var button1 = new Rect(buttonsX, buttonsY, buttonsSizeX, buttonsSizeY);
            var button2 = new Rect(buttonsX + buttonsDistance, buttonsY, buttonsSizeX, buttonsSizeY);

            if (button1.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) ||
               button2.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}