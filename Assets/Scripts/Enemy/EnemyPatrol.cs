using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrol : MonoBehaviour {
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Patrol")]
    public float speed = 3f;

    [Header("Knock-back")]
    public float defaultKbDuration = .35f;

    enum State { Patrol, Knockback }
    State _state = State.Patrol;

    Rigidbody _rb;
    int _currentWp;

    void Awake() => _rb = GetComponent<Rigidbody>();

    void FixedUpdate() {
        switch (_state) {
            case State.Patrol: Patrol(); break;
            case State.Knockback: /* do nothing */ break;
        }
    }

    void Patrol() {
        Vector3 target = waypoints[_currentWp].position;
        Vector3 dir = (target - transform.position).normalized;

        _rb.MovePosition(_rb.position + dir * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, target) < .2f)
            _currentWp = (_currentWp + 1) % waypoints.Length;
    }

    /// <summary>Call this from EnemyDamage when the player hits the enemy.</summary>
    public void ApplyKnockback() {
        if (knockRoutine != null) StopCoroutine(knockRoutine);
        knockRoutine = StartCoroutine(KnockbackRoutine(defaultKbDuration));
    }

    Coroutine knockRoutine;
    IEnumerator KnockbackRoutine(float time) {
        _state = State.Knockback;

        yield return new WaitForSeconds(time);

        // When the effect is over, resume patrol from the *nearest* waypoint.
        _currentWp = FindNearestWaypointIndex();
        _state = State.Patrol;
    }

    int FindNearestWaypointIndex() {
        int idx = 0;
        float best = Mathf.Infinity;
        for (int i = 0; i < waypoints.Length; ++i) {
            float d = (transform.position - waypoints[i].position).sqrMagnitude;
            if (d < best) { best = d; idx = i; }
        }
        return idx;
    }
}
