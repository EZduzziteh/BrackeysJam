using UnityEngine;

public class jimmy_face_swapper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] faces;
    [SerializeField] private DialogueCoreBundle[] coreDialogues;
    [SerializeField] public int selectedFace = 0;

    private void Start()
    {
        //selectedFace = Random.Range(0, 2);
        //swapSprite();
    }
    public void swapSprite()
    {
        for (int i=0; i<faces.Length; i++)
        {
            if (i == selectedFace)
                faces[i].enabled = true;
            else
                faces[i].enabled = false;
        }
    }
    public void selectNewFace()
    {
        if (selectedFace >= faces.Length-1)
            selectedFace = 0;
        else
        {
            selectedFace++;
        }
        swapSprite();
    }
    public DialogueCoreBundle getDialogueBundle() 
    {
        return coreDialogues[selectedFace];
    }
    private void OnValidate()
    {
        swapSprite();
    }
}
