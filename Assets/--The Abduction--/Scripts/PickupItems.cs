using UnityEngine;
using Invector.vCharacterController;
public class PickupItems : MonoBehaviour
{

    [SerializeField] string PickUpName;
    public GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(PickUpName == "HealthPotion")
            {
                var controller = other.GetComponent<vThirdPersonController>();
                if (controller != null)
                {
                    controller.AddHealth(5);
                    Destroy(gameObject);
                }
            }
            if (PickUpName == "Pill")
            {
                var controller = other.GetComponent<vThirdPersonController>();
                if (controller != null)
                {
                    controller.AddHealth(10);
                    Destroy(gameObject);
                }
            }
            if (PickUpName == "Key")
            {
                // other.GetComponent<Show>().shw();
                other.GetComponent<Show>().k = true;
                Destroy(gameObject);
                Destroy(particles);
            }
        }
    }
}
