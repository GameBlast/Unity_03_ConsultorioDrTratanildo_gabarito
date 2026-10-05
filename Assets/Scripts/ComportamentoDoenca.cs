using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamentoDoenca : MonoBehaviour
{
    public bool isVerde, isVermelho, isAmarelo, isAzul, isRoxo;
    public float fatorDeslocamento;
    public bool liberadoCaminhada = false;
    private char direcaoCaminhada = 'S';
    private bool emProcessoDeMudancaDeRota = false;
    private bool iniciouProcessoDeTratamento = false;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "medicamentoVerde":
                if (isVerde) TratamentoCerto(other.gameObject); else TratamentoErrado(other.gameObject);
                break;

            case "medicamentoVermelho":
                if (isVermelho) TratamentoCerto(other.gameObject); else TratamentoErrado(other.gameObject);
                break;

            case "medicamentoAmarelo":
                if (isAmarelo) TratamentoCerto(other.gameObject); else TratamentoErrado(other.gameObject);
                break;

            case "medicamentoAzul":
                if (isAzul) TratamentoCerto(other.gameObject); else TratamentoErrado(other.gameObject);
                break;

            case "medicamentoRoxo":
                if (isRoxo) TratamentoCerto(other.gameObject); else TratamentoErrado(other.gameObject);
                break;

            case "parede":
            case "agenteDoenca":
                liberadoCaminhada = false;
                break;
        }
    }

    public void TratamentoCerto(GameObject pilula)
    {
        if (!iniciouProcessoDeTratamento)
        {
            iniciouProcessoDeTratamento = true;

            // Atualiza contador de monstros restantes
            ControllerFase.instance.ReduzirContadorDoenca(isVerde, isVermelho, isAmarelo, isAzul, isRoxo);
            ControllerFase.instance.AtualizarMensagemAgentesRestantes();

            // Remover pílula do labirinto
            Destroy(pilula);

            // Remover agente de doenças do labirinto
            Destroy(gameObject);

            // Verificar se o jogador venceu a fase
            ControllerFase.instance.VerificarCondicoesFinalizacaoFase();
        }
    }

    public void TratamentoErrado(GameObject pilula)
    {
        if (!iniciouProcessoDeTratamento && Geral.Dificuldade=="Alta")
        {
            iniciouProcessoDeTratamento = true;

            // Aumenta a velocidade do agente de doenças
            fatorDeslocamento = Mathf.Min(10, fatorDeslocamento + 2);

            // Remover pílula do labirinto
            Destroy(pilula);

            // Efeito sonoro de erro
            ControllerFase.instance.efeitoSonoroRuim.Play();

            iniciouProcessoDeTratamento = false;
        }
    }

    private void RotacionarAgente()
    {
        switch (direcaoCaminhada)
        {
            case 'N':
                gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case 'S':
                gameObject.transform.localEulerAngles = new Vector3(0, 270, 0);
                break;
            case 'L':
                gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case 'O':
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

    private void CaminharAgente()
    {
        // Aplicando fator de dificuldade à velocidade dos monstrinhos
        float novoFatorDeslocamento = fatorDeslocamento;
        if (Geral.Dificuldade == "Alta")
            novoFatorDeslocamento *= 1.5f;

        switch (direcaoCaminhada)
        {
            case 'N':
                gameObject.transform.localPosition += new Vector3(Time.deltaTime * novoFatorDeslocamento, 0, 0);
                break;
            case 'S':
                gameObject.transform.localPosition -= new Vector3(Time.deltaTime * novoFatorDeslocamento, 0, 0);
                break;
            case 'L':
                gameObject.transform.localPosition -= new Vector3(0, 0, Time.deltaTime * novoFatorDeslocamento);
                break;
            case 'O':
                gameObject.transform.localPosition += new Vector3(0, 0, Time.deltaTime * novoFatorDeslocamento);
                break;
        }
    }

    private IEnumerator MudarRota()
    {
        yield return new WaitForSeconds(0.25f);

        switch (direcaoCaminhada)
        {
            case 'N':
                if (isVerde || isRoxo) direcaoCaminhada = 'S';
                if (isAmarelo) direcaoCaminhada = 'L';
                if (isVermelho || isAzul) direcaoCaminhada = 'O';
                break;
            case 'S':
                if (isVerde || isAzul) direcaoCaminhada = 'L';
                if (isRoxo) direcaoCaminhada = 'O';
                if (isVermelho || isAmarelo) direcaoCaminhada = 'N';
                break;
            case 'L':
                if (isVerde || isAmarelo) direcaoCaminhada = 'O';
                if (isAzul || isRoxo) direcaoCaminhada = 'N';
                if (isVermelho) direcaoCaminhada = 'S';
                break;
            case 'O':
                if (isVerde) direcaoCaminhada = 'N';
                if (isAmarelo || isAzul) direcaoCaminhada = 'S';
                if (isVermelho || isRoxo) direcaoCaminhada = 'L';
                break;
        }

        RotacionarAgente();

        yield return new WaitForSeconds(0.25f);
        emProcessoDeMudancaDeRota = false;
        liberadoCaminhada = true;
    }

    private void Update()
    {
        if (liberadoCaminhada)
            CaminharAgente();
        else
            if (!emProcessoDeMudancaDeRota)
            {
                emProcessoDeMudancaDeRota = true;
                StartCoroutine(MudarRota());
            }
    }

    internal void OnTriggerExterno(Collider colisorExterno)
    {
        OnTriggerEnter(colisorExterno);
    }

}
