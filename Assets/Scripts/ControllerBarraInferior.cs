using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerBarraInferior : MonoBehaviour
{
    public Text TextoTeclaOpcao, TextoBarraInferior;
    public Transform grupoPilulasMaoEsquerda, grupoPilulasMaoDireita;
    public ControllerFase controladorFase;
    internal string tipoPilula;
    internal string pilulaAConceder_1, pilulaAConceder_2;

    void Update()
    {
        if ((Geral.ModoDeControle.Contains("Teclado") && Input.GetKeyDown(KeyCode.X)) || (Geral.ModoDeControle == "Controle" && Input.GetButtonDown("Fire1")))
        {
            // Caso a opção informada no texto da barra inferior seja para pegar pílula,
            // será verificado se já tem pílula na mão direita:
            // Se positivo, a outra mão (esquerda) receberá a pílula de tipo 'tipoPilula'
            // Se negativo, a própria mão direita é que segurará a pílula

            if (TextoBarraInferior.text.StartsWith("Pegar"))
            {
                bool jaAtivouDir = false;

                for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                    if (grupoPilulasMaoDireita.GetChild(i).gameObject.activeSelf)
                        jaAtivouDir = true;

                if (!jaAtivouDir)
                {
                    for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                        if (grupoPilulasMaoDireita.GetChild(i).name == "Pilula_" + tipoPilula)
                        {
                            grupoPilulasMaoDireita.GetChild(i).gameObject.SetActive(true);
                            controladorFase.pilulaMaoDireita = tipoPilula;
                        }
                }
                else
                {
                    for (int i = 0; i < grupoPilulasMaoEsquerda.childCount; i++)
                        if (grupoPilulasMaoEsquerda.GetChild(i).name == "Pilula_" + tipoPilula)
                        {
                            grupoPilulasMaoEsquerda.GetChild(i).gameObject.SetActive(true);
                            controladorFase.pilulaMaoEsquerda = tipoPilula;
                        }
                }
            }

            // Caso a opção informada no texto da barra inferior seja para devolver a pílula,
            // será verificado de qual mão do médico será retirada a pílula em questão,
            // além de se apagar o conteúdo da variável controladorFase.pilulaMaoDireita
            // ou controladorFase.pilulaMaoEsquerda, caso aplicável.

            else if (TextoBarraInferior.text.StartsWith("Devolver"))
            {
                for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                    if (grupoPilulasMaoDireita.GetChild(i).name == "Pilula_" + tipoPilula)
                    {
                        grupoPilulasMaoDireita.GetChild(i).gameObject.SetActive(false);
                        controladorFase.pilulaMaoDireita = "";
                    }

                for (int i = 0; i < grupoPilulasMaoEsquerda.childCount; i++)
                    if (grupoPilulasMaoEsquerda.GetChild(i).name == "Pilula_" + tipoPilula)
                    {
                        grupoPilulasMaoEsquerda.GetChild(i).gameObject.SetActive(false);
                        controladorFase.pilulaMaoEsquerda = "";
                    }
            }

            // Caso a opção informada no texto da barra inferior seja para conceder a(s) pílula(s),
            // as pílulas sairão das mãos do médico, sendo limpas as respectivas variáveis,
            // e MontaLabirinto será informado de que precisa introduzir as pílulas
            // no labirinto já montado.

            else if (TextoBarraInferior.text.StartsWith("Conceder pílula(s)"))
            {
                for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                    grupoPilulasMaoDireita.GetChild(i).gameObject.SetActive(false);

                for (int i = 0; i < grupoPilulasMaoEsquerda.childCount; i++)
                    grupoPilulasMaoEsquerda.GetChild(i).gameObject.SetActive(false);

                controladorFase.pilulaMaoDireita = "";
                controladorFase.pilulaMaoEsquerda = "";

                controladorFase.controladorLabirinto.InserirPilulas(pilulaAConceder_1, pilulaAConceder_2);
                controladorFase.MostrarLabirintoTemporariamente(2, false);
            }

            // Caso a opção informada no texto da barra inferior seja para manusear o equipamento do leito,
            // o procedimento para se iniciar o controle do labirinto será executado.

            else if (TextoBarraInferior.text.StartsWith("Manusear equipamento"))
                controladorFase.IniciarControleLabirinto();

            // Caso a opção informada no texto da barra inferior seja para deixar de manusear o equipamento,
            // é acionado o procedimento de interrupção do controle do labirinto.

            else if (TextoBarraInferior.text.StartsWith("Interromper o manuseio do equipamento"))
                controladorFase.InterromperControleLabirinto();

            // Por fim, desliga-se a barra inferior, exceto se o modo de jogo corrente for "Labirinto".
            // Nesse caso, o texto é alterado para refletir a disponibilização da opção
            // de saída do modo de controle do labirinto ao jogador.

            if (Geral.ModoDeJogoCorrente == "Labirinto")
                TextoBarraInferior.text = "Interromper o manuseio do equipamento";
            else
                gameObject.SetActive(false);
        }
    }
}
