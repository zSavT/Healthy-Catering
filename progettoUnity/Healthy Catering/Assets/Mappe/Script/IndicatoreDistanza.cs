using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatoreDistanza : MonoBehaviour
{
    [SerializeField] private GameObject pannelloWayPoint;

    // Indicator icon
    [SerializeField] private Image img;
    // The target (location, enemy, etc..)
    private Transform target = null;

    [SerializeField] Transform posizioneMovimento;
    [SerializeField] Transform posizioneZio;
    [SerializeField] Transform posizioneRistorante;
    [SerializeField] Transform posizioneNegozio;

    // UI Text to display the distance
    [SerializeField] private TextMeshProUGUI meter;
    // To adjust the position of the icon
    public Vector3 offset;

    [SerializeField] private MenuInGame menuInGame;
    private Transform ultimoTarget = null;
    private bool checkMenuInGameAperto = false;

    private void Start()
    {
        disattivaWayPoint();
    }

    private void Update()
    {
        //gestione menu in game
        if (menuInGame.getMenuInGameAperto())
        {
            setUltimoTarget();
            setTarget("reset");
            checkMenuInGameAperto = true;
        }
        if (checkMenuInGameAperto)
        {
            if (!menuInGame.getMenuInGameAperto())
            {
                setTarget(getUltimoTarget());
                checkMenuInGameAperto = false;
            }
        }
        
        //gestione waypoint
        if (target != null)
        {
            // Giving limits to the icon so it sticks on the screen
            // Below calculations witht the assumption that the icon anchor point is in the middle
            // Minimum X position: half of the icon width
            float minX = img.GetPixelAdjustedRect().width / 2;
            // Maximum X position: screen width - half of the icon width
            float maxX = Screen.width - minX;

            // Minimum Y position: half of the height
            float minY = img.GetPixelAdjustedRect().height / 2;
            // Maximum Y position: screen height - half of the icon height
            float maxY = Screen.height - minY;

            // Temporary variable to store the converted position from 3D world point to 2D screen point
            Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

            // Check if the target is behind us, to only show the icon once the target is in front
            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if (pos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    pos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    pos.x = minX;
                }
            }

            // Limit the X and Y positions
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            // Update the marker's position
            img.transform.position = pos;
            // Change the meter text to the distance with the meter unit 'm'
            meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
        }
    }

    public void setTarget(string cosa)
    {
        attivaWayPoint();
        if (cosa.Equals("zio"))
        {
            target = posizioneZio;
        }
        else if (cosa.Equals("ristorante"))
        {
            target = posizioneRistorante;
        }
        else if (cosa.Equals("negozio"))
        {
            target = posizioneNegozio;
        }
        else if (cosa.Equals("Cono"))
        {
            target = posizioneMovimento;
        }
        else
        {
            target = null;
            disattivaWayPoint();
        }
    }

    /// <summary>
    /// Il metodo permette di impostare la scale del wayPoint
    /// </summary>
    /// <param name="size">Vector3 size da impostare</param>
    public void impostaSizeWayPoint(Vector3 size)
    {
        pannelloWayPoint.transform.localScale = size;
    }

    public void setTarget(Transform newTarget)
    {
        if (newTarget == null)
        {
            target = null;
            disattivaWayPoint();
        }
        else
        {
            attivaWayPoint();
            target = newTarget;
        }
    }

    public void setUltimoTarget()
    {
        ultimoTarget = target;
    }

    public Transform getUltimoTarget()
    {
        return ultimoTarget;
    }

    private void attivaWayPoint()
    {
        pannelloWayPoint.SetActive(true);
    }

    private void disattivaWayPoint()
    {
        pannelloWayPoint.SetActive(false);
    }
}