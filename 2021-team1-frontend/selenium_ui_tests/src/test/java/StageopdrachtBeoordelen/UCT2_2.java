package StageopdrachtBeoordelen;

import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

public class UCT2_2 {



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


        driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-coor-internship-details/div/div[32]/button[3]")).click();

        TimeUnit.SECONDS.sleep(1);

        driver.findElement(By.id("otherRemarks")).sendKeys("stageopdracht is niet goedgekeurd om .... reden ");


        driver.findElement(By.xpath("/html/body/app-root/div/app-coordinator/nav/app-coor-internship-details/div/app-coor-rejection/form/input")).click();



        /*

        String loggedInTitle = "http://localhost:4200/#/coordinator/start";


        TimeUnit.SECONDS.sleep(3);

        String pageTitle = driver.getTitle();
        //String pageTitle = driver.getCurrentUrl();

        */

        //assertEquals(loggedInTitle,pageTitle);
    }
}
