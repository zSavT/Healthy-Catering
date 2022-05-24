using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Wilberforce;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent clickCrediti;             //serve per eliminare altri elementi in visualilzzazione
    [SerializeField] private Camera camera;
    private List<Player> player;

    void Start()
    {
        camera.GetComponent<Colorblind>().Type = PlayerSettings.caricaImpostazioniDaltonismo();
        letturaNomiUtenti();
        if (!presentePlayer())
        {
            caricaCreazioneProfilo();
        }
    }

    void Update()
    {
        attivaDisattivaLivelli();
    }


    private void letturaNomiUtenti()
    {
        player = Database.getDatabaseOggetto<Player>(new Player());
    }


    private bool presentePlayer()
    {
        if (player.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void caricaSelezioneModificaProfilo()
    {
        SceneManager.LoadScene(4);
    }

    public void caricaCreazioneProfilo()
    {
        SceneManager.LoadScene(5);
    }

    private void attivaDisattivaLivelli()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(PlayerSettings.caricaProgressoLivello1() == 0)
            {
                PlayerSettings.salvaProgressoLivello1(true);
                print("livello 1 attivato");
            } else
            {
                print("livello 1 disattivato");
                PlayerSettings.salvaProgressoLivello1(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PlayerSettings.caricaProgressoLivello2() == 0)
            {
                PlayerSettings.salvaProgressoLivello2(true);
                print("livello 2 attivato");
            }
            else
            {
                print("livello 2 disattivato");
                PlayerSettings.salvaProgressoLivello2(false);
            }
        }

    }

    public void menuPrincipale()
    {
        SceneManager.LoadScene(0);
    }

    public void crediti()
    {
        clickCrediti.Invoke();
    }

    public void menuSelezioneLivelli()
    {
        SceneManager.LoadScene(3);
    }

    public void menuOpzioni()
    {
        SceneManager.LoadScene(1);
    }
    public void chiudi()
    {
        Application.Quit();
    }
}
