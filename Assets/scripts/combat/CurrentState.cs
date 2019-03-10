using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class CurrentState : MonoBehaviour {

    private GameObject[] targets; 
    private GameObject _currentPlayer;
    private GameObject _currentTarget;
    private bool _targetSelected;
    private int _enemyCount;
    private int _enemyDead = 0;
    private int _partyCount;
    private int _partyDead = 0;
    private string _action;

    public string Action
    {
        get { return _action; }
        set
        {
            _action = value;
            if(OnActionChange != null)
                OnActionChange(_action);
        }
    }

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

    public delegate void OnActionChangeDelegate(string newVal);
    public event OnActionChangeDelegate OnActionChange;

    // Use this for initialization
    void Start () {
        _state = CombatStates.StaminaFilling;
        _targetSelected = false;
        _partyCount = GameObject.FindGameObjectsWithTag("party").Length;
        _enemyCount = GameObject.FindGameObjectsWithTag("enemy").Length;
        targets = GameObject.FindGameObjectsWithTag("party").Concat(GameObject.FindGameObjectsWithTag("enemy")).ToArray();
        gameObject.GetComponent<CurrentState>().OnVariableChange += VariableChangeHandler;
        gameObject.GetComponent<CurrentState>().OnActionChange += ActionChangeHandler;
    }

    private void ActionChangeHandler(string newVal)
    {
        Debug.Log("Getting Action: " + newVal);
        GetAttack();
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
        
        foreach(var target in targets)
        {
            if (target.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue <= 0)
            {

                ///Mark as dead
                if(target.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Alive).StatValue < 1)
                {
                    target.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Alive).StatBaseValue = 1;
                    if (target.tag == "enemy")
                    {
                        _enemyDead += 1;
                    }
                    if (target.tag == "party")
                    {
                        _partyDead += 1;
                    } 
                    
                }
                
            }
        }
        _currentTarget = null;
        _currentPlayer = null;
        VariableChangeHandler(CombatStates.CheckingStamina);
    }

    private void BattleWon()
    {
        Debug.Log("Party Won!");
        //Open Win Screen
    }

    private void BattleLost()
    {
        Debug.Log("Party Lost");
        ///Open Dead screen
    }

    public static IEnumerator WaitInput(bool wait, GameObject _currentPlayer, GameObject _currentTarget, string action, bool _targetSelected, Action<CombatStates> VariableChangeHandler)
    {
        while (wait)
        {
            ///What if the person
            if(_currentPlayer.tag == "enemy")
            {
                _currentTarget = GameObject.Find("Player");
                var method = _currentPlayer.GetComponent<AllAttacks>().GetAttack(action);
                method(_currentPlayer, _currentTarget);
                _targetSelected = false;
                action = "";
                VariableChangeHandler(CombatStates.Attacking);
                wait = false;
            } else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
                    if (hit.collider)
                    {
                        Debug.Log("This is the attack: " + action);
                        _currentTarget = hit.collider.gameObject;
                        Debug.Log("Current Target: " + _currentTarget);
                        var method = _currentPlayer.GetComponent<AllAttacks>().GetAttack(action);
                        method(_currentPlayer, _currentTarget);
                        _targetSelected = false;
                        action = "";
                        VariableChangeHandler(CombatStates.Attacking);
                        wait = false;
                    }
                }
            }
            
            yield return null;
        }
    }

    private void PartyAction()
    {
        var buttonBase = 0f;
        ///AttackPanel setup
        GameObject attackCanvas = (GameObject)Instantiate(Resources.Load("Prefabs/AttackPanel"));
        attackCanvas.transform.SetParent(GameObject.Find("Canvas").transform, false);
        attackCanvas.name = "AttackPanel";
        var attackCanvasPosition = attackCanvas.GetComponent<RectTransform>().position;

        ///Charater Portrait Setup
        var attackerImage = _currentPlayer.GetComponentInChildren<SpriteRenderer>().name;
        GameObject attacker = (GameObject)Instantiate(Resources.Load("Prefabs/" + attackerImage));
        attacker.transform.SetParent(GameObject.Find("Canvas").transform, false);
        var attackerPosition = attacker.GetComponent<Transform>().position;

        attackCanvasPosition.y = Screen.height / 2;
        attackerPosition.x = 100;


        Debug.Log(_currentPlayer.name);
        AttackOptions[] attacks = _currentPlayer.GetComponent<AllAttacks>().GetTargetAttacks(_currentPlayer);

        ///Attack Buttons setup
        for (var i = 0; i < attacks.Length; i++)
        {

            GameObject button = (GameObject)Instantiate(Resources.Load("Prefabs/Button"));
            var buttonPosition = button.GetComponent<RectTransform>().position;
            var buttonHeight = button.GetComponent<RectTransform>().rect.height;

            buttonPosition.x = 70;

            if (i == 0)
            {
                buttonPosition.y = attackCanvas.transform.up.y + 150;
                buttonBase = buttonPosition.y - (buttonHeight + 10);

            }
            else
            {
                buttonPosition.y = buttonBase;
                buttonBase += buttonHeight;
            }

            button.GetComponent<RectTransform>().SetPositionAndRotation(buttonPosition, (button.GetComponent<RectTransform>().localRotation));
            button.transform.SetParent(GameObject.Find("AttackPanel").transform, false);
            button.GetComponentInChildren<Text>().text = attacks[i].ToString();
            button.name = attacks[i].ToString();
            button.tag = attacks[i].ToString();

        }

        ///Set Canvas and Character position
        attackCanvas.GetComponent<RectTransform>().SetPositionAndRotation(attackCanvasPosition, attackCanvas.GetComponent<RectTransform>().localRotation);

        attacker.GetComponent<Transform>().SetPositionAndRotation(attackerPosition, attacker.GetComponent<Transform>().localRotation);
    }

    private void EnemyAction()
    {
        ///Set Attack based on _currentPlayer attacks
        Action = "Attack";
    }

    private void SelectAction()
    {
        if(_enemyDead < _enemyCount && _partyDead < _partyCount)
        {
            if (_currentPlayer.tag == "party" && _currentPlayer.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Alive).StatValue < 1)
            {
                PartyAction();
            }
            else if(_currentPlayer.tag == "enemy" && _currentPlayer.GetComponent<RPGDefaultStats>().GetStat<RPGAttribute>(RPGStatType.Alive).StatValue < 1)
            {
                EnemyAction();
            }
        } else
        {
            ///if all enemy are dead
            if (_enemyDead >= _enemyCount)
            {
                BattleWon();
            }
            ///if all party are dead
            if (_partyDead >= _partyCount)
            {
                BattleLost();
            }
        }


    }

    

    private void GetAttack()
    {
        _targetSelected = true;
 
        while (_targetSelected)
        {            
            StartCoroutine(WaitInput(true, _currentPlayer, _currentTarget, Action, _targetSelected, VariableChangeHandler));

            if(_currentPlayer && _currentPlayer.tag == "party")
            {
                var canvas = GameObject.Find("Canvas");
                var buttons = canvas.GetComponentsInChildren<Button>();
                var attackPanel = GameObject.Find("AttackPanel");
                Debug.Log("Current Player: " + _currentPlayer.name);
                var attacker = GameObject.Find(_currentPlayer.GetComponentInChildren<SpriteRenderer>().name + "(Clone)");
                for (var i = buttons.Length - 1; i > -1; i--)
                {
                    Destroy(buttons[i].gameObject);
                    Destroy(attackPanel.gameObject);
                    Destroy(attacker.gameObject);
                }
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
