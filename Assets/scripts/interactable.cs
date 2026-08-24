using UnityEngine;

public class interactable : MonoBehaviour
{
    [Header("Functionality")]
    public string effectName;
    [Header("highlight settings")]
    SpriteRenderer sr; Color defaultColor;
    [Header("Cursor settings")]
    [SerializeField] private Texture2D cursorTexture;[SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private Texture2D tempCursorTextureDefault;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor= sr.color;
    }
    public void triggerEffect()
    {
        switch (effectName)
        {
            case "transition":
                FindFirstObjectByType<car_interior_controller>().startNewTransition();
                return;
            case "radio":
                print("jamming jamming jamming, what's brackening party ppl?");
                return;
            default:
                print("you interacted to no effect!");
                return;
        }
    }
    public void showOutline()
    {
        Color outline = Color.red;
        outline.a = sr.color.a;
        sr.color = outline;
        //update cursor.
        if(cursorTexture)
            Cursor.SetCursor(cursorTexture, hotSpot,CursorMode.Auto);
    }
    public void hideOutline(bool updateCursor=true)
    {
        sr.color = defaultColor;
        //update cursor.
        if (cursorTexture)
            Cursor.SetCursor(tempCursorTextureDefault, hotSpot, CursorMode.Auto);

    }
}
