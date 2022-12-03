using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    private string messaggio;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.impostaAttivaToolTip(messaggio);
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.nascondiResettaToolTip();
    }

    public void setMessaggio(string messaggio)
    {
        this.messaggio = messaggio;
    }

}
