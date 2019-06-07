using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        PlayerPrefs.DeleteAll();
                
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Recursos
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // Referencias 
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu;

    // Logica
    public int ouro;
    public int exp;

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    
    // Upgrade da Arma
    public bool TryUpgradeWeapon()
    {
        // Verifica se a arma esta no nivel maximo
        if(weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if(ouro >= weaponPrices[weapon.weaponLevel])
        {
            ouro -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Barra de vida
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Sistema de Experiência
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (exp >= add)
        {
            add += xpTable[r];
            r++;

            if(r == xpTable.Count) // Nivel maximo
            {
                return r;
            }
        }

        return r;
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while( r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        exp += xp;
        if(currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        player.OnLevelUp();
        OnHitpointChange();
    }

    // Carregamento do load da cena
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death Menu e Restart
    public void Restart()
    {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Restart();
    }

    // Save State
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += ouro.ToString() + "|";
        s += exp.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s); 
    }
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Mudar Skin do player
        ouro = int.Parse(data[1]);

        // Experiencia
        exp = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());
        }
        

        // Mudar level da arma
        weapon.weaponLevel = int.Parse(data[3]);
    }
}
