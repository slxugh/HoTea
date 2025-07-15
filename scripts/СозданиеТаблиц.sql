CREATE TABLE ������� (
    ���������� INT PRIMARY KEY IDENTITY,
    �������� NVARCHAR(100) NOT NULL,
    ����� NVARCHAR(255),
    ����� NVARCHAR(50),
    ������������� NVARCHAR(50)
);

CREATE TABLE ������� (
    ��������� INT PRIMARY KEY IDENTITY,
    �������� NVARCHAR(50) NOT NULL
);

CREATE TABLE ��� (
    ������ INT PRIMARY KEY IDENTITY,
    �������� NVARCHAR(100) NOT NULL,
    ��������� INT NOT NULL,
	������������� INT NOT NULL,
    �������� NVARCHAR(255),
    ���� DECIMAL(10, 2) NOT NULL,
    ������� INT NOT NULL
);

CREATE TABLE ���������� (
    ������������� INT PRIMARY KEY IDENTITY,
    �������� NVARCHAR(100) NOT NULL,
    ������ NVARCHAR(50),
    ������� NVARCHAR(30)
);

CREATE TABLE ������� (
    ���������� INT PRIMARY KEY IDENTITY,
    ��������� NVARCHAR(100) NOT NULL,
    ����� NVARCHAR(100),
    ������� NVARCHAR(30)
);

CREATE TABLE ������ (
    ��������� INT PRIMARY KEY IDENTITY,
    ���������� INT NOT NULL,
    ���������� INT NOT NULL,
    ���������� DATE NOT NULL,
    ���������� DECIMAL(10, 2) NOT NULL
);

CREATE TABLE ������ (
	��������� INT PRIMARY KEY IDENTITY,
    ���������� INT NOT NULL,
    ������ INT NOT NULL,
    ������ INT NOT NULL, -- ������ �� 1 �� 5
    ����������� NVARCHAR(500),
    ���������� DATE NOT NULL
);

CREATE TABLE ����� (
    �������� INT PRIMARY KEY IDENTITY,
    �������� NVARCHAR(100) NOT NULL,
    ���������� DATE NOT NULL,
    ������������� DATE NOT NULL,
    ������������� DECIMAL(5, 2) NOT NULL
);



CREATE TABLE ���������� (
    ������������� INT NOT NULL IDENTITY(1,1),
    ����� NVARCHAR(100) NOT NULL UNIQUE,
    ��� NVARCHAR(100) NOT NULL,
    ���� NVARCHAR(100) NOT NULL,
    ������ NVARCHAR(256) NOT NULL,
    PRIMARY KEY (�������������)
);

CREATE TABLE ������������� (
    ��������������� INT PRIMARY KEY IDENTITY,
    ��������� INT NOT NULL,
    ������ INT NOT NULL,
    ���������� INT NOT NULL CHECK (���������� > 0),
    ���� DECIMAL(10, 2) NOT NULL,
    ����� DECIMAL(10, 2) NOT NULL
);

