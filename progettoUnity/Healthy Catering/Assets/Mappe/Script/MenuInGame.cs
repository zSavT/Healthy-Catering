using UnityEngine;
using UnityEngine.Events;

public class MenuInGame : MonoBehaviour
{
    [Header("Menu Opzioni")]
    public KeyCode tastoMenu;

    private bool giocoInPausa = false;
    [SerializeField] public GameObject menuPausa;
    public UnityEvent aperturaMenuGioco;
    public UnityEvent chiusuraMenuGioco;


    // Start is called before the first frame update
    void Start()
    {
        giocoInPausa = false;
        menuPausa.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkTastoMenu();
    }

    private void checkTastoMenu()
    {
        if(Input.GetKeyDown(tastoMenu))
        {
            if (!Interactor.pannelloAperto)
            {
                if (giocoInPausa)
                {
                    resumeGame();
                    PuntatoreMouse.disabilitaCursore();
                }
                else
                {
                    pauseGame();
                    PuntatoreMouse.abilitaCursore();
                }
            }
        }
    }

    void resumeGame()
    {
        chiusuraMenuGioco.Invoke();
        menuPausa.SetActive(false);
        Time.timeScale = 1f; //sblocca il tempo
        giocoInPausa = false;
    }
    void pauseGame()
    {
        aperturaMenuGioco.Invoke();
        menuPausa.SetActive(true);
        Time.timeScale = 0f; //blocca il tempo
        giocoInPausa = true;
    }

}
