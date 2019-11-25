using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using PixelCrushers.LoveHate;
using Fungus;

public class CurrentState : MonoBehaviour {

    public bool inCombat = false;
    private GameObject _attackPanelSub;
    private GameObject[] targets; 
    private GameObject _currentPlayer;
    private GameObject _currentTarget;
    private GameObject _lastPlayer;
    private GameObject _player;
    private bool _targetSelected;
    private int _enemyCount;
    private int _enemyDead = 0;
    private int _partyCount;
    private int _partyDead = 0;
    private string _action;
    private bool _isPlayerTurn = true;
    private string saveDataKey;
    private Flowchart flowchart;

    public void LoadCombat(string combat, string scene, GameObject characters)
    {
        saveDataKey = scene;
        SceneManager.LoadScene(combat, LoadSceneMode.Single);
    }
    

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    // Use this for initialization
    void OnLevelLoaded (Scene scene, LoadSceneMode mode) {

        if (scene.name.StartsWith("Start"))
        {
            //FungusManager.Instance.SaveManager.Load("save_data");
        }

        if (scene.name.StartsWith("Combat"))
        {
            Debug.Log("Scene loading");
            _player = gameObject;

            _player.GetComponent<AttacksPlayer>().negotiation = FindObjectsOfType<Flowchart>()[1];

            inCombat = true;
            flowchart = _player.GetComponent<AttacksPlayer>().negotiation;
            _state = CombatStates.StaminaFilling;
            _targetSelected = false;
            _partyCount = GameObject.FindGameObjectsWithTag("party").Length;
            _enemyCount = GameObject.FindGameObjectsWithTag("enemy").Length;
            targets = GameObject.FindGameObjectsWithTag("party").Concat(GameObject.FindGameObjectsWithTag("enemy")).ToArray();
            gameObject.GetComponent<CurrentState>().OnVariableChange += VariableChangeHandler;
            gameObject.GetComponent<CurrentState>().OnActionChange += ActionChangeHandler;
            EmotionBubble(GameObject.FindGameObjectsWithTag("enemy").ToArray());

            foreach(var target in targets)
            {
                //TODO: Add everyone who could be in the fight so that their status bars exist
                if (target.name.StartsWith("Player")) target.GetComponent<StatsPlayer>().SetPlayerStatusBars();
                if (target.name.StartsWith("Chace")) target.GetComponent<StatsChace>().SetPlayerStatusBars();
                if (target.name.StartsWith("Guard")) target.GetComponent<StatsGuard>().SetPlayerStatusBars();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flowchart)
        {
            if (!flowchart.GetBooleanVariable("negotiating") && inCombat) FillStamina();
        }
    }

    private void EmotionBubble(GameObject[] enemies)
    {
        foreach (var enemy in enemies)
        {
            var bubblePosition = enemy.GetComponentInChildren<Transform>().position;
            bubblePosition.y += 385;
            bubblePosition.x += 160;
            var bubble = (GameObject)Instantiate(Resources.Load("Prefabs/EmotionBubble"));
            bubble.name = enemy.name + "-EmotionBubble";
            bubble.transform.SetParent(GameObject.Find("Canvas").transform, false);
            bubble.GetComponent<Transform>().SetPositionAndRotation(bubblePosition, bubble.GetComponent<Transform>().localRotation);

            SetEmotion(enemy);
            

        } 
    }

    private void SetEmotion(GameObject enemy)
    {
        var affinity = GetAffinity(enemy);
        string emotion = CalculateAffinity(affinity);
        var bubble = enemy.name + "-EmotionBubble";

        ///Deactiavate all
        for(var i = 0; i < GameObject.Find(bubble).transform.childCount; i++)
        {
            GameObject.Find(bubble).transform.GetChild(i).gameObject.SetActive(false);
        }

        ///Set new emotion
        ///
        GameObject.Find(bubble).transform.Find(emotion).gameObject.SetActive(true);
    }


    private string CalculateAffinity(Dictionary<Emotions, float> affinity)
    {
        ///Calculate Primary Emotion
        ///
       return affinity.Aggregate((l, r) => l.Value > r.Value ? l : r).Key.ToString();
    }

    private void ActionChangeHandler(string newVal)
    {
        GetAttack();
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
                case CombatStates.Negotiating:
                    Negotiating();
                    break;
                default:
                    Debug.Log("State is horked up");
                    break;
            }
        }
    }

    private void Negotiating()
    {
        Debug.Log("Negotiating");
    }

    private void CheckStamina()
    {
        if (_state == CombatStates.CheckingStamina)
        {
            List<GameObject> ready = new List<GameObject>();
            int order = 0;

            for (var i = 0; i < targets.Length; i++)
            {
                var stamina = targets[i].GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Stamina);

                if (stamina.StatCurrentValue >= stamina.StatBaseValue)
                {
                    ready.Add(targets[i]);
                }
            }
            Debug.Log("Ready queue: " + ready.Count);
            if(ready.Count > 1)
            {
                ///TODO: CHANGE THIS SO IT PULLS FROM CURRENT STAT
                ///Do them in order of speed
                ///
                List<GameObject> sortedReady = ready;

                if (_lastPlayer)
                {
                    if(_lastPlayer.name == sortedReady[order].name)
                    {
                        order += 1;
                    }
                }
                _currentPlayer = sortedReady[order];
                VariableChangeHandler(CombatStates.CharacterTurn);
                return;

            } else if(ready.Count == 1)
            {
                _currentPlayer = ready[order];
                VariableChangeHandler(CombatStates.CharacterTurn);
                return;
            } else
            {
                VariableChangeHandler(CombatStates.StaminaFilling);
                return;
            }
        }  
    }

    private void Attacking()
    {
        _currentPlayer.GetComponent<ModifyStats>().ModifyTheStat(RPGStatType.Stamina, -_currentPlayer.GetComponent<RPGDefaultStats>().GetStat<RPGVital>(RPGStatType.Stamina).StatBaseValue);

        foreach (var target in targets)
        {
            if (target.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Health).StatCurrentValue <= 0)
            {

                ///Mark as dead
                if(target.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.Alive).StatValue < 1)
                {
                    target.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.Alive).StatBaseValue = 1;
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
        _lastPlayer = _currentPlayer;
        _currentTarget = null;
        _currentPlayer = null;
        VariableChangeHandler(CombatStates.CheckingStamina);
    }

    private void BattleWon()
    {
        Debug.Log("Party Won!");
        //Open Win Screen
        GameObject screen = (GameObject)Instantiate(Resources.Load("Prefabs/CombatWon"));
        screen.transform.SetParent(GameObject.Find("Canvas").transform, false);
        inCombat = false;

        //TODO: Set up experience menu

        SceneManager.LoadScene("Start", LoadSceneMode.Single);
        //FungusManager.Instance.SaveManager.Load("save_data");
        FungusManager.Instance.SaveManager.LoadSpecific("save_data", "AfterCombat");
        

    }

    private void BattleLost()
    {
        Debug.Log("Party Lost");
        ///Open Dead screen
        GameObject screen = (GameObject)Instantiate(Resources.Load("Prefabs/CombatLost"));
        screen.transform.SetParent(GameObject.Find("Canvas").transform, false);
        inCombat = false;
    }

    public static IEnumerator WaitInput(bool wait, GameObject _currentPlayer, GameObject _currentTarget, string action, bool _targetSelected, Action<CombatStates> VariableChangeHandler, bool _isPlayerTurn)
    {
        bool finished = false;
        while (wait)
        {
            if (_currentPlayer.tag == "enemy")
            {
                if(_isPlayerTurn)
                {
                    VariableChangeHandler(CombatStates.Negotiating);
                    wait = false;
                }
                _currentTarget = GameObject.Find("Player");
                var method = _currentPlayer.GetComponent<AllAttacks>().GetAttack(action);
                while (!finished)
                {
                    finished = method(_currentPlayer, _currentTarget);
                    if (finished)
                    {
                        _targetSelected = false;
                        action = "";
                        VariableChangeHandler(CombatStates.Attacking);
                        wait = false;
                    }
                }
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
                        
                        while (!finished)
                        {
                            finished = method(_currentPlayer, _currentTarget);
                            if (finished)
                            {
                                _targetSelected = false;
                                action = "";
                                VariableChangeHandler(CombatStates.Attacking);
                                wait = false; 
                            }
                        }
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

        ///AttackPanelSub
        _attackPanelSub = GameObject.Find("AttackPanelSub");
        _attackPanelSub.name = "AttackPanelSub";
        var willpowerCanvasPosition = _attackPanelSub.GetComponent<RectTransform>().position;

        ///Charater Portrait Setup
        var attackerImage = _currentPlayer.GetComponentInChildren<SpriteRenderer>().name;
        GameObject attacker = (GameObject)Instantiate(Resources.Load("Prefabs/" + attackerImage));
        attacker.transform.SetParent(GameObject.Find("Canvas").transform, false);
        var attackerPosition = attacker.GetComponent<Transform>().position;

        attackCanvasPosition.y = Screen.height / 2;
        attackerPosition.x = 100;

        willpowerCanvasPosition.y = Screen.height / 2;

        AttackOptions[] attacks = _currentPlayer.GetComponent<AllAttacks>().GetTargetAttacks(_currentPlayer);
        AttackOptions[] willpowers = _currentPlayer.GetComponent<AllAttacks>().GetTargetWillpowers(_currentPlayer);

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
                buttonBase -= buttonHeight + 10;
            }

            button.GetComponent<RectTransform>().SetPositionAndRotation(buttonPosition, (button.GetComponent<RectTransform>().localRotation));
            button.transform.SetParent(GameObject.Find("AttackPanel").transform, false);
            button.GetComponentInChildren<Text>().text = attacks[i].ToString();
            button.name = attacks[i].ToString();
            button.tag = attacks[i].ToString();

        }

        ///Willpower Buttons setup
        for (var i = 0; i < willpowers.Length; i++)
        {

            GameObject button = (GameObject)Instantiate(Resources.Load("Prefabs/Button"));
            var buttonPosition = button.GetComponent<RectTransform>().position;
            var buttonHeight = button.GetComponent<RectTransform>().rect.height;

            if (i == 0)
            {
                buttonPosition.y = _attackPanelSub.transform.up.y;
                buttonBase = buttonPosition.y - (buttonHeight + 10);
            }
            else
            {
                buttonPosition.y = buttonBase;
                buttonBase -= buttonHeight + 10;
            }

            button.GetComponent<RectTransform>().SetPositionAndRotation(buttonPosition, (button.GetComponent<RectTransform>().localRotation));
            button.transform.SetParent(GameObject.Find("AttackPanelSub").transform, false);
            button.GetComponentInChildren<Text>().text = willpowers[i].ToString();
            button.name = willpowers[i].ToString();
            button.tag = willpowers[i].ToString();

        }

        ///Set Canvas and Character position
        ///
        _attackPanelSub.SetActive(false);
        attackCanvas.GetComponent<RectTransform>().SetPositionAndRotation(attackCanvasPosition, attackCanvas.GetComponent<RectTransform>().localRotation);

        _attackPanelSub.GetComponent<RectTransform>().SetPositionAndRotation(willpowerCanvasPosition, _attackPanelSub.GetComponent<RectTransform>().localRotation);

        attacker.GetComponent<Transform>().SetPositionAndRotation(attackerPosition, attacker.GetComponent<Transform>().localRotation);
    }

    private void EnemyAction()
    {
        ///Check for _currentPlayer anger, love and fear
        Dictionary<Emotions, float> affinity = GetAffinity(_currentPlayer);

        ///Get attack pattern based on affinity
        Action = EnemyAttackPattern(affinity).ToString();
    }

    private Dictionary<Emotions, float> GetAffinity(GameObject target)
    {
        var factionID = _player.GetComponent<FactionMember>().factionManager.GetFactionID(target.name);

        float neutral = target.GetComponent<FactionMember>().factionManager.GetFaction(factionID).GetPersonalRelationshipTrait(0, 1);
        float angry = target.GetComponent<FactionMember>().factionManager.GetFaction(factionID).GetPersonalRelationshipTrait(0, 2);
        float afraid = target.GetComponent<FactionMember>().factionManager.GetFaction(factionID).GetPersonalRelationshipTrait(0, 3);
        float charmed = target.GetComponent<FactionMember>().factionManager.GetFaction(factionID).GetPersonalRelationshipTrait(0, 4);
        float confused = target.GetComponent<FactionMember>().factionManager.GetFaction(factionID).GetPersonalRelationshipTrait(0, 5);

        Dictionary<Emotions, float> affinity = new Dictionary<Emotions, float>
        {
            {Emotions.Angry, angry },
            {Emotions.Afraid, afraid },
            {Emotions.Charmed, charmed },
            {Emotions.Confused, confused },
            { Emotions.Neutral, neutral },
        };

        return affinity;

    }

    private AttackOptions EnemyAttackPattern(Dictionary<Emotions, float> affinity)
    {
        return _currentPlayer.GetComponent<AttacksEnemy>().AttackPattern(affinity);
    }

    private void SelectAction()
    {
        if(_enemyDead < _enemyCount && _partyDead < _partyCount)
        {
            if (_currentPlayer.tag == "party" && _currentPlayer.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.Alive).StatValue < 1)
            {
                PartyAction();
            }
            else if(_currentPlayer.tag == "enemy" && _currentPlayer.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.Alive).StatValue < 1 && !_isPlayerTurn)
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
            _isPlayerTurn = flowchart.GetBooleanVariable("negotiating");
            StartCoroutine(WaitInput(true, _currentPlayer, _currentTarget, Action, _targetSelected, VariableChangeHandler, _isPlayerTurn));

            if(_currentPlayer && _currentPlayer.tag == "party" )
            {
                var canvas = GameObject.Find("Canvas");
                var buttons = canvas.GetComponentsInChildren<Button>();
                var attackPanel = GameObject.Find("AttackPanel");
                var attacker = GameObject.Find(_currentPlayer.GetComponentInChildren<SpriteRenderer>().name + "(Clone)");

                for (var i = buttons.Length - 1; i > -1; i--)
                {
                    if(Action == "Willpower")
                    {
                        _attackPanelSub.SetActive(true);
                        _attackPanelSub.transform.SetParent(canvas.transform, false);
                    }
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
       VariableChangeHandler(CombatStates.SelectAction);
    }

    private void FillStamina()
    {
        if (_state == CombatStates.StaminaFilling)
        {

            foreach (var target in targets)
            {
                var staminaStat = target.GetComponent<ModifyStats>().GetRPGVitalStat(RPGStatType.Stamina);
                var stamina = staminaStat.StatCurrentValue;
                var speed = target.GetComponent<ModifyStats>().GetRPGAttributeStat(RPGStatType.Speed).StatValue;

                if(stamina >= staminaStat.StatBaseValue)
                {
                    VariableChangeHandler(CombatStates.CheckingStamina);
                    return;
                }
                else if (stamina < staminaStat.StatBaseValue && _state == CombatStates.StaminaFilling)
                {
                    ///Set emotion for enemeies
                    if(stamina * 2 >= staminaStat.StatBaseValue)
                    {
                        if (target.tag == "enemy")
                        {
                            ///set emotion
                            SetEmotion(target);
                        }
                    }
                    target.GetComponent<ModifyStats>().ModifyTheStat(RPGStatType.Stamina, speed);

                    if(stamina >= staminaStat.StatBaseValue)
                    { VariableChangeHandler(CombatStates.CheckingStamina);
                        return;
                    }
                }
            }
        }
    }


    


}
