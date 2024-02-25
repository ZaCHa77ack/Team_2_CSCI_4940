using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
    }
    //public float CurrentHealth = 0.5f;

    private VisualElement m_HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);
        //healthBar.style.width = Length.Percent(CurrentHealth * 100.0f);
    }

    public void SetHealthValue(float percentage)
    {
        m_HealthBar.style.width = Length.Percent(100 * percentage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
