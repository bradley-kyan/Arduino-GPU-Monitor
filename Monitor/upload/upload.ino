#include <Wire.h> 
#include <LiquidCrystal.h>

int initialBoot;
int Li = 16;
int Lii = 0;

LiquidCrystal lcd( 8, 9, 4, 5, 6, 7 );
String inData;

void setup() {
    Serial.begin(9600);
    lcd.begin(16,2);
    lcd.setCursor(0,0);
}

String CursorLeft(String input){
  String output;

  output = input.substring(Li, Lii);
  Li++;
  Lii++;

  if(Li>input.length()){
    Li=16;
    Lii=0;
  }
  return output;
}


void loop() {
  
  if(initialBoot == 0){
    lcd.setCursor(0,0);
    lcd.print("Disconnected");
    lcd.setCursor(0,1);
    lcd.print(CursorLeft("Connect to system       Connect to syste"));
    delay(400);
    
  }
    while (Serial.available() > 0)
    {
        char recieved = Serial.read();
        inData += recieved; 
        if(inData == "DIS*"){
          lcd.clear();
          initialBoot = 0; 
        }
        else{
          initialBoot = 1;          
          if (recieved == '*')
          {
              inData.remove(inData.length() - 1, 1);
              lcd.setCursor(0,0);
              lcd.print("GPU Temp: " + inData + char(223)+"C     ");
              inData = ""; 
              
          } 
          if (recieved == '#')
          {
              inData.remove(inData.length() - 1, 1);
              lcd.setCursor(0,1);
              lcd.print("GPU Freq: " + inData + "     ");
              inData = ""; 
          }
        }
    }
}
