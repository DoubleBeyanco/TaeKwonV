using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TootipInput : MonoBehaviour
{
    private TextMeshProUGUI Text = null;
    [SerializeField] private string InputTooltip;

    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Text.text = InputTooltip;
    }
}
