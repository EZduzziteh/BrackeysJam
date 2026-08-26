using UnityEngine;

[CreateAssetMenu(fileName = "cursorPackage", menuName = "Scriptable Objects/cursorPackage")]
public class cursorPackage : ScriptableObject
{
    public Texture2D[] cursorFrames;
    public Vector2 hotSpot = Vector2.zero;
    public bool animated = false;
    public float animFrameRate = 0.1f; //only used if animated.
}
