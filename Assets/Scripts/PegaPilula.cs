using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegaPilula : MonoBehaviour
{
    public string tipoPilula;
    public ControllerBarraInferior barraInferior;
    public Transform grupoPilulasMaoEsquerda, grupoPilulasMaoDireita;

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "DoutorCenario" && Geral.ModoDeJogoCorrente == "BuscaMedicamentos")
        {
            // Variáveis de controle para verificar se o doutor já está segurando pílulas em suas mãos
            bool temPilulaMaoDir = false;
            bool temPilulaMaoEsq = false;

            // Variáveis de controle para verificar se, dentre os tipos de pílulas nas mãos do doutor, uma delas é
            // igual ao tipo da pílula que está com esse script atrelado
            bool jaPegouEssaPilulaMaoDir = false;
            bool jaPegouEssaPilulaMaoEsq = false;

            // Verificação de presença de pílula na mão direita do médico
            for (int i = 0; i < grupoPilulasMaoDireita.childCount; i++)
                if (grupoPilulasMaoDireita.GetChild(i).gameObject.activeSelf)
                {
                    temPilulaMaoDir = true;
                    if (grupoPilulasMaoDireita.GetChild(i).name == "Pilula_" + tipoPilula)
                        jaPegouEssaPilulaMaoDir = true;
                }

            // Verificação de presença de pílula na mão esquerda do médico
            for (int i = 0; i < grupoPilulasMaoEsquerda.childCount; i++)
                if (grupoPilulasMaoEsquerda.GetChild(i).gameObject.activeSelf)
                {
                    temPilulaMaoEsq = true;
                    if (grupoPilulasMaoEsquerda.GetChild(i).name == "Pilula_" + tipoPilula)
                        jaPegouEssaPilulaMaoEsq = true;
                }

            // Regras do algoritmo a seguir:
            //   - Se não pegou uma pílula do tipo da que está com esse script atrelado:
            //      - Está com, ao menos, uma mão vazia:
            //        Opção a ser exibida: Pegar pílula
            //      - Está com as duas mãos cheias:
            //        Não mostrar opção
            //   - Se já está com uma pílula do tipo em mãos:
            //      - Está com uma mão vazia:
            //        Opção a ser exibida: Pegar outra pílula
            //      - Está com as duas mãos cheias:
            //         - As duas mãos têm pílulas do mesmo tipo:    
            //           Opção a ser exibida: Devolver as duas pílulas
            //         - Somente uma mão tem pílulas do tipo:    
            //           Opção a ser exibida: Devolver pílula

            bool ativarBarraInferior = true;

            if (!jaPegouEssaPilulaMaoDir & !jaPegouEssaPilulaMaoEsq)
            {
                if (temPilulaMaoDir & temPilulaMaoEsq)
                    ativarBarraInferior = false;
                else barraInferior.TextoBarraInferior.text = "Pegar pílula " + tipoPilula;
            }
            else if (!(jaPegouEssaPilulaMaoDir & jaPegouEssaPilulaMaoEsq))
            {
                if (temPilulaMaoDir & temPilulaMaoEsq)
                    barraInferior.TextoBarraInferior.text = "Devolver pílula " + tipoPilula;
                else barraInferior.TextoBarraInferior.text = "Pegar outra pílula " + tipoPilula;
            }
            else
                barraInferior.TextoBarraInferior.text = "Devolver as duas pílulas";

            if (ativarBarraInferior)
            {
                if (Geral.ModoDeControle.Contains("Teclado"))
                    barraInferior.TextoTeclaOpcao.text = "X";
                else if (Geral.ModoDeControle == "Controle")
                    barraInferior.TextoTeclaOpcao.text = "A";
                barraInferior.tipoPilula = tipoPilula;
                barraInferior.gameObject.SetActive(ativarBarraInferior);
            }

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