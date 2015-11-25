using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class TextRecolour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
    }
}
