using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Jogo"); // Substitua com o nome da sua cena do jogo
    }
}
