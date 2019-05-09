# WIS-RFID

Prihlasovanie na termíny v informačnom systéme WIS pomocou ISIC karty. Projekt sa skladá z 2 častí:

* **firmware** - *pre mikrokontrolér ESP32 s pripojenou RFID čítačkou a OLED displejom,*
* **rfidServer** - *ukladanie záznamov študentov do databázy a prístup na WIS.*

## Firmware

Na vývoj bol použitý [PlatformIO](https://docs.platformio.org/en/latest/). Použité knižnice sa automaticky stiahnú pri preklade programu; jedná sa o:

* [MFRC522](https://github.com/miguelbalboa/rfid) - *RFID modul,*
* [ESP8266_SSD1306](https://github.com/ThingPulse/esp8266-oled-ssd1306) - *OLED displej.*

Pred prekladom je potrebné nastaviť v súbore `src/wis_rfid.cpp` prihlasovacie údaje na Wifi - `ssid` a `password`, IP adresu a port servera - `host`. Pri zmenení makra `WPA2_ENTERPRISE` na `true` sa naviac použije aj autentizácia pomocou identity a hesla - `EAP_IDENTITY` a `EAP_PASSWORD`. Okrem toho sa podľa potreby dajú nastaviť piny na signály RST a SS pre RFID - `RST_PIN` a `SS_PIN`. Pre správnu rotáciu displeja sa v `setup()` odkomentuje riadok `display.flipScreenVertically()`.

Po pripojení na Wifi je mikrokontrolér pripravený čitať ISIC karty. Z nich si vyčíta identifikátor UUID, ktorý pošle na server a obratom čaká login majiteľa karty. Pri úspechu ho vypíše na 5 sekúnd na displej. Inak čaká cca 10 sekúnd na odpoveď a vypíše chybu.

## RfidServer

Beží na platforme ***ASP.NET Core 2.2***. Aplikcia je orchestrovateľná cez ***Docker***, kde potom beží v Linuxovom kontajnery na porte 80. Súčasťou je aj ***SQLite*** databáza pre uchovanie študentov a termínov na registráciu.

### Štruktúra projektu

* ***RfidServer*** - klientské rozhranie,
    * ***Rfid*** - kontrolér pre prijatie UUID a odoslanie loginu na mikrokontrolér,
    * ***Web*** - modely, pohľady a kontroléry pre webové rozhranie,
* ***RfidServer.DAL*** - prístup k databázy, entity,
* ***RfidServer.WisAPI*** - rozhranie na prístup do WIS.

## Autori

* **[Róbert Mysza](mailto:xmysza00@stud.fit.vutbr.cz)**
* **[Juraj Chripko](mailto:xchrip00@stud.fit.vutbr.cz)**

## Licencia

[GNU GPL](LICENSE)