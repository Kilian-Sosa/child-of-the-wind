using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // Reference to the player's transform (assign in the Inspector)
    public Transform player;

    // Optional vertical offset to fine-tune the camera's position relative to the player
    public float verticalOffset = 0f;

    // Smoothing factor for movement (adjust for faster or slower follow)
    public float smoothSpeed = 0.125f;

    void LateUpdate() {
        // Get the current camera position
        Vector3 currentPos = transform.position;

        // Smoothly interpolate the camera's X and Y position to match the player's position plus optional offset
        currentPos.x = Mathf.Lerp(transform.position.x, player.position.x, smoothSpeed);
        currentPos.y = Mathf.Lerp(transform.position.y, player.position.y + verticalOffset, smoothSpeed);

        // Update the camera position while keeping its original Z value
        transform.position = currentPos;
    }
}
