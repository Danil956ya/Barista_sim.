using UnityEngine;
using UnityEngine.Animations;

public class ItemActivity : MonoBehaviour
{
    [SerializeField] public  Inventory Inventory;
    [SerializeField] private Material _alfa_geen, _alfa_red;
    [SerializeField] private Transform _target_for_item;
    [SerializeField] private Collider _ghostCollider;
    public LayerMask raycastLayers;
    public bool IsBlocking;
    public bool IsPlacing;
    public bool CanAction;
    private GameObject _ghostObject;
    private float _rotationPerSecond = 5000;
    Vector3 newPosition, normalOfHit;


    public void PickUp(GameObject @object, int id)
    {   
        @object.transform.parent = _target_for_item.transform;
        @object.GetComponent<BoxCollider>().enabled = false;
        @object.GetComponent<ParentConstraint>().constraintActive = true;
        Inventory.id_item(id);
    }
    public void DropItem(GameObject @object)
    {
        @object.GetComponent<BoxCollider>().enabled = true;
        @object.transform.parent = null;
        @object.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
        Inventory.id_item(0);
    }
    public void PutItem(GameObject @object, bool enabled)
    {
        if(enabled)
        {
            @object.transform.position = newPosition;
            @object.transform.rotation = _ghostObject.transform.rotation;
            @object.transform.parent = null;
            @object.GetComponent<Item>().isPickuped = false;
            @object.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
            @object.GetComponent<BoxCollider>().enabled = true;
            @object.SetActive(true);
            Inventory.id_item(0);
        }    
    }
    private void Update()
    {
        switch(IsPlacing)
        {
            case true:

                if(_ghostObject == null)
                {
                    _ghostObject = Instantiate(Inventory.InHand) as GameObject;
                }
                DestroyImmediate(_ghostObject.GetComponent<Rigidbody>());
                DestroyImmediate(_ghostObject.GetComponent<Item>());

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                _ghostObject.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                _ghostObject.GetComponent<BoxCollider>().enabled = true;
                _ghostObject.GetComponent<BoxCollider>().isTrigger = true;
                _ghostObject.tag = "ghost_item";

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayers))
                {
                    normalOfHit = hit.normal;
                    normalOfHit = hit.transform.TransformDirection(normalOfHit);                  
                    if (normalOfHit == hit.transform.up)
                    {
                        Inventory.InHand.SetActive(false);
                        _ghostObject.transform.position = hit.point;

                        if (Input.GetMouseButtonDown(0) && !IsBlocking)
                        {
                            newPosition = hit.point;
                            IsPlacing = false;
                            PutItem(Inventory.InHand, true);
                            Destroy(_ghostObject);
                        }

                        float Zoom = Input.GetAxis("Mouse ScrollWheel");
                        if(Input.GetAxis("Mouse ScrollWheel") >= 0)
                        {
                            Debug.Log("Zoom " + Zoom);
                            _ghostObject.transform.Rotate(new Vector3(0,0 + _rotationPerSecond, 0) * Time.deltaTime);
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") <= 0)
                        {
                            _ghostObject.transform.Rotate(new Vector3(0, 0 - _rotationPerSecond, 0) * Time.deltaTime);
                        }

                        if(IsBlocking)
                        { GetAllChilds(false); }
                        else { GetAllChilds(true); }

                    }
                    else
                    {
                        GetAllChilds(false);
                    }

                }
           break;
        }

    }
    void GetAllChilds(bool enabled)
    {
        int children = _ghostObject.transform.childCount;
        if(enabled)
        {
            for (int i = 0; i < children; ++i)
            {
                _ghostObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = _alfa_geen;
            }
        }
        else
        {
            for (int i = 0; i < children; ++i)
            {
                _ghostObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = _alfa_red;
            }
        }
    }
}
