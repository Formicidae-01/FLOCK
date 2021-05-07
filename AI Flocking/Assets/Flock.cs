using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Refer�ncia do administrador de Flock (administra a velocidade, rota��o, destino do objeto, etc)
    FlockManager myManager;
    // Vari�vel de velocidade atual do peixe
    public float speed;
    // Bool que diz se o peixe est� virando/rotacionando ou n�o
    bool turning = false;

    private void Start()
    {
        // Obtendo o administrador atrav�s do m�todo que encontra um objeto do tipo
        SetManager(FindObjectOfType<FlockManager>());
        // Definindo a velocidade com base na velocidade m�nima e m�xima que o objeto pode ter
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        // Criando limites do cen�rio, com base na posi��o do administrador e os limites de nata��o
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        // Executa uma fun��o se este objeto estiver dentro do limite do cen�rio
        if (!b.Contains(transform.position))
        {
            // Define que o objeto est� rotacionando se estiver no limite do cen�rio
            turning = true;
        }

        // Define que o objeto n�o est� rotacionando se n�o estiver no limite do cen�rio
        else
        {
            turning = false;
        }

        // Executa fun��o caso o objeto esteja rotacionando
        if (turning)
        {
            // Define a dire��o de rota��o, sendo o destino final a posi��o do administrador
            Vector3 direction = myManager.transform.position - transform.position;
            // Executa a rota��o atrav�s do m�todo Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * myManager.rotationSpeed);
        }

        else
        {
            // Executa uma func�o com 10% de chance
            if (Random.Range(0,100) < 10)
            {
                // Atualiza a velocidade do objeto, para que mude constantemente
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);                
            }

            // Executa uma fun��o com 20% de chance
            if (Random.Range(0,100) < 20)
            {
                // Fun��o que aplica regras, administra colis�o com outros objetos Flock
                ApplyRules();
            }
            
        }

        // Movimentando o personagem para sua frente
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        // Criando uma lista que receber� os outros peixes
        GameObject[] fishes;
        // Preenchendo a lista com os peixes do administrador
        fishes = myManager.fishes;

        // Criando dois vetores vazios
        Vector3 vcentre = Vector3.zero, vavoid = Vector3.zero;

        // Criando dois floats, um ser� usado para a velocidade de "evas�o" ao estar pr�ximo de outro peixe
        // outro ser� usado para receber a dist�ncia entre outro peixe
        float gSpeed = 0.01f, nDistance;

        // Criando vari�vel que identifica o n�mero de peixes pr�ximos
        int groupSize = 0;

        // Executa m�todo para cada peixe na lista de peixes criada no in�cio do m�todo
        foreach(GameObject fish in fishes)
        {
            // Realizando c�lculo com base nos outros peixes, se o peixe atual da lista for este objeto, n�o ser�o feitos c�lculos
            if (fish != gameObject)
            {
                // Verificando dist�ncia entre esse peixe e os demais
                nDistance = Vector3.Distance(fish.transform.position, transform.position);
                // executa fun��o se a dist�ncia entre outro peixe for menor do que a dist�ncia m�nima
                if (nDistance <= myManager.neighbourDistance)
                {
                    // Define o centro de encontro entre os peixes
                    vcentre += fish.transform.position;
                    // Aumenta a quantidade de peixes que se encontraram
                    groupSize++;

                    // Executa m�todo se a dist�ncia for menor que 1
                    if (nDistance < 1f)
                    {
                        // Gera posi��o para evitar o peixe pr�ximo
                        vavoid = vavoid + (transform.position - fish.transform.position);
                    }

                    // Recebendo o script Flock do outro peixe
                    Flock otherFlock = fish.GetComponent<Flock>();
                    // Definindo a velocidade de evas�o
                    gSpeed = gSpeed + otherFlock.speed;
                }
            }
        }

        // Executa m�todo se o grupo de peixes pr�ximos for maior que 0
        if (groupSize > 0)
        {
            // Definindo o centro de encontro entre os peixes com base na quantidade de peixes e outras posi��es
            vcentre = vcentre / groupSize + (myManager.goalPos - transform.position);
            // Definindo a velocidade que o peixe assumir� agora com base no tamanho do grupo e velocidade de evas�o
            speed = gSpeed / groupSize;

            // Define dire��o para qual o peixe rotacionar� para se afastar dos outros
            Vector3 direction = (vcentre + vavoid) - transform.position;
            // Executa m�todo se a dire��o de rota��o n�o for 0
            if (direction != Vector3.zero)
            {
                // rotacionando peixe atrav�s do m�todo slerp
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }

    // M�todo que atribui o administrador a esse objeto
    public void SetManager(FlockManager fManager)
    {
        // Definindo o administrador desse objeto como o administrador decidido atrav�s do par�metro acima
        myManager = fManager;
    }
}
