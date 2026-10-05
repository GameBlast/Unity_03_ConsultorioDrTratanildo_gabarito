using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcedePilula : MonoBehaviour
{
    public ControllerBarraInferior barraInferior;
    public Transform grupoPilulasMaoEsquerda, grupoPilulasMaoDireita;
    internal string nomePilulaDir, nomePilulaEsq;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "DoutorCenario" && Geral.ModoDeJogoCorrente == "BuscaMedicamentos")
        {
            // Variáveis de controle para verificar se o doutor já está segurando pílulas em suas mãos
            bool temPilulaMaoDir = false;
            bool temPilulaMaoEsq = false;
            nomePilulaDir = "N/A";
            nomePilulaEsq = "N/A";

            // Verificação de presença de pílula na mão direita do médico
            for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                if (grupoPilulasMaoDireita.GetChild(i).gameObject.activeSelf)
                {
                    temPilulaMaoDir = true;
                    nomePilulaDir = grupoPilulasMaoDireita.GetChild(i).name;
                }

            // Verificação de presença de pílula na mão esquerda do médico
            for (int i = 0; i < grupoPilulasMaoEsquerda.childCount; i++)
                if (grupoPilulasMaoEsquerda.GetChild(i).gameObject.activeSelf)
                {
                    temPilulaMaoEsq = true;
                    nomePilulaEsq = grupoPilulasMaoEsquerda.GetChild(i).name;
                }

            //   Se tiver pílulas nas mãos
            //   - Opção a ser exibida: Conceder pílula(s)
            //   Se não tiver:
            //   - Opção a ser exibida: Manusear equipamento

            if (temPilulaMaoDir || temPilulaMaoEsq)
            {
                barraInferior.pilulaAConceder_1 = nomePilulaDir;
                barraInferior.pilulaAConceder_2 = nomePilulaEsq;
                barraInferior.TextoBarraInferior.text = "Conceder pílula(s)";
            }
            else barraInferior.TextoBarraInferior.text = "Manusear equipamento";

            if (Geral.ModoDeControle.Contains("Teclado"))
                barraInferior.TextoTeclaOpcao.text = "X";
            else if (Geral.ModoDeControle == "Controle")
                barraInferior.TextoTeclaOpcao.text = "A";

            barraInferior.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "DoutorCenario")
        {
            barraInferior.gameObject.SetActive(false);
        }
    }
}
