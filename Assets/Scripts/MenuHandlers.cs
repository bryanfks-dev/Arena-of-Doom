using MikeNspired.UnityXRHandPoser;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class MenuHandlers : MonoBehaviour
{
    public GameObject Menus;
    public GameObject CreditContent;
    public TransitionManager TransitionManager;
    public GameObject Player;
    public Transform PlayerInventory;

    [Header("Debugging Purposes")]
    public GameObject StartingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayHandler()
    {
        bool IsReady = false;

        for (int i = 0; i < PlayerInventory.childCount; i++)
        {
            InventorySlot Slot = PlayerInventory.GetChild(i).GetComponent<InventorySlot>();

            if (Slot.CurrentSlotItem != null)
            {
                IsReady = true;
            }
        }

        if (IsReady)
        {
            if (Player != null)
            {
                DontDestroyObjects.Player = Player;
                DontDestroyObjects.TransitionManager = TransitionManager.gameObject;

                DontDestroyOnLoad(Player);
                DontDestroyOnLoad(TransitionManager.gameObject);
            }

            // Hide menus
            gameObject.SetActive(false);

            // Change scene
            TransitionManager.GoToScene(1);
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
}
