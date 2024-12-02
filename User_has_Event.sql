-- phpMyAdmin SQL Dump
-- version 4.7.1
-- https://www.phpmyadmin.net/
--
-- Host: sql5.freesqldatabase.com
-- Generation Time: Dec 02, 2024 at 02:57 AM
-- Server version: 5.5.62-0ubuntu0.14.04.1
-- PHP Version: 7.0.33-0ubuntu0.16.04.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sql5749035`
--

-- --------------------------------------------------------

--
-- Table structure for table `User_has_Event`
--

CREATE TABLE `User_has_Event` (
  `User_user_id` int(11) NOT NULL,
  `Event_event_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `User_has_Event`
--

INSERT INTO `User_has_Event` (`User_user_id`, `Event_event_id`) VALUES
(1, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `User_has_Event`
--
ALTER TABLE `User_has_Event`
  ADD PRIMARY KEY (`User_user_id`,`Event_event_id`),
  ADD KEY `fk_User_has_Event_Event1_idx` (`Event_event_id`),
  ADD KEY `fk_User_has_Event_User_idx` (`User_user_id`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `User_has_Event`
--
ALTER TABLE `User_has_Event`
  ADD CONSTRAINT `fk_User_has_Event_User` FOREIGN KEY (`User_user_id`) REFERENCES `User` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_User_has_Event_Event1` FOREIGN KEY (`Event_event_id`) REFERENCES `Event` (`event_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
