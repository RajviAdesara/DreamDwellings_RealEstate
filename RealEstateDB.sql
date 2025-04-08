-- Create Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(50) CHECK (Role IN ('Admin', 'Buyer', 'Seller', 'User')),
);

-- Create Properties Table
CREATE TABLE Properties (
    PropertyId INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(150) NOT NULL,
    Description VARCHAR(MAX),
    PropertyType VARCHAR(50) CHECK (PropertyType IN ('Apartment', 'House', 'Commercial')),
    Category VARCHAR(50) CHECK (Category IN ('Buy', 'Rent', 'Sell')),
    Price DECIMAL(18,2) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Area FLOAT NOT NULL,
    Bedrooms INT,
    Bathrooms INT,
    Amenities VARCHAR(MAX),
    ListedDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(50) CHECK (Status IN ('Available', 'Sold', 'Pending')),
	ImageName VARCHAR(255) NOT NULL,
    ImagePath VARCHAR(MAX),
);


CREATE TABLE Properties (
    PropertyId INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(150) NOT NULL,
    Description VARCHAR(MAX),
    PropertyTypeId INT NOT NULL,  -- Foreign Key to PropertyTypes table
    Category VARCHAR(50) CHECK (Category IN ('Buy', 'Rent', 'Sell')),
    Price DECIMAL(18,2) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Area FLOAT NOT NULL,
    Bedrooms INT,
    Bathrooms INT,
    Amenities VARCHAR(MAX),
    ListedDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(50) CHECK (Status IN ('Available', 'Sold', 'Pending')),
    ImageName VARCHAR(255) NOT NULL,
    ImagePath VARCHAR(MAX),
    
    -- Define Foreign Key Constraint
    CONSTRAINT FK_Property_PropertyType FOREIGN KEY (PropertyTypeId) 
    REFERENCES PropertyTypes (PropertyTypeId) ON DELETE CASCADE
);

CREATE TABLE PropertyTypes (
    PropertyTypeId INT PRIMARY KEY IDENTITY(1,1),
    PropertyType VARCHAR(50) UNIQUE NOT NULL
);


CREATE TABLE Agents (
    AgentId INT PRIMARY KEY IDENTITY(1,1),
    AgentName VARCHAR(100) UNIQUE, 
    LicenseNumber VARCHAR(50) NOT NULL UNIQUE,
    ExperienceYears INT NOT NULL,
    ContactNumber VARCHAR(15) NOT NULL,
    OfficeAddress VARCHAR(255),
    ProfilePicture VARCHAR(MAX),
    Status VARCHAR(50) CHECK (Status IN ('Active', 'Inactive', 'Suspended')),
);

CREATE TABLE Appointments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserEmail NVARCHAR(255),
    AppointmentDate DATETIME,
    Status NVARCHAR(50) DEFAULT 'Pending' 
        CHECK (Status IN ('Pending', 'Confirmed', 'Canceled'))
);

CREATE TABLE Feedback (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
);

ALTER TABLE Agents
ADD ProfilePicturePath VARCHAR(MAX);

Select * from users

---- Create PropertyImages Table
--CREATE TABLE PropertyImages (
--    ImageId INT PRIMARY KEY IDENTITY(1,1),
--    PropertyId INT FOREIGN KEY REFERENCES Properties(PropertyId) ON DELETE CASCADE,
--    GroupId INT NOT NULL,
--    ImageName VARCHAR(255) NOT NULL,
--    Image VARCHAR(MAX),
--    UploadedDate DATETIME DEFAULT GETDATE()
--);


INSERT INTO Users (UserName, Email, Password, Role)
VALUES 
('Meet Parekh', 'vinodparekh83@gmail.com', 'meet029', 'User')

INSERT INTO Properties (Title, Description, PropertyType, Category, Price, Location, Area, Bedrooms, Bathrooms, Amenities, Status, ImageName, ImagePath)
VALUES 
('Luxury Apartment', 'Spacious 3 BHK in downtown', 'Apartment', 'Sell', 250000.00, 'Downtown City', 1500.0, 3, 2, 'Gym, Pool, Parking', 'Available','house1.jpg', '/img/house1.jpg'),
('Cozy House', '2 BHK house in the suburbs', 'House', 'Rent', 1500.00, 'Suburb Area', 1200.0, 2, 2, 'Garden, Garage', 'Available','house2.jpg', '/img/house2.jpg'),
('Commercial Office', '5000 sqft office space', 'Commercial', 'Buy', 1000000.00, 'Business District', 5000.0, NULL, NULL, 'Conference Room, Parking', 'Pending','house3.jpg', '/img/house3.jpg'),
('Modern Villa', '4 BHK villa with a swimming pool', 'House', 'Sell', 500000.00, 'Luxury Area', 3000.0, 4, 3, 'Swimming Pool, Garage', 'Available', 'house4.jpg', '/img/house4.jpg'),
('Penthouse Apartment', 'Luxury penthouse with a great view', 'Apartment', 'Sell', 750000.00, 'Uptown City', 2000.0, 3, 3, 'Roof Garden, Pool', 'Available', 'house5.jpg', '/img/house5.jpg'),
('Country House', 'Cozy 3 BHK house in the countryside', 'House', 'Rent', 2500.00, 'Countryside Area', 1800.0, 3, 2, 'Garden, Fireplace', 'Available', 'house6.jpg', '/img/house6.jpg'),
('Retail Space', '1000 sqft retail space in city center', 'Commercial', 'Rent', 1000.00, 'City Center', 1000.0, NULL, NULL, 'Parking, Security', 'Available', 'house7.jpg', '/img/house7.jpg'),
('Beach House', '5 BHK luxury house near the beach', 'House', 'Sell', 850000.00, 'Beach Area', 4000.0, 5, 4, 'Private Beach, Pool', 'Available', 'house8.jpg', '/img/house8.jpg'),
('Studio Apartment', 'Affordable studio apartment', 'Apartment', 'Rent', 800.00, 'Downtown City', 500.0, 1, 1, 'Security, Parking', 'Available', 'house9.jpg', '/img/house9.jpg');


INSERT INTO Properties 
(Title, Description, PropertyTypeId, Category, Price, Location, Area, Bedrooms, Bathrooms, Amenities, Status, ImageName, ImagePath)
VALUES 
('Luxury Apartment', 'Spacious 3 BHK in downtown', 1, 'Sell', 250000.00, 'Downtown City', 1500.0, 3, 2, 'Gym, Pool, Parking', 'Available','house1.jpg', '/img/house1.jpg'),
('Cozy House', '2 BHK house in the suburbs', 2, 'Rent', 1500.00, 'Suburb Area', 1200.0, 2, 2, 'Garden, Garage', 'Available','house2.jpg', '/img/house2.jpg'),
('Commercial Office', '5000 sqft office space', 3, 'Buy', 1000000.00, 'Business District', 5000.0, NULL, NULL, 'Conference Room, Parking', 'Pending','house3.jpg', '/img/house3.jpg'),
('Modern Villa', '4 BHK villa with a swimming pool', 2, 'Sell', 500000.00, 'Luxury Area', 3000.0, 4, 3, 'Swimming Pool, Garage', 'Available', 'house4.jpg', '/img/house4.jpg'),
('Penthouse Apartment', 'Luxury penthouse with a great view', 1, 'Sell', 750000.00, 'Uptown City', 2000.0, 3, 3, 'Roof Garden, Pool', 'Available', 'house5.jpg', '/img/house5.jpg'),
('Country House', 'Cozy 3 BHK house in the countryside', 2, 'Rent', 2500.00, 'Countryside Area', 1800.0, 3, 2, 'Garden, Fireplace', 'Available', 'house6.jpg', '/img/house6.jpg'),
('Retail Space', '1000 sqft retail space in city center', 3, 'Rent', 1000.00, 'City Center', 1000.0, NULL, NULL, 'Parking, Security', 'Available', 'house7.jpg', '/img/house7.jpg'),
('Beach House', '5 BHK luxury house near the beach', 2, 'Sell', 850000.00, 'Beach Area', 4000.0, 5, 4, 'Private Beach, Pool', 'Available', 'house8.jpg', '/img/house8.jpg'),
('Studio Apartment', 'Affordable studio apartment', 1, 'Rent', 800.00, 'Downtown City', 500.0, 1, 1, 'Security, Parking', 'Available', 'house9.jpg', '/img/house9.jpg');


INSERT INTO PropertyTypes (PropertyType) VALUES 
('Apartment'),
('House'),
('Commercial');

INSERT INTO Feedback (Name, Email, Message) VALUES
('John Doe', 'john.doe@example.com', 'Great service! I really appreciate the support.'),
('Jane Smith', 'jane.smith@example.com', 'The website is user-friendly, but I would love to see more features.'),
('Michael Johnson', 'michael.j@example.com', 'Had an issue with login, but customer service resolved it quickly.'),
('Emily Davis', 'emily.davis@example.com', 'Excellent platform! I would highly recommend it.'),
('Robert Brown', 'robert.brown@example.com', 'Good experience overall, but response time could be improved.');



UPDATE Properties
SET ImageName = 'house1.jpg'
WHERE ImagePath = '/img/house1.jpg';

	

INSERT INTO PropertyImages (PropertyId, GroupId, ImageName, Image)
VALUES 
(1, 1, 'living_room.jpg', 'R:/PropertyImages/living_room.jpg'),
(1, 1, 'bedroom.jpg', 'R:/PropertyImages/bedroom.jpg'),
(1, 1, 'kitchen.jpg', 'R:/PropertyImages/kitchen.jpg'),
(1, 1, 'bathroom.jpg', 'R:/PropertyImages/bathroom.jpg'),
(1, 1, 'balcony.jpg', 'R:/PropertyImages/balcony.jpg');

Select * from PropertyImages where PropertyId=1;


INSERT INTO Agents (AgentName, LicenseNumber, ExperienceYears, ContactNumber, OfficeAddress, ProfilePicture, ProfilePicturePath, Status)
VALUES
('John Doe', 'LIC123456', 10, '9876543210', '123 Main St, New York, NY', 'agent1.jpg', '/img/agent1.jpg', 'Active'),
('Alice Smith', 'LIC789012', 5, '9876543211', '456 Elm St, Los Angeles, CA', 'agent2.jpg', '/img/agent2.jpg', 'Active'),
('Michael Johnson', 'LIC345678', 7, '9876543212', '789 Oak St, Chicago, IL', 'agent3.jpg', '/img/agent3.jpg', 'Inactive'),
('Emily Davis', 'LIC901234', 12, '9876543213', '101 Pine St, Houston, TX', 'agent4.jpg', '/img/agent4.jpg', 'Active'),
('David Brown', 'LIC567890', 3, '9876543214', '202 Cedar St, Phoenix, AZ', 'agent5.jpg', '/img/agent5.jpg', 'Suspended')

Delete from Agents
where AgentId = 17

Select * from Agents

Drop table Appointments

-------------------------- FEEDBACK SP -------------------------

--1.
CREATE OR ALTER PROCEDURE PR_Feedback_SelectAll
AS
BEGIN
    SELECT 
        Id,
        Name,
        Email,
        Message
    FROM Feedback
    ORDER BY Id DESC
END


--2.
CREATE PROCEDURE PR_Feedback_Insert
    @Name NVARCHAR(100),
    @Email NVARCHAR(255),
    @Message NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Feedback (Name, Email, Message)
    VALUES (@Name, @Email, @Message)
END



----------------------------------- APPOINTMENT SP ------------------------------

--1.
CREATE PROCEDURE PR_Appointment_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT Id, UserEmail, AppointmentDate, Status
    FROM Appointments;
END;

--2.
CREATE or ALTER PROCEDURE PR_Appointment_UpdateStatus
    @Id INT,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Appointments
    SET Status = @Status
    WHERE Id = @Id;
END;

EXEC PR_Appointment_UpdateStatus @Id = 1, @Status = 'Approved';

--3.
CREATE PROCEDURE PR_Appointment_Insert
    @UserEmail NVARCHAR(255),
    @AppointmentDate DATETIME,
    @Status NVARCHAR(50)
AS
BEGIN
    INSERT INTO Appointments (UserEmail, AppointmentDate, Status)
    VALUES (@UserEmail, @AppointmentDate, @Status);
END;



--------------------------------------- USER SP ------------------------------------------

--1.
CREATE or ALTER PROCEDURE PR_User_SelectAll
AS
BEGIN
    SELECT 
        UserId,
        UserName,
        Email,
        Password,
        Role
    FROM Users
END

--2.
CREATE or ALTER PROCEDURE PR_User_SelectByPK 
    @UserId INT
AS
BEGIN
    SELECT 
        UserId,
        UserName,
        Email,
        Password,
        Role
    FROM Users
    WHERE UserId = @UserId
END

--3.
CREATE or ALTER PROCEDURE PR_User_Insert
    @UserName VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(255),
    @Role VARCHAR(50)
AS
BEGIN
    INSERT INTO Users (UserName, Email, Password, Role)
    VALUES (@UserName, @Email, @Password, @Role)
END

--4.
CREATE PROCEDURE PR_User_Update
    @UserId INT,
    @UserName VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(255),
    @Role VARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET 
        UserName = @UserName,
        Email = @Email,
        Password = @Password,
        Role = @Role
    WHERE UserId = @UserId
END

--5.
CREATE PROCEDURE PR_User_Delete
    @UserId INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserId = @UserId
END


----------------------------------------------------------------- PROPERTY SP --------------------------------------------------------

--1.
CREATE OR ALTER PROCEDURE PR_Property_SelectAll
AS
BEGIN
    SELECT 
        PropertyId,
        Title,
        Description,
        pt.PropertyType AS PropertyType,  -- Join PropertyTypes table for PropertyType
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId  -- Join with PropertyTypes table
    ORDER BY ListedDate DESC
END

--2.
CREATE or ALTER PROCEDURE PR_Property_SelectByPK
    @PropertyId INT
AS
BEGIN
    SELECT 
        PropertyId,
        Title,
        Description,
        pt.PropertyType AS PropertyType,  -- Join PropertyTypes table for PropertyType
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId  -- Join with PropertyTypes table
    WHERE PropertyId = @PropertyId
END

--3.
CREATE or ALTER PROCEDURE PR_Property_Insert
    @Title VARCHAR(150),
    @Description VARCHAR(MAX),
    @PropertyTypeId INT, 
    @Category VARCHAR(50),
    @Price DECIMAL(18, 2),
    @Location VARCHAR(255),
    @Area FLOAT,
    @Bedrooms INT,
    @Bathrooms INT,
    @Amenities VARCHAR(MAX),
    @Status VARCHAR(50),
    @ImageName VARCHAR(255),
    @ImagePath VARCHAR(MAX)
AS
BEGIN
    INSERT INTO Properties (
        Title, 
        Description, 
        PropertyTypeId, 
        Category, 
        Price, 
        Location, 
        Area, 
        Bedrooms, 
        Bathrooms, 
        Amenities, 
        ListedDate, 
        Status, 
        ImageName, 
        ImagePath
    )
    VALUES (
        @Title, 
        @Description, 
        @PropertyTypeId, 
        @Category, 
        @Price, 
        @Location, 
        @Area, 
        @Bedrooms, 
        @Bathrooms, 
        @Amenities, 
        GETDATE(), 
        @Status, 
        @ImageName, 
        @ImagePath
    )
END

--4.
CREATE or ALTER PROCEDURE PR_Property_Update
    @PropertyId INT,
    @Title VARCHAR(150),
    @Description VARCHAR(MAX),
    @PropertyTypeId INT,
    @Category VARCHAR(50),
    @Price DECIMAL(18, 2),
    @Location VARCHAR(255),
    @Area FLOAT,
    @Bedrooms INT,
    @Bathrooms INT,
    @Amenities VARCHAR(MAX),
    @Status VARCHAR(50),
    @ImageName VARCHAR(255),
    @ImagePath VARCHAR(MAX)
AS
BEGIN
    UPDATE Properties
    SET 
        Title = @Title,
        Description = @Description,
        PropertyTypeId = @PropertyTypeId,  -- Use PropertyTypeId here
        Category = @Category,
        Price = @Price,
        Location = @Location,
        Area = @Area,
        Bedrooms = @Bedrooms,
        Bathrooms = @Bathrooms,
        Amenities = @Amenities,
        Status = @Status,
        ImageName = @ImageName,
        ImagePath = @ImagePath
    WHERE PropertyId = @PropertyId
END

--5.
CREATE PROCEDURE PR_Property_Delete
    @PropertyId INT
AS
BEGIN
    DELETE FROM Properties
    WHERE PropertyId = @PropertyId
END

------------------------------------------ PROPERTY TYPE -----------------------------------------

--1.
CREATE or ALTER PROCEDURE PR_GetAllPropertyTypes
AS
BEGIN
    SELECT DISTINCT PropertyType FROM PropertyTypes;
END;

--2.
CREATE or ALTER PROCEDURE PR_GetPropertyTypeById
    @PropertyTypeId INT
AS
BEGIN
    SELECT PropertyType 
    FROM PropertyTypes
    WHERE PropertyTypeId = @PropertyTypeId;
END;



--3.
CREATE or ALTER PROCEDURE PR_UpdatePropertyType 
    @PropertyTypeId INT,
    @PropertyType VARCHAR(50)
AS
BEGIN
    UPDATE PropertyTypes
    SET PropertyType = @PropertyType
    WHERE PropertyTypeId = @PropertyTypeId;
END;


--4.
CREATE or ALTER PROCEDURE PR_DeleteProperty
    @PropertyTypeId INT
AS
BEGIN
    DELETE FROM PropertyTypes
    WHERE PropertyTypeId = @PropertyTypeId;
END;

-------------------------------------------------------- SELL / RENT ---------------------------------------------------

--1.
CREATE or ALTER PROCEDURE PR_GetPropertiesForCategory
    @Category NVARCHAR(50)
AS
BEGIN
    SELECT 
        PropertyId,
        Title,
        Description,
        pt.PropertyType,
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId  -- Join with PropertyTypes table
    WHERE 
        Category = @Category
    ORDER BY 
        ListedDate DESC; -- Optional: Order by the most recently listed properties
END

Exec PR_GetPropertiesForCategory @category = 'sell'

--2.
CREATE or ALTER PROCEDURE PR_GetPropertiesByType 
    @PropertyType NVARCHAR(50)
AS
BEGIN
    SELECT 
        PropertyId,
        Title,
        Description,
        pt.PropertyType,
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId  -- Join with PropertyTypes table
    WHERE PropertyType = @PropertyType
    ORDER BY 
        ListedDate DESC; -- Order by the most recently listed properties
END

--3.
CREATE OR ALTER PROCEDURE PR_GetPropertiesByTypeAndCategory
    @PropertyType NVARCHAR(50),
    @Category NVARCHAR(50) = NULL  -- Optional parameter for filtering by category
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PropertyId,
        Title,
        Description,
        pt.PropertyType,
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId  -- Join with PropertyTypes table
    WHERE 
        pt.PropertyType = @PropertyType
        AND (@Category IS NULL OR Category = @Category)  -- Apply category filter if provided
    ORDER BY 
        ListedDate DESC; -- Order by the most recently listed properties
END;

EXEC PR_GetPropertiesByTypeAndCategory @PropertyType = 'Apartment', @Category = 'Sell';


--------------------------------------------------------- AGENT SP -------------------------------------------------------
--1.
CREATE OR ALTER PROCEDURE PR_Agent_SelectAll
AS
BEGIN
    SELECT 
        AgentId,
        AgentName,
        LicenseNumber,
        ExperienceYears,
        ContactNumber,
        OfficeAddress,
        ProfilePicture,
        ProfilePicturePath,
        Status
    FROM Agents
END


-- 2. 
CREATE OR ALTER PROCEDURE PR_Agent_SelectByPK 
    @AgentId INT
AS
BEGIN
    SELECT 
        AgentId,
        AgentName,
        LicenseNumber,
        ExperienceYears,
        ContactNumber,
        OfficeAddress,
        ProfilePicture,
        ProfilePicturePath,
        Status
    FROM Agents
    WHERE AgentId = @AgentId
END


-- 3.
CREATE OR ALTER PROCEDURE PR_Agent_Insert
    @AgentName VARCHAR(100),
    @LicenseNumber VARCHAR(50),
    @ExperienceYears INT,
    @ContactNumber VARCHAR(15),
    @OfficeAddress VARCHAR(255),
    @ProfilePicture VARCHAR(MAX),
    @ProfilePicturePath VARCHAR(MAX),
    @Status VARCHAR(50)
AS
BEGIN
    INSERT INTO Agents (AgentName, LicenseNumber, ExperienceYears, ContactNumber, OfficeAddress, ProfilePicture, ProfilePicturePath, Status)
    VALUES (@AgentName, @LicenseNumber, @ExperienceYears, @ContactNumber, @OfficeAddress, @ProfilePicture, @ProfilePicturePath, @Status)
END

Exec 


-- 4.
CREATE OR ALTER PROCEDURE PR_Agent_Update
    @AgentId INT,
    @AgentName VARCHAR(100),
    @LicenseNumber VARCHAR(50),
    @ExperienceYears INT,
    @ContactNumber VARCHAR(15),
    @OfficeAddress VARCHAR(255),
    @ProfilePicture VARCHAR(MAX),
    @ProfilePicturePath VARCHAR(MAX),
    @Status VARCHAR(50)
AS
BEGIN
    UPDATE Agents
    SET 
        AgentName = @AgentName,
        LicenseNumber = @LicenseNumber,
        ExperienceYears = @ExperienceYears,
        ContactNumber = @ContactNumber,
        OfficeAddress = @OfficeAddress,
        ProfilePicture = @ProfilePicture,
        ProfilePicturePath = @ProfilePicturePath,
        Status = @Status
    WHERE AgentId = @AgentId
END


--5.
CREATE OR ALTER PROCEDURE PR_Agent_Delete
    @AgentId INT
AS
BEGIN
    DELETE FROM Agents
    WHERE AgentId = @AgentId
END


----------------------------------------------------------- ADMIN -------------------------------------------------------

--1.
CREATE OR ALTER PROCEDURE PR_GetTotalUsers
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Users) AS TotalUsers
END;

--2.
CREATE OR ALTER PROCEDURE PR_GetTotalProperties
AS
BEGIN 
	SELECT
		(SELECT COUNT(*) FROM Properties) AS TotalProperties
END;

--3.
CREATE OR ALTER PROCEDURE PR_GetTotalAgents
AS
BEGIN 
	SELECT
		(SELECT COUNT(*) FROM Agents) AS TotalAgents
END;

--4.
CREATE PROCEDURE PR_GetTotalBuyProperties
AS
BEGIN    
    SELECT COUNT(*) AS TotalBuyProperties 
    FROM Properties 
    WHERE Category = 'Buy'
END;

--5.
CREATE or ALTER PROCEDURE PR_GetTotalSellProperties
AS
BEGIN    
    SELECT COUNT(*) AS TotalSellProperties 
    FROM Properties 
    WHERE Category = 'Sell'
END;

--6.
CREATE PROCEDURE PR_GetTotalRentProperties
AS
BEGIN    
    SELECT COUNT(*) AS TotalRentProperties 
    FROM Properties 
    WHERE Category = 'Rent'
END;

--7.
CREATE OR ALTER PROCEDURE PR_GetTotalActiveAgents
AS
BEGIN    
    SELECT COUNT(*) AS TotalActiveAgents 
    FROM Agents 
    WHERE Status = 'Active'
END;

-- 8.
CREATE OR ALTER PROCEDURE PR_GetTotalInactiveAgents
AS
BEGIN    
    SELECT COUNT(*) AS TotalInactiveAgents 
    FROM Agents 
    WHERE Status = 'Inactive'
END;

--9.
CREATE OR ALTER PROCEDURE PR_GetTotalSuspendedAgents
AS
BEGIN    
    SELECT COUNT(*) AS TotalSuspendedAgents 
    FROM Agents 
    WHERE Status = 'Suspended'
END;


--10.
CREATE OR ALTER PROCEDURE PR_Property_Dashboard
AS
BEGIN
    SELECT TOP 10
        PropertyId,
        Title,
        Description,
        pt.PropertyType AS PropertyType,  -- Join PropertyTypes table for PropertyType
        Category,
        Price,
        Location,
        Area,
        Bedrooms,
        Bathrooms,
        Amenities,
        ListedDate,
        Status,
        ImageName,
        ImagePath
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId 
	where p.category = 'Sell' 
	ORDER BY ListedDate DESC
END

--11.
CREATE OR ALTER PROCEDURE PR_GetTotalPropertiesByType
AS
BEGIN
    SELECT 
        pt.PropertyType, 
        COUNT(*) AS TotalCount
    FROM Properties p
    JOIN PropertyTypes pt ON p.PropertyTypeId = pt.PropertyTypeId 
    GROUP BY pt.PropertyType
END;

------------------------------------------------ AUTHENTICATION ------------------------------------

--1.
CREATE or Alter PROCEDURE [dbo].[PR_User_Login] 
    @Email NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT 
        [dbo].[Users].[UserID], 
        [dbo].[Users].[UserName], 
        [dbo].[Users].[Email], 
        [dbo].[Users].[Password],
        [dbo].[Users].[Role]
    FROM 
        [dbo].[Users] 
    WHERE 
        [dbo].[Users].[Email] = @Email 
        AND [dbo].[Users].[Password] = @Password;
END

EXEC PR_User_Login @Email = 'john.doe@example.com', @Password = 'hashedpassword1'


select * from users


--1.
CREATE PROCEDURE PR_Property_Filter     
    @MinPrice INT = NULL,
    @MaxPrice INT = NULL,
    @Bedrooms INT = NULL,
    @Bathrooms INT = NULL
AS
BEGIN
    SELECT * FROM Properties
    WHERE (@MinPrice IS NULL OR Price >= @MinPrice)
      AND (@MaxPrice IS NULL OR Price <= @MaxPrice)
      AND (@Bedrooms IS NULL OR Bedrooms = @Bedrooms)
      AND (@Bathrooms IS NULL OR Bathrooms = @Bathrooms)
END
