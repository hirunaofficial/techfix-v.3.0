USE TechFix;

-- Drop existing tables if they exist (to avoid conflicts during re-creation)
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Inventory;

-- Create Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255),
    Role NVARCHAR(50),
    Name NVARCHAR(100),
    Location NVARCHAR(100),
    ContactNumber NVARCHAR(20),
    Email NVARCHAR(100) UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Sample Data for Users
INSERT INTO Users (Username, Password, Role, Name, Location, ContactNumber, Email)
VALUES 
('admin1', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Admin', 'John Doe', 'New York', '1234567890', 'admin1@example.com'),
('supplier1', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Supplier', 'Alice Smith', 'Los Angeles', '0987654321', 'supplier1@example.com'),
('supplier2', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Supplier', 'Bob Johnson', 'San Francisco', '5555555555', 'supplier2@example.com'),
('admin2', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Admin', 'Jane Doe', 'Chicago', '1112223333', 'admin2@example.com');

-- Create Products Table
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Quantity INT,
    Price DECIMAL(18,2),
    Discount DECIMAL(5,2),
    SupplierId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id)
);

-- Sample Data for Products with correct Supplier IDs
INSERT INTO Products (ItemName, Quantity, Price, Discount, SupplierId)
VALUES 
('Laptop', 30, 1200.00, 10.00, 2),  -- SupplierId 2 for 'Alice Smith'
('Smartphone', 100, 800.00, 5.00, 2),
('Monitor', 75, 300.00, 15.00, 2),
('Tablet', 50, 400.00, 8.00, 3),     -- SupplierId 3 for 'Bob Johnson'
('Printer', 20, 150.00, 12.00, 3);   -- Another product from 'Bob Johnson'

-- Create Inventory Table
CREATE TABLE Inventory (
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Quantity INT,
    Price DECIMAL(18,2),
    Discount DECIMAL(5,2),
    SupplierId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id)
);

-- Sample Data for Inventory with consistent Supplier IDs
INSERT INTO Inventory (ItemName, Quantity, Price, Discount, SupplierId)
VALUES 
('Laptop', 25, 1200.00, 10.00, 2),  -- Corresponds to ProductId 1, realistic inventory for Laptop
('Smartphone', 90, 800.00, 5.00, 2), -- Corresponds to ProductId 2
('Monitor', 50, 300.00, 15.00, 2),   -- Corresponds to ProductId 3
('Tablet', 40, 400.00, 8.00, 3),     -- Corresponds to ProductId 4
('Printer', 10, 150.00, 12.00, 3);    -- Corresponds to ProductId 5

-- Create Orders Table
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    AdminId INT,   -- Foreign key reference to Users
    SupplierId INT,  -- Foreign key reference to Users
    ItemId INT,   -- Foreign key reference to Inventory
    Quantity INT,
    Status NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AdminId) REFERENCES Users(Id),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id),
    FOREIGN KEY (ItemId) REFERENCES Inventory(ItemId)
);

-- Sample Data for Orders with correct IDs
INSERT INTO Orders (AdminId, SupplierId, ItemId, Quantity, Status)
VALUES 
(1, 2, 1, 10, 'Pending'),  -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 1 (Laptop)
(1, 2, 2, 20, 'Shipped'),  -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 2 (Smartphone)
(1, 2, 3, 5, 'Delivered'),  -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 3 (Monitor)
(4, 3, 4, 15, 'Pending'),   -- AdminId 4 (Jane Doe), SupplierId 3 (Bob Johnson), ItemId 4 (Tablet)
(4, 3, 5, 10, 'Shipped');   -- AdminId 4 (Jane Doe), SupplierId 3 (Bob Johnson), ItemId 5 (Printer)
