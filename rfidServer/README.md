## RfidServer

Beží na platforme ***ASP.NET Core 2.2***. Aplikcia je orchestrovateľná cez ***Docker***, kde potom beží v Linuxovom kontajnery na porte 80. Súčasťou je aj ***SQLite*** databáza pre uchovanie študentov a termínov na registráciu.

### Štruktúra projektu

* ***RfidServer*** - klientské rozhranie,
    * ***Rfid*** - kontrolér pre prijatie UUID a odoslanie loginu na mikrokontrolér,
    * ***Web*** - modely, pohľady a kontroléry pre webové rozhranie,
* ***RfidServer.DAL*** - prístup k databázy, entity,
* ***RfidServer.WisAPI*** - rozhranie na prístup do WIS.
