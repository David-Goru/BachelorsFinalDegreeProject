using UnityEngine;

[System.Serializable]
public class MainCharacterData : SaveElement
{
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float zPosition;
    [SerializeField] private float yRotation;
    [SerializeField] private int currentHealth;

    public override SaveElement Save()
    {
        Name = "Main Character";
        Transform mainCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        xPosition = mainCharacter.position.x;
        yPosition = mainCharacter.position.y;
        zPosition = mainCharacter.position.z;
        yRotation = mainCharacter.eulerAngles.y;
        currentHealth = mainCharacter.GetComponent<MainCharacter>().CurrentHealth;

        return this;
    }

    public override bool Load()
    {
        try
        {
            GameObject mainCharacter = GameObject.FindGameObjectWithTag("Player");
            mainCharacter.transform.position = new Vector3(xPosition, yPosition, zPosition);
            mainCharacter.transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
            mainCharacter.GetComponent<MainCharacter>().Load(currentHealth);
        }
        catch (UnityException e)
        {
            // Add to text file?
            Debug.Log("Main character data error: " + e);

            return false;
        }

        return true;
    }
}