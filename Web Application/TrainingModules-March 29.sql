---Create Training Modules SQL Script
---CONESTOGA COLLEGE STAFF TRAINING MODULES REGISTRATIOn
---Created by Group1 Hongwei Cao, Yongwu, Raj Kannan 
----Revision History
----2014.03.16 Created, Version 1.0 complete
----2014.03.19 Version 1.1, added more tables and revised some table according to the ERD. 

USE Master;
SET NOCOUNT ON;

GO

PRINT '>>> Does an TrainingRegistion database already exist?';

GO 

IF EXISTS (SELECT "name"
           FROM Sysdatabases
           WHERE "name" = 'TrainingRegistion')    
    BEGIN
        PRINT '>>> Yes, an TrainingRegistion database already exists';
        PRINT '>>> Rolling back pending TrainingRegistion transactions';
         
        ALTER DATABASE TrainingRegistion 
            SET SINGLE_USER
            WITH ROLLBACK IMMEDIATE;
 
        PRINT '>>> Dropping the existing TrainingRegistion database';   
        
        DROP DATABASE TrainingRegistion;
    END
ELSE
    BEGIN
        PRINT '>>> No, there is no TrainingRegistion database';    
    END
GO

--IF EXISTS (SELECT "name"
--           FROM Sysdatabases
--           WHERE "name" = 'TrainingRegistion') 
--BEGIN
--   DROP DATABASE TrainingRegistion;
--END
GO

CREATE DATABASE TrainingRegistion;
GO

USE TrainingRegistion;
GO


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Quiz_Module_FK')
BEGIN
	ALTER TABLE Quiz
	DROP CONSTRAINT Quiz_Module_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='TrainingModule_Module_FK')
BEGIN
	ALTER TABLE TrainingModule
	DROP CONSTRAINT TrainingModule_Module_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Document_Training_Module_FK')
BEGIN
	ALTER TABLE Document
	DROP CONSTRAINT Document_Training_Module_FK
END

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Agreement_Module_FK')
BEGIN
	ALTER TABLE Account
	DROP CONSTRAINT Agreement_Module_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Account_Person_FK')
BEGIN
	ALTER TABLE Account
	DROP CONSTRAINT Account_Person_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Quiz_record_Account_FK')
BEGIN
	ALTER TABLE Quiz_record
	DROP CONSTRAINT Quiz_record_Account_FK
END

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Quiz_record_Quiz_FK')
BEGIN
	ALTER TABLE Quiz_record
	DROP CONSTRAINT Quiz_record_Quiz_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Exception_Account_FK')
BEGIN
	ALTER TABLE Exception
	DROP CONSTRAINT Exception_Account_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Study_Document_Record_Document_FK')
BEGIN
	ALTER TABLE Study_Document_Record
	DROP CONSTRAINT Study_Document_Record_Document_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Study_Document_Record_Account_FK')
BEGIN
	ALTER TABLE Study_Document_Record
	DROP CONSTRAINT Study_Document_Record_Account_FK
END


-------------------------------

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Person_Department_Campus_Campus_FK')
BEGIN
	ALTER TABLE Person_Department_Campus
	DROP CONSTRAINT Person_Department_Campus_Campus_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Person_Department_Campus_Department_FK')
BEGIN
	ALTER TABLE Person_Department_Campus
	DROP CONSTRAINT Person_Department_Campus_Department_FK
END

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Person_Department_Campus_Person_FK')
BEGIN
	ALTER TABLE Person_Department_Campus
	DROP CONSTRAINT Person_Department_Campus_Person_FK
END
-----------------------


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Question_Answer_Quiz_FK')
BEGIN
	ALTER TABLE Question_Answer
	DROP CONSTRAINT Question_Answer_Quiz_FK
END

-------------------

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Quiz_Account_Answer_Account_FK')
BEGIN
	ALTER TABLE Quiz_Account_Answer
	DROP CONSTRAINT Quiz_Account_Answer_Account_FK
END

--------------------

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Qustion_Answer_Multiple_Selection_QA_FK')
BEGIN
	ALTER TABLE Qustion_Answer_Multiple_Selection
	DROP CONSTRAINT Qustion_Answer_Multiple_Selection_QA_FK
END


------------------------------------------------
IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Account_Module_Account_FK')
BEGIN
	ALTER TABLE Account_Module
	DROP CONSTRAINT Account_Module_Account_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Account_Module_Module_FK')
BEGIN
	ALTER TABLE Account_Module
	DROP CONSTRAINT Account_Module_Module_FK
END
-------------------------

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='QAAQA_Quiz_Account_Answer_FK')
BEGIN
	ALTER TABLE Quiz_Account_Answer_Question_Answer
	DROP CONSTRAINT QAAQA_Quiz_Account_Answer_FK
END


IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='QAAQA_Question_Answer_FK')
BEGIN
	ALTER TABLE Quiz_Account_Answer_Question_Answer
	DROP CONSTRAINT QAAQA_Question_Answer_FK
END
--------------------------------------

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Exception_Module_Exception_FK')
BEGIN
	ALTER TABLE Exception_Module
	DROP CONSTRAINT Exception_Module_Exception_FK
END

IF EXISTS (SELECT *
			FROM sys.foreign_keys
			WHERE name='Exception_Module_Module_FK')
BEGIN
	ALTER TABLE Exception_Module
	DROP CONSTRAINT Exception_Module_Module_FK
END
------------------------------


CREATE TABLE Module(
  moduleId INTEGER NOT NULL IDENTITY,
  moduleName VARCHAR(255) UNIQUE,
  description VARCHAR(255),  
  expiryDate DATETIME,
  creationDate DATETIME
  CONSTRAINT Module_PK PRIMARY KEY(moduleId)
  );  
GO

INSERT INTO Module (moduleName, description, expiryDate,creationDate) VALUES ('Language Teaching Methodology', 'Methodology in language teaching has been characterized in a variety of ways.', '2014-01-11','12-Jan-2014');
INSERT INTO Module (moduleName, description, expiryDate,creationDate) VALUES ('First Aid', 'Learn essential first aid and CPR skills with our wide range of courses to meet every need.', '12-Dec-2016','12-Jan-2014');
INSERT INTO Module (moduleName, description, expiryDate,creationDate) VALUES ('Microsoft Excel Tutorial', 'Understanding how to create spreadsheets', '12-May-2015','12-Jan-2014');

--revised
CREATE TABLE Quiz (
  quizId INTEGER  NOT NULL IDENTITY, 
  quizName VARCHAR(512) UNIQUE,
  description VARCHAR(512),
  passingGrade INTEGER,
  moduleId INTEGER, 
  CONSTRAINT Quiz_PK PRIMARY KEY(quizId),
  CONSTRAINT Quiz_Module_FK
    FOREIGN KEY (moduleId)
	REFERENCES Module(moduleId)
	);
GO

INSERT INTO Quiz(quizName, description, passingGrade,moduleId) VALUES ('Language Teaching Methodology Quiz', 'This Quiz includes 30 questions.', '67','1');
INSERT INTO Quiz(quizName, description, passingGrade,moduleId) VALUES ('First Aid Quiz', 'This Quiz includes 15 questions.', '67','2');
INSERT INTO Quiz(quizName, description, passingGrade,moduleId) VALUES ('Microsoft Excel Tutorial Quiz', 'This Quiz includes 23 questions.', '67','3');

--revised
CREATE TABLE Person (
  personId INTEGER NOT NULL IDENTITY,
  firstName VARCHAR(15),
  lastName VARCHAR(15),
  gender CHAR(1),
  supervisorId INTEGER,
  address VARCHAR(255),
  phoneNumber VARCHAR(15),
  email VARCHAR(35),
  CONSTRAINT Person_PK	PRIMARY KEY(personId));
GO

INSERT INTO Person(firstName,lastName,gender,supervisorId,address,phoneNumber,email)VALUES('John', 'Michel', 'M', '1','234 Albert Street', '519-234-4567', 'JohnMichel@conestogac.on.ca');
INSERT INTO Person(firstName,lastName,gender,supervisorId,address,phoneNumber,email)VALUES('Steven', 'Casler', 'M','1','234 Albert Street', '226-234-4567', 'StevenCasler@conestogac.on.ca');
INSERT INTO Person(firstName,lastName,gender,supervisorId,address,phoneNumber,email)VALUES('Nick', 'Goetz', 'M', '1','234 Albert Street', '315-234-4567', 'NickGoetz@conestogac.on.ca');

--revised
CREATE TABLE AgreementInformation (
  agreementInforId INTEGER NOT NULL IDENTITY,
  userType VARCHAR(255),
  content VARCHAR(2048),
  moduleId INTEGER,
  CONSTRAINT AgreementInformation_PK PRIMARY KEY(agreementInforId),
  CONSTRAINT Agreement_Module_FK
    FOREIGN KEY (moduleId)
	REFERENCES Module(moduleId)
	ON DELETE CASCADE);
GO

INSERT INTO AgreementInformation(userType,content,moduleId)VALUES('Administrator','Please following the steps of creating new modules in the Introduction page. ','1');
INSERT INTO AgreementInformation(userType,content,moduleId)VALUES('Manager','You can only check the training information of your subordinates','2');
INSERT INTO AgreementInformation(userType,content,moduleId)VALUES('User','After you click CONFORM button, it is your responsibity to study all the materials and take the quiz if it has been attached.','3');

--revised
CREATE TABLE TrainingModule (
  trainingId INTEGER  NOT NULL IDENTITY,
  trainingName VARCHAR(50),
  description VARCHAR(512),
  type VARCHAR(35),  
  moduleId INTEGER,
  CONSTRAINT TrainingModule_PK PRIMARY KEY(trainingId),
  CONSTRAINT TrainingModule_Module_FK
    FOREIGN KEY (moduleId)
	REFERENCES Module(moduleId));
GO

INSERT INTO TrainingModule(trainingName,description,type,moduleId)VALUES('Language Teaching Methodology Training','','Teaching','1');
INSERT INTO TrainingModule(trainingName,description,type,moduleId)VALUES('First Aid Training','','Safty','2');
INSERT INTO TrainingModule(trainingName,description,type,moduleId)VALUES('Microsoft Excel Tutorial Training','','IT','3');

--revised
CREATE TABLE Document (
  documentId INTEGER  NOT NULL IDENTITY,
  name VARCHAR(255),
  type VARCHAR(35),
  uploadDate DATETIME,
  description VARCHAR(255),
  size INTEGER,
  trainingId INTEGER,
  CONSTRAINT Document_PK PRIMARY KEY(documentId),
  CONSTRAINT Document_Training_Module_FK
	FOREIGN KEY(trainingId)
	REFERENCES TrainingModule(trainingId)
  );
GO

INSERT INTO Document(name,type,uploadDate,description,size,trainingId)VALUES('English as second language','ppt','2014-01-23','','2','1');
INSERT INTO Document(name,type,uploadDate,description,size,trainingId)VALUES('English as second language','Doc','2013-02-3','','3','1');
INSERT INTO Document(name,type,uploadDate,description,size,trainingId)VALUES('English as second language','PDF','2014-01-23','','1','1');


--revised
CREATE TABLE Campus (
  campusId INTEGER  NOT NULL IDENTITY,
  name VARCHAR(255),
  CONSTRAINT Campus_PK PRIMARY KEY(campusId));
GO

INSERT INTO Campus ("name") VALUES ('Cambridge');
INSERT INTO Campus ("name") VALUES ('Doon');
INSERT INTO Campus ("name") VALUES ('Guelph');
INSERT INTO Campus ("name") VALUES ('Ingersoll');
INSERT INTO Campus ("name") VALUES ('Stratford');
INSERT INTO Campus ("name") VALUES ('Waterloo');

--revised
CREATE TABLE Department (
  departmentId INTEGER  NOT NULL IDENTITY,
  name VARCHAR(255),
  description VARCHAR(255),
  CONSTRAINT Department_PK PRIMARY KEY(departmentId));
GO

INSERT INTO Department(name,description) VALUES('BUS','Business and Hospitality');
INSERT INTO Department(name,description) VALUES('CLI','Conestoga Language Institute');
INSERT INTO Department(name,description) VALUES('HLC','Health and Life Sciences and Community Services');


--revised
CREATE TABLE Account (
  accountId INTEGER  NOT NULL IDENTITY,
  userName VARCHAR(255),
  password VARCHAR(255),
  userType VARCHAR(255),
  creationDate DATETIME,
  lastLoginDate DATETIME,
  personId INTEGER NOT NULL,
  CONSTRAINT Account_PK PRIMARY KEY(accountId),
  CONSTRAINT Account_Person_FK
	FOREIGN KEY(personId)
	REFERENCES Person(personId)
	ON DELETE CASCADE);
GO
INSERT INTO Account(userName,password,personId,creationDate)VALUES('Jonh-cc','1234567','1','23-Jan-2014');
INSERT INTO Account(userName,password,creationDate,lastLoginDate,personId)VALUES('Steven-cc','1234567','12-Dec-2013','23-Jan-2014','1');
INSERT INTO Account(userName,password,creationDate,lastLoginDate,personId)VALUES('Barharui-cc','1234567','12-Dec-2013','23-Jan-2014','1');


--revised
CREATE TABLE Quiz_record (
  quizRecordId INTEGER NOT NULL IDENTITY,
  accountId INTEGER NOT NULL,
  attendTimes INTEGER,
  highGradeTime DATETIME,
  quizId INTEGER NOT NULL,
  highestGrade INTEGER,
  CONSTRAINT Quiz_record_PK PRIMARY KEY(quizRecordId)    ,
  CONSTRAINT Quiz_record_Account_FK
	FOREIGN KEY(accountId)
    REFERENCES Account(accountId)
      ON DELETE CASCADE,
   CONSTRAINT Quiz_record_Quiz_FK
	FOREIGN KEY(quizId)
    REFERENCES Quiz(quizId));
GO

INSERT INTO Quiz_record(accountId,quizId,attendTimes,highestGrade,highGradeTime)VALUES('1','1','2','78','23-Feb-2014');
INSERT INTO Quiz_record(accountId,quizId,attendTimes,highestGrade,highGradeTime)VALUES('1','2','1','89','2-Feb-2014');
INSERT INTO Quiz_record(accountId,quizId,attendTimes,highestGrade,highGradeTime)VALUES('1','3','4','98','26-Feb-2014');

--revised
CREATE TABLE Exception (
  exceptionId INTEGER  NOT NULL IDENTITY,  
  accountId INTEGER NOT NULL,
  moduleId INTEGER NOT NULL,
  reason VARCHAR(255),
  creationDate DATETIME,
  expiryDate DATETIME,
  CONSTRAINT Exception_PK PRIMARY KEY (exceptionId),
  CONSTRAINT Exception_Account_FK
	FOREIGN KEY(accountId)
    REFERENCES Account(accountId)
    ON DELETE CASCADE);
GO

INSERT INTO Exception(accountId,moduleId,reason,creationDate,expiryDate)VALUES('1','2','illness','12-Mar-2014','31-May-2014');
INSERT INTO Exception(accountId,moduleId,reason,creationDate,expiryDate)VALUES('1','1','illness','12-Mar-2014','31-May-2014');
INSERT INTO Exception(accountId,moduleId,reason,creationDate,expiryDate)VALUES('1','3','illness','12-Mar-2014','31-May-2014');

--revised
CREATE TABLE Study_Document_Record (
  study_document_recordId INTEGER NOT NULL IDENTITY,  
  documentId INTEGER NOT NULL,
  method VARCHAR(30),
  accountId INTEGER NOT NULL,
  visitingDate DATETIME,
  CONSTRAINT Study_Document_Record_PK PRIMARY KEY (study_document_recordId),
  CONSTRAINT Study_Document_Record_Document_FK
	FOREIGN KEY(documentId)
    REFERENCES Document(documentId),
  CONSTRAINT Study_Document_Record_Account_FK
	FOREIGN KEY(accountId)
    REFERENCES Account(accountId)
    ON DELETE CASCADE);
GO

INSERT INTO Study_Document_Record(documentId,method,accountId,visitingDate)VALUES('1','online','1','12-April-2013');
INSERT INTO Study_Document_Record(documentId,method,accountId,visitingDate)VALUES('2','online','1','12-April-2013');
INSERT INTO Study_Document_Record(documentId,method,accountId,visitingDate)VALUES('3','online','1','12-April-2013');

--- revised
CREATE TABLE Person_Department_Campus (
  Id INTEGER  NOT NULL,
  departmentId INTEGER NOT NULL,
  campusId INTEGER NOT NULL,
  personId INTEGER NOT NULL,
  CONSTRAINT Person_Department_Campus_PK PRIMARY KEY(Id),
  CONSTRAINT Person_Department_Campus_Campus_FK
	FOREIGN KEY(campusId)
    REFERENCES Campus(campusId),
  CONSTRAINT Person_Department_Campus_Department_FK
	FOREIGN KEY(departmentId)
    REFERENCES Department(departmentId),
  CONSTRAINT Person_Department_Campus_Person_FK
	FOREIGN KEY(personId)
    REFERENCES Person(personId)
    ON DELETE CASCADE);
GO

INSERT INTO Person_Department_Campus(Id,departmentId,campusId,personId)VALUES('1','1','2','1');
INSERT INTO Person_Department_Campus(Id,departmentId,campusId,personId)VALUES('2','1','2','1');
INSERT INTO Person_Department_Campus(Id,departmentId,campusId,personId)VALUES('3','1','2','1');


--revised
CREATE TABLE Question_Answer (
  questionAnswerId INTEGER  NOT NULL IDENTITY,
  quizId INTEGER NOT NULL,
  questionDescription VARCHAR(255),
  type VARCHAR(35),
  Answer VARCHAR(255),
  CONSTRAINT Question_Answer_PK PRIMARY KEY(questionAnswerId),
  CONSTRAINT Question_Answer_Quiz_FK
	FOREIGN KEY(quizId)
    REFERENCES Quiz(quizId)
    ON DELETE CASCADE);
GO

INSERT INTO Question_Answer(quizId,questionDescription,type,Answer)VALUES('1','Health care reform will affect my ability to choose a doctor?','True/False','False');
INSERT INTO Question_Answer(quizId,questionDescription,type,Answer)VALUES('2','Where did Randall probably grow up? ','Multipule Choice','A');
INSERT INTO Question_Answer(quizId,questionDescription,type,Answer)VALUES('3','What is SIS?','Multipule Choice','C');

--revised
CREATE TABLE Quiz_Account_Answer (
  qAccountAnswerId INTEGER NOT NULL IDENTITY,
  questionAnswerId INTEGER,
  accountId INTEGER NOT NULL,
  answer VARCHAR(255),
  submitDate DATETIME,
  CONSTRAINT Quiz_Account_Answer_PK PRIMARY KEY(qAccountAnswerId),
  CONSTRAINT Quiz_Account_Answer_Account_FK
    FOREIGN KEY(accountId)
    REFERENCES Account(accountId)
    ON DELETE CASCADE);
GO

INSERT INTO Quiz_Account_Answer(questionAnswerId,accountId,answer,submitDate)VALUES('2','1','D','31-Aug-2013');
INSERT INTO Quiz_Account_Answer(questionAnswerId,accountId,answer,submitDate)VALUES('2','2','D','31-Aug-2013');
INSERT INTO Quiz_Account_Answer(questionAnswerId,accountId,answer,submitDate)VALUES('2','3','D','31-Aug-2013');



--revised
CREATE TABLE Qustion_Answer_Multiple_Selection (
  mulSelectionId INTEGER  NOT NULL IDENTITY,
  questionAnswerId INTEGER NOT NULL,
  choiceDescription VARCHAR(255),
  choiceSequenceNumber VARCHAR(255),
  CONSTRAINT Qustion_Answer_Multiple_Selection_PK PRIMARY KEY(mulSelectionId),
  CONSTRAINT Qustion_Answer_Multiple_Selection_QA_FK
	FOREIGN KEY(questionAnswerId)
    REFERENCES Question_Answer(questionAnswerId)
    ON DELETE CASCADE);
GO


INSERT INTO Qustion_Answer_Multiple_Selection(questionAnswerId,choiceDescription,choiceSequenceNumber)VALUES('2','in Indiana','A');
INSERT INTO Qustion_Answer_Multiple_Selection(questionAnswerId,choiceDescription,choiceSequenceNumber)VALUES('2','in Venezuela','B');
INSERT INTO Qustion_Answer_Multiple_Selection(questionAnswerId,choiceDescription,choiceSequenceNumber)VALUES('2','in Utah','C');

--Added Table
CREATE TABLE Account_Module(  
  accountId INTEGER NOT NULL,
  moduleId INTEGER NOT NULL,
  status VARCHAR(16),
  visitingDate DATETIME,
  CONSTRAINT Account_Module_PK PRIMARY KEY (accountId, moduleId),
  CONSTRAINT Account_Module_Account_FK 
    FOREIGN KEY (accountId)
	REFERENCES Account(accountId)
	ON DELETE CASCADE,
  CONSTRAINT Account_Module_Module_FK
    FOREIGN KEY(moduleId)
	REFERENCES Module(moduleId)
	ON DELETE CASCADE,
);
GO

INSERT INTO Account_Module(accountId,moduleId,status,visitingDate)VALUES('1','1','pass','2-Jan-2014');
INSERT INTO Account_Module(accountId,moduleId,status,visitingDate)VALUES('1','2','pass','5-Jan-2014');
INSERT INTO Account_Module(accountId,moduleId,status,visitingDate)VALUES('1','3','pass','12-Jan-2014');



---Added Table

CREATE TABLE Quiz_Account_Answer_Question_Answer(
  qAccountAnswerId INTEGER NOT NULL,
  questionAnswerId INTEGER NOT NULL,
  CONSTRAINT Quiz_Account_Answer_Question_Answer_PK PRIMARY KEY (qAccountAnswerId,questionAnswerId),
  CONSTRAINT QAAQA_Quiz_Account_Answer_FK
   FOREIGN KEY (qAccountAnswerId)
   REFERENCES Quiz_Account_Answer(qAccountAnswerId)
   ON DELETE CASCADE,
  CONSTRAINT QAAQA_Question_Answer_FK
   FOREIGN KEY(questionAnswerId)
   REFERENCES Question_Answer(questionAnswerId)
   ON DELETE CASCADE);
GO

INSERT INTO Quiz_Account_Answer_Question_Answer(qAccountAnswerId,questionAnswerId)VALUES('1','1');
INSERT INTO Quiz_Account_Answer_Question_Answer(qAccountAnswerId,questionAnswerId)VALUES('2','2');
INSERT INTO Quiz_Account_Answer_Question_Answer(qAccountAnswerId,questionAnswerId)VALUES('3','2');

----Added Table
CREATE TABLE Exception_Module(
  ExceptionId INTEGER NOT NULL,
  moduleId INTEGER NOT NULL,
  CONSTRAINT Exception_Module_PK PRIMARY KEY (ExceptionId,moduleId),
  CONSTRAINT Exception_Module_Exception_FK
    FOREIGN KEY (ExceptionId)
	REFERENCES Exception(ExceptionId)
	ON DELETE CASCADE,
  CONSTRAINT Exception_Module_Module_FK
    FOREIGN KEY (moduleId)
	REFERENCES Module(moduleId)
	ON DELETE CASCADE);
GO

INSERT INTO Exception_Module(ExceptionId,moduleId)VALUES('1','2');
INSERT INTO Exception_Module(ExceptionId,moduleId)VALUES('2','2');
INSERT INTO Exception_Module(ExceptionId,moduleId)VALUES('1','1');

--++++++++++++++++++++++++++++
--Procuders as following can be added.
--+++++++++++++++++++++++++++=

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AddModule
(
  @moduleName VARCHAR(255) ,
  @description VARCHAR(255) = NULL,  
  @expiryDate DATETIME = NULL,
  @creationDate DATETIME = NULL,
  @userType VARCHAR(255),
  @content VARCHAR(2048) = NULL
)
AS 
BEGIN
DECLARE
    @moduleId int
--insert data into module table	
INSERT INTO Module (moduleName, description, expiryDate,creationDate) VALUES (@moduleName, @description, @expiryDate, @creationDate);
--get moduleId
SELECT @moduleId = moduleId FROM Module WHERE moduleName = @moduleName;
--insert data into AgreementInformation table	
INSERT INTO AgreementInformation(userType,content,moduleId)VALUES(@userType,@content, @moduleId);
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE AddQuestion
(
  @questionAnswerId INTEGER OUTPUT ,
  @quizId INTEGER ,
  @questionDescription VARCHAR(255) = NULL,  
  @type VARCHAR(35) = NULL,
  @Answer VARCHAR(255) = NULL
)
AS 
BEGIN
--insert data into module table	
INSERT INTO Question_Answer(quizId, questionDescription, type,Answer) VALUES (@quizId, @questionDescription, @type, @Answer);
--get quizId
SELECT @questionAnswerId = Max(questionAnswerId) FROM Question_Answer
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE AddQuiz
(
  @quizId INTEGER OUTPUT ,
  @moduleId INTEGER = NULL,  
  @passingGrade INTEGER = NULL,
  @quizName VARCHAR(512),
  @description VARCHAR(512) = NULL
)
AS 
BEGIN
--insert data into module table	
INSERT INTO Quiz(quizName, description, passingGrade,moduleId) VALUES (@quizName, @description, @passingGrade, @moduleId);
--get quizId
SELECT @quizId = Max(quizId) FROM Quiz
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE AddTraining
(
  @trainingName VARCHAR(50) ,
  @description VARCHAR(512) = NULL,  
  @type VARCHAR(35) = NULL,
  @moduleId INTEGER = NULL
)
AS 
BEGIN
--insert data into training table	
INSERT INTO TrainingModule(trainingName,description,type,moduleId)VALUES(@trainingName, @description, @type, @moduleId);

return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE AddUser
(
  @firstName VARCHAR(15) = NULL,
  @lastName VARCHAR(15) = NULL,
  @gender CHAR(1) = NULL,
  @supervisorId INTEGER=null,
  @address VARCHAR(255) = NULL,
  @phoneNumber VARCHAR(15) = NULL,
  @email VARCHAR(35) = NULL,
  @userName VARCHAR(255),
  @password VARCHAR(255),
  @userType VARCHAR(255),
  @creationDate DATETIME = NULL,
  @lastLoginDate DATETIME = NULL,
  @departmentId INTEGER,
  @campusId INTEGER 
)
AS 
BEGIN
DECLARE
    @personId int,
	@pdcId int
	
--insert person table
INSERT INTO Person(firstName,lastName,gender,supervisorId,address,phoneNumber,email)VALUES(@firstName, @lastName, @gender, @supervisorId,@address, @phoneNumber, @email);
--get personId
SELECT @personId = personId FROM Person WHERE firstName = @firstName AND lastName = @lastName;
--insert account table
INSERT INTO Account(userName,password,userType, creationDate,lastLoginDate,personId)VALUES(@userName, @password, @userType, @creationDate,@lastLoginDate, @personId);

SELECT @pdcId = COUNT(*)+1 FROM Person_Department_Campus;
--insert Person_Department_Campus table
INSERT INTO Person_Department_Campus(Id,departmentId,campusId,personId)VALUES(@pdcId, @departmentId,@campusId,@personId);
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertDocument
(
  @name VARCHAR(255),
  @type VARCHAR(35) = NULL,
  @uploadDate DATETIME = NULL,
  @description VARCHAR(255) = NULL,
  @size INTEGER = NULL,
  @trainingId INTEGER = NULL
)
AS 
BEGIN
INSERT INTO Document(name,type,uploadDate,description,size,trainingId)VALUES(@name, @type, @uploadDate, @description , @size, @trainingId);
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UpdateDocument
(
  @documentId INTEGER,
  @name VARCHAR(255),
  @type VARCHAR(35) = NULL,
  @uploadDate DATETIME = NULL,
  @description VARCHAR(255) = NULL,
  @size INTEGER = NULL,
  @trainingId INTEGER = NULL
)
AS 
BEGIN
UPDATE Document
SET name=@name, type=@type, uploadDate= @uploadDate, description=@description, size=@size, trainingId=@trainingId
WHERE documentId=@documentId;
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UpdateModule
(
  @moduleId INTEGER,
  @moduleName VARCHAR(255) ,
  @description VARCHAR(255) = NULL,  
  @expiryDate DATETIME = NULL,
  @creationDate DATETIME = NULL,
  @userType VARCHAR(255),
  @content VARCHAR(2048) = NULL
)
AS 
BEGIN
UPDATE Module
	SET moduleName=@moduleName, description=@description, expiryDate= @expiryDate, creationDate=@creationDate
	WHERE moduleId=@moduleId;

UPDATE AgreementInformation
	SET content=@content, moduleId=@moduleId
	WHERE userType=@userType;

return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UpdateTraining
(
  @trainingId INTEGER,
  @trainingName VARCHAR(50) ,
  @description VARCHAR(512) = NULL,  
  @type VARCHAR(35) = NULL,
  @moduleId INTEGER = NULL
)
AS 
BEGIN
UPDATE TrainingModule
SET trainingName=@trainingName, description=@description, type= @type, moduleId=@moduleId
WHERE trainingId=@trainingId;
return (0);
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UpdateUser
(
  @personId INTEGER,
  @firstName VARCHAR(15) = NULL,
  @lastName VARCHAR(15) = NULL,
  @gender CHAR(1) = NULL,
  @supervisorId INTEGER=null,
  @address VARCHAR(255) = NULL,
  @phoneNumber VARCHAR(15) = NULL,
  @email VARCHAR(35) = NULL,
  @accountId INTEGER,
  @userName VARCHAR(255),
  @password VARCHAR(255),
  @userType VARCHAR(255),
  @creationDate DATETIME = NULL,
  @lastLoginDate DATETIME = NULL,
  @departmentId INTEGER,
  @campusId INTEGER 
)
AS 
BEGIN
DECLARE
	@pdcId int
	
--update person table
UPDATE Person
SET firstName=@firstName, lastName=@lastName, gender= @gender, supervisorId=@supervisorId, address=@address, phoneNumber=@phoneNumber, email=@email
WHERE personId=@personId;
--update account table
UPDATE Account
SET userName=@userName, password=@password, userType= @userType, creationDate=@creationDate, lastLoginDate=@lastLoginDate, personId=@personId
WHERE accountId=@accountId;

SELECT @pdcId = COUNT(*)+1 FROM Person_Department_Campus
--update Person_Department_Campus table
IF EXISTS (SELECT *
           FROM Person_Department_Campus
           WHERE personId = 1)    
    BEGIN
        UPDATE Person_Department_Campus
			SET departmentId=@departmentId, campusId=@campusId
			WHERE personId=@personId;
    END
ELSE
    BEGIN
		SET IDENTITY_INSERT Person_Department_Campus ON
        INSERT INTO Person_Department_Campus(Id,departmentId,campusId,personId)VALUES(@pdcId, @departmentId,@campusId,@personId);    
		SET IDENTITY_INSERT Person_Department_Campus OFF
    END
return (0);
END
