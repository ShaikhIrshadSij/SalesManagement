CREATE DATABASE [CarManagement];
GO;
USE [CarManagement];
GO;
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);

-- Insert Dummy Users
INSERT INTO Users (Username, PasswordHash, Role) 
VALUES 
('admin', '$2a$11$/mBdicenHaOKuD122DZ9IeegvC1GZPob0aTTzhbBpUo7xRqWEMwQ.', 'Admin'), 
('salesman1', '$2a$11$/mBdicenHaOKuD122DZ9IeegvC1GZPob0aTTzhbBpUo7xRqWEMwQ.', 'Salesman');

CREATE TABLE Brands (
    BrandId INT IDENTITY(1,1) PRIMARY KEY,
    BrandName NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE Classes (
    ClassId INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(50) UNIQUE NOT NULL
);

-- Insert Dummy Brands
INSERT INTO Brands (BrandName) VALUES ('Audi'), ('BMW'), ('Mercedes'), ('Tesla');

-- Insert Dummy Classes
INSERT INTO Classes (ClassName) VALUES ('A-Class'), ('B-Class'), ('C-Class');

CREATE TABLE CarModels (
    ModelId INT IDENTITY(1,1) PRIMARY KEY,
    BrandId INT,
    ClassId INT,
    ModelName NVARCHAR(100),
    ModelCode NVARCHAR(50) UNIQUE,
    Description NVARCHAR(500),
    Features NVARCHAR(500),
    Price DECIMAL(18,2),
    DateOfManufacturing DATE,
    IsActive BIT DEFAULT 1,
    SortOrder INT DEFAULT 0,
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

-- Insert Dummy Car Models
INSERT INTO CarModels (BrandId, ClassId, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, IsActive, SortOrder) 
VALUES 
(1, 1, 'Audi A4', 'AUDI-A4', 'Luxury Sedan', 'Sunroof, Leather Seats', 45000, '2023-01-10', 1, 1),
(2, 2, 'BMW X5', 'BMW-X5', 'SUV with high performance', 'Panoramic Roof, Heated Seats', 60000, '2022-11-20', 1, 2),
(3, 3, 'Mercedes C-Class', 'MERC-C', 'Elegant Sedan', 'Adaptive Cruise, Smart Drive', 50000, '2023-05-15', 1, 3);

CREATE TABLE ModelImages (
    ImageId INT IDENTITY(1,1) PRIMARY KEY,
    ModelId INT NOT NULL,
    ImagePath NVARCHAR(255) NOT NULL,
    FOREIGN KEY (ModelId) REFERENCES CarModels(ModelId) ON DELETE CASCADE
);

-- Insert Dummy Model Images
INSERT INTO ModelImages (ModelId, ImagePath) 
VALUES 
(1, '/images/audi_a4_1.jpg'),
(1, '/images/audi_a4_2.jpg'),
(2, '/images/bmw_x5_1.jpg'),
(3, '/images/mercedes_c_1.jpg');

CREATE TABLE Salesmen (
    SalesmanId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    LastYearSales DECIMAL(18,2) NOT NULL
);

-- Insert Dummy Salesmen
INSERT INTO Salesmen (Name, LastYearSales) 
VALUES 
('John Doe', 550000), 
('Jane Smith', 400000);

CREATE TABLE Sales (
    SaleId INT IDENTITY(1,1) PRIMARY KEY,
    SalesmanId INT NOT NULL,
    ModelId INT NOT NULL,
    SaleDate DATE NOT NULL,
    FOREIGN KEY (SalesmanId) REFERENCES Salesmen(SalesmanId) ON DELETE CASCADE,
    FOREIGN KEY (ModelId) REFERENCES CarModels(ModelId) ON DELETE CASCADE
);

-- Insert Dummy Sales
INSERT INTO Sales (SalesmanId, ModelId, SaleDate) 
VALUES 
(1, 1, '2024-01-10'),
(1, 2, '2024-01-12'),
(2, 3, '2024-01-15'),
(2, 1, '2024-01-18');
