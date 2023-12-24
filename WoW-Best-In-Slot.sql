DROP DATABASE IF EXISTS WoW_BIS;
CREATE DATABASE WoW_BIS;
use WoW_BIS;

-- Löschen der existierenden Tabellen, falls vorhanden
DROP TABLE IF EXISTS EquipmentStat;
DROP TABLE IF EXISTS MonsterDrop;   
DROP TABLE IF EXISTS EquipmentSlot;
DROP TABLE IF EXISTS Monsters;
DROP TABLE IF EXISTS Equipments;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Characters;
DROP TABLE IF EXISTS Users;

-- Erstellen der Characters-Tabelle
CREATE TABLE Users(
    userID int NOT NULL AUTO_INCREMENT,
    userName varchar(128) NOT NULL,
    userEmail varchar(128) NOT NULL,
    userPassword varchar(256) NOT NULL,
    userRole varchar(64) DEFAULT 'USER',
    PRIMARY KEY (userID)
);

-- Erstellen der Characters-Tabelle
CREATE TABLE Characters(
    characterID int NOT NULL AUTO_INCREMENT,
    characterName varchar(128) NOT NULL,
    PRIMARY KEY (characterID),
);

-- Erstellen der Categories-Tabelle
CREATE TABLE Categories(
    categoryID int NOT NULL AUTO_INCREMENT,
    categoryName varchar(128) NOT NULL,
    PRIMARY KEY (categoryID)
);

-- Erstellen der Equipments-Tabelle
CREATE TABLE Equipments(
    equipmentID int NOT NULL AUTO_INCREMENT,
    equipmentName varchar(128) NOT NULL,
    categoryID int NOT NULL,
    PRIMARY KEY (equipmentID),
    FOREIGN KEY (categoryID) REFERENCES Categories(categoryID) ON DELETE CASCADE
);

-- Erstellen der EquipmentStat-Tabelle
CREATE TABLE EquipmentStat(
    equipmentID int NOT NULL,
    statName varchar(128) NOT NULL,
    statValue int NOT NULL,
    PRIMARY KEY (equipmentID),
    FOREIGN KEY (equipmentID) REFERENCES Equipments(equipmentID) ON DELETE CASCADE
);

-- Erstellen der Monsters-Tabelle
CREATE TABLE Monsters(
    monsterID int NOT NULL AUTO_INCREMENT,
    monsterName varchar(128) NOT NULL,
    PRIMARY KEY (monsterID)
);

-- Erstellen der MonsterDrop-Tabelle
CREATE TABLE MonsterDrop(
    monsterID int NOT NULL,
    equipmentID int NOT NULL,
    dropChance float NOT NULL,
    PRIMARY KEY (monsterID, equipmentID),
    FOREIGN KEY (monsterID) REFERENCES Monsters(monsterID) ON DELETE CASCADE,
    FOREIGN KEY (equipmentID) REFERENCES Equipments(equipmentID) ON DELETE CASCADE
);

-- Erstellen der EquipmentSlot-Tabelle
CREATE TABLE EquipmentSlot(
    equipmentID int,
    characterID int,
    PRIMARY KEY (equipmentID, characterID),
    FOREIGN KEY (equipmentID) REFERENCES Equipments(equipmentID) ON DELETE CASCADE,
    FOREIGN KEY (characterID) REFERENCES Characters(characterID) ON DELETE CASCADE
);

INSERT INTO Users (userName,userEmail,userPassword,userRole) VALUES ('cayik', 'admin@admin.com', 'admin', 'ADMIN');

-- Einfügen von Daten in die Characters-Tabelle
INSERT INTO Characters (characterName) VALUES ('Cayan');
INSERT INTO Characters (characterName) VALUES ('Chris');
INSERT INTO Characters (characterName) VALUES ('David');
INSERT INTO Characters (characterName) VALUES ('Klemens');

-- Einfügen von Daten in die Categories-Tabelle
INSERT INTO Categories (categoryName) VALUES ('Waffe');
-- check UTF-8
INSERT INTO Categories (categoryName) VALUES ('Rüstung');
INSERT INTO Categories (categoryName) VALUES ('Accessoire');


-- Waffen
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Flammenklinge', 1); -- ID 1
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Baguett', 1); -- ID 2
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Donnerbrecher', 1); -- ID 3

-- Rüstungen
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Schattenpanzer', 2); -- ID 4
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Sonnenschild', 2); -- ID 5
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Drachenleder', 2); -- ID 6

-- Ringe
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Ring der Stärke', 3); -- ID 7
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Ring der Weisheit', 3); -- ID 8
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Ring des JIRESCH', 3); -- ID 9
INSERT INTO Equipments (equipmentName, categoryID) VALUES ('Ring der Ausdauer', 3); -- ID 10

-- Statistiken für die Schwerter (Flammenklinge, Eisenschneider, Donnerbrecher)
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (1, 'Stärke', 30);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (2, 'Stärke', 25);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (3, 'Stärke', 35);

-- Statistiken für die Rüstungen (Schattenpanzer, Sonnenschild, Drachenleder)
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (4, 'Ausdauer', 40);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (5, 'Ausdauer', 35);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (6, 'Geschicklichkeit', 45);

-- Statistiken für die Ringe (Ring der Stärke, Weisheit, Geschwindigkeit, Ausdauer)
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (7, 'Stärke', 5);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (8, 'Ausdauer', 5);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (9, 'Geschwindigkeit', 5);
INSERT INTO EquipmentStat (equipmentID, statName, statValue) VALUES (10, 'Geschicklichkeit', 5);

-- Beispielmonster
INSERT INTO Monsters (monsterName) VALUES ('Goblin');
INSERT INTO Monsters (monsterName) VALUES ('Oger');
INSERT INTO Monsters (monsterName) VALUES ('Skelett');
INSERT INTO Monsters (monsterName) VALUES ('Zombie');
INSERT INTO Monsters (monsterName) VALUES ('Drache');
INSERT INTO Monsters (monsterName) VALUES ('Werwolf');
INSERT INTO Monsters (monsterName) VALUES ('Vampir');
INSERT INTO Monsters (monsterName) VALUES ('Dämon');
INSERT INTO Monsters (monsterName) VALUES ('Kobold');
INSERT INTO Monsters (monsterName) VALUES ('Elementar');

-- Monster 1 (Goblin) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (1, 1, 0.10); 
-- Monster 2 (Oger) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (2, 2, 0.15); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (2, 8, 0.25);
-- Monster 3 (Skelett) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (3, 3, 0.12); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (3, 9, 0.18);
-- Monster 4 (Zombie) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (4, 4, 0.15); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (4, 10, 0.20); 
-- Monster 5 (Drache) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (5, 5, 0.05); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (5, 1, 0.03);
-- Monster 6 (Werwolf) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (6, 6, 0.10);
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (6, 2, 0.12); 
-- Monster 7 (Vampir) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (7, 7, 0.18); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (7, 3, 0.08);
-- Monster 8 (Dämon) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (8, 8, 0.20); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (8, 4, 0.10);
-- Monster 9 (Kobold) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (9, 9, 0.22); 
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (9, 5, 0.07);
-- Monster 10 (Elementar) Drops
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (10, 10, 0.25);
INSERT INTO MonsterDrop (monsterID, equipmentID, dropChance) VALUES (10, 6, 0.05); 
