using MikeNspired.UnityXRHandPoser;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float HealthPoint = 100f;
    public TransitionManager TransitionManager;
    public GameManager GameManager;
    public Transform Inventory;
    public MenuHandlers MainMenu;

    public Transform Head;
    public Transform Setup;
    public Transform SpawnPoint;

    private bool _IsDead;

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.FadeScreen.enabled = false;
        TransitionManager.FadeScreen.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HealthPoint -= other.gameObject.GetComponent<Enemy>().AtkDmg;

            if (HealthPoint <= 0 && !_IsDead)
            {
                _IsDead = true;

                // Set fade attributes
                TransitionManager.FadeScreen.FadeColor = Color.red;
                TransitionManager.FadeScreen.FadeDuration = 5f;

                Invoke(nameof(FadeIn), 4f);

                TransitionManager.FadeScreen.FadeOut();

                // Teleport to select weapon room
                Head.position = SpawnPoint.position;
                Setup.position = Vector3.up * -0.247f;
                gameObject.transform.localPosition = Vector3.zero;

                // Reset gamemanager
                GameManager.IsReady = false;

                GameManager.ResetArena();

                GameManager.enabled = false;

                MainMenu.Reset();

                Reset();
            }
        }
    }

    IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
    }

    private void FadeIn()
    {
        TransitionManager.FadeScreen.FadeIn();
    }

    private void Reset()
    {
        HealthPoint = 100f;
        _IsDead = false;

        // Reset inventory
        for (int i = 0; i < Inventory.childCount; i++)
        {
            InventorySlot Slot = Inventory.GetChild(i).GetComponent<InventorySlot>();

            if (Slot.CurrentSlotItem != null)
            {
                Destroy(Slot.CurrentSlotItem.gameObject);
            }
        }
    }
}
