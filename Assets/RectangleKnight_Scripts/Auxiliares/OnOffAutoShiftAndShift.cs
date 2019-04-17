using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffAutoShiftAndShift : MonoBehaviour
{
    #region inspector
    [SerializeField] private string autoShiftID = default;
    [SerializeField] private KeyShift shift = default;
    [SerializeField] private bool autoShiftDisableWith = default;
    [SerializeField] private bool shiftDisableWith = default;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        KeyVar mykeys = GameController.g.MyKeys;

        if (mykeys.VerificaAutoShift(autoShiftID) == autoShiftDisableWith 
            && 
            mykeys.VerificaAutoShift(shift) == shiftDisableWith)
            Destroy(gameObject);
    }

    
}
