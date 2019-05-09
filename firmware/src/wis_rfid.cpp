/*
 * Authors: Robert Mysza (xmysza00), Juraj Chripko (xchrip00)
 * Students of: FIT VUT
 * 
 * ISIC (RFID) reader with sending its UID to the server (WIS)
 *
 * ESP32 Pinout:
 * 
 * Signal     Pin
 * ----------------
 * RST/Reset  17
 * SPI SS     5
 * SPI MOSI   23
 * SPI MISO   19
 * SPI SCK    18
 */

#include <Arduino.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <SPI.h>
#include <MFRC522.h>
#include "SSD1306Wire.h"
#include "esp_wpa2.h"

// Configurable pins
#define RST_PIN         17
#define SS_PIN          5

// If using WPA2 with identity
#define WPA2_ENTERPRISE false

// Wifi configuration
#define EAP_IDENTITY "identity@vutbr.cz"
#define EAP_PASSWORD "password"
const char * ssid     = "ssid";
const char * password = "password";
const char * host = "host:port";
const char * url = "/api/rfid/create";

MFRC522 rfid(SS_PIN, RST_PIN);
SSD1306Wire display (0x3c, 4, 15);

String uidPICC; // Incoming UID
int counter = 0;

String toHex(byte *buffer, byte bufferSize) {
  String hex_data = "";
  for (byte i = 0; i < bufferSize; i++) {
    hex_data += buffer[i] < 0x10 ? "0" : "";
    hex_data += String(buffer[i], HEX);
  }
  hex_data.toUpperCase();
  return hex_data;
}

void wifi_connect() {
  WiFi.disconnect(true);
  if (WPA2_ENTERPRISE) {
    WiFi.mode(WIFI_STA); // Init wifi mode
    esp_wifi_sta_wpa2_ent_set_identity((uint8_t *)EAP_IDENTITY, strlen(EAP_IDENTITY)); 
    esp_wifi_sta_wpa2_ent_set_username((uint8_t *)EAP_IDENTITY, strlen(EAP_IDENTITY));
    esp_wifi_sta_wpa2_ent_set_password((uint8_t *)EAP_PASSWORD, strlen(EAP_PASSWORD));
    esp_wpa2_config_t config = WPA2_CONFIG_INIT_DEFAULT(); // Set config settings to default
    esp_wifi_sta_wpa2_ent_enable(&config); // Set config settings to enable function
    WiFi.begin(ssid);
  }
  else {
    WiFi.begin(ssid, password);
  }
  Serial.print(F("Wifi connecting to "));
  Serial.print(String(ssid));
  display.clear(); 
  display.setFont(ArialMT_Plain_10);
  display.drawString(0, 0, "Wifi connecting to:");
  display.drawString(0, 20, ssid);
  display.display();
  while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.print(".");
      counter++;
      if (counter >= 60) {
        ESP.restart();
      }
  }
  counter = 0;
  Serial.println();
  Serial.print(F("IP address: "));
  Serial.println(WiFi.localIP());
  Serial.println(F("Waiting for some card..."));
}

void setup() { 
  Serial.begin(115200);
  SPI.begin(); // Init SPI bus
  rfid.PCD_Init(); // Init MFRC522

  // Init OLED
  pinMode(16, OUTPUT);
  pinMode(2, OUTPUT);
  digitalWrite(16, LOW); // Set GPIO16 low to reset OLED
  delay(50);
  digitalWrite(16, HIGH); // While OLED is running, GPIO16 must go high
  display.init();
  //display.flipScreenVertically();
  display.setFont(ArialMT_Plain_10);
  display.setTextAlignment(TEXT_ALIGN_LEFT);

  // Connect to Wifi
  wifi_connect();
}

void loop() {
  // Counter for clearing display
  if (counter > 0)
    counter--;
  else if (counter == 0) {
    display.clear();
    display.display();
    counter = -1;
  }

  /********** Read RFID card and find out its UID **********/

  // Look for new cards
  if ( ! rfid.PICC_IsNewCardPresent())
    return;

  // Verify if the UID has been readed
  if ( ! rfid.PICC_ReadCardSerial())
    return;

  // Clear display  
  display.clear();
  display.display();

  Serial.println(F("-------------------------------"));
  Serial.print(F("PICC type: "));
  MFRC522::PICC_Type piccType = rfid.PICC_GetType(rfid.uid.sak);
  Serial.println(rfid.PICC_GetTypeName(piccType));

  // Check if PICC is of Classic MIFARE type used in ISIC cards
  if (piccType != MFRC522::PICC_TYPE_MIFARE_1K &&
      rfid.uid.size != 4) {
    Serial.println(F("Not a valid card"));
    display.setFont(ArialMT_Plain_16);
    display.drawString(0, 0, "Not a valid card");
    display.display();
    counter = 200;
    return;
  }

  // Save 32bit incoming UID as hex
  uidPICC = toHex(rfid.uid.uidByte, rfid.uid.size);
  Serial.print(F("The UID tag is: "));
  Serial.println(uidPICC);

  // Halt PICC
  rfid.PICC_HaltA();

  // Stop encryption on PCD
  rfid.PCD_StopCrypto1();

  /********** Send UID information to the server **********/
  if (WiFi.status() != WL_CONNECTED) {
    wifi_connect();
    return;
  }

  HTTPClient http;
  http.begin("http://" + String(host) + url);
  http.addHeader("Host", host);
  http.addHeader("Content-Type", "application/x-www-form-urlencoded");
  // String payload = "{\"uid\":" + String(uidPICC) + "}";
  String payload = "uid=" + uidPICC;
  int httpCode = http.POST(payload);
  if (httpCode > 0) {
    String payload = http.getString();
    Serial.println("Response from server:");
    Serial.println(payload);
    display.setFont(ArialMT_Plain_24);
    payload = (httpCode == 404) ? "Not found" : payload;
    display.drawString(15, 15, payload);
    display.display();
    counter = 200;
  }
  else {
    Serial.print("Error sending HTTP POST: ");
    Serial.println(http.errorToString(httpCode));
    display.setFont(ArialMT_Plain_10);
    display.drawString(0, 0, http.errorToString(httpCode));
    display.display();
  }
}
