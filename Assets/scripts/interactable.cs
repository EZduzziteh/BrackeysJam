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
    public bool shouldHideCursorAfterUse=false;
    private float cursorTimer; private int cursorIndex;
    private bool doAnim=false;
    private car_interior_controller interiorController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        switch (effectName)
        {
            case "transition":
                interiorController.startNewTransition(car_interior_controller.transitionType.full_blink,car_interior_controller.transitionGameEffect.moveScene);
                break;
            case "stepPassenger":
                interiorController.startNewTransition(car_interior_controller.transitionType.wide_blink,car_interior_controller.transitionGameEffect.passengerCutscene);
                gameObject.SetActive(false);
                break;
            case "radio":
                print("jamming jamming jamming, what's brackening party ppl?");
                break;
            default:
                print("you interacted to no effect!");
                break;
        }
    }

    private GameObject myOutline;
    [SerializeField]
    private float highlightThickness=1.1f;

    public void showOutline()
    {
        //color change
        Color outline = Color.red;
        outline.a = sr.color.a;
        sr.color = outline;

        if (!myOutline)
        {
            //create highlight object
            myOutline = Instantiate(gameObject);
            myOutline.name = "Highlight of " + gameObject;
            //remove components that break things
            Destroy(myOutline.GetComponent<interactable>());
            Destroy(myOutline.GetComponent<Collider2D>());

            //setup highlight options.
            myOutline.transform.localScale = myOutline.transform.localScale * highlightThickness;
            SpriteRenderer outlineSR = myOutline.GetComponent<SpriteRenderer>();
            outlineSR.sortingOrder -= 1;
            Color highlightColor = Color.yellow;
            highlightColor.a = outlineSR.color.a; //transfer alpha channel
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
