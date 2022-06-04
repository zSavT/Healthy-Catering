using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InterazionePassanti : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnityNPCPassivi = 7;
    [SerializeField] private GameObject pannelloInterazionePassanti;
    [SerializeField] private TextMeshProUGUI testoInterazionePassanti;

    [SerializeField] private UnityEvent playerStop;
    [SerializeField] private UnityEvent playerRiprendiMovimento;

    private void Start()
    {
        pannelloInterazionePassanti.SetActive(false);
    }

    private void Update()
    {
        if (NPCPassivoPuntato())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                apriPannelloInterazionePassanti();
            }
        }
    }

    private bool NPCPassivoPuntato()
    {
        RaycastHit NPCPassivoInquadrato;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out NPCPassivoInquadrato, 3, layerUnityNPCPassivi))
        {
            return true;
        }
        print("ciao2");
        return false;
    }

    private void apriPannelloInterazionePassanti()
    { 
        pannelloInterazionePassanti.SetActive(true);
        testoInterazionePassanti.text = trovaScrittaDaMostrare();

    }

    private string trovaScrittaDaMostrare()
    {
        return "ciao";
    }
}
