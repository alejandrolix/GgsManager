-- MySqlBackup.NET 2.0.11
-- Dump Time: 2018-05-31 22:55:43
-- --------------------------------------
-- Server version 10.1.22-MariaDB mariadb.org binary distribution

CREATE DATABASE ggsmanager;
USE ggsmanager;


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES latin1 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of clientes
-- 

DROP TABLE IF EXISTS `clientes`;
CREATE TABLE IF NOT EXISTS `clientes` (
  `IdCliente` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(40) NOT NULL,
  `Apellidos` varchar(80) NOT NULL,
  `DNI` varchar(10) NOT NULL,
  `Direccion` varchar(50) NOT NULL,
  `Poblacion` varchar(50) NOT NULL,
  `Provincia` varchar(40) NOT NULL,
  `Movil` varchar(9) NOT NULL,
  `FechaHoraAlta` datetime NOT NULL,
  `Observaciones` longtext,
  PRIMARY KEY (`IdCliente`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table clientes
-- 

/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes`(`IdCliente`,`Nombre`,`Apellidos`,`DNI`,`Direccion`,`Poblacion`,`Provincia`,`Movil`,`FechaHoraAlta`,`Observaciones`) VALUES
(1,'María','Jiménez Saura','50197836T','C/ Los Almendros, 45','Alicante','Alicante','690857492','2018-05-31 22:21:37',''),
(2,'Laura','Del Castillo','85209148W','Avda. Poeta Zorrilla, 1','Alicante','Alicante','678945612','2018-05-31 22:35:54','Es Amiga de María'),
(4,'Fermín','De la Torre Álvarez','38295873S','Avda. Sergio Cardell, 3','Madrid','Madrid','638295867','2018-05-31 22:35:47',''),
(5,'Antonia','Santos','85201208E','C/ Carratalá, 3','Madrid','Madrid','603692590','2018-05-31 22:37:55','');
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;

-- 
-- Definition of facturas
-- 

DROP TABLE IF EXISTS `facturas`;
CREATE TABLE IF NOT EXISTS `facturas` (
  `IdFactura` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha` date NOT NULL,
  `IdCliente` int(11) DEFAULT NULL,
  `IdGaraje` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdFactura`),
  KEY `FK_IdCliente_idx` (`IdCliente`),
  KEY `IdGaraje` (`IdGaraje`),
  CONSTRAINT `Facturas_ibfk_1` FOREIGN KEY (`IdCliente`) REFERENCES `clientes` (`IdCliente`),
  CONSTRAINT `Facturas_ibfk_2` FOREIGN KEY (`IdGaraje`) REFERENCES `garajes` (`IdGaraje`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table facturas
-- 

/*!40000 ALTER TABLE `facturas` DISABLE KEYS */;

/*!40000 ALTER TABLE `facturas` ENABLE KEYS */;

-- 
-- Definition of garajes
-- 

DROP TABLE IF EXISTS `garajes`;
CREATE TABLE IF NOT EXISTS `garajes` (
  `IdGaraje` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(60) NOT NULL,
  `Direccion` varchar(80) NOT NULL,
  `NumPlazas` int(11) NOT NULL,
  `NumPlazasLibres` int(11) NOT NULL,
  `NumPlazasOcupadas` int(11) NOT NULL,
  `Observaciones` longtext,
  PRIMARY KEY (`IdGaraje`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table garajes
-- 

/*!40000 ALTER TABLE `garajes` DISABLE KEYS */;
INSERT INTO `garajes`(`IdGaraje`,`Nombre`,`Direccion`,`NumPlazas`,`NumPlazasLibres`,`NumPlazasOcupadas`,`Observaciones`) VALUES
(1,'Alicante','C/ Poeta Antonio Calvo, 5',23,21,2,''),
(2,'Madrid','Avda. Luisa Giménez, 56',30,28,2,'En Obras');
/*!40000 ALTER TABLE `garajes` ENABLE KEYS */;

-- 
-- Definition of plazas
-- 

DROP TABLE IF EXISTS `plazas`;
CREATE TABLE IF NOT EXISTS `plazas` (
  `IdPlaza` int(11) NOT NULL AUTO_INCREMENT,
  `IdGaraje` int(11) NOT NULL,
  `IdSituacion` int(11) NOT NULL,
  PRIMARY KEY (`IdPlaza`,`IdGaraje`),
  KEY `FK_IdGaraje_idx` (`IdGaraje`),
  KEY `FK_IdSituacion_idx` (`IdSituacion`),
  CONSTRAINT `FK_IdGarajePlazas` FOREIGN KEY (`IdGaraje`) REFERENCES `garajes` (`IdGaraje`),
  CONSTRAINT `FK_IdSituacion` FOREIGN KEY (`IdSituacion`) REFERENCES `situacionesplaza` (`IdSituacion`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table plazas
-- 

/*!40000 ALTER TABLE `plazas` DISABLE KEYS */;
INSERT INTO `plazas`(`IdPlaza`,`IdGaraje`,`IdSituacion`) VALUES
(3,1,1),
(3,2,1),
(4,1,1),
(4,2,1),
(5,1,1),
(5,2,1),
(6,1,1),
(6,2,1),
(7,1,1),
(7,2,1),
(8,1,1),
(8,2,1),
(9,1,1),
(9,2,1),
(10,1,1),
(10,2,1),
(11,1,1),
(11,2,1),
(12,1,1),
(12,2,1),
(13,1,1),
(13,2,1),
(14,1,1),
(14,2,1),
(15,1,1),
(15,2,1),
(16,1,1),
(16,2,1),
(17,1,1),
(17,2,1),
(18,1,1),
(18,2,1),
(19,1,1),
(19,2,1),
(20,1,1),
(20,2,1),
(21,1,1),
(21,2,1),
(22,1,1),
(22,2,1),
(23,1,1),
(23,2,1),
(24,2,1),
(25,2,1),
(26,2,1),
(27,2,1),
(28,2,1),
(29,2,1),
(30,2,1),
(1,1,2),
(1,2,2),
(2,1,2),
(2,2,2);
/*!40000 ALTER TABLE `plazas` ENABLE KEYS */;

-- 
-- Definition of situacionesplaza
-- 

DROP TABLE IF EXISTS `situacionesplaza`;
CREATE TABLE IF NOT EXISTS `situacionesplaza` (
  `IdSituacion` int(11) NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  PRIMARY KEY (`IdSituacion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table situacionesplaza
-- 

/*!40000 ALTER TABLE `situacionesplaza` DISABLE KEYS */;
INSERT INTO `situacionesplaza`(`IdSituacion`,`Tipo`) VALUES
(1,'Libre'),
(2,'Ocupada');
/*!40000 ALTER TABLE `situacionesplaza` ENABLE KEYS */;

-- 
-- Definition of usuarios
-- 

DROP TABLE IF EXISTS `usuarios`;
CREATE TABLE IF NOT EXISTS `usuarios` (
  `IdUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(14) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `EsGestor` tinyint(4) NOT NULL,
  PRIMARY KEY (`IdUsuario`),
  KEY `IndiceNombre` (`Nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table usuarios
-- 

/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios`(`IdUsuario`,`Nombre`,`Password`,`EsGestor`) VALUES
(3,'Alejandro','8b8e92d17af4f1fcd02ecf281f6cf80b796b596a',1),
(4,'Pepe','e04820372e7f2ebb2d76987433579219b11c2ba5',0),
(5,'Mario','addb47291ee169f330801ce73520b96f2eaf20ea',0);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;

-- 
-- Definition of vehiculos
-- 

DROP TABLE IF EXISTS `vehiculos`;
CREATE TABLE IF NOT EXISTS `vehiculos` (
  `IdVehiculo` int(11) NOT NULL AUTO_INCREMENT,
  `Matricula` varchar(10) NOT NULL,
  `Marca` varchar(10) NOT NULL,
  `Modelo` varchar(40) NOT NULL,
  `IdCliente` int(11) NOT NULL,
  `IdGaraje` int(11) NOT NULL,
  `IdPlaza` int(11) NOT NULL,
  `PrecioBase` float NOT NULL,
  `PrecioTotal` float NOT NULL,
  PRIMARY KEY (`IdVehiculo`),
  KEY `FK_IdCliente_idx` (`IdCliente`),
  KEY `FK_IdCliente` (`IdCliente`),
  KEY `FK_IdCliente_idxx` (`IdCliente`),
  KEY `FK_IdGarajeVehiculos_idx` (`IdGaraje`),
  CONSTRAINT `FK_IdClienteVehiculos` FOREIGN KEY (`IdCliente`) REFERENCES `clientes` (`IdCliente`),
  CONSTRAINT `FK_IdGarajeVehiculos` FOREIGN KEY (`IdGaraje`) REFERENCES `garajes` (`IdGaraje`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table vehiculos
-- 

/*!40000 ALTER TABLE `vehiculos` DISABLE KEYS */;
INSERT INTO `vehiculos`(`IdVehiculo`,`Matricula`,`Marca`,`Modelo`,`IdCliente`,`IdGaraje`,`IdPlaza`,`PrecioBase`,`PrecioTotal`) VALUES
(1,'3456 NHY','Citroen','C4',4,1,1,34.4,41.624),
(2,'9809 XCX','Seat','Ibiza',2,1,2,45,54.45),
(3,'6776 BGH','Renault','Scenic',1,2,1,56,67.76),
(4,'9263 WSX','Citroen','Sara Picasso',5,2,2,12,14.52);
/*!40000 ALTER TABLE `vehiculos` ENABLE KEYS */;

-- 
-- Dumping functions
-- 

DROP FUNCTION IF EXISTS `ComprobarPasswords`;
DELIMITER |
CREATE FUNCTION `ComprobarPasswords`(passwordIntroducida VARCHAR(255), passwordBd VARCHAR(255)) RETURNS tinyint(1)
BEGIN
RETURN STRCMP(passwordIntroducida, passwordBd) = 0;
END |
DELIMITER ;

DROP FUNCTION IF EXISTS `ExisteUsuario`;
DELIMITER |
CREATE FUNCTION `ExisteUsuario`(nombreUsuario VARCHAR(14)) RETURNS tinyint(1)
BEGIN
DECLARE numUsuarios INT;
SELECT COUNT(IdUsuario) INTO numUsuarios
FROM   Usuarios
WHERE  Nombre = nombreUsuario;
IF numUsuarios = 1 THEN
RETURN TRUE;
ELSE
RETURN FALSE;
END IF;
END |
DELIMITER ;

DROP FUNCTION IF EXISTS `ObtenerPasswordUsuario`;
DELIMITER |
CREATE FUNCTION `ObtenerPasswordUsuario`(nombreUsuario VARCHAR(14)) RETURNS varchar(255) CHARSET utf8
BEGIN
DECLARE passUsuario VARCHAR(255);
SELECT Password INTO passUsuario
FROM   Usuarios
WHERE  Nombre = nombreUsuario;
RETURN passUsuario;
END |
DELIMITER ;

-- 
-- Dumping triggers
-- 

DROP TRIGGER /*!50030 IF EXISTS */ `BeforeDeleteClientes`;
DELIMITER |
CREATE TRIGGER BeforeDeleteClientes
BEFORE DELETE ON Clientes FOR EACH ROW
BEGIN
DECLARE NumVehiculos INT;
DECLARE NumFacturas INT;
IF NOT EXISTS(SELECT 1
FROM   Clientes
WHERE  IdCliente = OLD.IdCliente) THEN
SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No Existe el Cliente';
ELSE
SELECT COUNT(IdVehiculo) INTO NumVehiculos
FROM   Vehiculos
WHERE  IdCliente = OLD.IdCliente;
IF NumVehiculos = 1 THEN				-- Eliminamos el veh?culo del cliente. Si un cliente no tuviese ni veh?culo ni factura, tambi?n se elimina.
DELETE FROM Vehiculos
WHERE  IdCliente = OLD.IdCliente;
END IF;
SELECT COUNT(IdFactura) INTO NumFacturas
FROM   Facturas
WHERE  IdCliente = OLD.IdCliente;
IF NumFacturas >= 1 THEN			-- Eliminamos las facturas del cliente.
DELETE FROM Facturas
WHERE  IdCliente = OLD.IdCliente;
END IF;
END IF;
END |
DELIMITER ;

DROP TRIGGER /*!50030 IF EXISTS */ `BeforeDeleteGarajes`;
DELIMITER |
CREATE TRIGGER BeforeDeleteGarajes
BEFORE DELETE ON Garajes FOR EACH ROW
BEGIN
DECLARE NumVehiculos INT;
DECLARE NumFacturas INT;
IF NOT EXISTS(SELECT 1
FROM   Garajes
WHERE  IdGaraje = OLD.IdGaraje) THEN
SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No Existe el Garaje';
ELSE
SELECT COUNT(IdVehiculo) INTO NumVehiculos
FROM   Vehiculos
WHERE  IdGaraje = OLD.IdGaraje;
IF NumVehiculos >= 1 THEN
DELETE FROM Vehiculos
WHERE  IdGaraje = OLD.IdGaraje;
END IF;
DELETE FROM Plazas
WHERE  IdGaraje = OLD.IdGaraje;
SELECT COUNT(IdFactura) INTO NumFacturas
FROM   Facturas
WHERE  IdGaraje = OLD.IdGaraje;
IF NumFacturas >= 1 THEN
DELETE FROM Facturas
WHERE  IdGaraje = OLD.IdGaraje;
END IF;
END IF;
END |
DELIMITER ;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2018-05-31 22:55:43
-- Total time: 0:0:0:0:564 (d:h:m:s:ms)
