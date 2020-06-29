-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: users
-- ------------------------------------------------------
-- Server version	8.0.20

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
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_name` varchar(45) NOT NULL,
  `user_cep` varchar(8) NOT NULL,
  `user_address` varchar(100) NOT NULL,
  `user_addressNumber` varchar(10) NOT NULL,
  `user_neighborhood` varchar(45) NOT NULL,
  `user_city` varchar(45) NOT NULL,
  `user_cpf_cnpj` varchar(45) NOT NULL,
  `user_rg_ie` varchar(45) NOT NULL,
  `user_email` varchar(100) NOT NULL,
  `user_site` varchar(45) NOT NULL,
  `user_telefone` varchar(45) NOT NULL,
  `user_cellphone` varchar(45) NOT NULL,
  `user_photo` longblob NOT NULL,
  `user_obs` varchar(250) NOT NULL,
  `user_type` varchar(1) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'users'
--
/*!50003 DROP PROCEDURE IF EXISTS `DeleteUserByID` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`Augusto`@`%` PROCEDURE `DeleteUserByID`(
	_user_id int
    )
BEGIN
	DELETE FROM user	
    WHERE user_id = _user_id;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SearchByValue` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`Augusto`@`%` PROCEDURE `SearchByValue`(
	_SearchInput varchar(50)
)
BEGIN
	SELECT 
		user_name as 'Nome Completo', 
        user_cep as 'CEP', 
        user_address as 'Endereço', 
        user_addressNumber as 'Numero', 
        user_neighborhood as 'Bairro', 
        user_city as 'Cidade', 
        user_cpf_cnpj as 'CPF/CNPJ', 
        user_rg_ie as 'RG/IE', 
        user_email as 'E-Mail', 
        user_site as 'Site', 
        user_telefone as 'Telefone', 
        user_cellphone as 'Celular', 
        user_photo , 
        user_obs as 'Observação', 
        user_type
    FROM user
    WHERE user_name like CONCAT('%', _SearchInput , '%')
    or user_address like CONCAT('%', _SearchInput , '%')
    or user_cpf_cnpj = _SearchInput;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `UserAddOrEdit` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`Augusto`@`%` PROCEDURE `UserAddOrEdit`(
	_user_id int,
    _user_cep VARCHAR(8),
	_user_name VARCHAR(45),
	_user_address VARCHAR(100),
	_user_addressNumber varchar(10),
	_user_neighborhood VARCHAR(45),
    _user_city VARCHAR(45),
	_user_cpf_cnpj VARCHAR(45),
	_user_rg_ie VARCHAR(45),
	_user_email VARCHAR(100),
	_user_site VARCHAR(45),
	_user_telefone VARCHAR(45),
	_user_cellphone VARCHAR(45),
	_user_photo longblob,
	_user_obs VARCHAR(250),
    _user_type varchar(1)
)
BEGIN
	IF _user_id = 0 then
		INSERT INTO user (user_name, user_cep, user_address, user_addressNumber, user_neighborhood, user_city, user_cpf_cnpj, user_rg_ie, user_email, user_site, user_telefone, user_cellphone, user_photo, user_obs, user_type)
		values (_user_name,
			_user_cep,
			_user_address,
			_user_addressNumber,
			_user_neighborhood,
            _user_city,
			_user_cpf_cnpj,
			_user_rg_ie,
			_user_email,
			_user_site,
			_user_telefone,
			_user_cellphone,
			_user_photo,
			_user_obs,
            _user_type
		);
	else
		update user
        set user_name = _user_name,
			user_address = _user_address,
            user_addressNumber = _user_addressNumber,
            user_neighborhood = _user_neighborhood,
            user_cpf_cnpj = _user_cpf_cnpj,
            user_rg_ie = _user_rg_ie,
            user_email = _user_email,
            user_site = _user_site,
            user_telefone = _user_telefone,
            user_cellphone = _user_cellphone,
            user_photo = _user_photo,
            user_obs = _user_obs,
            user_type = _user_type
		where user_id = _user_id;
	end if;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ViewAllUser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`Augusto`@`%` PROCEDURE `ViewAllUser`()
BEGIN
	SELECT user_id , 
		user_name as 'Nome Completo', 
        user_cep as 'CEP', 
        user_address as 'Endereço', 
        user_addressNumber as 'Numero', 
        user_neighborhood as 'Bairro', 
        user_city as 'Cidade', 
        user_cpf_cnpj as 'CPF/CNPJ', 
        user_rg_ie as 'RG/IE', 
        user_email as 'E-Mail', 
        user_site as 'Site', 
        user_telefone as 'Telefone', 
        user_cellphone as 'Celular', 
        user_photo , 
        user_obs as 'Observação', 
        user_type FROM user;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-06-28 22:43:06
