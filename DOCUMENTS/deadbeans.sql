-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Jun 10, 2025 at 09:51 AM
-- Server version: 5.7.35-0ubuntu0.18.04.2
-- PHP Version: 8.0.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `deadbeans`
--

-- --------------------------------------------------------

--
-- Table structure for table `armors`
--

CREATE TABLE `armors` (
  `item_id` int(10) UNSIGNED NOT NULL,
  `def` int(10) UNSIGNED NOT NULL,
  `magic_def` int(10) UNSIGNED NOT NULL,
  `agility` int(10) UNSIGNED NOT NULL,
  `speed` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `armors`
--

INSERT INTO `armors` (`item_id`, `def`, `magic_def`, `agility`, `speed`) VALUES
(4, 12, 3, 8, 0),
(7, 25, 8, 0, -1),
(9, 5, 2, 15, 2),
(12, 8, 18, 5, 0),
(15, 15, 15, 3, 0),
(17, 20, 5, 0, -1);

-- --------------------------------------------------------

--
-- Table structure for table `consumables`
--

CREATE TABLE `consumables` (
  `item_id` int(10) UNSIGNED NOT NULL,
  `duration` int(10) UNSIGNED NOT NULL,
  `reusable` tinyint(1) NOT NULL,
  `power` smallint(5) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `consumables`
--

INSERT INTO `consumables` (`item_id`, `duration`, `reusable`, `power`) VALUES
(3, 0, 0, 50),
(8, 0, 0, 75),
(13, 300, 0, 15),
(18, 0, 0, 100);

-- --------------------------------------------------------

--
-- Table structure for table `drops`
--

CREATE TABLE `drops` (
  `enemy_id` int(10) UNSIGNED NOT NULL,
  `item_id` int(10) UNSIGNED NOT NULL,
  `max_quantity` tinyint(3) UNSIGNED NOT NULL,
  `min_quantity` tinyint(3) UNSIGNED NOT NULL,
  `drop_rate` tinyint(3) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `drops`
--

INSERT INTO `drops` (`enemy_id`, `item_id`, `max_quantity`, `min_quantity`, `drop_rate`) VALUES
(1, 1, 2, 1, 45),
(1, 2, 1, 1, 15),
(1, 3, 1, 1, 25),
(2, 1, 3, 1, 40),
(2, 8, 1, 1, 30),
(2, 11, 1, 1, 12),
(3, 1, 2, 1, 50),
(3, 4, 1, 1, 20),
(3, 16, 1, 1, 8),
(4, 5, 2, 1, 35),
(4, 12, 1, 1, 18),
(4, 20, 1, 1, 10),
(5, 1, 1, 1, 30),
(5, 4, 1, 1, 25),
(5, 13, 1, 1, 40),
(6, 6, 1, 1, 25),
(6, 10, 3, 1, 60),
(6, 19, 2, 1, 45),
(7, 1, 4, 2, 65),
(7, 7, 1, 1, 30),
(7, 19, 1, 1, 35),
(8, 3, 2, 1, 50),
(8, 8, 1, 1, 35),
(8, 14, 1, 1, 20),
(9, 1, 3, 1, 55),
(9, 13, 2, 1, 40),
(9, 16, 1, 1, 15),
(10, 5, 2, 1, 45),
(10, 15, 1, 1, 25),
(10, 18, 1, 1, 30);

-- --------------------------------------------------------

--
-- Table structure for table `enemies`
--

CREATE TABLE `enemies` (
  `enemy_id` int(10) UNSIGNED NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `health` int(10) UNSIGNED NOT NULL,
  `strength` int(10) UNSIGNED NOT NULL,
  `speed` decimal(4,2) UNSIGNED NOT NULL,
  `attack_speed` decimal(4,2) UNSIGNED NOT NULL,
  `def` int(10) UNSIGNED NOT NULL,
  `magic_def` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `enemies`
--

INSERT INTO `enemies` (`enemy_id`, `name`, `description`, `health`, `strength`, `speed`, `attack_speed`, `def`, `magic_def`) VALUES
(1, 'slime', 'Pequeño pero feroz goblin armado con una espada oxidada.', 120, 25, '3.50', '2.80', 8, 4),
(2, 'Esqueleto Arquero', 'Esqueleto no-muerto que ataca a distancia con flechas.', 80, 30, '2.20', '3.10', 5, 12),
(3, 'Orc Berserker', 'Orc salvaje que entra en furia durante el combate.', 200, 45, '2.80', '2.50', 15, 6),
(4, 'Mago Oscuro', 'Hechicero corrupto que domina la magia negra.', 150, 20, '2.00', '1.80', 8, 25),
(5, 'Lobo Gigante', 'Bestia feroz con colmillos afilados y gran velocidad.', 180, 35, '4.20', '3.50', 12, 8),
(6, 'Dragón Menor', 'Joven dragón con aliento de fuego devastador.', 350, 60, '3.00', '2.20', 22, 18),
(7, 'Gólem de Piedra', 'Construcción mágica extremadamente resistente.', 280, 40, '1.50', '1.20', 30, 15),
(8, 'Araña Venenosa', 'Araña gigante que paraliza a sus víctimas.', 100, 28, '3.80', '4.00', 6, 10),
(9, 'Troll de Montaña', 'Troll primitivo con regeneración natural.', 320, 55, '2.10', '1.80', 20, 12),
(10, 'Espectro', 'Espíritu vengativo inmune a ataques físicos normales.', 90, 35, '3.20', '2.90', 5, 28);

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `item_id` int(10) UNSIGNED NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `value` int(10) UNSIGNED NOT NULL,
  `subtype` enum('weapon','armor','material','consumable') NOT NULL DEFAULT 'material'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`item_id`, `name`, `description`, `value`, `subtype`) VALUES
(1, 'Acero', 'Un trozo de acero viejo obtenido del arma de tus enemigos', 50, 'material'),
(2, 'Espada de Hierro', 'Una espada básica forjada con hierro puro. Ideal para principiantes.', 250, 'weapon'),
(3, 'Poción de Salud Menor', 'Restaura 50 puntos de salud instantáneamente.', 25, 'consumable'),
(4, 'Armadura de Cuero', 'Protección ligera que no reduce la agilidad del portador.', 180, 'armor'),
(5, 'Cristal Mágico', 'Un cristal imbuido con energía arcana. Útil para encantamientos.', 150, 'material'),
(6, 'Espada Flamígera', 'Espada encantada que causa daño de fuego adicional.', 850, 'weapon'),
(7, 'Escudo de Acero', 'Escudo resistente que proporciona excelente protección.', 320, 'weapon'),
(8, 'Poción de Maná', 'Restaura 75 puntos de maná mágico.', 40, 'consumable'),
(9, 'Botas de Velocidad', 'Aumentan significativamente la velocidad de movimiento.', 450, 'armor'),
(10, 'Gema de Poder', 'Gema rara que aumenta el poder mágico del portador.', 600, 'material'),
(11, 'Arco Élfico', 'Arco ligero y preciso usado por los elfos.', 380, 'weapon'),
(12, 'Túnica del Mago', 'Vestimenta que aumenta la defensa mágica.', 280, 'armor'),
(13, 'Elixir de Fuerza', 'Poción que aumenta temporalmente la fuerza física.', 65, 'consumable'),
(14, 'Daga Envenenada', 'Daga rápida que aplica veneno en cada golpe.', 190, 'weapon'),
(15, 'Amuleto Protector', 'Amuleto que reduce el daño recibido.', 220, 'armor'),
(16, 'Martillo de Guerra', 'Arma pesada capaz de destruir armaduras.', 520, 'weapon'),
(17, 'Casco de Batalla', 'Protege la cabeza de golpes críticos.', 160, 'armor'),
(18, 'Pergamino de Teletransporte', 'Permite escapar instantáneamente del combate.', 80, 'consumable'),
(19, 'Lingote de Oro', 'Oro puro usado para crear objetos valiosos.', 300, 'material'),
(20, 'Báculo Arcano', 'Báculo que amplifica el poder de los hechizos.', 720, 'weapon');

-- --------------------------------------------------------

--
-- Table structure for table `materials`
--

CREATE TABLE `materials` (
  `item_id` int(10) UNSIGNED NOT NULL,
  `type` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `materials`
--

INSERT INTO `materials` (`item_id`, `type`) VALUES
(1, 'Componente'),
(5, 'Gema'),
(10, 'Gema'),
(19, 'Metal');

-- --------------------------------------------------------

--
-- Table structure for table `players`
--

CREATE TABLE `players` (
  `player_id` int(10) UNSIGNED NOT NULL,
  `username` varchar(100) NOT NULL,
  `level` tinyint(3) UNSIGNED NOT NULL DEFAULT '1',
  `cryptos` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `last_connection` date NOT NULL,
  `runs` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `experience` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `players`
--

INSERT INTO `players` (`player_id`, `username`, `level`, `cryptos`, `last_connection`, `runs`, `experience`) VALUES
(1, 'DragonSlayer', 25, 15000, '2025-06-07', 143, 62500),
(2, 'ShadowMage', 18, 8500, '2025-06-06', 87, 32400),
(3, 'IronWarrior', 22, 12300, '2025-06-07', 156, 48400),
(4, 'StealthNinja', 31, 25600, '2025-06-05', 203, 96100),
(5, 'FireWizard', 15, 6200, '2025-06-07', 65, 22500),
(6, 'HolyPaladin', 28, 18900, '2025-06-04', 178, 78400),
(7, 'DarkAssassin', 35, 35200, '2025-06-06', 267, 122500),
(8, 'FrostMage', 20, 9800, '2025-06-07', 98, 40000),
(9, 'BattleAxe', 26, 16700, '2025-06-03', 189, 67600),
(10, 'MysticArcher', 19, 7900, '2025-06-07', 76, 36100);

-- --------------------------------------------------------

--
-- Table structure for table `sells`
--

CREATE TABLE `sells` (
  `sale_id` int(10) UNSIGNED NOT NULL,
  `item_id` int(10) UNSIGNED NOT NULL,
  `player_id` int(10) UNSIGNED NOT NULL,
  `income` int(10) UNSIGNED NOT NULL,
  `quantity` tinyint(3) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `sells`
--

INSERT INTO `sells` (`sale_id`, `item_id`, `player_id`, `income`, `quantity`) VALUES
(1, 1, 1, 25, 5),
(2, 3, 2, 75, 3),
(3, 5, 3, 600, 4),
(4, 1, 4, 15, 3),
(5, 8, 5, 120, 3),
(6, 4, 6, 180, 1),
(7, 19, 7, 900, 3),
(8, 3, 8, 50, 2),
(9, 1, 9, 35, 7),
(10, 10, 10, 1200, 2),
(11, 5, 1, 300, 2),
(12, 13, 3, 195, 3),
(13, 1, 5, 20, 4),
(14, 8, 7, 80, 2),
(15, 3, 9, 100, 4);

-- --------------------------------------------------------

--
-- Table structure for table `weapons`
--

CREATE TABLE `weapons` (
  `item_id` int(10) UNSIGNED NOT NULL,
  `strength` int(10) UNSIGNED NOT NULL,
  `range` int(10) UNSIGNED NOT NULL,
  `attack_speed` decimal(4,2) UNSIGNED NOT NULL,
  `category` enum('Physical','Magic') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `weapons`
--

INSERT INTO `weapons` (`item_id`, `strength`, `range`, `attack_speed`, `category`) VALUES
(2, 35, 1, '2.50', 'Physical'),
(6, 55, 1, '2.20', 'Physical'),
(11, 42, 8, '1.80', 'Physical'),
(14, 28, 1, '3.50', 'Physical'),
(16, 65, 1, '1.50', 'Physical'),
(20, 48, 6, '2.00', 'Magic');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `armors`
--
ALTER TABLE `armors`
  ADD PRIMARY KEY (`item_id`);

--
-- Indexes for table `consumables`
--
ALTER TABLE `consumables`
  ADD PRIMARY KEY (`item_id`);

--
-- Indexes for table `drops`
--
ALTER TABLE `drops`
  ADD PRIMARY KEY (`enemy_id`,`item_id`),
  ADD KEY `drops_items_FK` (`item_id`);

--
-- Indexes for table `enemies`
--
ALTER TABLE `enemies`
  ADD PRIMARY KEY (`enemy_id`),
  ADD UNIQUE KEY `enemies_unique` (`name`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`item_id`),
  ADD UNIQUE KEY `items_unique` (`name`);

--
-- Indexes for table `materials`
--
ALTER TABLE `materials`
  ADD PRIMARY KEY (`item_id`);

--
-- Indexes for table `players`
--
ALTER TABLE `players`
  ADD PRIMARY KEY (`player_id`),
  ADD UNIQUE KEY `players_unique` (`username`);

--
-- Indexes for table `sells`
--
ALTER TABLE `sells`
  ADD PRIMARY KEY (`sale_id`,`item_id`,`player_id`),
  ADD KEY `sells_items_FK` (`item_id`),
  ADD KEY `sells_players_FK` (`player_id`);

--
-- Indexes for table `weapons`
--
ALTER TABLE `weapons`
  ADD PRIMARY KEY (`item_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `enemies`
--
ALTER TABLE `enemies`
  MODIFY `enemy_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `item_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `players`
--
ALTER TABLE `players`
  MODIFY `player_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `sells`
--
ALTER TABLE `sells`
  MODIFY `sale_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `armors`
--
ALTER TABLE `armors`
  ADD CONSTRAINT `armors_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `consumables`
--
ALTER TABLE `consumables`
  ADD CONSTRAINT `consumables_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `drops`
--
ALTER TABLE `drops`
  ADD CONSTRAINT `drops_enemies_FK` FOREIGN KEY (`enemy_id`) REFERENCES `enemies` (`enemy_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `drops_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `materials`
--
ALTER TABLE `materials`
  ADD CONSTRAINT `materials_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `sells`
--
ALTER TABLE `sells`
  ADD CONSTRAINT `sells_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `sells_players_FK` FOREIGN KEY (`player_id`) REFERENCES `players` (`player_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `weapons`
--
ALTER TABLE `weapons`
  ADD CONSTRAINT `weapons_items_FK` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
