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
    // Limite de natação dos peixes (distância para onde podem ir
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    // Posição de destino dos peixes
    public Vector3 goalPos;

    // o Header faz com que seja criado um texto que pode ser visualizado na engine
    [Header("Configurações do Cardume")]
    // "Range" cria um slider com um valor mínimo e máximo, funciona como um slider de volume em rádios
    [Range(0f, 5f)]
    // Velocidade mínima dos peixes
    public float minSpeed;
    [Range(0f, 5f)]
    // Velocidade máxima dos peixes
    public float maxSpeed;
    [Range(0f, 5f)]
    // Distância mínima que um peixe pode ter do outro
    public float neighbourDistance;
    [Range(0f, 5f)]
    // Velocidade de rotação dos peixes
    public float rotationSpeed;

    public void Start()
    {
        // Criando uma lista de objetos com o tamanho da quantidade de peixes
        fishes = new GameObject[fishAmount];
        // Executa um procedimento no mesmo número de vezes que a quantidade de peixes
        for (int i = 0; i < fishAmount; i++)
        {
            // Definindo a posição em que o peixe será instanciado, usando como base os limites para onde o peixe pode se mover
            Vector3 pos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
            // Instanciando o peixe na posição definida anteriormente
            fishes[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }

        // Definindo a posição de destino como a posição desse objeto
        goalPos = transform.position;
    }

    public void Update()
    {
        // Executando uma função com 10% de chance
        if (Random.Range(0, 100) < 10)
        {
            // Atualizando constantemente a posição de destino, com base na posição desse objeto e os limites de natação
            goalPos = transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
        }
    }
}
