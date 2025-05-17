using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackForce = 20f;
    public float knockbackRadius = 5f;
    [Range(0f, 180f)]
    public float knockbackAngle = 90f;
    public Transform knockbackOrigin;
    public LayerMask enemyLayer;

    private PlayerInventory playerData;

    private void Start()
    {
        playerData = GetComponent<PlayerInventory>();
        if (knockbackOrigin == null)
            knockbackOrigin = this.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2) && playerData.HasMagic1())
        {
            PerformKnockback();
        }
    }

    void PerformKnockback()
    {
        Vector3 forward = transform.right;
        Collider[] hitColliders = Physics.OverlapSphere(knockbackOrigin.position, knockbackRadius, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            Debug.Log("Hit: " + hit.name);
            Vector3 directionToEnemy = (hit.transform.position - knockbackOrigin.position).normalized;
            float angle = Vector3.Angle(forward, directionToEnemy);

            if (angle <= knockbackAngle / 2f)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(directionToEnemy * knockbackForce, ForceMode.Impulse);
                }
            }
        }

        Debug.Log("Knockback ejecutado");
    }

    private void OnDrawGizmosSelected()
    {
        if (knockbackOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(knockbackOrigin.position, knockbackRadius);

            Vector3 forward = knockbackOrigin.right;

            Quaternion leftRotation = Quaternion.AngleAxis(-knockbackAngle / 2f, Vector3.up);
            Quaternion rightRotation = Quaternion.AngleAxis(knockbackAngle / 2f, Vector3.up);

            Vector3 leftDirection = leftRotation * forward;
            Vector3 rightDirection = rightRotation * forward;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(knockbackOrigin.position, knockbackOrigin.position + leftDirection * knockbackRadius);
            Gizmos.DrawLine(knockbackOrigin.position, knockbackOrigin.position + rightDirection * knockbackRadius);
        }
    }



}
