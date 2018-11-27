--
-- Скрипт сгенерирован Devart dbForge Studio for MySQL, Версия 6.3.358.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/mysql/studio
-- Дата скрипта: 27.11.2018 23:17:08
-- Версия сервера: 5.0.67-community-nt
-- Версия клиента: 4.1
--

CREATE DATABASE IF NOT EXISTS ggmc;
USE ggmc;

CREATE TABLE groups (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Name varchar(50) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 14
AVG_ROW_LENGTH = 5461
CHARACTER SET utf8
COLLATE utf8_general_ci;

CREATE TABLE rooms (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Name varchar(50) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 19
AVG_ROW_LENGTH = 5461
CHARACTER SET utf8
COLLATE utf8_general_ci;

CREATE TABLE schedule (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Lesson int(11) DEFAULT NULL,
  WeekDay int(11) DEFAULT NULL,
  Subject varchar(255) DEFAULT NULL,
  Room varchar(255) DEFAULT NULL,
  Teacher1 varchar(255) DEFAULT NULL,
  Teacher2 varchar(255) DEFAULT NULL,
  GroupName varchar(255) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 176
AVG_ROW_LENGTH = 1489
CHARACTER SET utf8
COLLATE utf8_general_ci;

CREATE TABLE settings (
  Id int(11) NOT NULL AUTO_INCREMENT,
  GroupName varchar(255) DEFAULT NULL,
  Subject varchar(255) DEFAULT NULL,
  Teacher1 varchar(255) DEFAULT NULL,
  Teacher2 varchar(255) DEFAULT NULL,
  Room varchar(255) DEFAULT NULL,
  Hours varchar(255) DEFAULT NULL,
  HoursAll varchar(255) DEFAULT NULL,
  HoursDay varchar(255) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 30
AVG_ROW_LENGTH = 8192
CHARACTER SET utf8
COLLATE utf8_general_ci;

CREATE TABLE subjects (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Name varchar(50) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 31
AVG_ROW_LENGTH = 5461
CHARACTER SET utf8
COLLATE utf8_general_ci;

CREATE TABLE teachers (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Name varchar(50) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 28
AVG_ROW_LENGTH = 5461
CHARACTER SET utf8
COLLATE utf8_general_ci;