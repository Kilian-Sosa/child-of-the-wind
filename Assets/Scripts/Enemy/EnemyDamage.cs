using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int life = 350;
    [SerializeField] int damage = 1;
    public ParticleSystem damageParticles;

    public void Start()
    {
        int playerStrength =  GameManager.instance.GetStrengthLevel();
        Debug.Log("Player Strength Level: " + playerStrength);

        if (playerStrength == 1)
        {
            damage = 50;
        }
        else if (playerStrength == 2)
        {
            damage = 75;
        }
    }

    public void TakeDamage()
    {

        life -= damage;
        if (life <= 0)
        {
            GameManager.instance.AddXP(300);
            Destroy(gameObject);

        }
        else
        {
            Debug.Log("Enemy took damage: " + damage + ", remaining life: " + life);
            if (damageParticles != null)
            {
                damageParticles.Play();
            }
        }
    }
}
