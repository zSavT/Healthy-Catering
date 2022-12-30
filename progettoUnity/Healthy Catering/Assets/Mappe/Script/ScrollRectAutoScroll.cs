using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;
    private ControllerInput controllerInput;
    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;

    void OnEnable()
    {
        controllerInput = new ControllerInput();
        controllerInput.UI.Enable();
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        m_NextScrollPosition = Vector2.up;
        m_ScrollRect.normalizedPosition = Vector2.up;
    }

    private void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        ScrollToSelected(false);
    }

    private void OnDisable()
    {
        controllerInput.UI.Disable();
    }

    private void OnDestroy()
    {
        controllerInput.Disable();
    }

    void Update()
    {
        // Scroll via input.

        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        InputScroll();
        if (!mouseOver)
        {
            // Lerp scrolling code.
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            //m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
    }
    void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            //remove the rePlayer getaxis calls is you aren't using Rewired
            //if it still doesn't work, check your input manager settings's axes and make sure they are defined properly
            //if you're using the new input system, this is also probably where you should replace the calls to the old one
            if (controllerInput.UI.Navigate.WasPerformedThisFrame())
            {
                ScrollToSelected(false);
            }
        }
    }
    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        Selectable selectedElement;

        
        selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

    
           


        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}