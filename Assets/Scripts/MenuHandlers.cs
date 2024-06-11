using MikeNspired.UnityXRHandPoser;
using UnityEngine;

public class MenuHandlers : MonoBehaviour
{
    public GameObject Menus;
    public GameObject CreditContent;
    public TransitionManager TransitionManager;
    public GameObject Player;
    public GameObject Origin;
    public Transform PlayerInventory;

    public GameManager GameManager;

    [Header("Debugging Purposes")]
    public GameObject StartingWeapon = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.enabled = false;
    }

    public void PlayHandler()
    {
        // Check if user has weapon inside inventory
        for (int i = 0; i < PlayerInventory.childCount; i++)
        {
            InventorySlot Slot = PlayerInventory.GetChild(i).GetComponent<InventorySlot>();
        
            if (Slot.CurrentSlotItem != null)
            {
                GameManager.IsReady = true;
                break;
            }
        }

        if (GameManager.IsReady)
        {
            Player.transform.position = new Vector3(81.18f, 1.76f, -11.16f);
            Origin.transform.position = new Vector3(81.18f, 1.76f, -11.16f);

            // Hide menus
            gameObject.SetActive(!GameManager.IsReady);

            GameManager.enabled = true;
        }
    }

    private void _ToggleActiveCredit()
    {
        Menus.SetActive(!Menus.activeSelf);
        CreditContent.SetActive(!Menus.activeSelf);
    }

    public void CreditHandler()
    {
        // Methodo to handle credit button in menu
        _ToggleActiveCredit();
    }

    public void CreditBackBtnHandler()
    {
        // Method to handle back button in credit page
        _ToggleActiveCredit();
    }

    public void ExitHandler()
    {
        // Method to handle exit button in menu
        Application.Quit();
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
