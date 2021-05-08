using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPet : MonoBehaviour
{
    public static int PetTimes = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerInteraction") && Input.GetButton("R")) other.transform.parent.GetComponent<MainCharacter>().Pet();
    }
}
