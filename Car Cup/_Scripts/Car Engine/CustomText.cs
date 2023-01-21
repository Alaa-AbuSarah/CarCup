using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarCup
{
    [RequireComponent(typeof(Text))]
    public class CustomText : MonoBehaviour
    {
        private Text _text;
        private void Awake() => _text = GetComponent<Text>();

        public void UpdatText(string text) => _text.text = text;
    }
}