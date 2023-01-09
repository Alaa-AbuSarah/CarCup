using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CarCup
{
    [RequireComponent(typeof(Image))]
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool pressured = false;
        public CustomButtonType type = CustomButtonType.None;

        [Space]
        [SerializeField] private Color color = Color.red;

        private Image _image;
        private Color startColor = Color.white;

        private void Awake()
        {
            _image = GetComponent<Image>();
            startColor = _image.color;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressured = true;
            _image.color = color;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressured = false;
            _image.color = startColor;
        }
    }
}