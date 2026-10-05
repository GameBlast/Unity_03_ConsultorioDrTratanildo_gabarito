using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontaLabirinto : MonoBehaviour
{
    public GameObject cuboReferencia;
    public Vector3[] posicoesCubos, dimensoesCubos;
    internal GameObject[] cubosCriados = new GameObject[0];

    public Vector3[] posicoesPilulas, posicoesDoencas;
    public GameObject[] pilulasReferencia, doencasReferencia;

    public Transform grupoMedicamentos, grupoDoencas;

    internal GameObject[] pilulasCriadas = new GameObject[0];
    internal GameObject[] doencasCriadas = new GameObject[0];

    public GameObject pilulaRef_azul, pilulaRef_verde, pilulaRef_roxa, pilulaRef_amarela, pilulaRef_vermelha;
    public GameObject doencaRef_azul, doencaRef_verde, doencaRef_roxa, doencaRef_amarela, doencaRef_vermelha;

    public void Montar()
    {
        // Apagar cubos, pílulas e agentes de doenças, se já houver
        foreach (GameObject go in cubosCriados)
            DestroyImmediate(go);
        foreach (GameObject go in pilulasCriadas)
            DestroyImmediate(go);
        foreach (GameObject go in doencasCriadas)
            DestroyImmediate(go);

        // Criar novos cubos do labirinto, com base nos parâmetros
        // indicados em posicoesCubos e dimensoesCubos

        cubosCriados = new GameObject[posicoesCubos.Length];
        for (int i = 0; i < posicoesCubos.Length; i++)
        {
            cubosCriados[i] = Instantiate(cuboReferencia, cuboReferencia.transform.parent);
            cubosCriados[i].SetActive(true);
            cubosCriados[i].name = "CuboObstaculo_" + i;
            cubosCriados[i].transform.localPosition = posicoesCubos[i];
            cubosCriados[i].transform.localScale = dimensoesCubos[i];
        }

        // Posicionar no labirinto os agentes de doenças

        pilulasCriadas = new GameObject[0];
        doencasCriadas = new GameObject[posicoesDoencas.Length];

        /*
        for (int i = 0; i < posicoesPilulas.Length; i++)
        {
            pilulasCriadas[i] = Instantiate(pilulasReferencia[i], grupoMedicamentos);
            pilulasCriadas[i].SetActive(true);
            pilulasCriadas[i].name = "PilulaSaude_" + i;
            pilulasCriadas[i].transform.localPosition = posicoesPilulas[i];
        }
        */

        for (int i = 0; i < posicoesDoencas.Length; i++)
        {
            doencasCriadas[i] = Instantiate(doencasReferencia[i], grupoDoencas);
            doencasCriadas[i].SetActive(true);
            doencasCriadas[i].name = "AgenteDoencas_" + i;
            doencasCriadas[i].transform.localPosition = posicoesDoencas[i];
        }
    }

    public void LimparValoresVetoresVariaveis()
    {
        posicoesDoencas = new Vector3[0];
        posicoesPilulas = new Vector3[0];
        posicoesCubos = new Vector3[0];
        dimensoesCubos = new Vector3[0];
        doencasReferencia = new GameObject[0];
        pilulasReferencia = new GameObject[0];
    }

    public void ImportarValoresVetoresVariaveis(int faseCorrente, Vector3[] PosicaoMonstrosAzuis, Vector3[] PosicaoMonstrosVerdes, Vector3[] PosicaoMonstrosRoxos,
        Vector3[] PosicaoMonstrosAmarelos, Vector3[] PosicaoMonstrosVermelhos, Vector3[] PosicaoObstaculos, Vector3[] DimensaoObstaculos, Vector2[] PosicaoInicialPilulas,
        out int m_azuis, out int m_verdes, out int m_roxos, out int m_amarelos, out int m_vermelhos)
    {
        // Zerar o valor inicial dos monstrinhos restantes
        m_azuis = 0;
        m_verdes = 0;
        m_roxos = 0;
        m_amarelos = 0;
        m_vermelhos = 0;

        // Comandos para importação do tipo e posicionamento
        // dos agentes de doenças (monstrinhos)
        int i = 0;
        foreach (Vector3 posMonstroAzul in PosicaoMonstrosAzuis)
            if (posMonstroAzul.z == faseCorrente)
            {
                System.Array.Resize(ref posicoesDoencas, posicoesDoencas.Length + 1);
                System.Array.Resize(ref doencasReferencia, doencasReferencia.Length + 1);
                posicoesDoencas[i] = posMonstroAzul;
                doencasReferencia[i] = doencaRef_azul;
                i++;
                m_azuis++;
            }

        foreach (Vector3 posMonstroVerde in PosicaoMonstrosVerdes)
            if (posMonstroVerde.z == faseCorrente)
            {
                System.Array.Resize(ref posicoesDoencas, posicoesDoencas.Length + 1);
                System.Array.Resize(ref doencasReferencia, doencasReferencia.Length + 1);
                posicoesDoencas[i] = posMonstroVerde;
                doencasReferencia[i] = doencaRef_verde;
                i++;
                m_verdes++;
            }

        foreach (Vector3 posMonstroRoxo in PosicaoMonstrosRoxos)
            if (posMonstroRoxo.z == faseCorrente)
            {
                System.Array.Resize(ref posicoesDoencas, posicoesDoencas.Length + 1);
                System.Array.Resize(ref doencasReferencia, doencasReferencia.Length + 1);
                posicoesDoencas[i] = posMonstroRoxo;
                doencasReferencia[i] = doencaRef_roxa;
                i++;
                m_roxos++;
            }

        foreach (Vector3 posMonstroAmarelo in PosicaoMonstrosAmarelos)
            if (posMonstroAmarelo.z == faseCorrente)
            {
                System.Array.Resize(ref posicoesDoencas, posicoesDoencas.Length + 1);
                System.Array.Resize(ref doencasReferencia, doencasReferencia.Length + 1);
                posicoesDoencas[i] = posMonstroAmarelo;
                doencasReferencia[i] = doencaRef_amarela;
                i++;
                m_amarelos++;
            }

        foreach (Vector3 posMonstroVermelho in PosicaoMonstrosVermelhos)
            if (posMonstroVermelho.z == faseCorrente)
            {
                System.Array.Resize(ref posicoesDoencas, posicoesDoencas.Length + 1);
                System.Array.Resize(ref doencasReferencia, doencasReferencia.Length + 1);
                posicoesDoencas[i] = posMonstroVermelho;
                doencasReferencia[i] = doencaRef_vermelha;
                i++;
                m_vermelhos++;
            }

        // Comandos para importação do posicionamento inicial
        // das pílulas de saúde que serão ingeridas pelos pacientes
        posicoesPilulas = new Vector3[PosicaoInicialPilulas.Length];
        for (i = 0; i < posicoesPilulas.Length; i++)
        {
            posicoesPilulas[i].x = PosicaoInicialPilulas[i].x;
            posicoesPilulas[i].y = 0.5f;
            posicoesPilulas[i].z = PosicaoInicialPilulas[i].y;
        }

        // Comandos para importação da dimensão e do posicionamento
        // dos blocos que compõem o labirinto
        i = 0;
        for (int j = 0; j < PosicaoObstaculos.Length; j++)
            if (PosicaoObstaculos[j].z == faseCorrente)
            {
                System.Array.Resize(ref posicoesCubos, posicoesCubos.Length + 1);
                System.Array.Resize(ref dimensoesCubos, dimensoesCubos.Length + 1);
                posicoesCubos[i] = PosicaoObstaculos[j];
                dimensoesCubos[i] = DimensaoObstaculos[j];
                i++;
            }

        // Realizar conversão entre coordenadas Y e Z dos valores
        // de Vector3 importados até o momento
        for (i = 0; i < posicoesDoencas.Length; i++)
        {
            posicoesDoencas[i].z = posicoesDoencas[i].y;
            posicoesDoencas[i].y = 0;
        }

        for (i = 0; i < posicoesCubos.Length; i++)
        {
            posicoesCubos[i].z = posicoesCubos[i].y;
            posicoesCubos[i].y = 0;
        }

        for (i = 0; i < dimensoesCubos.Length; i++)
        {
            dimensoesCubos[i].z = dimensoesCubos[i].y;
            dimensoesCubos[i].y = 1;
        }
    }

    public void InserirPilulas(string pilula_1, string pilula_2)
    {
        if (pilula_1 != "N/A")
            ColocarPilulaNoLab(pilula_1);

        if (pilula_2 != "N/A")
            ColocarPilulaNoLab(pilula_2);
    }

    private void ColocarPilulaNoLab(string pilula)
    {
        System.Array.Resize(ref pilulasCriadas, pilulasCriadas.Length + 1);
        int pos = pilulasCriadas.Length - 1;

        switch (pilula)
        {
            case "Pilula_amarela":
                pilulasCriadas[pos] = Instantiate(pilulaRef_amarela, grupoMedicamentos);
                break;
            case "Pilula_azul":
                pilulasCriadas[pos] = Instantiate(pilulaRef_azul, grupoMedicamentos);
                break;
            case "Pilula_roxa":
                pilulasCriadas[pos] = Instantiate(pilulaRef_roxa, grupoMedicamentos);
                break;
            case "Pilula_verde":
                pilulasCriadas[pos] = Instantiate(pilulaRef_verde, grupoMedicamentos);
                break;
            case "Pilula_vermelha":
                pilulasCriadas[pos] = Instantiate(pilulaRef_vermelha, grupoMedicamentos);
                break;
        }

        pilulasCriadas[pos].name = "PilulaSaude_" + pos;
        pilulasCriadas[pos].transform.localPosition = posicoesPilulas[pos % posicoesPilulas.Length];

        //Validação da posição da pílula (tratamento de colisões)
        bool colisao = true;
        Vector3 posPilula = pilulasCriadas[pos].transform.position;
        while (colisao)
        {
            if (Physics.OverlapSphere(posPilula, 0.5f).Length > 0)
                posPilula -= new Vector3(0, 0, 0.5f);
            else 
                colisao = false;
        }
        pilulasCriadas[pos].transform.position = posPilula;

        // Aplicar uma pequena força para movimentar a pílula
        Vector3 forcinha = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));
        pilulasCriadas[pos].GetComponent<Rigidbody>().AddForce(forcinha);

        // Ativar pílula
        pilulasCriadas[pos].SetActive(true);
    }

    private void OnEnable()
    {
        Montar();
    }
}
