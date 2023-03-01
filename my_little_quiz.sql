-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 01 Mar 2023, 11:23
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
(2, 'papa', 'papagaj@papo.pa', 'asdfqwer');

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
  MODIFY `idLogon` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT dla tabeli `moderators`
--
ALTER TABLE `moderators`
  MODIFY `IdModerator` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT dla tabeli `quizzes`
--
ALTER TABLE `quizzes`
  MODIFY `QuizId` int(11) NOT NULL AUTO_INCREMENT;

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
