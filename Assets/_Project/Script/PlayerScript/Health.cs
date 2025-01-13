using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float _currentHealth;
    public EntityType entityType;

    public enum EntityType
    { 
        Enemy,
        Object,
        Player
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void UpdateHealth(float amount)
    {
        _currentHealth += amount;
        //Debug.Log($"{gameObject.name} took {amount} damage, {_currentHealth} health remaining.");

        if (_currentHealth <= 0)
        {
            if (entityType == EntityType.Player) return;
            Die();
        }

    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
