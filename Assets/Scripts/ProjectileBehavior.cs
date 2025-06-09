using System.Collections;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    float projDeathTime = 5f; // Time after which the projectile will be destroyed
    [SerializeField]
    public int projectileDamage = 10; // Damage dealt by the projectile
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterTime(projDeathTime)); // Destroy the projectile after 5 seconds
    }
    IEnumerator DestroyAfterTime(float time)
    {
        StopCoroutine(DestroyAfterTime(time)); // Stop any previous coroutine to avoid multiple calls
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy the projectile object
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
