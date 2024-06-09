using UnityEngine;

public class Player : MonoBehaviour
{
    public float HealthPoint = 100f;
    public TransitionManager TransitionManager;
    public GameObject GameOverCanvas;

    private bool _IsDead;

    // Start is called before the first frame update
    void Start()
    {
        if (DontDestroyObjects.Player != null)
        {
            Destroy(DontDestroyObjects.Player);
            Destroy(DontDestroyObjects.TransitionManager);
        }

        Reset();
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

                // Set attributes
                TransitionManager.FadeScreen.FadeColor = Color.red;
                TransitionManager.FadeScreen.FadeDuration = 5f;

                Invoke(nameof(ShowGameOverCanvas), 3f);

                TransitionManager.GoToScene(0);
            }
        }
    }

    private void ShowGameOverCanvas()
    {
        GameOverCanvas.SetActive(true);
    }

    private void Reset()
    {
        HealthPoint = 100f;
        _IsDead = false;
    }
}
