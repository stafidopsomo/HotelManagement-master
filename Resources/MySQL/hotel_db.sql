
CREATE DATABASE IF NOT EXISTS hotel;
USE hotel;


-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: hotel
-- ------------------------------------------------------
-- Server version	5.7.32-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `hot_bookings`
--

DROP TABLE IF EXISTS `hot_bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_bookings` (
  `booking_ID` int(11) NOT NULL AUTO_INCREMENT,
  `usr_ID` int(11) DEFAULT NULL,
  `room_ID` int(11) DEFAULT NULL,
  `booking_Date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `checkin_Date` date DEFAULT NULL,
  `checkout_Date` date DEFAULT NULL,
  `booking_Comments` text COLLATE utf8mb4_unicode_ci,
  `hasArrived` int(2) DEFAULT '0',
  `hasDeparted` int(2) DEFAULT '0',
  `booking_cost` float DEFAULT NULL,
  `booking_hasPaid` int(1) DEFAULT '0',
  PRIMARY KEY (`booking_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_bookings`
--

LOCK TABLES `hot_bookings` WRITE;
/*!40000 ALTER TABLE `hot_bookings` DISABLE KEYS */;
INSERT INTO `hot_bookings` VALUES (1,2,1,'2025-07-07 13:30:30','2024-07-07','2025-07-10',NULL,1,1,149.97,1),(2,2,2,'2025-07-07 13:48:51','2025-07-08','2023-07-10',NULL,1,1,219.98,1),(3,2,1,'2025-07-17 11:16:10','2025-07-18','2025-07-20',NULL,1,1,99.98,1),(4,2,1,'2025-08-21 07:30:04','2025-08-21','2025-08-24',NULL,1,1,149.97,1),(5,2,2,'2025-08-21 09:25:54','2025-08-22','2025-08-24',NULL,1,0,219.98,1),(6,2,3,'2025-08-21 11:53:44','2025-08-22','2025-08-24',NULL,0,0,143.98,1),(7,2,6,'2025-08-21 11:53:52','2025-08-22','2025-08-24',NULL,0,0,151.38,1);
/*!40000 ALTER TABLE `hot_bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hot_rooms`
--

DROP TABLE IF EXISTS `hot_rooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_rooms` (
  `room_ID` int(4) NOT NULL AUTO_INCREMENT,
  `room_Type` int(4) DEFAULT NULL,
  `room_TypeN` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `room_AC` int(1) DEFAULT '0',
  `room_Smokers` int(1) DEFAULT '0',
  `room_Heater` int(1) DEFAULT '0',
  `room_Family` int(1) DEFAULT '0',
  `room_AMEA` int(1) DEFAULT '0',
  `room_Balcony` int(1) DEFAULT NULL,
  `room_SeaView` int(1) DEFAULT NULL,
  `room_FreeWIFI` int(1) DEFAULT '0',
  `room_Price` float DEFAULT NULL,
  PRIMARY KEY (`room_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_rooms`
--

LOCK TABLES `hot_rooms` WRITE;
/*!40000 ALTER TABLE `hot_rooms` DISABLE KEYS */;
INSERT INTO `hot_rooms` VALUES (1,1,'Μονόκλινο',1,1,1,0,0,1,1,1,49.99),(2,1,'Μονόκλινο',1,1,1,0,0,1,0,1,109.99),(3,1,'Μονόκλινο',1,0,1,0,0,0,0,1,71.99),(4,2,'Δίκλινο',1,0,1,0,0,0,1,1,39.99),(5,3,'Τρίκλινο',1,0,1,1,0,1,1,1,99.99),(6,4,'Σουίτα',1,1,1,1,1,1,1,1,154.99);
/*!40000 ALTER TABLE `hot_rooms` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hot_rooms_pics`
--

DROP TABLE IF EXISTS `hot_rooms_pics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_rooms_pics` (
  `pic_ID` int(4) NOT NULL AUTO_INCREMENT,
  `room_ID` int(4) DEFAULT NULL,
  `pic_Path` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`pic_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_rooms_pics`
--

LOCK TABLES `hot_rooms_pics` WRITE;
/*!40000 ALTER TABLE `hot_rooms_pics` DISABLE KEYS */;
INSERT INTO `hot_rooms_pics` VALUES (1,1,'room1_1.png'),(2,1,'room1_2.png'),(3,1,'room1_3.png'),(4,2,'room2_1.png'),(5,2,'room2_2.png'),(6,2,'room2_3.png'),(7,2,'room2_4.png'),(8,3,'room3_1.png'),(9,3,'room3_2.png'),(10,3,'room3_3.png'),(11,3,'room3_4.png'),(12,4,'room4_1.png'),(13,4,'room4_2.png'),(14,4,'room4_3.png'),(15,4,'room4_4.png'),(16,5,'room5_1.png'),(17,5,'room5_2.png'),(18,5,'room5_3.png'),(19,5,'room5_4.png'),(20,6,'room6_1.png'),(21,6,'room6_2.png'),(22,6,'room6_3.png');
/*!40000 ALTER TABLE `hot_rooms_pics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hot_type`
--

DROP TABLE IF EXISTS `hot_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_type` (
  `room_type` int(4) NOT NULL AUTO_INCREMENT,
  `type_Name` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`room_type`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_type`
--

LOCK TABLES `hot_type` WRITE;
/*!40000 ALTER TABLE `hot_type` DISABLE KEYS */;
INSERT INTO `hot_type` VALUES (1,'Μονόκλινο'),(2,'Δίκλινο'),(3,'Τρίκλινο'),(4,'Τετράκλινο'),(5,'Σουίτα');
/*!40000 ALTER TABLE `hot_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hot_usr`
--

DROP TABLE IF EXISTS `hot_usr`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_usr` (
  `usr_ID` int(11) NOT NULL AUTO_INCREMENT,
  `usr_FName` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usr_LName` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usr_username` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usr_password` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usr_Email` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usr_Type` int(11) DEFAULT '0',
  PRIMARY KEY (`usr_ID`),
  UNIQUE KEY `usr_username` (`usr_username`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_usr`
--

LOCK TABLES `hot_usr` WRITE;
/*!40000 ALTER TABLE `hot_usr` DISABLE KEYS */;
INSERT INTO `hot_usr` VALUES (1,'Βύρωνας','Πανιπέρης','byron','1234','byron@gmail.com',1),(2,'Stelios','Dimitrakopoulos','stafidopsomo','1234','test@test.com',2),(3,'Νίκος','Δακουτρός','dakou','123','dakou@gmail.com',0),(4,'Giwrgos','Mhliwnis','korki','1234','korki@gmail.com',0),(5,'Final','Exaple','Final','1234','final@example.com',2);
/*!40000 ALTER TABLE `hot_usr` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hot_usr_det`
--

DROP TABLE IF EXISTS `hot_usr_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hot_usr_det` (
  `det_ID` int(4) NOT NULL AUTO_INCREMENT,
  `usr_ID` int(4) NOT NULL,
  `usrID_Street` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usrID_Region` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usrID_Country` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `usrID_Phone` varchar(15) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`det_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hot_usr_det`
--

LOCK TABLES `hot_usr_det` WRITE;
/*!40000 ALTER TABLE `hot_usr_det` DISABLE KEYS */;
INSERT INTO `hot_usr_det` VALUES (1,1,'Κουκάκι','Αθήνα, Αττική 11742','Ελλάδα','6988896560'),(2,2,'Παντειχίου 4','Νίκαια, Αττική 18451','Ελλάδα','6948784502'),(3,3,'Κουντουριώτου 268','Πειραιάς, Αττική 18525','Ελλάδα','6988238990'),(4,4,'Μαρ. Χατζηκυριακού 78','Πειραιάς, Αττική 18524','Ελλάδα','6978455896'),(5,5,'Σουλίου 11 1343','Αγ. Δημήτριος, Αθήνα','Ελλάδα','6978451025');
/*!40000 ALTER TABLE `hot_usr_det` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'hotel'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-08-22 16:41:48
