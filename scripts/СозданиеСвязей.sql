-- ����� ����� "���" � "�������"
ALTER TABLE ���
ADD CONSTRAINT FK_���_������� FOREIGN KEY (���������) REFERENCES �������(���������);

-- ����� ����� "���" � "����������"
ALTER TABLE ���
ADD CONSTRAINT FK_���_���������� FOREIGN KEY (�������������) REFERENCES ����������(�������������);

-- ����� ����� "������" � "�������"
ALTER TABLE ������
ADD CONSTRAINT FK_������_������� FOREIGN KEY (����������) REFERENCES �������(����������);

-- ����� ����� "������" � "�������"
ALTER TABLE ������
ADD CONSTRAINT FK_������_������� FOREIGN KEY (����������) REFERENCES �������(����������);

-- ����� ����� "������" � "�������"
ALTER TABLE ������
ADD CONSTRAINT FK_������_������� FOREIGN KEY (����������) REFERENCES �������(����������);

-- ����� ����� "������" � "���"
ALTER TABLE ������
ADD CONSTRAINT FK_������_��� FOREIGN KEY (������) REFERENCES ���(������);

ALTER TABLE �������������
ADD CONSTRAINT FK_�������������_������ FOREIGN KEY (���������) 
REFERENCES ������(���������) ON DELETE CASCADE;

-- ����� ����� "������ � ������" � "���"
ALTER TABLE �������������
ADD CONSTRAINT FK_�������������_��� FOREIGN KEY (������) REFERENCES ���(������);