CREATE TABLE tarifa (
	idtarifa VARCHAR2(37) PRIMARY KEY, -- identificacao unica da tarifa
	idcontacorrente VARCHAR2(37) NOT NULL, -- identificacao unica da conta corrente
	datamovimento VARCHAR2(25) NOT NULL, -- data do transferencia no formato DD/MM/YYYY
	valor NUMBER(18,2) NOT NULL -- valor da tarifa. Usar duas casas decimais.
)