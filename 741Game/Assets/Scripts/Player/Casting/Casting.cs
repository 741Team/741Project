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
    // Start is called before the first frame update
    void Start()
    {
        _adjustmentInputs = new Dictionary<KeyCode, Adjustment>();
        foreach (Adjustment a in _availableAdjustments)
        {
            _adjustmentInputs.Add(a._inputKey, a);
        }
    }

    // Update is called once per frame
    void Update()
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
            if (Input.GetKeyDown(_inputKey)) {
                CastSpell();
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
            }
        }
        else
        {
            Debug.LogError("Bolt object missing from casting");
        }

    }



}
