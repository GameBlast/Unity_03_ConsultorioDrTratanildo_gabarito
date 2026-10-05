using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMov_Labirinto : MonoBehaviour
{
    public float taxaInclinacao;
    public Vector3 inclinacoesMaximas, inclinacoesMinimas;
    internal Vector3 rotacaoCalculada;

    // Start is called before the first frame update
    void Start()
    {
        rotacaoCalculada = Vector3.zero;
        transform.localEulerAngles = rotacaoCalculada;
    }

    // Update is called once per frame
    void Update()
    {
        // Inclinações
        if (Geral.ModoDeJogoCorrente == "Labirinto")
        {
            Vector3 alteracoesInclinacao = new Vector3();

            switch (Geral.ModoDeControle)
            {
                case "SoTeclado":
                    // Para "frente" e para "trás"
                    if (Input.GetAxis("Vertical") > 0.5)
                        alteracoesInclinacao.z -= taxaInclinacao * Time.deltaTime;
                    if (Input.GetAxis("Vertical") < -0.5)
                        alteracoesInclinacao.z += taxaInclinacao * Time.deltaTime;
                    // Para os lados
                    if (Input.GetAxis("Horizontal") < -0.5)
                        alteracoesInclinacao.x += taxaInclinacao * Time.deltaTime;
                    if (Input.GetAxis("Horizontal") > 0.5)
                        alteracoesInclinacao.x -= taxaInclinacao * Time.deltaTime;
                    break;
                case "Controle":
                    alteracoesInclinacao.x -= (Time.deltaTime * Input.GetAxis("Horizontal") * 60);
                    alteracoesInclinacao.x -= (Time.deltaTime * Input.GetAxis("DPAD_h") * 45);
                    alteracoesInclinacao.z -= (Time.deltaTime * Input.GetAxis("Vertical") * 60);
                    alteracoesInclinacao.z -= (Time.deltaTime * Input.GetAxis("DPAD_v") * 45);
                    break;
                case "Teclado+Mouse":
                    alteracoesInclinacao.z -= taxaInclinacao * Time.deltaTime * Input.GetAxis("Mouse Y") * 5;
                    alteracoesInclinacao.x -= taxaInclinacao * Time.deltaTime * Input.GetAxis("Mouse X") * 5;
                    break;
            }
  
            // Calcular a posição prévia
            rotacaoCalculada += alteracoesInclinacao;

            // Enquadrar os valores entre o mínimo e o máximo estipulados
            rotacaoCalculada = Vector3.Min(
                Vector3.Max(rotacaoCalculada, inclinacoesMinimas),inclinacoesMaximas);

            // Passar a rotação para o GameObject
            transform.localEulerAngles = rotacaoCalculada;
        }
    }
}
