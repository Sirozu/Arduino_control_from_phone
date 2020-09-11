#include <SPI.h>      // библиотека для протокола SPI
#include <nRF24L01.h> // библиотека для nRF24L01+
#include <RF24.h>     // библиотека для радио модуля
#include "Command.h"

using namespace MyCommand;

const uint64_t pipe = 0xF0F1F2F3F4LL; // идентификатор передачи
RF24 radio(9,10); // Для MEGA2560 замените на RF24 radio(9,53);

void setup()
{
  pinMode(A0,INPUT);   // порт для датчика воды
  Serial.begin(9600);  // запускаем последовательный порт
  radio.begin();       // включаем радио модуль
  radio.setChannel(76); // выбираем канал (от 0 до 127)

    // скорость: RF24_250KBPS, RF24_1MBPS или RF24_2MBPS
  radio.setDataRate(RF24_1MBPS);
    // мощность: RF24_PA_MIN=-18dBm, RF24_PA_LOW=-12dBm, RF24_PA_MED=-6dBM
  radio.setPALevel(RF24_PA_HIGH);

  radio.openWritingPipe(pipe);      // открываем трубу на передачу
  radio.stopListening();

  
}

void loop() 
{
  Command com(0, 1);
  int data[5];
  data[0] = com.Header; 
  data[1] = com.Get_Num();
  data[2] = com.Get_Size();
  data[3] = com.Get_Data();
  data[4] = com.End; 
  radio.write(&data, sizeof(data)); // отправляем данные и указываем байты
  //Serial.print("data: ");
  Serial.println("sent");             // выводим данные на монитор порта
  delay(1000);
}
