using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteVelocidadeAnimacaoLegacy : MonoBehaviour
{
    public float velocidade = 1f;

    private void OnEnable()
    {
        foreach (AnimationState state in gameObject.GetComponent<Animation>())
            state.speed = velocidade;
    }

}
