using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Toggle _checkBoxSelectAll;
    [SerializeField] private Toggle _checkBoxHideWindowContent;
    [SerializeField] private GameObject _elementPrefab;
    [SerializeField] private GameObject _alternateSwitchPrefab;
    [SerializeField] private Transform _transformPanelElement;
    [SerializeField] private Transform _transformPanelAlternateSwitch;
    private List<Element> Elements = new List<Element>();
    private List<AlternateSwitch> AlternateSwitches = new List<AlternateSwitch>();
    private bool _isActiveElementsAll = true;

    private void Start()
    {
        foreach (Element element in Resources.FindObjectsOfTypeAll(typeof(Element)))
        {
            if (element.gameObject.activeInHierarchy)
                Elements.Add(element);
        }
        foreach (AlternateSwitch _switch in Resources.FindObjectsOfTypeAll(typeof(AlternateSwitch)))
        {
            if (_switch.gameObject.activeInHierarchy)
                AlternateSwitches.Add(_switch);
        }
    }

    public void HideWindowContent(GameObject panel)
    {
        if (_checkBoxHideWindowContent.isOn)
            panel.SetActive(false);
        else
            panel.SetActive(true);
    }

    public void SelectAll()
    {
        if (_checkBoxSelectAll.isOn)
            for (int i = 0; i < Elements.Count; i++)
            { 
                if(Elements[i].gameObject.activeSelf)
                Elements[i].GetComponentInChildren(typeof(Toggle)).gameObject.GetComponent<Toggle>().isOn = true;
            }
        else
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].GetComponentInChildren(typeof(Toggle)).gameObject.GetComponent<Toggle>().isOn = false;
    }

    public void AddElement()
    {
        var newElement = Instantiate(_elementPrefab, _transformPanelElement);
        Elements.Add(newElement.GetComponent<Element>());
        newElement.GetComponent<Element>().numElement = Elements.Count - 1;
        newElement.GetComponentInChildren(typeof(Button)).gameObject.GetComponent<Button>()
            .onClick.AddListener(delegate { SetActiveElement(newElement); });

        var newAlternateSwitch = Instantiate(_alternateSwitchPrefab, _transformPanelAlternateSwitch);
        AlternateSwitches.Add(newAlternateSwitch.GetComponent<AlternateSwitch>());
        newAlternateSwitch.GetComponent<AlternateSwitch>().numAlternateSwitches = AlternateSwitches.Count - 1;
        newAlternateSwitch.GetComponent<Button>().onClick.AddListener(delegate { AlternateSwitchButton(newAlternateSwitch); });
        _checkBoxSelectAll.isOn = false;
    }

    public void DeleteElement()
    {
        foreach (Element element in Resources.FindObjectsOfTypeAll(typeof(Element)))
        {
            if (element.GetComponentInChildren(typeof(Toggle)).gameObject.GetComponent<Toggle>().isOn)
            {
                foreach (AlternateSwitch _switch in Resources.FindObjectsOfTypeAll(typeof(AlternateSwitch)))
                {
                    if (element.numElement == _switch.numAlternateSwitches)
                    {
                        AlternateSwitches.Remove(_switch);
                        Destroy(_switch.gameObject);
                    }
                }
                Elements.Remove(element);
                Destroy(element.gameObject);
            }
        }
        RecalculationNumber();
        _checkBoxSelectAll.isOn = false;
    }

    private void RecalculationNumber()
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            Elements[i].numElement = i;
            AlternateSwitches[i].numAlternateSwitches = i;
        }
    }

    public void AlternateSwitchButton(GameObject buttonObject)
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            if (buttonObject.GetComponent<AlternateSwitch>().numAlternateSwitches == Elements[i].numElement)
                Elements[i].GetComponentInChildren(typeof(Toggle)).gameObject.GetComponent<Toggle>().isOn = true;
            else
                Elements[i].GetComponentInChildren(typeof(Toggle)).gameObject.GetComponent<Toggle>().isOn = false;
        }
    }

    public void SetActiveElement(GameObject element)
    {
        for (int i = 0; i < AlternateSwitches.Count; i++)
        {
            if(element.GetComponent<Element>().numElement == AlternateSwitches[i].numAlternateSwitches)
            {
                AlternateSwitches[i].gameObject.SetActive(false);
            }
        }
        element.gameObject.SetActive(false);
    }
    public void SetActiveElementsAll()
    {
        if (_isActiveElementsAll)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].gameObject.SetActive(false);
                AlternateSwitches[i].gameObject.SetActive(false);
            }
            _isActiveElementsAll = false;
        }
        else
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].gameObject.SetActive(true);
                AlternateSwitches[i].gameObject.SetActive(true);
            }
            _isActiveElementsAll = true;
        }
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
