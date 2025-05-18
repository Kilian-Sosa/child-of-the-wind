using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VerticalMover : MonoBehaviour {
    [Header("Way-points (exactly 2, same X-Z)")]
    public Transform[] waypoints = new Transform[2];

    [Header("Patrol")]
    public float speed = 3f;              // m/s
    public float arrivalThreshold = .03f; // metres
    public float uprightSnapSpeed = 6f;   // deg per physics step
    public float centreSnapSpeed = 10f;  // units per second

    [Header("Knock-back")]
    public float defaultKbDuration = .35f;

    enum State { Patrol, Knockback }
    State _state = State.Patrol;

    Rigidbody _rb;
    int _current;          // index of the waypoint we are heading TO
    Vector3 _anchorXZ;         // the rail (x,z) we return to

    // ───────────────────────────────────────────────────────────────────
    #region Unity lifecycle
    void Awake() {
        _rb = GetComponent<Rigidbody>();

        if (waypoints == null || waypoints.Length != 2)
            Debug.LogError($"{name}: VerticalWaypointMover needs exactly 2 way-points.");

        // remember the column
        _anchorXZ = new Vector3(transform.position.x, 0f, transform.position.z);

        // start heading toward whichever point is farther away so we never 'stall'
        _current = (Vector3.SqrMagnitude(transform.position - waypoints[0].position) >
                    Vector3.SqrMagnitude(transform.position - waypoints[1].position)) ? 0 : 1;
    }

    void FixedUpdate() {
        if (_state != State.Patrol) return;

        // gravity OFF while rail-bound
        if (_rb.useGravity) _rb.useGravity = false;
        _rb.linearVelocity = Vector3.zero;        // avoid drift from previous physics

        Vector3 targetPos = waypoints[_current].position;
        Vector3 step = (targetPos - transform.position).normalized * speed * Time.fixedDeltaTime;

        _rb.MovePosition(transform.position + step);

        Quaternion targetRot =
            Quaternion.Euler(0f, 0f, 0f);
        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation,
                                                  targetRot,
                                                  uprightSnapSpeed));
        float verticalDist =
        Mathf.Abs(transform.position.y - targetPos.y);

        if (verticalDist <= arrivalThreshold) {
            // snap onto rail so drift never accumulates
            _rb.position = new Vector3(_anchorXZ.x,
                                       _rb.position.y,
                                       _anchorXZ.z);
            _current = 1 - _current;
        }
    }
    #endregion
    // ───────────────────────── Knock-back API ──────────────────────────

    public void ApplyKnockback() {
        if (kbRoutine != null) StopCoroutine(kbRoutine);
        kbRoutine = StartCoroutine(KnockbackRoutine(defaultKbDuration));
    }

    Coroutine kbRoutine;
    IEnumerator KnockbackRoutine(float time) {
        _state = State.Knockback;
        _rb.useGravity = true;                 // physics owns it for a bit

        yield return new WaitForSeconds(time);

        // snap X-Z back onto the column but keep current Y
        _rb.position = new Vector3(_anchorXZ.x, _rb.position.y, _anchorXZ.z);
        _rb.linearVelocity = Vector3.zero;            // clear any leftover momentum
        _rb.useGravity = false;

        // pick the closer of the two way-points to resume from
        _current = (Vector3.SqrMagnitude(transform.position - waypoints[0].position) <
                    Vector3.SqrMagnitude(transform.position - waypoints[1].position)) ? 0 : 1;

        _state = State.Patrol;
    }
}