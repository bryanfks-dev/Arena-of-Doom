using UnityEngine;

[SerializeField]
public class DontDestroyObjects
{
    public static GameObject Player;
    public static GameObject TransitionManager;

    public static void SelectWeaponSpawn()
    {
        Player.transform.localScale = new Vector3(1, 1, 1);
        Player.transform.position = new Vector3(-0.59f, 0.467f, 1.68f);

        Player.SetActive(false);
        Player.SetActive(true);
    }

    public static void ArenaSpawn()
    {
        Player.transform.localScale = Vector3.one * 6;
        Player.transform.position = new Vector3(-0.59f, 21.7f, 1.68f);

        Player.SetActive(false);
        Player.SetActive(true);
    }
}
