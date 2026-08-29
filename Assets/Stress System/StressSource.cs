using UnityEngine;

[System.Serializable]
public struct StressSource
{
    public float amount;
    public GameObject gameObjectReference;//optional game object to reference for this specific stress source?
}
