using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorAuxiliarColisoes : MonoBehaviour
{
    public ComportamentoDoenca controladorAgente;

    private void OnTriggerStay(Collider other)
    {
        controladorAgente.OnTriggerExterno(other);
    }
}
