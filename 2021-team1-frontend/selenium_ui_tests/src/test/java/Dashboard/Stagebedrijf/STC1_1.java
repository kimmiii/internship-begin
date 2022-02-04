package Dashboard.Stagebedrijf;


import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;


public class STC1_1 {


    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();

        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("stagebedrijf@outlook.com");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(2);



    /*
        WebElement textDemo = driver.findElement(By.xpath("//*[text()='Niet alle velden zijn correct ingevuld.']"));
        //String melding = textDemo.getText();
        Boolean present = false ;

        if(textDemo.isDisplayed())
        {
            present = true ;
        }
*/
        //FOTO
        WebElement ImageFile = driver.findElement(By.xpath("//img[contains(@id,'pxlLogo')]"));

        Boolean ImagePresent = (Boolean) ((JavascriptExecutor)driver).executeScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", ImageFile);
        if (!ImagePresent)
        {
            System.out.println("Image not displayed.");
        }

        //BUTTONS
        WebElement buttonStart = driver.findElement(By.xpath("html/body/app-root/div/app-company/div/nav/ul/li[2]/div/a[1]"));
        boolean bButtonStart = false;

        if(buttonStart.getText().equals("START")){
            bButtonStart = true;
        }else{
            System.out.println("button START niet aanwezig");
        }

        WebElement buttonAanvraag = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/ul/li[2]/div/a[2]"));
        boolean bbuttonAanvraag = false;

        if(buttonAanvraag.getText().equals("AANVRAAG INDIENEN")){
            bbuttonAanvraag = true;
        }else{
            System.out.println("button AANVRAAG INDIENEN niet aanwezig");
        }


        WebElement buttonAfmelden = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/ul/li[2]/div/a[3]"));
        boolean bbuttonAfmelden = false;

        if(buttonAfmelden.getText().equals("AFMELDEN")){
            bbuttonAfmelden = true;
        }else{
            System.out.println("button AFMELDEN niet aanwezig");
        }

        //LABELS

        WebElement labelIngediend = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-start/div/div[1]/table[1]/thead"));
        boolean bLabelIngediend = false;

        if(labelIngediend.getText().equals("INGEDIEND")){
            bLabelIngediend = true;
        }else{
            System.out.println("tabel ingediend niet aanwezig");
        }

        WebElement labelWijziging = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-start/div/div[1]/table[2]/thead"));
        boolean blabelWijziging = false;

        if(labelWijziging.getText().equals("WIJZIGING GEVRAAGD")){
            blabelWijziging= true;
        }else{
            System.out.println("tabel WIJZIGING GEVRAAGD niet aanwezig");
        }


        WebElement labelGoedgekeurd = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-start/div/div[2]/table[1]/thead"));
        boolean blabelGoedgekeurd = false;

        if(labelGoedgekeurd.getText().equals("GOEDGEKEURD")){
            blabelGoedgekeurd = true;
        }else{
            System.out.println("tabel GOEDGEKEURD niet aanwezig");
        }


        WebElement labelAfgekeurd = driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-start/div/div[2]/table[2]/thead"));
        boolean blabelAfgekeurd = false;

        if(labelAfgekeurd.getText().equals("AFGEKEURD")){
            blabelAfgekeurd = true;
        }else{
            System.out.println("tabel AFGEKEURD niet aanwezig");
        }





        boolean correctLayout = false ;

        if(ImagePresent && bLabelIngediend && blabelWijziging && blabelGoedgekeurd && blabelAfgekeurd && bButtonStart && bbuttonAanvraag && bbuttonAfmelden){
            correctLayout = true;
        }

        assertTrue(correctLayout);

    }
}