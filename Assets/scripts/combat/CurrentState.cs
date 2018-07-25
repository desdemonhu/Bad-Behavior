using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CurrentState : MonoBehaviour {

    private GameObject[] targets; 
    private GameObject _currentPlayer;
    private GameObject _currentTarget;
    private bool _targetSelected;

    private CombatStates _state ;
    public CombatStates State
    {
        get { return _state; }
        set
        {
            if (_state == value) return;
            _state = value;
            if (OnVariableChange != null)
                OnVariableChange(_state);
        }
    }
    public delegate void OnVariableChangeDelegate(CombatStates newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    // Use this for initialization
    void Start () {
        _state = CombatStates.StaminaFilling;
        _targetSelected = false;
        targets = GameObject.FindGameObjectsWithTag("target");
        gameObject.GetComponent<CurrentState>().OnVariableChange += VariableChangeHandler;
    }

    // Update is called once per frame
    void Update () {
        FillStamina();
	}

    private void VariableChangeHandler(CombatStates newVal)
    {
         _state = newVal;
        Debug.Log("State has changed to: " + _state);
        StateSwitcher();
    }

    private void StateSwitcher()
    {
        if (_state != CombatStates.None)
        {
            switch (_state)
            {
                case CombatStates.StaminaFilling:
                    FillStamina();
                    break;
                case CombatStates.CharacterTurn:
                    SelectCharacter();
                    break;
                case CombatStates.SelectAction:
                    SelectAction();
                    break;
                case CombatStates.Attacking:
                    Attacking();
                    break;
                case CombatStates.CheckingStamina:
                    CheckStamina();
                    break;
                default:
                    Debug.Log("State is horked up");
                    break;
            }
        }
    }

    private void CheckStamina()
    {
        if (_state == CombatStates.CheckingStamina)
        {
            Debug.Log("Checking Stamina");
            for (var i = 0; i < targets.Length; i++)
            {
                var stamina = targets[i].GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Stamina);

                if (stamina.StatCurrentValue >= stamina.StatBaseValue)
                {
                    _currentPlayer = targets[i];
                    VariableChangeHandler(CombatStates.CharacterTurn);
                    return;
                }
            }
            VariableChangeHandler(CombatStates.StaminaFilling);
        }  
    }

    private void Attacking()
    {
        Debug.Log("Attack animation goes here");
        _currentPlayer.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Stamina).StatCurrentValue = 0;
        _currentTarget = null;
        _currentPlayer = null;
        VariableChangeHandler(CombatStates.CheckingStamina);
    }

    public static IEnumerator WaitInput(bool wait, GameObject _currentPlayer, GameObject _currentTarget, string attack, bool _targetSelected, Action<CombatStates> VariableChangeHandler)
    {
        while (wait)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
                if (hit.collider)
                {
                    Debug.Log("This is the attack: " + attack);
                    _currentTarget = hit.collider.gameObject;
                    Debug.Log("Current Target: " + _currentTarget);
                    var method = _currentPlayer.GetComponent<AllAttacks>().GetAttack(attack);
                        method(_currentPlayer, _currentTarget);
                    _targetSelected = false;
                    VariableChangeHandler(CombatStates.Attacking);
                    wait = false;
                }
            }
            yield return null;
        }
    }

    private void SelectAction()
    {
        Debug.Log(_currentPlayer.name);
            AttackOptions[] attacks = _currentPlayer.GetComponent<AllAttacks>().GetTargetAttacks(_currentPlayer);

        for(var i = 0; i < attacks.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(Resources.Load("Prefabs/Button"));
            var buttonPosition = button.GetComponent<RectTransform>().position;
            buttonPosition[1] -= (i * 100) + 50;
            button.GetComponent<RectTransform>().SetPositionAndRotation(buttonPosition, (button.GetComponent<RectTransform>().localRotation));
            button.transform.SetParent(GameObject.Find("Canvas").transform, false);
            button.GetComponentInChildren<Text>().text = attacks[i].ToString();
            button.name = attacks[i].ToString();
            button.tag = attacks[i].ToString();
            button.GetComponent<Button>().onClick.AddListener(
                () => { GetAttack(button.name); }
                );
        }
    }

    private void GetAttack(string attack)
    {
        _targetSelected = true;
 
        while (_targetSelected)
        {            
            StartCoroutine(WaitInput(true, _currentPlayer, _currentTarget, attack, _targetSelected, VariableChangeHandler));

            var canvas = GameObject.Find("Canvas");
            var buttons = canvas.GetComponentsInChildren<Button>();
            for(var i = buttons.Length -1; i > -1; i--)
            {
                Destroy(buttons[i].gameObject);
            }
            
            _targetSelected = false;
        }
    }

    private void SelectCharacter()
    {
       Debug.Log("Selecting character: " + _currentPlayer.name);
       VariableChangeHandler(CombatStates.SelectAction);
    }

    private void FillStamina()
    {
        if (_state == CombatStates.StaminaFilling)
        {
            foreach (var target in targets)
            {
                var stamina = target.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Stamina);
                var speed = target.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Speed).StatValue;

                if(stamina.StatCurrentValue >= stamina.StatBaseValue)
                {
                    _currentPlayer = target;
                    VariableChangeHandler(CombatStates.CharacterTurn);
                    return;
                }
                else if (stamina.StatCurrentValue < stamina.StatBaseValue)
                {
                    stamina.StatCurrentValue += speed;
                    for(var i = 0; i < targets.Length; i++)
                    {
                        if(targets[i].GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Stamina).StatCurrentValue >= stamina.StatBaseValue)
                        {
                            _currentPlayer = targets[i];
                            VariableChangeHandler(CombatStates.CharacterTurn);
                            return;
                        }
                    }
                }
            }
        }
    } 
    

}
