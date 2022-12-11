using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpzioniModello3D : MonoBehaviour
{
    private float r;
    private float target;
    private float angolo = 180;
    private GameObject modelloMaschile;
    private GameObject modelloFemminile;
    [Header("Texture pelle modello 3D")]
    //EVENTUALMENTE POSSONO ESSERE TOLTI SE CARICHIAMO LE TEXTURE DALLA CARTELLA RISORSE
    [SerializeField] private Material textureBianco;
    [SerializeField] private Material textureNero;
    [SerializeField] private Material textureMulatto;

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

        setTexturePelle(getModelloAttivo());
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

    /// <summary>
    /// Metodo per impostare il colore della pelle del modello 3D del giocatore
    /// </summary>
    private void setTexturePelle(GameObject modello)
    {
        if (PlayerSettings.caricaColorePelle(PlayerSettings.caricaNomePlayerGiocante()) == 0)
        {
            modello.GetComponentInChildren<Renderer>().material = textureBianco;
        }
        else if (PlayerSettings.caricaColorePelle(PlayerSettings.caricaNomePlayerGiocante()) == 1)
        {
            modello.GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (PlayerSettings.caricaColorePelle(PlayerSettings.caricaNomePlayerGiocante()) == 2)
        {
            modello.GetComponentInChildren<Renderer>().material = textureMulatto;
        }
    }



    /// <summary>
    /// Metodo per impostare il colore della pelle del modello 3D del giocatore
    /// </summary>
    /// <param name="scelta">int scelta colore pelle modello</param>
    public void setTexturePelle(int scelta)
    {
        if (scelta == 0)
        {
            getModelloAttivo().GetComponentInChildren<Renderer>().material = textureBianco;
        }
        else if (scelta == 1)
        {
            getModelloAttivo().GetComponentInChildren<Renderer>().material = textureNero;
        }
        else if (scelta == 2)
        {
            getModelloAttivo().GetComponentInChildren<Renderer>().material = textureMulatto;
        }
    }

    private GameObject getModelloAttivo()
    {
        if (modelloFemminile.activeSelf && !modelloMaschile.activeSelf)
            return modelloFemminile;
        else
            return modelloMaschile;
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
