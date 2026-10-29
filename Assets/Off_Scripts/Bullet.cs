using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 25f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            // You can reduce player health here later
        }
        Destroy(gameObject);
    }
}
