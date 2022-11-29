using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ScrollRect))]
public class ButtonSelectionController : MonoBehaviour
{
    private ControllerInput controllerInput;
    [SerializeField]
    private float m_lerpTime;
    private ScrollRect m_scrollRect;
    private Button[] m_buttons;
    private int m_index;
    private float m_verticalPosition;
    private bool m_up;
    private bool m_down;
    

    public void Start()
    {
        controllerInput = new ControllerInput();
        controllerInput.Enable();
        m_scrollRect = GetComponent<ScrollRect>();
        m_buttons = GetComponentsInChildren<Button>();
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
    }

    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    public void Update()
    {
        m_up = controllerInput.Player.Movimento.WasPressedThisFrame();
        m_down = controllerInput.Player.Movimento.WasPressedThisFrame();

        if (m_up ^ m_down)
        {
            if (m_up)
                m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Length - 1);
            else
                m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Length - 1);

            m_buttons[m_index].Select();
            m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
        }

        m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
    }
}