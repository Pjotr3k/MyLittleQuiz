-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 02 Mar 2023, 18:30
-- Wersja serwera: 10.4.25-MariaDB
-- Wersja PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `my_little_quiz`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `logons`
--

CREATE TABLE `logons` (
  `idLogon` int(11) NOT NULL,
  `login` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Zrzut danych tabeli `logons`
--

INSERT INTO `logons` (`idLogon`, `login`, `email`, `password`) VALUES
(1, 'admin', 'admin@adam.ad', 'adminadam'),
(2, 'papa', 'papagaj@papo.pa', 'asdfqwer'),
(3, 'Steev', 'swedstev@sweet.se', 'seagull'),
(4, 'ignorant', 'ign@ig.is', 'niewiem'),
(5, 'batman', 'nooneknows@howits.li', 'dc');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `moderators`
--

CREATE TABLE `moderators` (
  `IdModerator` int(11) NOT NULL,
  `IdQuiz` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IsCreator` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Zrzut danych tabeli `moderators`
--

INSERT INTO `moderators` (`IdModerator`, `IdQuiz`, `IdUser`, `IsCreator`) VALUES
(2, 28, 1, 1),
(3, 29, 2, 1),
(4, 30, 1, 1),
(5, 31, 1, 1),
(6, 32, 1, 1),
(7, 33, 1, 1),
(8, 34, 1, 1),
(9, 35, 1, 1),
(10, 36, 1, 1),
(11, 37, 1, 1),
(12, 38, 1, 1),
(13, 39, 1, 1),
(16, 40, 4, 1),
(17, 41, 5, 1),
(18, 42, 3, 1),
(19, 43, 3, 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `quizzes`
--

CREATE TABLE `quizzes` (
  `QuizId` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `IsPublic` tinyint(1) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `LastModification` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Zrzut danych tabeli `quizzes`
--

INSERT INTO `quizzes` (`QuizId`, `Name`, `Description`, `IsPublic`, `CreatorId`, `CreationDate`, `LastModification`) VALUES
(28, 'Czy dziala', 'czy też nie?', 0, 1, '2023-02-24 15:11:56', '2023-02-24 15:11:56'),
(29, 'dobra, jeszcze raz czy się mod dodaje', 'kurde, to nie powinno być required', 1, 2, '2023-02-28 11:14:04', '2023-02-28 11:14:04'),
(30, 'czy jak zaczne wpychac do dobrej tabeli to sie uda w koncu?', 'kurde, to nie powinno być required', 0, 1, '2023-02-28 12:38:39', '2023-02-28 12:38:39'),
(31, 'dobra, jeszcze raz czy się mod dodaje', 'eeeee', 0, 1, '2023-02-28 13:07:02', '2023-02-28 13:07:02'),
(32, 'dobra, jeszcze raz czy się mod dodaje', 'czy też nie?', 1, 1, '2023-02-28 13:19:03', '2023-02-28 13:19:03'),
(33, 'Czy dziala', 'suabo', 0, 1, '2023-02-28 13:21:46', '2023-02-28 13:21:46'),
(34, 'czy jak zaczne wpychac do dobrej tabeli to sie uda w koncu?', 'działa to działa', 0, 1, '2023-02-28 13:24:01', '2023-02-28 13:24:01'),
(35, 'Czy dziala', 'kurde, to nie powinno być required', 0, 1, '2023-02-28 13:29:04', '2023-02-28 13:29:04'),
(36, 'Czy getDateTime działa?', 'Blabla', 0, 1, '2023-02-28 13:55:59', '2023-02-28 13:55:59'),
(37, 'Czy jestem modem?', 'działa to działa', 0, 1, '2023-02-28 14:09:01', '2023-02-28 14:09:01'),
(38, 'Czy jestem modem?', 'działa to działa', 0, 1, '2023-03-01 11:15:03', '2023-03-01 11:15:03'),
(39, 'jeju jakie tu kombinacje ido?', 'nico', 0, 1, '2023-03-01 13:04:26', '2023-03-01 13:04:26'),
(40, 'czy jak zaczne wpychac do dobrej tabeli to sie uda w koncu?', 'adsfasdf', 0, 4, '2023-03-02 14:27:57', '2023-03-02 14:27:57'),
(41, 'Kto batmanem był', 'Patrick Bateman. Chyba', 0, 5, '2023-03-02 16:48:28', '2023-03-02 16:48:28'),
(42, 'Kolejne starcie', 'Słabe to pytanie', 0, 3, '2023-03-02 18:21:22', '2023-03-02 18:21:22'),
(43, 'Czy chodzi o apostrof', 'czy też nie?', 0, 3, '2023-03-02 18:29:28', '2023-03-02 18:29:28');

--
-- Wyzwalacze `quizzes`
--
DELIMITER $$
CREATE TRIGGER `add_creator_as_mod` AFTER INSERT ON `quizzes` FOR EACH ROW INSERT INTO moderators (IdQuiz, IdUser, IsCreator)
SELECT quizzes.QuizId, quizzes.CreatorId, 1
FROM (quizzes
LEFT JOIN moderators ON quizzes.QuizId = moderators.IdQuiz)
WHERE moderators.IdModerator IS NULL
$$
DELIMITER ;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `logons`
--
ALTER TABLE `logons`
  ADD PRIMARY KEY (`idLogon`),
  ADD UNIQUE KEY `login` (`login`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indeksy dla tabeli `moderators`
--
ALTER TABLE `moderators`
  ADD PRIMARY KEY (`IdModerator`),
  ADD KEY `IdQuiz` (`IdQuiz`),
  ADD KEY `IdUser` (`IdUser`);

--
-- Indeksy dla tabeli `quizzes`
--
ALTER TABLE `quizzes`
  ADD PRIMARY KEY (`QuizId`),
  ADD KEY `CreatorId` (`CreatorId`);

--
-- AUTO_INCREMENT dla zrzuconych tabel
--

--
-- AUTO_INCREMENT dla tabeli `logons`
--
ALTER TABLE `logons`
  MODIFY `idLogon` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT dla tabeli `moderators`
--
ALTER TABLE `moderators`
  MODIFY `IdModerator` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT dla tabeli `quizzes`
--
ALTER TABLE `quizzes`
  MODIFY `QuizId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- Ograniczenia dla zrzutów tabel
--

--
-- Ograniczenia dla tabeli `moderators`
--
ALTER TABLE `moderators`
  ADD CONSTRAINT `moderators_ibfk_1` FOREIGN KEY (`IdQuiz`) REFERENCES `quizzes` (`QuizId`),
  ADD CONSTRAINT `moderators_ibfk_2` FOREIGN KEY (`IdUser`) REFERENCES `logons` (`idLogon`);

--
-- Ograniczenia dla tabeli `quizzes`
--
ALTER TABLE `quizzes`
  ADD CONSTRAINT `quizzes_ibfk_1` FOREIGN KEY (`CreatorId`) REFERENCES `logons` (`idLogon`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
