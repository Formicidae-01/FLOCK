using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // Prefab de peixe, para serem criados na cena
    public GameObject fishPrefab;
    // Quantidade de peixes a serem criados
    public int fishAmount;
    // Array de objetos que guarda os peixes criados
    public GameObject[] fishes;
    // Limite de nata��o dos peixes (dist�ncia para onde podem ir
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    // Posi��o de destino dos peixes
    public Vector3 goalPos;

    // o Header faz com que seja criado um texto que pode ser visualizado na engine
    [Header("Configura��es do Cardume")]
    // "Range" cria um slider com um valor m�nimo e m�ximo, funciona como um slider de volume em r�dios
    [Range(0f, 5f)]
    // Velocidade m�nima dos peixes
    public float minSpeed;
    [Range(0f, 5f)]
    // Velocidade m�xima dos peixes
    public float maxSpeed;
    [Range(0f, 5f)]
    // Dist�ncia m�nima que um peixe pode ter do outro
    public float neighbourDistance;
    [Range(0f, 5f)]
    // Velocidade de rota��o dos peixes
    public float rotationSpeed;

    public void Start()
    {
        // Criando uma lista de objetos com o tamanho da quantidade de peixes
        fishes = new GameObject[fishAmount];
        // Executa um procedimento no mesmo n�mero de vezes que a quantidade de peixes
        for (int i = 0; i < fishAmount; i++)
        {
            // Definindo a posi��o em que o peixe ser� instanciado, usando como base os limites para onde o peixe pode se mover
            Vector3 pos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
            // Instanciando o peixe na posi��o definida anteriormente
            fishes[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }

        // Definindo a posi��o de destino como a posi��o desse objeto
        goalPos = transform.position;
    }

    public void Update()
    {
        // Executando uma fun��o com 10% de chance
        if (Random.Range(0, 100) < 10)
        {
            // Atualizando constantemente a posi��o de destino, com base na posi��o desse objeto e os limites de nata��o
            goalPos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
        }
    }
}
