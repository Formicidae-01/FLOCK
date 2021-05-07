using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Referência do administrador de Flock (administra a velocidade, rotação, destino do objeto, etc)
    FlockManager myManager;
    // Variável de velocidade atual do peixe
    public float speed;
    // Bool que diz se o peixe está virando/rotacionando ou não
    bool turning = false;

    private void Start()
    {
        // Obtendo o administrador através do método que encontra um objeto do tipo
        SetManager(FindObjectOfType<FlockManager>());
        // Definindo a velocidade com base na velocidade mínima e máxima que o objeto pode ter
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        // Criando limites do cenário, com base na posição do administrador e os limites de natação
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        // Executa uma função se este objeto estiver dentro do limite do cenário
        if (!b.Contains(transform.position))
        {
            // Define que o objeto está rotacionando se estiver no limite do cenário
            turning = true;
        }

        // Define que o objeto não está rotacionando se não estiver no limite do cenário
        else
        {
            turning = false;
        }

        // Executa função caso o objeto esteja rotacionando
        if (turning)
        {
            // Define a direção de rotação, sendo o destino final a posição do administrador
            Vector3 direction = myManager.transform.position - transform.position;
            // Executa a rotação através do método Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * myManager.rotationSpeed);
        }

        else
        {
            // Executa uma funcão com 10% de chance
            if (Random.Range(0,100) < 10)
            {
                // Atualiza a velocidade do objeto, para que mude constantemente
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);                
            }

            // Executa uma função com 20% de chance
            if (Random.Range(0,100) < 20)
            {
                // Função que aplica regras, administra colisão com outros objetos Flock
                ApplyRules();
            }
            
        }

        // Movimentando o personagem para sua frente
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        // Criando uma lista que receberá os outros peixes
        GameObject[] fishes;
        // Preenchendo a lista com os peixes do administrador
        fishes = myManager.fishes;

        // Criando dois vetores vazios
        Vector3 vcentre = Vector3.zero, vavoid = Vector3.zero;

        // Criando dois floats, um será usado para a velocidade de "evasão" ao estar próximo de outro peixe
        // outro será usado para receber a distância entre outro peixe
        float gSpeed = 0.01f, nDistance;

        // Criando variável que identifica o número de peixes próximos
        int groupSize = 0;

        // Executa método para cada peixe na lista de peixes criada no início do método
        foreach(GameObject fish in fishes)
        {
            // Realizando cálculo com base nos outros peixes, se o peixe atual da lista for este objeto, não serão feitos cálculos
            if (fish != gameObject)
            {
                // Verificando distância entre esse peixe e os demais
                nDistance = Vector3.Distance(fish.transform.position, transform.position);
                // executa função se a distância entre outro peixe for menor do que a distância mínima
                if (nDistance <= myManager.neighbourDistance)
                {
                    // Define o centro de encontro entre os peixes
                    vcentre += fish.transform.position;
                    // Aumenta a quantidade de peixes que se encontraram
                    groupSize++;

                    // Executa método se a distância for menor que 1
                    if (nDistance < 1f)
                    {
                        // Gera posição para evitar o peixe próximo
                        vavoid = vavoid + (transform.position - fish.transform.position);
                    }

                    // Recebendo o script Flock do outro peixe
                    Flock otherFlock = fish.GetComponent<Flock>();
                    // Definindo a velocidade de evasão
                    gSpeed = gSpeed + otherFlock.speed;
                }
            }
        }

        // Executa método se o grupo de peixes próximos for maior que 0
        if (groupSize > 0)
        {
            // Definindo o centro de encontro entre os peixes com base na quantidade de peixes e outras posições
            vcentre = vcentre / groupSize + (myManager.goalPos - transform.position);
            // Definindo a velocidade que o peixe assumirá agora com base no tamanho do grupo e velocidade de evasão
            speed = gSpeed / groupSize;

            // Define direção para qual o peixe rotacionará para se afastar dos outros
            Vector3 direction = (vcentre + vavoid) - transform.position;
            // Executa método se a direção de rotação não for 0
            if (direction != Vector3.zero)
            {
                // rotacionando peixe através do método slerp
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Método que atribui o administrador a esse objeto
    public void SetManager(FlockManager fManager)
    {
        // Definindo o administrador desse objeto como o administrador decidido através do parâmetro acima
        myManager = fManager;
    }
}
