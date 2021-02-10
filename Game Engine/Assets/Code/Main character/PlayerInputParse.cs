using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputParse", menuName = "PlayerInputParse", order = 0)]
public class PlayerInputParse : ScriptableObject
{
    [SerializeField] List<InputReference> inputReferences;

    public string ParseInput(string buttonName)
    {
        return inputReferences.Find(x => x.ButtonName == buttonName).InputInEngine;
    }
}

[System.Serializable]
public struct InputReference
{
    public string ButtonName;
    public string InputInEngine;
}