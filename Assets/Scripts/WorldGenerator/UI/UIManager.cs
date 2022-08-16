using UnityEngine;

namespace Assets.Scripts.WorldGenerator.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject legendContainer;
        [SerializeField] private GameObject settingsContainer;


        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                legendContainer.SetActive(!legendContainer.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                settingsContainer.SetActive(!settingsContainer.activeSelf);
            }
        }
    }
}
