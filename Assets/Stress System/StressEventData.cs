using UnityEngine;

[CreateAssetMenu(fileName = "StressEventData", menuName = "Scriptable Objects/StressEventData")]
public class StressEventData : ScriptableObject
{
    public string stressEventID;
    public AudioClip soundEffect;
    public Sprite ScreenEffect;
}
