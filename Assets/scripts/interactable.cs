using UnityEngine;
using UnityEngine.Events;

public class interactable : MonoBehaviour
{
    public enum interactableEffectType { none, transition, radio }

    [Header("Functionality")]
    public interactableEffectType effectName =interactableEffectType.none;
    public UnityEvent transitionedTriggered;

    [Header("highlight settings")]
    SpriteRenderer sr; Color defaultColor;
    [SerializeField] private Color objectColor;

    [Header("Cursor settings")]
    [SerializeField] private cursorPackage defaultCursor;
    [SerializeField] private cursorPackage altCursor;
    public bool shouldHideCursorAfterUse=false;
    private float cursorTimer; private int cursorIndex; private bool doAnim=false;
    private car_interior_controller interiorController;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor= sr.color;
        interiorController=FindFirstObjectByType<car_interior_controller>();
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
        switch(effectName)
        {
            case interactableEffectType.transition:
                interiorController.startNewTransition(car_interior_controller.transitionType.full_blink, car_interior_controller.transitionGameEffect.moveScene);
                transitionedTriggered.Invoke();
                break;
            case interactableEffectType.radio:
                print("jamming jamming jamming, what's brackening party ppl?");
                GetComponent<SpriteStateSwapper>().spriteIndex++;
                break;
            case interactableEffectType.none:
                print("you interacted to no effect!");
                break;
        }
    }

    private GameObject myOutline;
    [SerializeField] private Color outlineColor=Color.yellow;
    [SerializeField] private float highlightThickness=1.1f;

    public void showOutline()
    {
        //color change
        Color outline = objectColor;
        //outline.a = sr.color.a;
        sr.color = outline;

        if (!myOutline)
        {
            //create highlight object
            myOutline = Instantiate(gameObject);
            myOutline.transform.position = gameObject.transform.position; // dumb but needed apparently?
            myOutline.name = "Highlight of " + gameObject;
            //remove components that break things
            Destroy(myOutline.GetComponent<interactable>());
            Destroy(myOutline.GetComponent<Collider2D>());

            //setup highlight options.
            myOutline.transform.localScale = myOutline.transform.localScale * highlightThickness;
            SpriteRenderer outlineSR = myOutline.GetComponent<SpriteRenderer>();
            outlineSR.sortingOrder -= 1;
            Color highlightColor = outlineColor;
            //highlightColor.a = outlineSR.color.a; //transfer alpha channel
            outlineSR.color = highlightColor;
            
        }
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
        //color change
        sr.color = defaultColor;

        //highlight cleanup
        if (myOutline)
            Destroy(myOutline);

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
