using UnityEngine;

/// <summary>
/// Classe per la gestione del modello 3D da visualizzare nella scelta/creazione profilo.<para>
/// <strong>Da aggiungere a:</strong><br></br>
/// Contenitore dei modelli 3D.
/// </para>
/// </summary>
public class OpzioniModello3D : MonoBehaviour
{
    private float r;
    private float target;
    private float angolo = 180;
    private GameObject modelloMaschile;
    private GameObject modelloFemminile;
    private bool puoGirare = true;
    [Header("Texture pelle modello 3D")]
    //EVENTUALMENTE POSSONO ESSERE TOLTI SE CARICHIAMO LE TEXTURE DALLA CARTELLA RISORSE
    [SerializeField] private Material textureBianco;
    [SerializeField] private Material textureNero;
    [SerializeField] private Material textureMulatto;

    private void Start()
    {
        angolo = 180;
        puoGirare = true;
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
    }



    private void Update()
    {
        if (puoGirare)
        {
            if (modelloMaschile.activeSelf && !modelloFemminile.activeSelf)
            {
                angolo = Mathf.SmoothDampAngle(modelloMaschile.transform.eulerAngles.y, target, ref r, 0.1f);
                modelloMaschile.transform.rotation = Quaternion.Euler(0, angolo, 0);
                if (target >= 360)
                    target = 0;
                target += 0.08f;
            }
            else if (!modelloMaschile.activeSelf && modelloFemminile.activeSelf)
            {
                angolo = Mathf.SmoothDampAngle(modelloFemminile.transform.eulerAngles.y, target, ref r, 0.1f);
                modelloFemminile.transform.rotation = Quaternion.Euler(0, angolo, 0);
                if (target >= 360)
                    target = 0;
                target += 0.08f;
            }
        }
    }


    /// <summary>
    /// Il metodo blocca o sblocca il modello 3D del giocatore
    /// </summary>
    public void switchMovimento()
    {
        puoGirare = !puoGirare;
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
    /// <param name="scelta">int scelta colore pelle modello<strong>0: Caucasico<br>1: Asiatico</br><br>2: Afro</br></strong></param>
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

    /// <summary>
    /// Il metodo restituisce il modello 3D attivo
    /// </summary>
    /// <returns>GameObject modello attivo</returns>
    private GameObject getModelloAttivo()
    {
        if (modelloFemminile.activeSelf && !modelloMaschile.activeSelf)
            return modelloFemminile;
        else
            return modelloMaschile;
    }

    /// <summary>
    /// Il metodo attiva il modello maschile e disattiva quello femminile ed aggiorna l'angolo
    /// </summary>
    private void attivaModelloMaschile()
    {
        modelloFemminile.SetActive(false);
        modelloMaschile.transform.rotation = Quaternion.Euler(0, target, 0);
        modelloMaschile.SetActive(true);
    }

    /// <summary>
    /// Il metodo attiva il modello femminile e disattiva il modello maschile ed aggiorna l'angolo
    /// </summary>
    private void attivaModelloFemminile()
    {
        modelloFemminile.SetActive(true);
        modelloFemminile.transform.rotation = Quaternion.Euler(0, target, 0);
        modelloMaschile.SetActive(false);
    }

    /// <summary>
    /// Il metodo attiva il modello corrispondente all'indice scelto passato in input
    /// </summary>
    /// <param name="valoreScelta">int indice genere<br>0: Modello Maschile</strong><br></br><strong>1: Modello Femminile</strong></br></param>
    public void attivaGenereModello(int valoreScelta)
    {
        if(valoreScelta == 0)
            attivaModelloMaschile();
        else if (valoreScelta == 1)
            attivaModelloFemminile();
    }

    /// <summary>
    /// Il metodo permette di muovere il modello 3D con l'angolazione passata in input
    /// </summary>
    /// <param name="angoloTarget">float angolo di movimento</param>
    public void rotazioneModello3D(float angoloTarget)
    {
        getModelloAttivo().transform.rotation = Quaternion.Euler(0, target+angoloTarget, 0);
        if (target >= 360)
            target = 0;
        target += angoloTarget;
    }
}
