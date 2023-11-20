using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBlockFunctionality : BaseBlock
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerRB.simulated = false;
    }
    //TODO: dodaæ sprawdzenie kiedy gracz chce wyskoczyæ
}
