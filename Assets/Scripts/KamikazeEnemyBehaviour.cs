using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KamikazeEnemyBehaviour : MonoBehaviour
{
    float health = 10f; // Health of the enemy
    [SerializeField]
    float fireSpeed = 20f;
    [SerializeField]
    float shootInterval = 5f; // Time interval between shots
    [SerializeField]
    float detectionRange = 10f; // Range within which the enemy can detect the player
    [SerializeField]
    float detonationRange = 10f;
    bool isActive = false; // Flag to control attacks
    [SerializeField]
    GameObject explosion;
    bool isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            Explode();
        }
        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            UnityEngine.Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            // Check if the player is within detection range
            float distance = UnityEngine.Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                RaycastHit hitInfo;
                // Check if the player is within detection range

                if (player != null)
                {


                    if (Physics.Raycast(transform.position, directionToPlayer, out hitInfo))
                    {

                        if (hitInfo.collider.CompareTag("PlayerCollider"))
                        {
                            // If the player is detected, start shooting

                            StartCoroutine(AttackCountdown(shootInterval));
                            // Rotate towards the player
                            directionToPlayer.y = 0; // Keep the rotation on the horizontal plane
                            transform.rotation = UnityEngine.Quaternion.LookRotation(directionToPlayer);
                        }
                        else
                        {
                            // If the player is not detected, stop shooting
                            isActive = false;
                        }
                    }
                }
            }
            else
            {
                // If the player is out of range, stop shooting
                isActive = false;
            }

            distance = UnityEngine.Vector3.Distance(transform.position, player.transform.position);
            if (distance > detonationRange && isActive)
            {

                gameObject.GetComponent<Rigidbody>().AddForce(directionToPlayer * fireSpeed);
                distance = UnityEngine.Vector3.Distance(transform.position, player.transform.position);
            }
            if (distance <= detonationRange && isMoving)
            {
                Explode();
                isActive = false;
            }
        }
    }

    IEnumerator AttackCountdown(float shootInterval)
    {
        yield return new WaitForSeconds(shootInterval); // Wait for 2 seconds before shooting
        isActive = true;
        isMoving = true;

    }
    public void ModifyHealth(int amount)
    {
        health += amount;
    }
    void Explode()
    {
        Instantiate(explosion, gameObject.transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
