package Login;


import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class UCT1_2 {


    @Test
    public void UCT1punt2() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();


        String loginPage = "Stagebeheer - Aanmelden";

        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("fout@gmail.com");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(3);

        String pageTitle = driver.getTitle();

        assertEquals(loginPage,pageTitle);
    }


}
