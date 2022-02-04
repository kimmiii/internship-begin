package StageopdrachtBeoordelen;

import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class UCT2_1 {



    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();


        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("STAGECOORDINATOR@outlook.com");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(2);

        driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-start/div/div[1]/table[1]/tbody/tr[1]")).click();

        TimeUnit.SECONDS.sleep(2);


        driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-coor-internship-details/div/div[32]/button[1]")).click();
        driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-coor-internship-details/div/div[33]/button[1]")).click();



        /*

        String loggedInTitle = "http://localhost:4200/#/coordinator/start";


        TimeUnit.SECONDS.sleep(3);

        String pageTitle = driver.getTitle();
        //String pageTitle = driver.getCurrentUrl();

        */

        //assertEquals(loggedInTitle,pageTitle);
    }
}
