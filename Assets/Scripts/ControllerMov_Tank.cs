using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMov_Tank : MonoBehaviour
{
    public Animator personagemAnim;    
    float velocidadeCaminhada = 0;

    // Start is called before the first frame update
    void Start()
    {
        personagemAnim.SetFloat("Passo_caminhada", 0);
    }

    private void OnDisable()
    {
        personagemAnim.SetFloat("Passo_caminhada", 0);
    }

    private void OnAnimatorMove()
    {
        transform.position += personagemAnim.deltaPosition * velocidadeCaminhada;
        transform.rotation = personagemAnim.deltaRotation * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Controle do avanço para frente
        switch (Geral.ModoDeControle)
        {
            case "SoTeclado":
            case "Teclado+Mouse":
                if (Input.GetAxis("Vertical") > 0.5)
                    velocidadeCaminhada = Mathf.Min(velocidadeCaminhada + (Time.deltaTime * 2), 2.0f);
                else velocidadeCaminhada = 0.0f;
                break;
            case "Controle":
                if (Input.GetAxis("DPAD_v") > 0.5)
                    velocidadeCaminhada = Mathf.Min(velocidadeCaminhada + (Time.deltaTime * 2), 2.0f);
                else if (Input.GetAxis("Vertical") > 0.1)
                    velocidadeCaminhada = Mathf.Max(0, Input.GetAxis("Vertical") * 2.0f);
                else velocidadeCaminhada = 0.0f;
                break;
        }

        personagemAnim.SetFloat("Passo_caminhada", velocidadeCaminhada);

        //Controle da movimentação lateral (tank)

        Vector3 rotacaoPersonagem = transform.eulerAngles;

        switch (Geral.ModoDeControle)
        {
            case "SoTeclado":
                if (Input.GetAxis("Horizontal") > 0.5)
                    rotacaoPersonagem.y += (Time.deltaTime * 120);
                else if (Input.GetAxis("Horizontal") < -0.5)
                    rotacaoPersonagem.y -= (Time.deltaTime * 120);
                break;
            case "Controle":
                rotacaoPersonagem.y += (Time.deltaTime * Input.GetAxis("Horizontal") * 180);
                rotacaoPersonagem.y += (Time.deltaTime * Input.GetAxis("DPAD_h") * 120);
                break;
            case "Teclado+Mouse":
                rotacaoPersonagem.y += (Time.deltaTime * Input.GetAxis("Mouse X") * 600);
                break;
        }

        transform.eulerAngles = rotacaoPersonagem;
    }
}
