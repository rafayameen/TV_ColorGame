using QuickOutline;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;


public class ObjectHighlightScript : MonoBehaviour
{
    // public Transform imageTransform;   // Assign the transform of the canvas image here
    public LayerMask layerMask;        // The layers to raycast against
    public float rayDistance = 50;

    private GameObject highlightedObject; // The currently highlighted object
    private Outline highlightedOutline;   // Outline component of the highlighted object

    // Define a dictionary to map tags to specific actions
    //private Dictionary<string, System.Action<GameObject>> tagActions;


    public Camera mainCam;

    private Rigidbody m_RB;

    public delegate void SimpleEvent();
    public static event SimpleEvent deliveryTruckClickedEvent;
    public static event SimpleEvent deliveryBoxClickedEvent;


    public delegate void CustomEvent(object obj, bool enable);

    public static event CustomEvent doorLockTriggeredEvent;
    public static event CustomEvent placedItemTriggeredEvent;
    public static event CustomEvent roomdoubleIncomTriggered;
    public static event CustomEvent bankTruckTriggered;

    // Cache of GameObject to its Outline components
    private Dictionary<GameObject, List<Outline>> outlineCache = new Dictionary<GameObject, List<Outline>>();

    // Define the tags you want to check and their corresponding actions
    public Dictionary<string, System.Action<GameObject>> tagActions = new Dictionary<string, System.Action<GameObject>>();


    private void OnEnable()
    {
       

        EventManager.interactButtonClicked += HandleClick;
        //EventManager.TouchpadClicked += HandlePickOrThrow;


        //DeliveryBoxScript.OnBoxOpened += HandleBoxOpened;



    }

    private void OnDisable()
    {

        EventManager.interactButtonClicked -= HandleClick;
        //EventManager.TouchpadClicked -= HandlePickOrThrow;
        //DeliveryBoxScript.OnBoxOpened -= HandleBoxOpened;

        //EventManager.checkin_PC_Clicked -= HandlePC_Clicked;
        //EventManager.led_Clicked -= HandleLED_Clicked;
    }

    //private void HandleBoxOpened(DeliveryBoxScript box, ItemInfo itemInfo)
    //{
    //    ResetPickedObject();
    //}

    //private void HandleLED_Clicked()
    //{
    //    Debug.Log("LED_Clicked");

    //    //posBeforeCheckin = transform.position;
    //    //rotationBeforeCheckin = transform.rotation;

    //    EnableMovement(false);

    //    mainCam.transform.localEulerAngles = Vector3.zero;

    //    transform.DOMove(shoppingPCPostion.position, 0.5f).OnComplete(() =>
    //    {
    //        transform.DORotateQuaternion(shoppingPCPostion.rotation, 0.5f)
    //     .OnComplete(() =>
    //     {
    //         //isPlayerAtShoppingPC = true;


    //         GameManager.instance.pcPanelView = PCViewType.HOME;
    //         ViewManager.Show(Views.PC_Panel, null, false, true);
    //     });
    //    });


    //}


    private void HandleClick()
    {
        RaycastFromImage();

        if (highlightedObject != null)
        {
            // Handle the click if the object is highlighted
            HandleClickOnHighlightedObject();
        }
    }

    private void HandlePickOrThrow()
    {
        RaycastFromImage(); // Update the pickable object reference based on raycast

    }

    public void ResetPickedObject()
    {

    }
 

    private void Start()
    {


    }




    private void Update()
    {
        RaycastFromImage();

        if (Input.GetKeyDown(KeyCode.Return))
            HandleClick();
    }



    //[SerializeField] private Transform pickHolder; // Where the picked object will attach
    //[SerializeField] private float throwForce = 10f;
    //private PickableObject pickedObject;

    //bool isPicked = false;

    //PickableObject pickable;
    private void RaycastFromImage()
    {
        // Create a ray starting from the camera's position, going forward
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);


        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            //pickable = hit.collider.GetComponent<PickableObject>();


            IHighlightable highlightable = hit.collider.GetComponent<IHighlightable>();

            if (highlightable != null)
            {
                HighlightableObject hObj = hitObject.GetComponent<HighlightableObject>();
                EventManager.DoFireObjectHighlighted(hObj);

                if (highlightedObject != hitObject)
                {
                    //RemoveHighlight();
                    HighlightObject(hitObject);
                }
            }
            else
            {
                RemoveHighlight();
            }
        }
        else
        {

            //pickable = null;
            RemoveHighlight();
        }


    }


    private void HighlightObject(GameObject obj)
    {
        // Skip if the object is already highlighted
        if (highlightedObject == obj) return;

        // Remove highlight from the previously highlighted object, if any
        RemoveHighlight();

        highlightedObject = obj;

        IHighlightable highlightable = highlightedObject.GetComponent<IHighlightable>();

        if (highlightable != null)
        {
            // Enable highlight
            highlightable.OnHighlight(true);

            Outline outline = highlightedObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }

    }

    private void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            IHighlightable highlightable = highlightedObject.GetComponent<IHighlightable>();

            if (highlightable != null)
            {
                // Disable highlight
                highlightable.OnHighlight(false);

                Outline outline = highlightedObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }

            highlightedObject = null; // Clear the reference to the previously highlighted object

        }
    }


    private void HandleClickOnHighlightedObject()
    {

        // Get the IHighlightable component from the highlighted object
        var highlightable = highlightedObject.GetComponent<IHighlightable>();
        if (highlightable != null)
        {
            highlightable.HandleClick();
        }
        else
        {
            Debug.LogWarning("Highlighted object does not implement IHighlightable: " + highlightedObject.name);
        }
    }



}
