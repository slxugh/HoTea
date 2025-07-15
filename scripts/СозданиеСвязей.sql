-- Связь между "Чай" и "ТипыЧая"
ALTER TABLE Чай
ADD CONSTRAINT FK_Чай_ТипыЧая FOREIGN KEY (КодТипЧая) REFERENCES ТипыЧая(КодТипЧая);

-- Связь между "Чай" и "Поставщики"
ALTER TABLE Чай
ADD CONSTRAINT FK_Чай_Поставщики FOREIGN KEY (КодПоставщика) REFERENCES Поставщики(КодПоставщика);

-- Связь между "Заказы" и "Клиенты"
ALTER TABLE Заказы
ADD CONSTRAINT FK_Заказы_Клиенты FOREIGN KEY (КодКлиента) REFERENCES Клиенты(КодКлиента);

-- Связь между "Заказы" и "Филиалы"
ALTER TABLE Заказы
ADD CONSTRAINT FK_Заказы_Филиалы FOREIGN KEY (КодФилиала) REFERENCES Филиалы(КодФилиала);

-- Связь между "Отзывы" и "Клиенты"
ALTER TABLE Отзывы
ADD CONSTRAINT FK_Отзывы_Клиенты FOREIGN KEY (КодКлиента) REFERENCES Клиенты(КодКлиента);

-- Связь между "Отзывы" и "Чай"
ALTER TABLE Отзывы
ADD CONSTRAINT FK_Отзывы_Чай FOREIGN KEY (КодЧая) REFERENCES Чай(КодЧая);

ALTER TABLE ТоварыВЗаказе
ADD CONSTRAINT FK_ТоварыВЗаказе_Заказы FOREIGN KEY (КодЗаказа) 
REFERENCES Заказы(КодЗаказа) ON DELETE CASCADE;

-- Связь между "Товары в заказе" и "Чай"
ALTER TABLE ТоварыВЗаказе
ADD CONSTRAINT FK_ТоварыВЗаказе_Чай FOREIGN KEY (КодЧая) REFERENCES Чай(КодЧая);