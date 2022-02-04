package Dashboard.Coördinator;

import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertTrue;


public class STC2_1 {


    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();

        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("STAGECOORDINATOR@OUTLOOK.COM");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(2);


        //FOTO
        WebElement ImageFile = driver.findElement(By.xpath("//img[contains(@id,'pxlLogo')]"));

        Boolean ImagePresent = (Boolean) ((JavascriptExecutor)driver).executeScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", ImageFile);
        if (!ImagePresent)
        {
            System.out.println("Image not displayed.");
        }

        //BUTTONS
        WebElement buttonStart = driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/ul/li[2]/div/a[1]"));
        boolean bButtonStart = false;

        if(buttonStart.getText().equals("START")){
            bButtonStart = true;
        }else{
            System.out.println("button START niet aanwezig");
        }

        WebElement buttonToevoegen = driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/ul/li[2]/div/a[2]"));
        boolean bbuttonToevoegen = false;

        if(buttonToevoegen.getText().equals("GEBRUIKERS TOEVOEGEN")){
            bbuttonToevoegen = true;
        }else{
            System.out.println("button GEBRUIKERS TOEVOEGEN  niet aanwezig");
        }


        WebElement buttonAfmelden = driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/ul/li[2]/div/a[3]"));
        boolean bbuttonAfmelden = false;

        if(buttonAfmelden.getText().equals("AFMELDEN")){
            bbuttonAfmelden = true;
        }else{
            System.out.println("button AFMELDEN niet aanwezig");
        }

        boolean bbuttonGA = false;

        if(driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/p/button")).isDisplayed())
        {
            bbuttonGA = true;
        }else{
            System.out.println("button GOED- EN AFGEKEURDE  OPDRACHTEN niet aanwezig");
        }
        //LABELS

        WebElement labelEvalueren = driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/div[1]/table[1]/thead"));
        boolean blabelEvalueren = false;

        if(labelEvalueren.getText().equals("TE EVALUEREN")){
            blabelEvalueren = true;
        }else{
            System.out.println("tabel TE EVALUEREN niet aanwezig");
        }

        WebElement labelHerziening= driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/div[1]/table[2]/thead"));
        boolean blabelHerziening = false;

        if(labelHerziening.getText().equals("HERZIENING")){
            blabelHerziening= true;
        }else{
            System.out.println("tabel HERZIENING  niet aanwezig");
        }


        WebElement labelEvalueren2 = driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/div[2]/table[1]/thead"));
        boolean blabelEvalueren2 = false;

        if(labelEvalueren2.getText().equals("TE EVALUEREN")){
            blabelEvalueren2 = true;
        }else{
            System.out.println("tabel TE EVALUEREN niet aanwezig");
        }


        WebElement labelControle= driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/div[2]/table[2]/thead"));
        boolean blabelControle = false;

        if(labelControle.getText().equals("CONTROLE")){
            blabelControle = true;
        }else{
            System.out.println("tabel CONTROLE niet aanwezig");
        }


        WebElement header= driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/h2"));
        boolean bheader = false;

        if(header.getText().equals("STAGEAANVRAGEN")){
            bheader = true;
        }else{
            System.out.println("tabel STAGEAANVRAGEN niet aanwezig");
        }

        boolean correctLayout = false ;

        if(ImagePresent && blabelEvalueren && blabelHerziening && blabelEvalueren2 && blabelControle && bButtonStart && bbuttonToevoegen && bbuttonAfmelden && bheader && bbuttonGA){
            correctLayout = true;
        }

        assertTrue(correctLayout);

    }
}