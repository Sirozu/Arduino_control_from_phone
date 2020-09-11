#include <SPI.h>      // библиотека для протокола SPI
#include <nRF24L01.h> // библиотека для nRF24L01+
#include <RF24.h>     // библиотека для радио модуля
#include "Command.h"
#include <Stepper.h>

#define LED_PIN 2
using namespace MyCommand;
const uint64_t pipe = 0xF0F1F2F3F4LL; // идентификатор передачи
RF24 radio(9,10); // Для MEGA2560 замените на RF24 radio(9,53);

const int stepsPerRevolution = 200;
Stepper myStepper(stepsPerRevolution, 4, 6, 5, 7);

void setup()
{
  Serial.begin(9600);  // запускаем последовательный порт
  pinMode(LED_PIN,OUTPUT); 
  radio.begin();       // включаем радио модуль
  radio.setChannel(76); // выбираем канал (от 0 до 127)

    // скорость: RF24_250KBPS, RF24_1MBPS или RF24_2MBPS
  radio.setDataRate(RF24_1MBPS);
    // мощность: RF24_PA_MIN=-18dBm, RF24_PA_LOW=-12dBm, RF24_PA_MED=-6dBM
  radio.setPALevel(RF24_PA_HIGH);

  radio.openReadingPipe(1, pipe);    // открываем первую трубу
  radio.startListening();            // начинаем слушать трубу

  pinMode(LED_PIN,OUTPUT); 

  myStepper.setSpeed(60);
}

void loop() 
{
   Recv();
}

void Recv()
{
  
  int data[5];
  
  if (radio.available())             // проверяем буфер обмена
  {
    radio.read(&data, sizeof(data)); // читаем данные

//  Command com;
//
//    if (data == com.Header)
//    {
//      Serial.print("Enter: ");
//      int NumberCommand;
//      radio.read(&NumberCommand, sizeof(NumberCommand));
//      Serial.print("NUmCOm  ");
//      Serial.println(NumberCommand);
//      switch(NumberCommand)
//      {
//        case 0:
//
//            break;
//        case 1:
//            Serial.print("Lol");
//            break;
//        case 2:
//
//            break;
//        case 3:
//
//            break;
//        default:
//          break;
//      }
//    }
    
    Serial.print("Header: ");
    Serial.println(data[0]);
    Serial.print("Number: ");
    Serial.println(data[1]);
    Serial.print("Size: ");
    Serial.println(data[2]);
    Serial.print("data: ");
    Serial.println(data[3]);
    Serial.print("end: ");
    Serial.println(data[4]);

    int Head = data[0]; 
    int Num = data[1];
    int Size = data[2];
    int Data = data[3];
    int End = data[4]; 



     switch(Num)
     {
        case 0:
            digitalWrite(LED_PIN, LOW); 
            break;
        case 1:
            digitalWrite(LED_PIN, HIGH); 
            break;
        case 2:
            myStepper.step(stepsPerRevolution*50);
            delay(1000);
            myStepper.step(0);
            delay(1000);
            break;
        case 3:
            myStepper.step(-stepsPerRevolution*50);
            delay(1000);
            myStepper.step(0);
            delay(1000);
            break;
        default:
          break;
      }
  }
}
