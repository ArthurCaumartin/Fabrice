using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class LineFollow : MonoBehaviour
{
    [SerializeField] private float _lenght = 5;
    [SerializeField, Range(0, 1)] private float _inputMag = 1;
    private LineRenderer _line;
    private Player _player;

    private void Start()
    {
        _player = transform.parent.GetComponent<PlayerManager>().player; 
        _line = GetComponent<LineRenderer>();
    }

    private void OnValidate()
    {
        _line = GetComponent<LineRenderer>();
        UpdatePos(1);
    }

    public void Update()
    {
        if (!_line || _player == null) Start();
        _inputMag = new Vector2(_player.GetAxis("Horizontal"), _player.GetAxis("Vertical")).magnitude;
        UpdatePos(_inputMag);
    }

    private void UpdatePos(float input)
    {
        _line.enabled = input > 0;
        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, transform.position + (transform.forward * _lenght * input));
    }
}
