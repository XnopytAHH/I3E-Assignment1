using System.Collections;
using UnityEngine;


public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    float projDeathTime = 5f; // Time after which the projectile will be destroyed
    [SerializeField]
    public int projectileDamage = 10; // Damage dealt by the projectile
    [SerializeField]
    AudioClip projectileAudioClip; // Audio source for the projectile sound
    bool canDamage = true;
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
    public void collidedWithPlayer(PlayerBehaviour player)
    {
        if (canDamage)
        {
            canDamage = false; // Prevent further damage from this projectile
            player.ModifyHealth(-projectileDamage);
        }
        else
        {
            return; // If already damaged, do nothing
        }
        // Apply damage to the player
        
        Destroy(gameObject); // Destroy the projectile after hitting the player
    }
    void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(projectileAudioClip, transform.position, 0.3f); // Play the projectile sound on collision
        if (collision.gameObject.CompareTag("Enemy") && gameObject.name != "EnemyProjectile")
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().ModifyHealth(-projectileDamage);
        }
        
        Destroy(gameObject);
    }
}
