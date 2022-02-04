package StageopdrachtWijzigen;

import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class EPTC2_1 {



    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();



        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("stagebedrijf@OUTLOOK.COM");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(1);

        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-start/div/div[1]/table[1]/tbody/tr[1]")).click();

        TimeUnit.SECONDS.sleep(1);

        String voorWijzigen = driver.getCurrentUrl();

        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-internship-details/div/div[25]/button[2]")).click();




        //BEDRIJF
        driver.findElement(By.id("companyStreet")).sendKeys("chakamaka");



        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-internship-edit/div/form/div[5]/input[2]")).click();

        TimeUnit.SECONDS.sleep(1);


        String naWijzigen = driver.getCurrentUrl();

        assertEquals(voorWijzigen,naWijzigen);
}
}