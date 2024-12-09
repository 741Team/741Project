using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public KeyCode _inputKey;


    [SerializeField]
    List<Adjustment> _availableAdjustments = new List<Adjustment>();
    [SerializeField]
    List<Adjustment> _usedAdjustments = new List<Adjustment>();
    [SerializeField]
    GameObject _bolt;
    Dictionary<KeyCode, Adjustment> _adjustmentInputs;
    CharacterAnimations _characterAnimations;
    PlayerController _playerController;
    ExitFocus _exitFocus;
    Vector2 direction;
    private Bolt _boltScript;
    bool _startUp = true;

    bool _canInput = false;
    // Start is called before the first frame update
    void Start()
    {
        _characterAnimations = GetComponent<CharacterAnimations>();
        _exitFocus = GetComponent<ExitFocus>();
        _playerController = GetComponent<PlayerController>();

    }

    private void OnEnable()
    {
        if (_startUp)
        {
            _startUp = false;
            return;
        }
        _adjustmentInputs = new Dictionary<KeyCode, Adjustment>();
        foreach (Adjustment a in _availableAdjustments)
        {
            _adjustmentInputs.Add(a._inputKey, a);
        }
        Invoke("CreateBolt", 0.1f);
    }

    public void EnableInputs(bool enable)
    {
        _canInput = enable;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canInput)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in _adjustmentInputs.Keys)
                {
                    if (Input.GetKey(key))
                    {
                        _boltScript.ApplySingleAdjustment(_adjustmentInputs[key]);
                        float cost = _adjustmentInputs[key].GetFocusCost();
                        _playerController.ChangeFocus(-cost);
                        //_usedAdjustments.Add(_adjustmentInputs[key]);
                    }
                }
                if (Input.GetKeyDown(_inputKey))
                {
                    CastSpell();
                }
            }
        }

    }

    void CastSpell()
    {
        if (_boltScript != null)
        {
            _boltScript.FireBolt();
            _characterAnimations.Cast();
            EnableInputs(false);
            PlayerController player = GetComponent<PlayerController>();
            player.PlayCastSound();
            StartCoroutine(WaitForBolt(_boltScript));
        }
    }
    void CastSpellWithBulkAdjust()
    {
        if (_bolt != null)
        {
            GameObject newBolt = Instantiate(_bolt, transform.position, Quaternion.identity);
            _boltScript = newBolt.GetComponent<Bolt>();
            if (_boltScript != null)
            {
                _boltScript.OnCreate();
                _boltScript.SetDirection(direction);
                _boltScript.SetAdjustments(_usedAdjustments);
                _boltScript.ApplyAdjustments();
                _boltScript.FireBolt();
                _characterAnimations.Cast();
                _usedAdjustments.Clear();
                EnableInputs(false);
                PlayerController player = GetComponent<PlayerController>();
                player.PlayCastSound();
                StartCoroutine(WaitForBolt(_boltScript));
            }
        }
        else
        {
            Debug.LogError("Bolt object missing from casting");
        }

    }

    private void OnDisable()
    {
        _usedAdjustments.Clear();
        if(_boltScript != null)
        {
            Destroy(_boltScript.gameObject);
        }
    }

    private void CreateBolt()
    {
        GameObject newBolt = Instantiate(_bolt, transform.position, Quaternion.identity);
        _boltScript = newBolt.GetComponent<Bolt>();
        if (_boltScript != null)
        {
            _boltScript.OnCreate();
            _boltScript.SetDirection(direction);
        }
        else
        {
            Debug.LogError("Bolt object missing from casting");
        }
    }

    public void OutOfTIme()
    {
        if (_usedAdjustments.Count > 0)
        {
            CastSpellWithBulkAdjust();
        }
        else
        {
            _exitFocus.FocusOver();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    IEnumerator WaitForBolt(Bolt bolt)
    {
        bool ended = false;

        while (!ended)
        {
            ended = bolt.IsBoltEnded();
            yield return null;
        }
        _exitFocus.FocusOver();
    }



}
