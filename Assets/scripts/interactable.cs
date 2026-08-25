using UnityEngine;

public class interactable : MonoBehaviour
{
    [Header("Functionality")]
    public string effectName;
    [Header("highlight settings")]
    SpriteRenderer sr; Color defaultColor;
    [Header("Cursor settings")]
    [SerializeField] private cursorPackage defaultCursor;
    [SerializeField] private cursorPackage altCursor;
    private float cursorTimer; private int cursorIndex;
    private bool doAnim=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor= sr.color;
    }
    private void Update()
    {
        if (doAnim)
        {
            cursorUpdate(altCursor);
        }
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
        if (altCursor)
        {
            if (altCursor.animated)
            {
                doAnim = true;
            }
            Cursor.SetCursor(altCursor.cursorFrames[0], altCursor.hotSpot, CursorMode.Auto);
        }
    }
    public void hideOutline(bool updateCursor = true)
    {
        sr.color = defaultColor;
        //update cursor.
        if (altCursor)
        { 
            if (altCursor.animated)
                {
                    doAnim = false;
                }
            Cursor.SetCursor(defaultCursor.cursorFrames[0], defaultCursor.hotSpot, CursorMode.Auto);
        }
    }
    private void cursorUpdate(cursorPackage cursor)
    {
        cursorTimer += Time.deltaTime;
        if (cursorTimer >= cursor.animFrameRate)
        {
            cursorTimer -= cursor.animFrameRate;
            cursorIndex = (cursorIndex + 1) % cursor.cursorFrames.Length;
        }
        Cursor.SetCursor(cursor.cursorFrames[cursorIndex], cursor.hotSpot, CursorMode.Auto);
    }
}
