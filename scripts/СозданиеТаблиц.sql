CREATE TABLE Филиалы (
    КодФилиала INT PRIMARY KEY IDENTITY,
    Название NVARCHAR(100) NOT NULL,
    Адрес NVARCHAR(255),
    Город NVARCHAR(50),
    НомерТелефона NVARCHAR(50)
);

CREATE TABLE ТипыЧая (
    КодТипЧая INT PRIMARY KEY IDENTITY,
    Название NVARCHAR(50) NOT NULL
);

CREATE TABLE Чай (
    КодЧая INT PRIMARY KEY IDENTITY,
    Название NVARCHAR(100) NOT NULL,
    КодТипЧая INT NOT NULL,
	КодПоставщика INT NOT NULL,
    Описание NVARCHAR(255),
    Цена DECIMAL(10, 2) NOT NULL,
    Остаток INT NOT NULL
);

CREATE TABLE Поставщики (
    КодПоставщика INT PRIMARY KEY IDENTITY,
    Название NVARCHAR(100) NOT NULL,
    Страна NVARCHAR(50),
    Телефон NVARCHAR(30)
);

CREATE TABLE Клиенты (
    КодКлиента INT PRIMARY KEY IDENTITY,
    ПолноеИмя NVARCHAR(100) NOT NULL,
    Почта NVARCHAR(100),
    Телефон NVARCHAR(30)
);

CREATE TABLE Заказы (
    КодЗаказа INT PRIMARY KEY IDENTITY,
    КодКлиента INT NOT NULL,
    КодФилиала INT NOT NULL,
    ДатаЗаказа DATE NOT NULL,
    ОбщаяСумма DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Отзывы (
	КодОтзыва INT PRIMARY KEY IDENTITY,
    КодКлиента INT NOT NULL,
    КодЧая INT NOT NULL,
    Оценка INT NOT NULL, -- Оценка от 1 до 5
    Комментарий NVARCHAR(500),
    ДатаОтзыва DATE NOT NULL
);

CREATE TABLE Акции (
    КодАкции INT PRIMARY KEY IDENTITY,
    Название NVARCHAR(100) NOT NULL,
    ДатаНачала DATE NOT NULL,
    ДатаОкончания DATE NOT NULL,
    ПроцентСкидки DECIMAL(5, 2) NOT NULL
);



CREATE TABLE Сотрудники (
    КодСотрудника INT NOT NULL IDENTITY(1,1),
    Логин NVARCHAR(100) NOT NULL UNIQUE,
    ФИО NVARCHAR(100) NOT NULL,
    Роль NVARCHAR(100) NOT NULL,
    Пароль NVARCHAR(256) NOT NULL,
    PRIMARY KEY (КодСотрудника)
);

CREATE TABLE ТоварыВЗаказе (
    КодТовараЗаказа INT PRIMARY KEY IDENTITY,
    КодЗаказа INT NOT NULL,
    КодЧая INT NOT NULL,
    Количество INT NOT NULL CHECK (Количество > 0),
    Цена DECIMAL(10, 2) NOT NULL,
    Сумма DECIMAL(10, 2) NOT NULL
);

