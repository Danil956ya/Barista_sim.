using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;  
    [SerializeField] private GameObject @object;
    [SerializeField] private ItemActivity _pickup;
    [SerializeField] private Animator _animator;
    [SerializeField] public bool isPickuped;
    [SerializeField] public bool isAnimated;
    [SerializeField] public static bool isAction;
    [SerializeField] public int idItem;
    [SerializeField] public bool IsLooking;


    void Awake()
    {
       
        _animator = gameObject.GetComponent<Animator>();
        _pickup = FindObjectOfType<ItemActivity>();
        _name = this.gameObject.name;
        @object = this.gameObject;
    }

    void OnMouseExit()
    {
        IsLooking = false;
    }
    private void Update()
    {
        isAnimated = _animator.enabled;
        isAction = _pickup.IsPlacing;
        GetComponent<Outline>().enabled = IsLooking;

        if ((Input.GetKeyDown(KeyCode.G) && isPickuped))
        {
            _pickup.DropItem(@object);
            this.isPickuped = false;
        }
        if ((Input.GetKeyDown(KeyCode.E) && isPickuped && !isAnimated))
        {
            _pickup.IsPlacing = true;
            this.isPickuped = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "ghost_item")
        {
            _pickup.IsBlocking = true;
        }
        Debug.Log(other + "Stay on " + _name);
    }
    private void OnTriggerExit(Collider other)
    {
        _pickup.IsBlocking = false;
    }

}
