-- phpMyAdmin SQL Dump
-- version 5.1.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Mar 27, 2025 at 12:38 PM
-- Server version: 5.7.24
-- PHP Version: 8.3.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `loc_moto`
--

-- --------------------------------------------------------

--
-- Table structure for table `marque`
--

CREATE TABLE `marque` (
  `id_marque` int(11) NOT NULL,
  `nom_marque` varchar(100) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `marque`
--

INSERT INTO `marque` (`id_marque`, `nom_marque`) VALUES
(1, 'Honda'),
(2, 'Yamaha'),
(3, 'Kawasaki'),
(4, 'Ducati'),
(5, 'BMW'),
(6, 'Harley-Davidson');

-- --------------------------------------------------------

--
-- Table structure for table `modele`
--

CREATE TABLE `modele` (
  `id_modele` int(11) NOT NULL,
  `nom_modele` varchar(100) NOT NULL,
  `id_marque` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `modele`
--

INSERT INTO `modele` (`id_modele`, `nom_modele`, `id_marque`) VALUES
(1, 'CB650R', 1),
(2, 'Africa Twin', 1),
(3, 'MT-07', 2),
(4, 'MT-09', 2),
(5, 'Ninja 650', 3),
(6, 'Z900', 3),
(7, 'Panigale V4', 4),
(8, 'Monster', 4),
(9, 'R1250GS', 5),
(10, 'S1000RR', 5),
(11, 'Sportster', 6),
(12, 'Road King', 6),
(13, 'Tenere 700', 2);

-- --------------------------------------------------------

--
-- Table structure for table `moto`
--

CREATE TABLE `moto` (
  `id_moto` int(11) NOT NULL,
  `id_modele` int(11) NOT NULL,
  `annee` int(11) NOT NULL,
  `cylindree` int(11) NOT NULL,
  `prix_journalier` decimal(10,2) NOT NULL,
  `immatriculation` varchar(20) NOT NULL,
  `disponible` tinyint(1) DEFAULT '1'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `moto`
--

INSERT INTO `moto` (`id_moto`, `id_modele`, `annee`, `cylindree`, `prix_journalier`, `immatriculation`, `disponible`) VALUES
(1, 1, 2022, 650, '75.00', 'AA-123-BB', 1),
(2, 2, 2021, 1100, '120.00', 'CC-456-DD', 1),
(3, 3, 2023, 700, '80.00', 'EE-789-FF', 1),
(5, 5, 2021, 650, '75.00', 'II-345-JJ', 1),
(11, 6, 2022, 750, '100.00', 'EJ-000-EJ', 1),
(7, 7, 2022, 1100, '150.00', 'MM-901-NN', 1),
(8, 8, 2021, 900, '100.00', 'OO-234-PP', 1),
(9, 9, 2023, 1250, '130.00', 'QQ-567-RR', 1),
(10, 10, 2022, 1000, '140.00', 'SS-890-TT', 1);

-- --------------------------------------------------------

--
-- Table structure for table `reservation`
--

CREATE TABLE `reservation` (
  `id_reservation` int(11) NOT NULL,
  `id_utilisateur` int(11) NOT NULL,
  `id_moto` int(11) NOT NULL,
  `date_debut` date NOT NULL,
  `date_fin` date NOT NULL,
  `date_reservation` datetime DEFAULT CURRENT_TIMESTAMP,
  `prix_total` decimal(10,2) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `reservation`
--

INSERT INTO `reservation` (`id_reservation`, `id_utilisateur`, `id_moto`, `date_debut`, `date_fin`, `date_reservation`, `prix_total`) VALUES
(1, 2, 1, '2023-04-10', '2023-04-15', '2025-03-25 02:25:10', '375.00'),
(2, 3, 3, '2023-04-12', '2023-04-14', '2025-03-25 02:25:10', '160.00'),
(3, 2, 5, '2023-04-20', '2023-04-25', '2025-03-25 02:25:10', '375.00'),
(6, 5, 10, '2025-03-27', '2025-03-28', '2025-03-25 13:35:20', '280.00'),
(7, 6, 2, '2025-03-27', '2025-03-28', '2025-03-25 13:39:51', '240.00'),
(8, 5, 3, '2025-03-25', '2025-03-26', '2025-03-25 13:43:41', '160.00');

-- --------------------------------------------------------

--
-- Table structure for table `utilisateurs`
--

CREATE TABLE `utilisateurs` (
  `id_utilisateur` int(11) NOT NULL,
  `nom` varchar(100) NOT NULL,
  `prenom` varchar(100) NOT NULL,
  `email` varchar(150) NOT NULL,
  `mot_de_passe` varchar(255) NOT NULL,
  `telephone` varchar(15) DEFAULT NULL,
  `est_admin` tinyint(1) DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `utilisateurs`
--

INSERT INTO `utilisateurs` (`id_utilisateur`, `nom`, `prenom`, `email`, `mot_de_passe`, `telephone`, `est_admin`) VALUES
(6, 'op', 'op', 'op@gmail.com', '037aeaeaf4bbf26ddabe7256a8294dc52da48d575a1247b5c2598c47de7aebab', '0708080808', 0),
(4, 'Jaffel', 'Elyes', 'admin@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', '0707070707', 1),
(5, 'aa', 'aa', 'aa@gmail.com', '961b6dd3ede3cb8ecbaacbd68de040cd78eb2ed5889130cceb4c49268ea4d506', '0606060606', 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `marque`
--
ALTER TABLE `marque`
  ADD PRIMARY KEY (`id_marque`),
  ADD UNIQUE KEY `nom_marque` (`nom_marque`);

--
-- Indexes for table `modele`
--
ALTER TABLE `modele`
  ADD PRIMARY KEY (`id_modele`),
  ADD KEY `id_marque` (`id_marque`);

--
-- Indexes for table `moto`
--
ALTER TABLE `moto`
  ADD PRIMARY KEY (`id_moto`),
  ADD UNIQUE KEY `immatriculation` (`immatriculation`),
  ADD KEY `id_modele` (`id_modele`);

--
-- Indexes for table `reservation`
--
ALTER TABLE `reservation`
  ADD PRIMARY KEY (`id_reservation`),
  ADD KEY `id_utilisateur` (`id_utilisateur`),
  ADD KEY `id_moto` (`id_moto`);

--
-- Indexes for table `utilisateurs`
--
ALTER TABLE `utilisateurs`
  ADD PRIMARY KEY (`id_utilisateur`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `marque`
--
ALTER TABLE `marque`
  MODIFY `id_marque` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `modele`
--
ALTER TABLE `modele`
  MODIFY `id_modele` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `moto`
--
ALTER TABLE `moto`
  MODIFY `id_moto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `reservation`
--
ALTER TABLE `reservation`
  MODIFY `id_reservation` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `utilisateurs`
--
ALTER TABLE `utilisateurs`
  MODIFY `id_utilisateur` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
