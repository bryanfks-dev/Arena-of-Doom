using UnityEngine;

public class Inventory
{
    private static string[] _WeaponsName = new string[2];


    public static void SaveWeaponsName(int SlotIndex, string WeaponName)
    {
        _WeaponsName[SlotIndex] = WeaponName;
    }

    public static GameObject[] LoadWeapons()
    {
        GameObject[] Weapons = new GameObject[2];

        for (int i = 0; i < _WeaponsName.Length; i++)
        {
            if (_WeaponsName[i] != "")
            {
                GameObject WeaponPrefab = (GameObject)Resources.Load("/Weapons/" + _WeaponsName[i], typeof(GameObject));

                Weapons[i] = WeaponPrefab;
            }
            else
            {
                Weapons[i] = null;
            }
        }

        return Weapons;
    }
}
