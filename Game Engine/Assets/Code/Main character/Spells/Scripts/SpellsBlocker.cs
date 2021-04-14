using UnityEngine;

public class SpellsBlocker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<MainCharacterSpells>() != null) other.GetComponent<MainCharacterSpells>().BlockSpells();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<MainCharacterSpells>() != null) other.GetComponent<MainCharacterSpells>().UnblockSpells();
    }
}