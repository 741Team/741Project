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
    ExitFocus _exitFocus;

    bool _canInput = false;
    // Start is called before the first frame update
    void Start()
    {
        _characterAnimations = GetComponent<CharacterAnimations>();
        _exitFocus = GetComponent<ExitFocus>();

    }

    private void OnEnable()
    {
        _adjustmentInputs = new Dictionary<KeyCode, Adjustment>();
        foreach (Adjustment a in _availableAdjustments)
        {
            _adjustmentInputs.Add(a._inputKey, a);
        }
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
                        _usedAdjustments.Add(_adjustmentInputs[key]);
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
        if (_bolt != null)
        {
            GameObject newBolt = Instantiate(_bolt, transform.position, Quaternion.identity);
            Bolt boltScript = newBolt.GetComponent<Bolt>();
            if (boltScript != null)
            {
                boltScript.OnCreate();
                boltScript.SetAdjustments(_usedAdjustments);
                boltScript.ApplyAdjustments();
                boltScript.FireBolt();
                _characterAnimations.Cast();
                _usedAdjustments.Clear();
                EnableInputs(false);
                StartCoroutine(WaitForBolt(boltScript));
            }
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
            CastSpell();
        }
        else
        {
            _exitFocus.FocusOver();
        }
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
