using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    public int damagePerSecond = 10;
    public float tickRate = 1f;
    private bool canDamage = true;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player") && canDamage)
        {
            var combat = other.GetComponent<Jsgaona.PlayerCombatController>();

            if (combat != null)
            {
                Debug.Log("EnemyDamage: daño por segundo → " + damagePerSecond);
                combat.TakeDamage(damagePerSecond);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(tickRate);
        canDamage = true;
    }
}
