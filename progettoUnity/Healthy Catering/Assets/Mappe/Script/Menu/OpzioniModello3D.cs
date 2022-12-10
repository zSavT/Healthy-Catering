using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpzioniModello3D : MonoBehaviour
{
    private float r;
    private float target;
    private float angolo = 180;
    private GameObject modelloMaschile;
    private GameObject modelloFemminile;

    private void Start()
    {
        angolo = 180;
        foreach (Transform temp in this.gameObject.GetComponentsInChildren<Transform>())
        {
            if (temp.gameObject.name == "Character_BusinessMan_Shirt_01")
                modelloMaschile = temp.gameObject;
            else if (temp.gameObject.name == "Character_Female_Jacket_01")
                modelloFemminile = temp.gameObject;
        }
        if (PlayerSettings.caricaGenereGiocatore(PlayerSettings.caricaNomePlayerGiocante()) == 0)
        {
            attivaModelloMaschile();
        }
        else if (PlayerSettings.caricaGenereGiocatore(PlayerSettings.caricaNomePlayerGiocante()) == 1)
        {
            attivaModelloFemminile();
        } else
        {
            if(PlayerSettings.caricaGenereModello3D(PlayerSettings.caricaNomePlayerGiocante()) == 0)
            {
                attivaModelloMaschile();
            } else
            {
                attivaModelloFemminile();
            }
        }
            

        Debug.Log(modelloMaschile);
        Debug.Log(modelloFemminile);
    }



    private void Update()
    {
        if (modelloMaschile.activeSelf && !modelloFemminile.activeSelf)
        {
            angolo = Mathf.SmoothDampAngle(modelloMaschile.transform.eulerAngles.y, target, ref r, 0.1f);
            modelloMaschile.transform.rotation = Quaternion.Euler(0, angolo, 0);
            if (target >= 360)
                target = 0;
            target += 0.08f;
        } else if (!modelloMaschile.activeSelf && modelloFemminile.activeSelf)
        {
            angolo = Mathf.SmoothDampAngle(modelloFemminile.transform.eulerAngles.y, target, ref r, 0.1f);
            modelloFemminile.transform.rotation = Quaternion.Euler(0, angolo, 0);
            if (target >= 360)
                target = 0;
            target += 0.08f;
        }
    }

    private void attivaModelloMaschile()
    {
        modelloFemminile.SetActive(false);
        modelloMaschile.SetActive(true);
    }

    private void attivaModelloFemminile()
    {
        modelloFemminile.SetActive(true);
        modelloMaschile.SetActive(false);
    }


    public void attivaGenereModello(int valoreScelta)
    {
        if(valoreScelta == 0)
            attivaModelloMaschile();
        else if (valoreScelta == 1)
            attivaModelloFemminile();
    }

    public void rotazioneModello3D(float angolo)
    {
        target = angolo;
    }
}
