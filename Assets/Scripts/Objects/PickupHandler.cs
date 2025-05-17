using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour {

    [SerializeField] GameObject _particles;
    [SerializeField] bool _isPowerUp;
    [SerializeField] string _powerUp;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            if (_isPowerUp) other.GetComponentInParent<PlayerInventory>().AddPowerUp(_powerUp);
            else other.GetComponentInParent<PlayerInventory>().AddItem();

            Destroy(gameObject);
        }
    }

    //void OnDestroy() => _particles.SetActive(true);
}
