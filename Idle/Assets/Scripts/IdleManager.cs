using UnityEngine;
using UnityEngine.UI;

public class IdleManager : MonoBehaviour
{
    
    //Text
        public Text coinsText;
        public Text coinsPerSecText;
        public Text productionUpgrade1Text;
        public Text productionUpgrade2Text;
        public Text sealsBoostText;
        public Text sealsText;
        public Text sealsToGetText;
        public Text buyProductionUp2MaxText;
        public Text buyProductionUp1MaxText;
    //Double
        public double seals;
        public double sealsBoosting;
        public double sealsToGet;
        public double coins;
        public double coinsPerSeconds; 
        public double productionPower1; 
        public double productionPower2; 
        public double prestigeCost;

    //Integer
        public int    productionUpgrade1Level;
        public int    productionUpgrade2Level;
    
    //Image
        public Image prestigeImageBar;
    
    //CanvasGroup
        public CanvasGroup options;
        public CanvasGroup mainMenuGroup;
        public CanvasGroup idle1Group;

    public void HardReset(){
        seals = 0;
        sealsBoosting = 0;
        sealsToGet = 0;
        coins = 10;
        coinsPerSeconds = 0; 
        productionUpgrade1Level = 0;
        productionPower1 = 1; 
        productionUpgrade2Level = 0;
        productionPower2 = 25; 
        prestigeCost = 1000;
    }


    public void Start(){
        Application.targetFrameRate = 60;
        canvasGroupChanger(true,mainMenuGroup);
        canvasGroupChanger(false,options);
        canvasGroupChanger(false,idle1Group);
        Load();
    }

    public void canvasGroupChanger(bool x, CanvasGroup y)
    {
        if(x)
        {
            y.alpha = 1;
            y.interactable = true;
            y.blocksRaycasts = true;
            return;
        }
        y.alpha = 0;
        y.interactable = false;
        y.blocksRaycasts = false;

    }

    public void Load(){
        coins = double.Parse(PlayerPrefs.GetString("Coins","10"));
        productionPower1 = double.Parse(PlayerPrefs.GetString("productionPower1","1"));
        productionPower2 = double.Parse(PlayerPrefs.GetString("productionPower2","25"));
        seals = double.Parse(PlayerPrefs.GetString("seals","0"));
        sealsBoosting = double.Parse(PlayerPrefs.GetString("sealsBoosting","1.00"));
        sealsToGet = double.Parse(PlayerPrefs.GetString("sealsToGet","0"));
        prestigeCost = double.Parse(PlayerPrefs.GetString("prestigeCost","1000"));
        productionUpgrade1Level = PlayerPrefs.GetInt("productionUpgrade1Level", 0);
        productionUpgrade2Level = PlayerPrefs.GetInt("productionUpgrade2Level", 0);

    }

    public void Save(){
        PlayerPrefs.SetString("Coins", coins.ToString());
        PlayerPrefs.SetString("productionPower1", productionPower1.ToString());
        PlayerPrefs.SetString("productionPower2", productionPower2.ToString());
        PlayerPrefs.SetString("seals", seals.ToString());
        PlayerPrefs.SetString("sealsBoosting", sealsBoosting.ToString());
        PlayerPrefs.SetString("sealsToGet", sealsToGet.ToString());
        PlayerPrefs.SetString("prestigeCost", prestigeCost.ToString());
        PlayerPrefs.SetInt("productionUpgrade1Level", productionUpgrade1Level);
        PlayerPrefs.SetInt("productionUpgrade2Level", productionUpgrade2Level);
        
    }

    public void goToIdle1()
    {
        canvasGroupChanger(false,mainMenuGroup);
        canvasGroupChanger(true, idle1Group);
    }

    public void goToMainMenu()
    {
        canvasGroupChanger(true,mainMenuGroup);
        canvasGroupChanger(false, idle1Group);
    }
    public void Update(){
        

       
        //Seals
            sealsText.text = "Seals : " + scientificNotations(seals, s : "F2");
            sealsToGet = (100 * System.Math.Sqrt( coins / 1e7));
            sealsToGetText.text = "Prestige :\n+" + scientificNotations(sealsToGet, s : "F2") + " Seals";
            sealsBoosting = (seals*0.1) + 1;
            sealsBoostText.text = scientificNotations(sealsBoosting, s :"F2") + "X";


        //ModifDeValeur
        
            coinsPerSeconds = ((productionPower1 * productionUpgrade1Level) + (productionPower2 * productionUpgrade2Level)) * sealsBoosting;
            coins += coinsPerSeconds * Time.deltaTime;

        //Text            
            coinsText.text = "Coins : " + scientificNotations(coins, s: "F2");
            coinsPerSecText.text = scientificNotations(coinsPerSeconds,s : "F2") + " Coins/s";
            var cost2 = 250 * (System.Math.Pow(1.09,productionUpgrade2Level));
            productionUpgrade2Text.text = "Buy A Swamp\nCost : " + scientificNotations(cost2, s: "F2") + " coins\nPower : +"+ scientificNotations((productionPower2* sealsBoosting), s :"F2") +" coins/s\nLevel : " + scientificNotations(productionUpgrade2Level, s : "F0");

            var cost = 10 * (System.Math.Pow(1.09,productionUpgrade1Level));
            productionUpgrade1Text.text = "Buy A Shrek\nCost : " + scientificNotations(cost, s: "F2") + " coins\nPower : +"+ scientificNotations((productionPower1* sealsBoosting), s :"F2") +" coins/s\nLevel : " + scientificNotations(productionUpgrade1Level, s : "F0");

        
        if (coins / prestigeCost < 0.01)
        {
            prestigeImageBar.fillAmount = 0;
        }else if (coins / prestigeCost > 10)
        {
            prestigeImageBar.fillAmount = 1;
        }else{
            prestigeImageBar.fillAmount = (float)(coins / prestigeCost);
        }

        buyProductionUp1MaxText.text = "Buy Max (" + BuyProductionUp1MaxCount() + ")";
        buyProductionUp2MaxText.text = "Buy Max (" + BuyProductionUp2MaxCount() + ")";
        Save();
    }

    public string scientificNotations(double d, string s)
    {
        if (d > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(d))));
            var mantisse = (d / System.Math.Pow(10, exponent));
            return mantisse.ToString(s)+ "e" + exponent;
        }
        return d.ToString(s);
    }

        public string scientificNotations(float d, string s)
    {
        if (d > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(d))));
            var mantisse = (d / System.Math.Pow(10, exponent));
            return mantisse.ToString(s)+ "e" + exponent;
        }
        return d.ToString(s);
    }

    public void Prestige()
    {
        if (coins > prestigeCost){
            coins = 10;
            productionPower1 *= 1.07 ;
            productionPower2 *= 1.07;
            productionUpgrade1Level = 0;
            productionUpgrade2Level = 0;
            seals += sealsToGet;
            prestigeCost *= 1.14;
        }
    }

    public double BuyProductionUp1MaxCount()
    {
        var b = 10;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade1Level;
        var n = System.Math.Floor(System.Math.Log(c * (r - 1) / (b * System.Math.Pow(r,k)) + 1,r));
        return n;
    }

    public double BuyProductionUp2MaxCount()
    {
        var b = 250;
        var c = coins;
        var r = 1.09;
        var k = productionUpgrade2Level;
        var n = System.Math.Floor(System.Math.Log(c * (r - 1) / (b * System.Math.Pow(r,k)) + 1,r));
        return n;
    }

    public void buyUpgrade(string upID)
    {
        switch(upID)
        {
            case "P1" :
                var cost1 = 10 * (System.Math.Pow(1.09,productionUpgrade1Level));
                if(coins >= cost1)
                {
                    productionUpgrade1Level ++;
                    coins -= cost1;
                }
                break;

            case "P1Max" :
                var b1M = 10;
                var c1M = coins;
                var r1M = 1.07;
                var k1M = productionUpgrade2Level;
                var n1M = System.Math.Floor(System.Math.Log(c1M * (r1M - 1) / (b1M * System.Math.Pow(r1M,k1M)) + 1,r1M));
                var cost1M = b1M * (System.Math.Pow(r1M,k1M) * (System.Math.Pow(r1M,n1M) - 1 ) / (r1M-1));
                if(coins >= cost1M)
                {
                    productionUpgrade1Level += (int)n1M;
                    coins -= cost1M;
                }
                break;

            case "P2" :
                var cost2 = 250 * (System.Math.Pow(1.09,productionUpgrade2Level));
                if(coins >= cost2)
                {
                    productionUpgrade2Level ++;
                    coins -= cost2;
                }
                break;

            case "P2Max" :
                var b2M = 250;
                var c2M = coins;
                var r2M = 1.09;
                var k2M = productionUpgrade2Level;
                var n2M = System.Math.Floor(System.Math.Log(c2M * (r2M - 1) / (b2M * System.Math.Pow(r2M,k2M)) + 1,r2M));
                var cost2M = b2M * (System.Math.Pow(r2M,k2M) * (System.Math.Pow(r2M,n2M) - 1 ) / (r2M-1));
                if(coins >= cost2M)
                {
                    productionUpgrade2Level += (int)n2M;
                    coins -= cost2M;
                } 
                break;
            default :
                Debug.Log(message : "I am not assigned to a proper upgrade");
                break;                
        }
    }
}

